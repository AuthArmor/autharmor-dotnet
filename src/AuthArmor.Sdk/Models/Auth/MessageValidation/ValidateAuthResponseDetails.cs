namespace AuthArmor.Sdk.Models.Auth.MessageValidation
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    public class ValidateAuthResponseDetails
    {
        /// <summary>
        /// Gets or sets the AuthHistory_Id - the Unique auth History ID 
        /// </summary>
        [JsonPropertyName("auth_history_id")]
        public Guid AuthHistory_Id { get; set; }

        /// <summary>
        /// Gets or sets the ResultCode for the request
        /// </summary>
        [JsonPropertyName("result_code")]
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the ResultMessage for the request
        /// </summary>
        [JsonPropertyName("result_message")]
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the Authorized Flag
        /// </summary>
        [JsonPropertyName("authorized")]
        public bool Authorized { get; set; }

        /// <summary>
        /// Gets or sets the AuthDetails
        /// </summary>
        [JsonPropertyName("auth_details")]
        public AuthDetails AuthDetails { get; set; }

        /// <summary>
        /// Gets or sets the Context Data
        /// </summary>
        [JsonPropertyName("context_data")]
        public Dictionary<string, string> ContextData { get; set; }

    }
}
