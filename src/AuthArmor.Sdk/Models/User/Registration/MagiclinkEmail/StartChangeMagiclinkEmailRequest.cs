namespace AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class StartChangeMagiclinkEmailRequest
    {
        [JsonPropertyName("new_email_address")]
        public string NewEmailAddress { get; set; }

        [JsonPropertyName("timeout_in_seconds")]
        public int? TimeoutInSeconds { get; set; }

        [JsonPropertyName("registration_redirect_url")]
        public string RegistrationRedirectUrl { get; set; }

        [JsonPropertyName("action_name")]
        public string ActionName { get; set; }

        [JsonPropertyName("short_msg")]
        public string ShortMessage { get; set; }

        [JsonPropertyName("context_data")]
        public Dictionary<string, string> ContextData { get; set; }
    }
}
