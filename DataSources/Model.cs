using System;
using System.Collections.Generic;
using Common.Annotations;
using Common.Events;
using DataSources.Behavior;
using DataSources.DataSource;
using DataSources.Exceptions;
using DataSources.Query;

namespace DataSources
{
    /// <summary>
    /// General data model.
    /// </summary>
    public class Model : DefaultModelEvents
    {
        /// <summary>
        /// Default options for models.
        /// </summary>
        private static readonly ModelOptions _defaults = new ModelOptions();

        /// <summary>
        /// The current primary ID value.
        /// </summary>
        public readonly PrimaryValue ID;

        /// <summary>
        /// The current data the model is working on.
        /// </summary>
        private Record _data;

        /// <summary>
        /// The behaviors for this model.
        /// </summary>
        public BehaviorContainer Behaviors { get; private set; }

        /// <summary>
        /// The current data the model is working on.
        /// </summary>
        public Record Data
        {
            get { return _data ?? new Record(); }
            protected set { _data = new Record(value); }
        }

        /// <summary>
        /// The DataSource for the Model.
        /// </summary>
        public iDataSource DataSource { get; private set; }

        /// <summary>
        /// Settings for this model.
        /// </summary>
        public ModelOptions Settings { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pSource"></param>
        protected Model(iDataSource pSource)
            : this(new ModelOptions(), pSource)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Model([NotNull] ModelOptions pOptions, [NotNull] iDataSource pSource)
        {
            if (pOptions == null)
            {
                throw new ArgumentNullException("pOptions");
            }
            if (pSource == null)
            {
                throw new ArgumentNullException("pSource");
            }

            Settings = PropertyMerger.ObjectMerge(_defaults, pOptions);
            Settings.Initialize(GetType());

            ID = new PrimaryValue(Settings.PrimaryType);

            Behaviors = new BehaviorContainer(this);
            Behaviors.RegisterEventListener(this);
            Behaviors.Add(new TimestampBehavior("created", true, false));
            Behaviors.Add(new TimestampBehavior("updated", true, true));

            DataSource = pSource;
            if (!DataSource.Exist(Settings.Table))
            {
                throw new ModelException(string.Format("Table:{0} doesn't exist in database {1}.", Settings.Table,
                    DataSource.Name()));
            }
        }

        /// <summary>
        /// Starts a SQL transaction.
        /// </summary>
        public void Begin()
        {
            DataSource.Begin();
        }

        /// <summary>
        /// Commits a SQL transaction.
        /// </summary>
        public void Commit()
        {
            DataSource.Commit();
        }

        /// <summary>
        /// Creates a query to count the records in a query.
        /// </summary>
        /// <returns>A new query object.</returns>
        public iQueryBuilder Count()
        {
            return new QueryBuilder(QueryBuilder.eTYPE.SELECT, this, new[] {"count(*)"});
        }

        /// <summary>
        /// Creates a new record using the values passed.
        /// </summary>
        /// <param name="pRecord">The data for the new record.</param>
        /// <returns>The new primary value, or null if creation aborted.</returns>
        public PrimaryValue Create([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return new PrimaryValue(Insert(pRecord).ExecuteLastID());
        }

        /// <summary>
        /// Creates a new record using a RecordEntity.
        /// </summary>
        public PrimaryValue Create([NotNull] RecordEntity pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return new PrimaryValue(Insert(pRecord).ExecuteLastID());
        }

        /// <summary>
        /// Decreases the value of a field.
        /// </summary>
        public void Decrease(UInt32 pID, [NotNull] string pFieldname, int pValue = 1)
        {
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            Update()
                .SetEx(pFieldname, Field(pFieldname) + " - @value")
                .Param("value", pValue)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Deletes a record with the ID.
        /// </summary>
        public bool Delete([NotNull] PrimaryValue pID)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            return Delete().Where().isID(pID).End().Execute() == 1;
        }

        /// <summary>
        /// Deletes a record with the ID.
        /// </summary>
        public bool Delete(UInt32 pID)
        {
            return Delete().Where().isID(pID).End().Execute() == 1;
        }

        /// <summary>
        /// Deletes a record with the ID.
        /// </summary>
        public bool Delete(Guid pID)
        {
            return Delete()
                .Where()
                .isID(pID)
                .End()
                .Execute() == 1;
        }

        /// <summary>
        /// Starts a delete query.
        /// </summary>
        public iQueryBuilder Delete()
        {
            return new QueryBuilder(QueryBuilder.eTYPE.DELETE, this);
        }

        /// <summary>
        /// Formats a fieldname in quotes for the SQL statement, and includes
        /// the table reference.
        /// </summary>
        public string Field([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }
            return DataSource.Field(Settings.Table, pName);
        }

        /// <summary>
        /// Checks if a field exits in the table.
        /// </summary>
        /// <param name="pFieldname">Name of the field.</param>
        /// <returns>True if it exists</returns>
        public bool HasField([NotNull] string pFieldname)
        {
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            return DataSource.FieldExists(Settings.Table, pFieldname);
        }

        /// <summary>
        /// Returns true if the record ID exists in the table.
        /// </summary>
        public bool HasRecord([NotNull] PrimaryValue pID)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            return Count().Where().isID(ID).End().Scalar<int>() == 1;
        }

        /// <summary>
        /// Increases the value of a field.
        /// </summary>
        public void Increase(UInt32 pID, [NotNull] string pFieldname, int pValue = 1)
        {
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            Update()
                .SetEx(pFieldname, Field(pFieldname) + " + @value")
                .Param("value", pValue)
                .Where()
                .isID(pID)
                .End()
                .Execute();
        }

        /// <summary>
        /// Creates a query to insert a record.
        /// </summary>
        public iQueryBuilder Insert()
        {
            return new QueryBuilder(QueryBuilder.eTYPE.INSERT, this);
        }

        /// <summary>
        /// Creates a query to insert a record.
        /// </summary>
        public iQueryBuilder Insert([NotNull] RecordEntity pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return Insert().Set(pRecord);
        }

        /// <summary>
        /// Creates a query to insert a record.
        /// </summary>
        public iQueryBuilder Insert([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return Insert().Set(pRecord);
        }

        /// <summary>
        /// Throws an exception if ID is empty.
        /// </summary>
        public void MustHaveID()
        {
            if (ID.Empty)
            {
                throw new ModelException("ID not set for model.");
            }
        }

        /// <summary>
        /// Reads the entity record for the ID.
        /// </summary>
        public T Read<T>([NotNull] PrimaryValue pID, [CanBeNull] IEnumerable<string> pFields = null) where T : RecordEntity, new()
        {
            T entity = new T();
            entity.setRecord(Read(pID, pFields));
            return entity;
        }

        /// <summary>
        /// Reads the record for the ID.
        /// </summary>
        public Record Read([NotNull] PrimaryValue pID, [CanBeNull] IEnumerable<string> pFields = null)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            return Select(pFields).Where().isID(pID).End().One();
        }

        /// <summary>
        /// Reads the record for the ID.
        /// </summary>
        public Record Read(UInt32 pID, params string[] pFields)
        {
            return Select(pFields).Where().isID(pID).End().One();
        }

        /// <summary>
        /// Reads the record for the ID.
        /// </summary>
        public Record Read(UInt32 pID, [CanBeNull] IEnumerable<string> pFields = null)
        {
            return Select(pFields).Where().isID(pID).End().One();
        }

        /// <summary>
        /// Reads the record for the ID.
        /// </summary>
        public Record Read(Guid pID, [CanBeNull] IEnumerable<string> pFields = null)
        {
            return Select(pFields).Where().isGuid(pID).End().One();
        }

        /// <summary>
        /// Reads the current record for the ID.
        /// </summary>
        public Record Read([CanBeNull] IEnumerable<string> pFields = null)
        {
            MustHaveID();
            return Select(pFields).Where().isID(ID).End().One();
        }

        /// <summary>
        /// Reads a single field from the record.
        /// </summary>
        public T ReadField<T>(UInt32 pID, [NotNull] string pField)
        {
            if (pField == null)
            {
                throw new ArgumentNullException("pField");
            }
            return Select(pField).Where().isID(pID).End().Scalar<T>();
        }

        /// <summary>
        /// Reads a single field from the record.
        /// </summary>
        public T ReadField<T>(Guid pID, [NotNull] string pField)
        {
            if (pField == null)
            {
                throw new ArgumentNullException("pField");
            }
            return Select(pField).Where().isID(pID).End().Scalar<T>();
        }

        /// <summary>
        /// Reads a single field from the record.
        /// </summary>
        public T ReadField<T>([NotNull] string pField)
        {
            if (pField == null)
            {
                throw new ArgumentNullException("pField");
            }
            MustHaveID();
            return ID.isGUID ? ReadField<T>((Guid)ID, pField) : ReadField<T>((UInt32)ID, pField);
        }

        /// <summary>
        /// Rolls back a SQL transaction.
        /// </summary>
        public void Rollback()
        {
            DataSource.Rollback();
        }

        /// <summary>
        /// Save for record with ID.
        /// </summary>
        public bool Save([NotNull] PrimaryValue pID, [NotNull] Record pRecord)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            ID.set(pID);
            return Save(pRecord);
        }

        /// <summary>
        /// Save record with ID.
        /// </summary>
        public bool Save([NotNull] PrimaryValue pID, [NotNull] RecordEntity pRecord)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return Save(pID, pRecord._Record);
        }

        /// <summary>
        /// Save for record with ID.
        /// </summary>
        public bool Save(UInt32 pID, [NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            ID.set(pID);
            return Save(pRecord);
        }

        /// <summary>
        /// Save for record with ID.
        /// </summary>
        public bool Save(Guid pID, [NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            ID.set(pID);
            return Save(pRecord);
        }

        /// <summary>
        /// Save for record with ID.
        /// </summary>
        public bool Save(UInt32 pID, [NotNull] RecordEntity pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return Save(pID, pRecord._Record);
        }

        /// <summary>
        /// Save for record with ID.
        /// </summary>
        public bool Save(Guid pID, [NotNull] RecordEntity pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            return Save(pID, pRecord._Record);
        }

        /// <summary>
        /// Updates a record with a matching ID.
        /// </summary>
        public bool Save([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }

            // fix: When an entity record is loaded, and resaved. These fields are in the update query, and this stops the timestamp behavior from working correctly.
            if (pRecord.Has("created"))
            {
                pRecord.Remove("created");
            }
            if (pRecord.Has("updated"))
            {
                pRecord.Remove("updated");
            }

            return Update().Set(pRecord).Where().isID(ID).End().Limit(1).Execute() == 1;
        }

        /// <summary>
        /// Updates a record with a matching ID.
        /// </summary>
        public bool Save([NotNull] RecordEntity pEntity)
        {
            if (pEntity == null)
            {
                throw new ArgumentNullException("pEntity");
            }
            if (!pEntity._Record.Has(Settings.PrimaryKey))
            {
                throw new ModelException("Record does not have {0} set.", Settings.PrimaryKey);
            }
            return Save(pEntity._Record.ID, pEntity);
        }

        /// <summary>
        /// Updates a single field.
        /// </summary>
        public bool SaveField([NotNull] PrimaryValue pID, [NotNull] string pFieldname, [CanBeNull] object pValue)
        {
            if (pID == null)
            {
                throw new ArgumentNullException("pID");
            }
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            return Update().Set(pFieldname, pValue).Where().isID(pID).End().Limit(1).Execute() == 1;
        }

        /// <summary>
        /// Updates a single field.
        /// </summary>
        public bool SaveField(UInt32 pID, [NotNull] string pFieldname, [CanBeNull] object pValue)
        {
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            return Update().Set(pFieldname, pValue).Where().isID(pID).End().Limit(1).Execute() == 1;
        }

        /// <summary>
        /// Updates a single field.
        /// </summary>
        public bool SaveField(Guid pID, [NotNull] string pFieldname, [CanBeNull] object pValue)
        {
            if (pFieldname == null)
            {
                throw new ArgumentNullException("pFieldname");
            }
            return Update().Set(pFieldname, pValue).Where().isGuid(pID).End().Limit(1).Execute() == 1;
        }

        /// <summary>
        /// Creates a SELECT query.
        /// </summary>
        /// <param name="pFields">A list of fieldnames or all fields by default.</param>
        /// <returns>A new query builder object.</returns>
        public iQueryBuilder Select(params string[] pFields)
        {
            return new QueryBuilder(QueryBuilder.eTYPE.SELECT, this, pFields);
        }

        /// <summary>
        /// Creates a SELECT query.
        /// </summary>
        /// <param name="pFields">A list of fieldnames or all fields by default.</param>
        /// <returns>A new query builder object.</returns>
        public iQueryBuilder Select([CanBeNull] IEnumerable<string> pFields)
        {
            return new QueryBuilder(QueryBuilder.eTYPE.SELECT, this, pFields);
        }

        /// <summary>
        /// Creates an UPDATE query. Use the SET methods
        /// to assign values.
        /// </summary>
        /// <returns>A new query builder object.</returns>
        public iQueryBuilder Update()
        {
            return new QueryBuilder(QueryBuilder.eTYPE.UPDATE, this);
        }
    }
}