using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assessment2.Models
{
    /// <summary>
    /// Represents a search result item for an apartment data search request
    /// </summary>
    public class ApartmentDataSearchEntry
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ApartmentDataType DataType { get; set; }

        public string Name { get; set; }
        public string Market { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        /// <summary>
        /// Computed relevance rank 
        /// </summary>
        public double Score { get; set; }
    }
}
