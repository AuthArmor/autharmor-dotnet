namespace AuthArmor.Sdk.Models.Auth.Authenticator
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class StartAuthenticatorAuthRequest
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
        /// Gets or sets the Nonce for your request - This will be returned during validation and should be verified to prevent abuse
        /// </summary>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets SendPush for your request - If true, a push message will be sent to the user. Note: You must provide a nickname for this feature
        /// </summary>
        [JsonPropertyName("send_push")]
        public bool SendPush { get; set; }

        /// <summary>
        /// Gets or sets UseVisualVerify for your request - If true, this feature will return a value to display on your UI for the user to match in the app.
        /// </summary>
        [JsonPropertyName("use_visual_verify")]
        public bool UseVisualVerify { get; set; }

        /// <summary>
        /// Gets or sets the TimeoutInSeconds -- Optional - If not sent, the project default will be used.
        /// </summary>
        [JsonPropertyName("timeout_in_seconds")]
        [Range(15, 300)]
        public int? TimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the OriginLocationData -- If location data if provided, location and map will display in the app
        /// </summary>
        [JsonPropertyName("origin_location_data")]
        public LocationData OriginLocationData { get; set; }

        /// <summary>
        /// Gets or sets the ActionName - Displayed in the app, Example: Login, Fund Transfer, Account Update, etc It is required
        /// </summary>
        [Required]
        [JsonPropertyName("action_name")]
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  this is the description/short message that shows up in the app and notification. It is required
        /// </summary>
        [Required]
        [JsonPropertyName("short_msg")]
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  this is the description/short message that shows up in the app and notification. It is required
        /// </summary>
        [Required]
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  this is the description/short message that shows up in the app and notification. It is required
        /// </summary>
        [Required]
        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }


    }
}
