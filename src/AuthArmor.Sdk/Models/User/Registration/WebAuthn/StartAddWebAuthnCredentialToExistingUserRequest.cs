namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System.Text.Json.Serialization;
    
    public class StartAddWebAuthnCredentialToExistingUserRequest
    {
        [JsonPropertyName("timeout_in_seconds")]
        public int? TimeoutInSeconds { get; set; }

        [JsonPropertyName("webauthn_client_id")]
        public string WebAuthnClientGuid { get; set; }

        [JsonPropertyName("attachment_type")]
        public Enums.AuthenticatorAttachmentType AuthenticatorAttachmentType { get; set; }

    }
}
