namespace MyTestProject.DTO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string CountryCode { get; set; }
        public string FormattedAddress { get; set; }
        public GoogleTimeZone GoogleTimeZone { get; set; }
    }
}
