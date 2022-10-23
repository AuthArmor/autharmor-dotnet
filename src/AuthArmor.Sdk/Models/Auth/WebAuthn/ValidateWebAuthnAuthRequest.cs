namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using System.Text.Json.Serialization;

    public class ValidateWebAuthnAuthRequest
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
