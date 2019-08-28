namespace Assessment2.Models
{
    public class RealProperty : ApartmentData
    {
        public string PropertyID { get; set; }
        public string FormerName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }
    

        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

    }
}
