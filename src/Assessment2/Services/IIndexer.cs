using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Assessment2.Services
{
    public interface IIndexer
    {
        void DoIndex<TDataItem>(Stream stream, Action<string> progressCallback, CancellationToken cancellationToken);
    }
}
