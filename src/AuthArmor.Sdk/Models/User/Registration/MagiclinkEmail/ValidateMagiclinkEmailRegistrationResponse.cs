namespace AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ValidateMagiclinkEmailRegistrationResponse
    {
        [JsonPropertyName("magiclink_email_registration_type")]
        public MagiclinkEmailRegistrationType MagiclinkEmailRegistrationType { get; set; }

        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("context_data")]
        public Dictionary<string, string> ContextData { get; set; }
    }
}
