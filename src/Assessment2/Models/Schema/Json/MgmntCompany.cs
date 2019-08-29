namespace Assessment2.Models.Schema.Json
{
    /// <summary>
    /// Represents the json schema for the apartment management companies
    /// </summary>
    public class MgmntCompany : ApartmentDataJsonDocument
    {
        public MgmntCompany()
        {
            DataType = ApartmentDataType.ManagementCompany;
        }
        public string MgmtID { get; set; }
    }
}
