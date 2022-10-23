namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System;
    using System.Text.Json.Serialization;

    public class StartWebAuthnRegistrationRequestForNewUserResponse
    {
        [JsonPropertyName("fido2_json_options")]
        public object FIDO2JsonOptions { get; set; }

        [JsonPropertyName("registration_id")]
        public Guid Registration_Id { get; set; }

        [JsonPropertyName("aa_sig")]
        public string aa_sig { get; set; }
    }
}
