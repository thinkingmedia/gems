namespace DataSources.Query
{
    public class AbstractQueryFeature : iParameters
    {
        /// <summary>
        /// The query reference
        /// </summary>
        private readonly iQueryBuilder _query;

        /// <summary>
        /// Constructor
        /// </summary>
        protected AbstractQueryFeature(iQueryBuilder pQuery)
        {
            _query = pQuery;
        }

        /// <summary>
        /// Generates a valid fieldname.
        /// </summary>
        public string Field(string pFieldname)
        {
            return _query.Field(pFieldname);
        }

        /// <summary>
        /// Gets the default primary key.
        /// </summary>
        /// <returns></returns>
        public string PrimaryKey()
        {
            return _query.PrimaryKey();
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
            return _query.Model();
        }

        /// <summary>
        /// Sets a parameter to be used in the query, and returns ID
        /// number of that parameter.
        /// </summary>
        public iQueryBuilder Param(string pName, object pValue)
        {
            return _query.Param(pName, pValue);
        }

        /// <summary>
        /// The next available parameter name.
        /// </summary>
        public string getNextParameterName()
        {
            return _query.getNextParameterName();
        }

        /// <summary>
        /// Attempts to extract the value of a field by following
        /// parameter references.
        /// </summary>
        public object getValue(string pFieldname)
        {
            return _query.getValue(pFieldname);
        }

        /// <summary>
        /// Access the variables for the query.
        /// </summary>
        public iParameters Variables()
        {
            return _query.Variables();
        }

        /// <summary>
        /// Gets the query reference.
        /// </summary>
        protected iQueryBuilder Query()
        {
            return _query;
        }
    }
}