using System.Collections.Generic;

namespace Assessment2.Models
{
    /// <summary>
    /// Represents a result of search for apartment data in an index  performed by <see cref = "IndexedSearchQuery" />
    /// </summary>
    public class AppartmentDataSearchResult
    {
        public IList<ApartmentDataSearchEntry> Results { get; set; } = new List<ApartmentDataSearchEntry>();
        public long TotalCount { get; set; }
    }
}
