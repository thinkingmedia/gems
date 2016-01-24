using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DataSources.Exceptions;
using DataSources.Query;
using GemsLogger;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DataSources.DataSource
{
    /// <summary>
    /// Defines a connection for the MySQL database.
    /// </summary>
    public class MySqlDataSource : iDataSource
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (MySqlDataSource));

        /// <summary>
        /// Holds the _name of the current database.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The password
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// The address of the server
        /// </summary>
        private readonly string _server;

        /// <summary>
        /// The username
        /// </summary>
        private readonly string _user;

        /// <summary>
        /// Holds the connection to MySQL
        /// </summary>
        private MySqlConnection _conn;

        /// <summary>
        /// The current transaction (if any)
        /// </summary>
        private MySqlTransaction _currentTransaction;

        /// <summary>
        /// The schema description for this database connection.
        /// </summary>
        private SchemaClass _schema;

        /// <summary>
        /// Used to control access to _schema
        /// </summary>
        private Object _schemaLock = new Object();

        /// <summary>
        /// Only adds the text to the collection if value is not empty.
        /// </summary>
        private static void AddSyntax(ICollection<string> pSQL, string pType, string pValue)
        {
            if (string.IsNullOrWhiteSpace(pValue))
            {
                return;
            }

            pSQL.Add(string.Format("{0} {1}", pType, pValue));
        }

        /// <summary>
        /// Reads the tables used for each column.
        /// </summary>
        private static List<string> GetTableNames(IDataReader pReader)
        {
            DataTable schemaTable = pReader.GetSchemaTable();
            if (schemaTable == null)
            {
                throw new ModelException("Unable to get schema table from reader.");
            }

            List<string> tables = new List<string>();
            for (int row = 0, rowc = schemaTable.Rows.Count; row < rowc; row++)
            {
                tables.Add(schemaTable.Rows[row]["BaseTableName"].ToString());
            }
            return tables;
        }

        /// <summary>
        /// Checks if a parameter is empty, and if so throws an exception.
        /// </summary>
        /// <param name="pName">The name of the parameter.</param>
        /// <param name="pValue">The value to check.</param>
        private static void ValidateParameter(string pName, string pValue)
        {
            if (!string.IsNullOrWhiteSpace(pValue))
            {
                return;
            }

            throw new ArgumentException(@"MySqlDataSource can not be created with null or empty parameter.", pName);
        }

        /// <summary>
        /// Creates a MySQL command.
        /// </summary>
        [Obsolete]
        private DbCommand Command(string pSQL)
        {
            Open();
            return new MySqlCommand(pSQL, _conn);
        }

        /// <summary>
        /// Executes a command using a transaction, unless there is
        /// already a transaction started.
        /// </summary>
        private void CommandExecuteSafely(MySqlCommand pCmd, RunQuery pCallback)
        {
            if (_currentTransaction != null)
            {
                pCallback(pCmd);
                return;
            }

            Begin();
            try
            {
                pCallback(pCmd);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                _logger.Error(pCmd.CommandText);
                throw;
            }
        }

        /// <summary>
        /// Generates a prepared command from a query.
        /// </summary>
        /// <param name="pQuery">The query</param>
        /// <returns>The command to be executed.</returns>
        private DbCommand Compile(QueryBuilder pQuery)
        {
            string where = ((Conditions)pQuery.Where()).Root.Render();

            List<string> sql = new List<string>();

            Parameters variables = (Parameters)pQuery.Variables();
            if (variables.Count > 0)
            {
                sql.Add(MySqlRender.Variables(variables));
            }

            string table = pQuery.Model().Settings.Table;

            switch (pQuery.Type())
            {
                case QueryBuilder.eTYPE.SELECT:
                    List<string> fields = new List<string>
                                          {
                                              MySqlRender.Fields(table, (Fields)pQuery.Fields())
                                          };

                    fields.AddRange(
                        pQuery.Joins.JoinTables.Select(
                            pJoinTable=>MySqlRender.Fields(pJoinTable.Table, pJoinTable.Fields)
                            )
                        );

                    sql.Add(string.Format("SELECT {0} FROM `{1}`", string.Join(",", fields), table));
                    break;
                case QueryBuilder.eTYPE.UPDATE:
                    sql.Add(string.Format("UPDATE `{0}` SET {1}", table, MySqlRender.Set(pQuery.Fields())));
                    break;
                case QueryBuilder.eTYPE.DELETE:
                    sql.Add(string.Format("DELETE FROM `{0}`", table));
                    break;
                case QueryBuilder.eTYPE.INSERT:
                    pQuery.OnInsert();
                    sql.Add(string.Format("INSERT INTO `{0}` SET {1}", table, MySqlRender.Set(pQuery.Fields())));
                    if (pQuery.OnDuplicateEnabled)
                    {
                        sql.Add(string.Format("ON DUPLICATE KEY UPDATE {0}", MySqlRender.Set(pQuery.Duplicates())));
                    }
                    break;
            }

            if (pQuery.Joins.isJoin())
            {
                sql.Add(MySqlRender.Join(pQuery.Joins));
            }

            switch (pQuery.Type())
            {
                case QueryBuilder.eTYPE.SELECT:
                case QueryBuilder.eTYPE.UPDATE:
                case QueryBuilder.eTYPE.DELETE:
                    AddSyntax(sql, "WHERE", where);
                    if (pQuery.Type() == QueryBuilder.eTYPE.SELECT)
                    {
                        AddSyntax(sql, "GROUP BY", MySqlRender.Groups(table, pQuery.Groups()));
                    }
                    AddSyntax(sql, "ORDER BY", MySqlRender.Order(pQuery));
                    if (pQuery.Limit > 0)
                    {
                        sql.Add(string.Format("LIMIT {0}", pQuery.Limit));
                        if (pQuery.Offset > 0)
                        {
                            sql.Add(string.Format("OFFSET {0}", pQuery.Offset));
                        }
                    }
                    break;
            }

            if (pQuery.Type() == QueryBuilder.eTYPE.SELECT && pQuery.Locked)
            {
                sql.Add("FOR UPDATE");
            }

            MySqlCommand cmd = new MySqlCommand(string.Join(" ", sql) + ";", _conn);

            foreach (KeyValuePair<string, object> pair in ((Parameters)pQuery.Parameters()).Data)
            {
                cmd.Parameters.AddWithValue(pair.Key, pair.Value);
            }

            return cmd;
        }

        /// <summary>
        /// Holds a cache of table schema.
        /// </summary>
        private SchemaClass getSchema()
        {
            lock (_schemaLock)
            {
                if (_schema != null)
                {
                    return _schema;
                }

                _schema = new SchemaClass();
                // load the _schema for this database
                string sql =
                    string.Format(
                        "SELECT `TABLE_NAME` AS `TABLE` FROM `INFORMATION_SCHEMA`.`TABLES` WHERE `TABLE_SCHEMA` = '{0}'",
                        _name);
                Records tables = Execute(sql);

                foreach (string tableName in tables.Select(pTable=>pTable.getString("TABLE")))
                {
                    sql =
                        string.Format(
                            "SELECT `COLUMN_NAME` AS `FIELD` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA` = '{0}' AND `TABLE_NAME` = '{1}'",
                            _name, tableName);
                    Records fields = Execute(sql);
                    List<string> fieldsList = fields.Select(pField=>pField.getString("FIELD")).ToList();
                    _schema.Add(tableName, fieldsList);
                }
                return _schema;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MySqlDataSource(string pServer, string pDatabase, string pUser, string pPassword)
        {
            ValidateParameter("Server", pServer);
            ValidateParameter("DataBase", pDatabase);
            ValidateParameter("User", pUser);
            ValidateParameter("Password", pPassword);

            _name = pDatabase;
            _server = pServer;
            _user = pUser;
            _password = pPassword;
        }

        /// <summary>
        /// Opens the db connection.
        /// </summary>
        public void Open()
        {
            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                {
                    return;
                }
                _conn.Close();
            }

            string user = MySqlHelper.EscapeString(_user);
            string password = MySqlHelper.EscapeString(_password);

            string connection =
                String.Format("server={0};database={1};user id='{2}';password='{3}';Allow User Variables=True", _server,
                    _name,
                    user, password);

            _logger.Debug(connection);

            _conn = new MySqlConnection(connection);
            _conn.Open();
        }

        /// <summary>
        /// Closes the db connection.
        /// </summary>
        public void Close()
        {
            if (_conn != null)
            {
                try
                {
                    _conn.Close();
                }
                catch (Exception e)
                {
                    _logger.Exception(e);
                }
            }
            _conn = null;
        }

        /// <summary>
        /// Closes the existing connection.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Generates a field _name.
        /// </summary>
        public string Field(string pTable, string pField)
        {
            return String.Format("`{0}`.`{1}`", pTable.Replace("`",""), pField.Replace("`",""));
        }

        /// <summary>
        /// Returns the _name of the current database.
        /// </summary>
        public string Name()
        {
            return _name;
        }

        /// <summary>
        /// Checks if a table exists in the current database.
        /// </summary>
        public bool Exist(string pTable)
        {
            return getSchema().Exist(pTable);
        }

        /// <summary>
        /// Checks if a table contains a field.
        /// </summary>
        public bool FieldExists(string pTable, string pField)
        {
            return getSchema().Schema(pTable, pField);
        }

        /// <summary>
        /// Executes a SQL statement and returns a set of CoreRecord _rows.
        /// </summary>
        [Obsolete]
        public Records Execute(string pSql)
        {
            Open();

            _logger.Debug("EXECUTING: {0}", pSql);

            Records rows = new Records();

            using (DbCommand cmd = Command(pSql))
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows.Add(new Record(reader));
                    }
                }
            }

            return rows;
        }

        /// <summary>
        /// Executes a non-query (UPDATE, DELETE)
        /// </summary>
        [Obsolete]
        public int ExecuteNonQuery(string pSql)
        {
            Open();

            _logger.Debug("EXECUTING: {0}", pSql);

            using (DbCommand cmd = Command(pSql))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the query and returns the results.
        /// </summary>
        /// <param name="pQuery">The query to execute.</param>
        /// <returns>The query results</returns>
        public QueryResult Execute(QueryBuilder pQuery)
        {
            Open();

            if (!pQuery.BeforeExecute())
            {
                return false;
            }

            using (MySqlCommand cmd = (MySqlCommand)Compile(pQuery))
            {
                QueryResult result = null;

                switch (pQuery.Type())
                {
                    case QueryBuilder.eTYPE.SELECT:
                    {
                        Records rows = new Records();
                        try
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
                            {
                                List<string> tableNames = GetTableNames(reader);

                                if (reader.FieldCount != tableNames.Count)
                                {
                                    throw new ModelException("Unable to read tables associated with columns.");
                                }

                                while (reader.Read())
                                {
                                    Record record = new Record(pQuery.Model());
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string table = tableNames[i];
                                        string name = reader.GetName(i);
                                        try
                                        {
                                            record.Get(table)[name] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                        }
                                        catch (MySqlConversionException ex)
                                        {
                                            _logger.Exception(ex);
                                            record.Get(table)[name] = null;
                                        }
                                    }

                                    rows.Add(record);
                                }
                            }
                            result = rows;
                        }
                        catch (Exception)
                        {
                            _logger.Error(cmd.CommandText);
                            throw;
                        }
                        break;
                    }

                    case QueryBuilder.eTYPE.INSERT:
                    {
                        CommandExecuteSafely(cmd,
                            pCmd=> { result = new QueryResult(pCmd.ExecuteNonQuery(), (uint)pCmd.LastInsertedId); });
                        break;
                    }

                    case QueryBuilder.eTYPE.UPDATE:
                    {
                        CommandExecuteSafely(cmd, pCmd=> { result = new QueryResult(pCmd.ExecuteNonQuery(), 0); });
                        break;
                    }

                    case QueryBuilder.eTYPE.DELETE:
                    {
                        CommandExecuteSafely(cmd, pCmd=> { result = pCmd.ExecuteNonQuery() == 1; });
                        break;
                    }
                }
                return pQuery.AfterExecute(result);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        public void Begin()
        {
            if (_currentTransaction != null)
            {
                throw new DataSourceException("Transaction already started.");
            }
            _currentTransaction = _conn.BeginTransaction();
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public void Commit()
        {
            if (_currentTransaction == null)
            {
                throw new DataSourceException("Unexpected transaction commit.");
            }
            _currentTransaction.Commit();
            _currentTransaction = null;
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public void Rollback()
        {
            if (_currentTransaction == null)
            {
                throw new DataSourceException("Unexpected transaction rollback.");
            }
            _currentTransaction.Rollback();
            _currentTransaction = null;
        }

        /// <summary>
        /// Callback for executing a SQL command.
        /// </summary>
        /// <param name="pCmd"></param>
        private delegate void RunQuery(MySqlCommand pCmd);

        /// <summary>
        /// Defines the type for the schema cache.
        /// </summary>
        private class SchemaClass : Dictionary<string, List<string>>
        {
            /// <summary>
            /// Checks if a table exists in the current database.
            /// </summary>
            public bool Exist(string pTable)
            {
                lock (this)
                {
                    return ContainsKey(pTable);
                }
            }

            /// <summary>
            /// Checks if a table contains a field.
            /// </summary>
            public bool Schema(string pTable, string pField)
            {
                lock (this)
                {
                    return ContainsKey(pTable) && this[pTable].Contains(pField);
                }
            }
        }
    }
}