namespace Assessment2.Models.Schema.Json
{
    /// <summary>
    /// Represents the json schema for the real property objects
    /// </summary>
    public class RealProperty : ApartmentDataJsonDocument
    {
        public RealProperty()
        {
            DataType = ApartmentDataType.Property;
        }

        public string PropertyID { get; set; }
        public string FormerName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }
    

        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

    }
}
