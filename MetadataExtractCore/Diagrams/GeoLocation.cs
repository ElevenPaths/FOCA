namespace MetadataExtractCore.Diagrams
{
    public class GeoLocation : MetadataValue
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Altitude { get; set; }

        public GeoLocation(string dmsLocation, double longitude, double latitude) : base(dmsLocation)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}
