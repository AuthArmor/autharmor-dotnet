namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using System;
    using System.Text.Json.Serialization;

    public class StartWebAuthnAuthResponse
    {
        [JsonPropertyName("fido2_json_options")]
        public string FIDO2JsonOptions { get; set; }

        [JsonPropertyName("auth_request_id")]
        public Guid AuthRequest_Id { get; set; }

        [JsonPropertyName("aa_guid")]
        public string AuthArmorSignature { get; set; }
    }
}
