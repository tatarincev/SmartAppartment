using System.Collections.Generic;

namespace Assessment2.Models
{
    /// <summary>
    /// Interface for implementing a data source iterator
    /// </summary>
    public interface IPagedDataSource<out TItem>
    {
        /// <summary>
        /// The number of the page that will returned by <see cref = "Fetch" />
        /// </summary>
        int CurrentPageNumber { get; }
        /// <summary>
        /// The number of records in the page
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// Gets the data page from the source according to currently set <see cref = "CurrentPageNumber" /> and <see cref = "PageSize" />.
        /// Implementations should increment <see cref="CurrentPageNumber"/> after fetch.
        /// </summary>
        /// <returns>Is some data received</returns>
        IEnumerable<TItem> FetchNextPage();

    }
}
