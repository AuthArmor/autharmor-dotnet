namespace AuthArmor.Sdk.Models.User.Registration.Authenticator
{
    using System;
    using System.Text.Json.Serialization;

    public class StartAuthArmorAuthenticatorRegistrationResponse
    {
        [JsonPropertyName("username")]
        public string Nickname { get; set; }

        [JsonPropertyName("registration_id")]
        public Guid RegistrationGuid { get; set; }

        [JsonPropertyName("date_expires")]
        public DateTime DateExpires { get; set; }        

        [JsonPropertyName("auth_method")]
        public string AuthMethod { get; set; }

        [JsonPropertyName("qr_code_data")]
        public string QRCodeData { get; set; }        
    }
}
