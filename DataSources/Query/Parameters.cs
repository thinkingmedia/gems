using System.Collections.Generic;
using System.Globalization;
using DataSources.DataSource;
using DataSources.Exceptions;

namespace DataSources.Query
{
    public class Parameters : iParameters
    {
        /// <summary>
        /// Parameters keyed by their name.
        /// </summary>
        public readonly Dictionary<string, object> Data;

        /// <summary>
        /// The model
        /// </summary>
        private readonly Model _model;

        /// <summary>
        /// The query builder.
        /// </summary>
        private readonly QueryBuilder _query;

        /// <summary>
        /// The name of the table for the query.
        /// </summary>
        private readonly ModelOptions _settings;

        /// <summary>
        /// The data source
        /// </summary>
        private readonly iDataSource _source;

        /// <summary>
        /// How many parameters
        /// </summary>
        public int Count
        {
            get { return Data.Count; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Parameters(QueryBuilder pQuery, Model pModel)
        {
            _query = pQuery;
            _model = pModel;
            _settings = pModel.Settings;
            _source = pModel.DataSource;

            Data = new Dictionary<string, object>();
        }

        /// <summary>
        /// Generates a `Table`.`fieldname`
        /// </summary>
        public string Field(string pFieldName)
        {
            if (!pFieldName.Contains("."))
            {
                return _source.Field(_settings.Table, pFieldName);
            }

            string[] strings = pFieldName.Split(new[] {'.'});
            if (strings.Length != 2)
            {
                throw new ModelException("Invalid fieldname: {0}", pFieldName);
            }

            return _source.Field(strings[0], strings[1]);
        }

        /// <summary>
        /// Gets the default primary key.
        /// </summary>
        /// <returns></returns>
        public string PrimaryKey()
        {
            return _settings.PrimaryKey;
        }

        /// <summary>
        /// Gets the default display field for the model.
        /// </summary>
        public string DisplayField()
        {
            return _query.DisplayField();
        }

        /// <summary>
        /// Gets the model
        /// </summary>
        public Model Model()
        {
            return _model;
        }

        /// <summary>
        /// Sets a parameter to be used in the query, and returns ID
        /// number of that parameter.
        /// </summary>
        public iQueryBuilder Param(string pName, object pValue)
        {
            string key = string.Format("@{0}", pName);
            Data.Add(key, pValue);
            return _query;
        }

        /// <summary>
        /// The next available parameter name.
        /// </summary>
        public string getNextParameterName()
        {
            return Data.Count.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Attempts to extract the value of a field by following
        /// parameter references.
        /// </summary>
        public object getValue(string pFieldname)
        {
            FieldValue fieldValue = _query.Fields().Get(pFieldname);
            if (fieldValue == null || fieldValue.Value == null)
            {
                return null;
            }
            if (fieldValue.Value.StartsWith("@"))
            {
                return Data.ContainsKey(fieldValue.Value) ? Data[fieldValue.Value] : null;
            }
            return fieldValue.Value;
        }

        /// <summary>
        /// Access the variables for the query.
        /// </summary>
        public iParameters Variables()
        {
            return _query.Variables();
        }
    }
}