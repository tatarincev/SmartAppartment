using Microsoft.Azure.Search;
using Microsoft.Spatial;

namespace Assessment2.Models.Index
{
    public class IndexDocument
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; set; }

        /// <summary>
        /// Represents a type of document in search index, supported values: company || property
        /// </summary>
        [IsSortable, IsFilterable]
        public string DocType { get; set; }

        [IsSearchable, IsSortable]
        public string Name { get; set; }

        [IsSearchable]
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

    }
}
