using System;
using Common.Utils;
using DataSources.Exceptions;

namespace DataSources.DataSource
{
    /// <summary>
    /// Maintains a list of data sources.
    /// </summary>
    // ReSharper disable MemberCanBePrivate.Global
    public sealed class DataSources : Singleton<DataSources>
    {
        /// <summary>
        /// Handles the creation of data sources.
        /// </summary>
        public Creators<string, iDataSource> Creators { get; private set; }

        /// <summary>
        /// Name of the default data source.
        /// </summary>
        public string Default { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataSources()
        {
            Creators = new Creators<string, iDataSource>();
        }

        /// <summary>
        /// Creates a data source connection for a given name.
        /// </summary>
        public iDataSource Create(string pName)
        {
            iDataSource source = Creators.Create(pName);
            source.Open();

            return source;
        }

        /// <summary>
        /// Creates the default data source connection.
        /// </summary>
        public iDataSource Create()
        {
            if (Default == null)
            {
                throw new NullReferenceException("Default DataSource has not been configured.");
            }

            return Create(Default);
        }

        /// <summary>
        /// Sets the default data source.
        /// </summary>
        public void setDefault(string pName)
        {
            if (!Creators.HasCreator(pName))
            {
                throw new ModelException(string.Format("DataSource [{0}] can not be default, because it does not exist.", pName));
            }
            Default = pName;
        }
    }
}
