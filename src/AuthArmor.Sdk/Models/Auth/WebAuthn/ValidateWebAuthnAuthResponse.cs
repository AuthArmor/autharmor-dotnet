namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using AuthArmor.Sdk.Models.Auth.MessageValidation;
    using System.Text.Json.Serialization;

    public class ValidateWebAuthnAuthResponse
    {
        /// <summary>
        /// Gets or sets the ValidateAuthResponse Information
        /// </summary>
        [JsonPropertyName("validate_auth_response_details")]
        public ValidateAuthResponseDetails ValidateAuthResponse { get; set; }

        /// <summary>
        /// Gets or sets the Auth Request Status Id
        /// </summary>
        [JsonPropertyName("auth_request_status_id")]
        public int AuthRequestStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Auth Request Status Name
        /// </summary>
        [JsonPropertyName("auth_request_status_name")]
        public string AuthRequestStatusName { get; set; }
    }
}
