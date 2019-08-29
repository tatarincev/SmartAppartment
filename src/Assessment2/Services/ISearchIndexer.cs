using System;
using System.IO;
using System.Threading;

namespace Assessment2.Services
{
    /// <summary>
    /// Provides an abstraction for indexing  apartment data in an index
    /// </summary>
    public interface ISearchIndexer
    {
        void DoIndex<TDataItem>(Stream stream, Action<string> progressCallback, CancellationToken cancellationToken);
    }
}
