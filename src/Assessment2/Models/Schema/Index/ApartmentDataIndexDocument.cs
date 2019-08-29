using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;

namespace Assessment2.Models.Schema.Index
{
    /// <summary>
    /// Represents index schema for apartment data 
    /// </summary>
    public class ApartmentDataIndexDocument
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; set; }

        /// <summary>
        /// Represents a type of apartment data in search index
        /// </summary>
        [IsSortable, IsFilterable]
        public string DataType { get; set; }

        [IsSearchable, IsSortable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string Name { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string FormerName { get; set; }

        [IsFilterable]
        public string State { get; set; }

        [IsFilterable, IsFacetable]
        public string Market { get; set; }

        [IsSearchable]
        public string StreetAddress { get; set; }

        [IsSearchable, IsSortable]
        public string City { get; set; }

        [IsFilterable, IsSortable]
        public GeographyPoint Location { get; set; }


        public override string ToString()
        {         
           return $"{Name} {City} ({State})";
        }
    }
}
