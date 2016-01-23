namespace DataSources.Query
{
    public interface iParameters
    {
        /// <summary>
        /// Generates a valid fieldname.
        /// </summary>
        string Field(string pFieldname);

        /// <summary>
        /// Gets the model
        /// </summary>
        Model Model();

        /// <summary>
        /// Sets a parameter to be used in the query, and returns ID
        /// number of that parameter.
        /// </summary>
        iQueryBuilder Param(string pName, object pValue);

        /// <summary>
        /// Gets the default primary key.
        /// </summary>
        string PrimaryKey();

        /// <summary>
        /// Gets the default display field for the model.
        /// </summary>
        string DisplayField();

        /// <summary>
        /// The next available parameter name.
        /// </summary>
        string getNextParameterName();

        /// <summary>
        /// Attempts to extract the value of a field by following
        /// parameter references.
        /// </summary>
        object getValue(string pFieldname);

        /// <summary>
        /// Access the variables for the query.
        /// </summary>
        iParameters Variables();
    }
}