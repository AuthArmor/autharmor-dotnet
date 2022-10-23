namespace AuthArmor.Sdk.Models.Auth.WebAuthn
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class StartWebAuthnAuthRequest
    {
        /// <summary>
        /// Gets or sets the Username of the user to send the auth request to. A username or user_id is required to target the user. Leave empty for usernameless requests
        /// </summary>        
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the User Id of the user to send the auth request to. A username or user_id is required to target the user. Leave empty for usernameless requests
        /// </summary>        
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the TimeoutInSeconds - Optional - If not sent, the project default will be used.
        /// </summary>
        [JsonPropertyName("timeout_in_seconds")]
        [Range(15, 300)]
        public int? TimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the OriginLocationData - Optional - WebAuthn does not display this during auth time, but this is recorded for history records.
        /// </summary>
        [JsonPropertyName("origin_location_data")]
        public LocationData OriginLocationData { get; set; }

        /// <summary>
        /// Gets or sets the ActionName - Required - WebAuthn does not display this during auth time, but this is recorded for history records.
        /// </summary>
        [Required]
        [JsonPropertyName("action_name")]
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  Required - WebAuthn does not display this during auth time, but this is recorded for history records.
        /// </summary>
        [Required]
        [JsonPropertyName("short_msg")]
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the WebAuthnClient_Id -  Required - The WebAuthn Client Id to be used for this WebAuthn request
        /// </summary>
        [Required]
        [JsonPropertyName("webauthn_client_id")]
        public string WebAuthnClient_Id { get; set; }
    }
}
