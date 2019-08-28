using System;
using System.IO;
using System.Threading;

namespace Assessment2.Services
{
    public interface IIndexer
    {
        void DoIndex<TDataItem>(string indexName, Stream stream, Action<string> progressCallback, CancellationToken cancellationToken);
      
    }
}
