using System.Collections.Generic;
using System.Linq;
using DataSources.Exceptions;

namespace DataSources.Query
{
    /// <summary>
    /// Dynamically builds SQL queries.
    /// </summary>
    public class QueryBuilder : iQueryBuilder
    {
        /// <summary>
        /// Sorting directions
        /// </summary>
        public enum eDIR
        {
            ASC,
            DESC
        }

        /// <summary>
        /// The type of query.
        /// </summary>
        public enum eTYPE
        {
            SELECT,
            UPDATE,
            INSERT,
            DELETE
        }

        /// <summary>
        /// List of order by rules.
        /// </summary>
        public readonly List<KeyValuePair<eDIR, string>> Order;

        /// <summary>
        /// The fields to update on duplicate key inserts.
        /// </summary>
        private readonly Fields _duplicate;

        /// <summary>
        /// The fields for the query.
        /// </summary>
        private readonly Fields _fields;

        /// <summary>
        /// The fields for grouping the results.
        /// </summary>
        private readonly Fields _groups;

        /// <summary>
        /// The JOIN part of the query.
        /// </summary>
        public readonly Joinable Joins;

        /// <summary>
        /// The model
        /// </summary>
        private readonly Model _model;

        /// <summary>
        /// The parameters for the query.
        /// </summary>
        private readonly Parameters _parameters;

        /// <summary>
        /// The type of query being created.
        /// </summary>
        private readonly eTYPE _type;

        /// <summary>
        /// The parameters for the query.
        /// </summary>
        private readonly Parameters _variables;

        /// <summary>
        /// The query WHERE conditions
        /// </summary>
        private readonly iCondition _where;

        /// <summary>
        /// The limit for the query.
        /// </summary>
        public int Limit;

        /// <summary>
        /// The offset
        /// </summary>
        public int Offset;

        /// <summary>
        /// True to set field values for duplicate entries.
        /// </summary>
        public bool OnDuplicateEnabled;

        /// <summary>
        /// Should newly added fields go to duplicate list?
        /// </summary>
        private bool _addFieldsToDuplicate;

        /// <summary>
        /// Tells if this query will lock records.
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryBuilder(eTYPE pType, Model pModel)
        {
            _where = new Conditions(this);
            _type = pType;
            _model = pModel;
            Joins = new Joinable(this);

            _parameters = new Parameters(this, pModel);
            _variables = new Parameters(this, pModel);

            _fields = new Fields(this);
            _duplicate = new Fields(this);
            _groups = new Fields(this);

            Order = new List<KeyValuePair<eDIR, string>>();
            Limit = 0;
            Offset = 0;
            Locked = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryBuilder(eTYPE pType, Model pModel, IEnumerable<string> pFields)
            : this(pType, pModel)
        {
            if (pFields != null)
            {
                pFields
                    .ToList()
                    .ForEach(pStr=>_fields.Add(new FieldValue(pStr)));
            }
        }

        /// <summary>
        /// The fields for the query.
        /// </summary>
        public iFields Fields()
        {
            return _addFieldsToDuplicate ? _duplicate : _fields;
        }

        /// <summary>
        /// The GROUP BY fields for the query.
        /// </summary>
        public iFields Groups()
        {
            return _groups;
        }

        /// <summary>
        /// Starts a WHERE condition chain.
        /// </summary>
        /// <returns></returns>
        public iCondition Where()
        {
            return _where;
        }

        /// <summary>
        /// The type of query.
        /// </summary>
        /// <returns></returns>
        public eTYPE Type()
        {
            return _type;
        }

        /// <summary>
        /// Used with data sources that allow the current query to write
        /// lock rows examined by the query until the current transaction
        /// is committed.
        /// </summary>
        public iQueryBuilder Lock()
        {
            Locked = true;
            return this;
        }

        /// <summary>
        /// Enable on duplicate key update.
        /// </summary>
        public iQueryBuilder OnDuplicate()
        {
            if (_type != eTYPE.INSERT)
            {
                throw new ModelException("Not an insert query.");
            }
            OnDuplicateEnabled = true;
            _addFieldsToDuplicate = true;
            return this;
        }

        /// <summary>
        /// Enables setting fields for the INSERT part of the query.
        /// Call this if you're not sure if OnDuplicate was called.
        /// </summary>
        /// <returns></returns>
        public iQueryBuilder OnInsert()
        {
            if (_type != eTYPE.INSERT)
            {
                throw new ModelException("Not an insert query.");
            }
            _addFieldsToDuplicate = false;
            return this;
        }

        /// <summary>
        /// Generates a valid fieldname.
        /// </summary>
        public string Field(string pFieldname)
        {
            return _parameters.Field(pFieldname);
        }

        /// <summary>
        /// Gets the model
        /// </summary>
        public Model Model()
        {
            return _parameters.Model();
        }

        /// <summary>
        /// Sets a parameter to be used in the query, and returns ID
        /// number of that parameter.
        /// </summary>
        public iQueryBuilder Param(string pName, object pValue)
        {
            return _parameters.Param(pName, pValue);
        }

        /// <summary>
        /// Gets the default primary key.
        /// </summary>
        /// <returns></returns>
        public string PrimaryKey()
        {
            return _parameters.PrimaryKey();
        }

        /// <summary>
        /// Gets the default display field for the model.
        /// </summary>
        public string DisplayField()
        {
            return _parameters.DisplayField();
        }

        /// <summary>
        /// The next available parameter name.
        /// </summary>
        public string getNextParameterName()
        {
            return _parameters.getNextParameterName();
        }

        /// <summary>
        /// Attempts to extract the value of a field by following
        /// parameter references.
        /// </summary>
        public object getValue(string pFieldname)
        {
            return _parameters.getValue(pFieldname);
        }

        /// <summary>
        /// Access the variables for the query.
        /// </summary>
        public iParameters Variables()
        {
            return _variables;
        }

        /// <summary>
        /// Called by the data source after executing the query.
        /// </summary>
        public QueryResult AfterExecute(QueryResult pResult)
        {
            switch (_type)
            {
                case eTYPE.SELECT:
                    pResult.Records = _model.Behaviors.AfterSelect(pResult.Records);
                    break;

                case eTYPE.INSERT:
                    _model.Behaviors.AfterInsert();
                    break;

                case eTYPE.UPDATE:
                    _model.Behaviors.AfterUpdate();
                    break;

                case eTYPE.DELETE:
                    _model.Behaviors.AfterDelete();
                    break;
            }

            return pResult;
        }

        /// <summary>
        /// Called by the data source before executing the query.
        /// </summary>
        public bool BeforeExecute()
        {
            switch (_type)
            {
                case eTYPE.SELECT:
                    _model.Behaviors.BeforeSelect(this);
                    break;
                case eTYPE.INSERT:
                    OnInsert();
                    return _model.Behaviors.BeforeInsert(this);
                case eTYPE.UPDATE:
                    return _model.Behaviors.BeforeUpdate(this);
                case eTYPE.DELETE:
                    return _model.Behaviors.BeforeDelete(this);
            }

            return true;
        }

        /// <summary>
        /// The fields to set on ON DUPLICATE UPDATE
        /// </summary>
        /// <returns></returns>
        public iFields Duplicates()
        {
            return _duplicate;
        }

        /// <summary>
        /// Access the parameters for the query.
        /// </summary>
        public iParameters Parameters()
        {
            return _parameters;
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder Join(eJOIN pJoinType, bool pAuto, Model pModel, IEnumerable<string> pFields)
        {
            return ((iJoinable)Joins).Join(pJoinType, pAuto, pModel, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder Join(eJOIN pJoinType, string pTable, IEnumerable<string> pFields)
        {
            return ((iJoinable)Joins).Join(pJoinType, pTable, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder LeftJoin(Model pModel, params string[] pFields)
        {
            return ((iJoinable)Joins).LeftJoin(pModel, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder LeftJoin(string pTable, params string[] pFields)
        {
            return ((iJoinable)Joins).LeftJoin(pTable, pFields);
        }

        /// <summary>
        /// Starts a JOIN ON condition chain
        /// </summary>
        public iCondition On()
        {
            return ((iJoinable)Joins).On();
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder RightJoin(string pTable, params string[] pFields)
        {
            return ((iJoinable)Joins).RightJoin(pTable, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder RightJoin(Model pModel, params string[] pFields)
        {
            return ((iJoinable)Joins).RightJoin(pModel, pFields);
        }
    }
}