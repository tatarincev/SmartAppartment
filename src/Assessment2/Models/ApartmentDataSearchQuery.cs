namespace Assessment2.Models
{
    /// <summary>
    /// Represents a query to search for apartment data in an index.
    /// </summary>
    public class IndexedSearchQuery
    {
        /// <summary>
        /// Search phrase
        /// </summary>
        public string Phrase { get; set; } = "*";
        /// <summary>
        /// Specify  a concrete apartment data type for filter search results 
        /// </summary>
        public ApartmentDataType? DataType { get; set; }
        public string[] Markets { get; set; }
        public int Top { get; set; } = 25;
        public string OrderBy { get; set; } = nameof(ApartmentDataSearchEntry.Name);
    }
}
