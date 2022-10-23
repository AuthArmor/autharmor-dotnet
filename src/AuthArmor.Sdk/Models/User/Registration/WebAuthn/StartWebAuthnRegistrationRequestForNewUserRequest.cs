namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System.Text.Json.Serialization;

    public class StartWebAuthnRegistrationRequestForNewUserRequest
    {

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("timeout_in_seconds")]
        public int? TimeoutInSeconds { get; set; }

        [JsonPropertyName("webauthn_client_id")]
        public string WebAuthnClient_Id { get; set; }

        [JsonPropertyName("attachment_type")]
        public Enums.AuthenticatorAttachmentType AuthenticatorAttachmentType { get; set; }
    }
}
