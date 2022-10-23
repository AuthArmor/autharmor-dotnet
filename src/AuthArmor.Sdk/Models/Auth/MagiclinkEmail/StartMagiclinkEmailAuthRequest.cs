namespace AuthArmor.Sdk.Models.Auth.MagiclinkEmail
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    public class StartMagiclinkEmailAuthRequest
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
        /// Gets or sets the OriginLocationData - Optional - If location data if provided, location and map will display in the email
        /// </summary>
        [JsonPropertyName("origin_location_data")]
        public LocationData OriginLocationData { get; set; }

        /// <summary>
        /// Gets or sets the ActionName - Required - Displayed in the app, Example: Login, Fund Transfer, Account Update, etc.
        /// </summary>
        [Required]
        [JsonPropertyName("action_name")]
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  Required - This is the description/short message that shows up in the email
        /// </summary>
        [Required]
        [JsonPropertyName("short_msg")]
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the ShortMessage -  Required - The redirect URL to use for this auth request
        /// </summary>
        [Required]
        [JsonPropertyName("auth_redirect_url")]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the Context Data -  Optional - Supply context data during start and this same data will be returned at validation time
        /// </summary>
        [JsonPropertyName("context_data")]
        public Dictionary<string, string> ContextData { get; set; }

        /// <summary>
        /// Gets or sets the Ip Address -  Optional - Supply an ip address value at start time, then provide it again during validation for extra security
        /// </summary>
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the User Agent -  Optional - Supply a user agent value at start time, then provide it again during validation for extra security
        /// </summary>
        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }
    }
}
