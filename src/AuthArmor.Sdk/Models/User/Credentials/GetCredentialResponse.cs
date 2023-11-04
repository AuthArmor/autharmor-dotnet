using System;
using System.Text.Json.Serialization;

namespace AuthArmor.Sdk.Models.User.Credentials
{
    public class GetCredentialResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("credential_id")]
        public string CredentialId { get; set; }
        
        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }
        
        [JsonPropertyName("device_type")]
        public string DeviceType { get; set; }
        
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
