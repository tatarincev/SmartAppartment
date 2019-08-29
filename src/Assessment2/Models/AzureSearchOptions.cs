namespace Assessment2.Models
{
    /// <summary>
    /// Options for configuring indexed search using Azure Search.
    /// </summary>
    public class AzureSearchOptions
    {
        public string IndexName { get; set; } = "smart-search-index";
        public string SearchServiceName { get; set; }
        public string SearchServiceAdminApiKey { get; set; }
        public string SearchServiceQueryApiKey { get; set; }
    }
}
