namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System.Text.Json.Serialization;
    using System;
    
    public class FinishAddNewWebAuthnCredentialResponse
    {
        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
