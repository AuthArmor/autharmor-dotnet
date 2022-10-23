namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using System.Text.Json.Serialization;
    public class FinishWebAuthnAuthRequest
    {

        [JsonPropertyName("authenticator_response_data")]
        public string AuthenticatorResponseData { get; set; }

        [JsonPropertyName("aa_sig")]
        public string AuthArmorSignature { get; set; }

        [JsonPropertyName("auth_request_id")]
        public string AuthRequest_Id { get; set; }

        [JsonPropertyName("webauthn_client_id")]
        public string WebAuthnClient_Id { get; set; }
    }
}
