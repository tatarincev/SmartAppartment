using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Assessment2.JsonConverters;
using Assessment2.Models;
using Newtonsoft.Json;

namespace Assessment2.Services
{
    public class JsonPagedDataSource<TItem> : IPagedDataSource<TItem>, IDisposable     
    {
        private readonly StreamReader _streamReader;
        private readonly JsonTextReader _jsonTextReader;
        private readonly JsonSerializer _serializer;

        public JsonPagedDataSource(Stream stream)
        {
            _streamReader = new StreamReader(stream);
            _jsonTextReader = new JsonTextReader(_streamReader);

            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new PolymorphTypesJsonConverter());
        }

        public IEnumerable<TItem> Items { get; private set; }

        public int CurrentPageNumber { get; protected set; } = 0;
        public int PageSize { get; set; } = 50;

        public IEnumerable<TItem> FetchNextPage()
        {
            var result = InnerFetchPage(CurrentPageNumber * PageSize, PageSize);
            CurrentPageNumber++;
            return result;
        }

        protected virtual IEnumerable<TItem> InnerFetchPage(int skip, int take)
        {
            var result = new List<TItem>();
            var processedCount = 0;

            if (_jsonTextReader.TokenType == JsonToken.None)
            {
                _jsonTextReader.Read();
                if (_jsonTextReader.TokenType == JsonToken.StartArray)
                {
                    _jsonTextReader.Read();
                }
            }
            while (_jsonTextReader.TokenType != JsonToken.EndArray && (processedCount == 0 || processedCount % PageSize > 0))
            {
                var item = _serializer.Deserialize<TItem>(_jsonTextReader);
                result.Add(item);
                processedCount++;
                _jsonTextReader.Read();
            }
            return result;
        }

      
        #region IDisposable pattern implementation
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Code to dispose the managed resources of the class
                _streamReader?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
