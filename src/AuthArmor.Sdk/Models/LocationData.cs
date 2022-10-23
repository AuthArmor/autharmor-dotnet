namespace AuthArmor.Sdk.Models
{
    using System.Text.Json.Serialization;

    public class LocationData
    {
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }
    }
}
