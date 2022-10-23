namespace AuthArmor.Sdk.Models.Auth.MagiclinkEmail
{
    using System.Text.Json.Serialization;

    public class ValidateMagiclinkEmailAuthRequest
    {
        [JsonPropertyName("auth_validation_token")]
        public string AuthValidationToken { get; set; }

        [JsonPropertyName("auth_request_id")]
        public string AuthRequestId { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }
    }
}
