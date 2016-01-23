using System;
using Common.Annotations;

namespace DataSources
{
    /// <summary>
    /// The base class for all generated Entity classes.
    /// </summary>
    public abstract class RecordEntity
    {
        /// <summary>
        /// Access to the underlined record object.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBeProtected.Global
        public Record _Record { get; private set; }

        /// <summary>
        /// Changes the record for this entity.
        /// </summary>
        public void setRecord([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            _Record = pRecord;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected RecordEntity([NotNull] Record pRecord)
        {
            if (pRecord == null)
            {
                throw new ArgumentNullException("pRecord");
            }
            _Record = pRecord;
        }
    }
}