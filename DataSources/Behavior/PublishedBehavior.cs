namespace DataSources.Behavior
{
    /// <summary>
    /// Common behaviors related to records that contain an active column for
    /// toggling an enabled/disabled state.
    /// </summary>
    public class PublishedBehavior : Behavior
    {
        /// <summary>
        /// Name of the column
        /// </summary>
        public readonly string FieldName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pFieldname">Name of the published column.</param>
        public PublishedBehavior(string pFieldname = "published")
        {
            FieldName = pFieldname;
        }
    }
}