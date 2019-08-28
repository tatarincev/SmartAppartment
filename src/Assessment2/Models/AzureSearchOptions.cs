namespace Assessment2.Models
{
    public class AzureSearchOptions
    {
        public string IndexName { get; set; } = "smart-search-index";
        public string SearchServiceName { get; set; }
        public string SearchServiceAdminApiKey { get; set; }
        public string SearchServiceQueryApiKey { get; set; }
    }
}
