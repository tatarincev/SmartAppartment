namespace Assessment2.Models.Schema.Json
{
    /// <summary>
    /// Represents a base class for json documents that can be used as a data source for an index
    /// </summary>
    public abstract class ApartmentDataJsonDocument
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Market { get; set; }
        public ApartmentDataType DataType { get; set; }
    }
}
