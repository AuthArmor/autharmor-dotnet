namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using System;
    using System.Text.Json.Serialization;

    public class FinishWebAuthnAuthResponse
    {
        [JsonPropertyName("auth_validation_token")]
        public string ValidationToken { get; set; }

        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("auth_request_id")]
        public Guid AuthRequest_Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
