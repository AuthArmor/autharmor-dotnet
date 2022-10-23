

namespace AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail
{
    using System.Text.Json.Serialization;

    public class ValidateMagiclinkEmailRegistrationRequest
    {
        [JsonPropertyName("registration_validation_token")]
        public string MagiclinkEmailRegistrationValidationToken { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }
    }
}
