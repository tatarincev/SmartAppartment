namespace Assessment2.Models
{
    public class IndexedSearchQuery
    {
        /// <summary>
        /// Search phrase
        /// </summary>
        public string Phrase { get; set; } = "*";
        /// <summary>
        /// Represents a type of document in search index, supported values: company || property
        /// </summary>
        public string DocType { get; set; }
        public string[] Markets { get; set; }
        public int Top { get; set; } = 25;
        public string OrderBy { get; set; }
    }
}
