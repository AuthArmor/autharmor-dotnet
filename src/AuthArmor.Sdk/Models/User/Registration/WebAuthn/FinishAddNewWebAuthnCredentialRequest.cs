namespace AuthArmor.Sdk.Models.User.Registration.WebAuthn
{
    using System.Text.Json.Serialization;

    public class FinishAddNewWebAuthnCredentialRequest
    {
        
        [JsonPropertyName("registration_id")]
        public string RegistrationGuid { get; set; }

        
        [JsonPropertyName("aa_sig")]
        public string AuthArmorSignature { get; set; }

        
        [JsonPropertyName("authenticator_response_data")]
        public FIDO2RegistrationData AuthenticatorResponseData { get; set; }

        [JsonPropertyName("webauthn_client_id")]
        public string WebAuthnClient_Id { get; set; }

        
        public class FIDO2RegistrationData
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("attestation_object")]
            public string AttestationObject { get; set; }

            [JsonPropertyName("client_data")]
            public string ClientData { get; set; }
        }
    }
}
