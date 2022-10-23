namespace AuthArmor.Sdk.Models.Auth.MessageValidation
{
    using System.Text.Json.Serialization;

    public class WebAuthnValidationDetails
    {
        [JsonPropertyName("fido2_options_json")]
        public string Fido2OptionsJson { get; set; }

        [JsonPropertyName("expected_origin")]
        public string ExpectedOrigin { get; set; }

        [JsonPropertyName("aa_guid")]
        public string AaGuid { get; set; }

        [JsonPropertyName("public_key")]
        public string PublicKeyBase64 { get; set; }

        [JsonPropertyName("rpid")]
        public string RpId { get; set; }

        [JsonPropertyName("authenticator_enrollment_data")]
        public AuthenticatorEnrollmentData AuthenticatorEnrollmentData { get; set; }
    }

    public class AuthenticatorEnrollmentData
    {
        [JsonPropertyName("client_data_json")]
        public string PublicKeyBase64 { get; set; }

        [JsonPropertyName("attestation_data")]
        public string AttestationCertBase64 { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }
    }

    public class Fido2AuthenticatorResponse
    {
        public string id { get; set; }
        public string attestation_object { get; set; }
        public string client_data { get; set; }
        public string signature { get; set; }
        public string client_extension_data { get; set; }
    }
}
