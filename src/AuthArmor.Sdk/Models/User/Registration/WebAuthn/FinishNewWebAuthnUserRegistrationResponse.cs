namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System;
    using System.Text.Json.Serialization;

    public class FinishNewWebAuthnUserRegistrationResponse
    {
        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
