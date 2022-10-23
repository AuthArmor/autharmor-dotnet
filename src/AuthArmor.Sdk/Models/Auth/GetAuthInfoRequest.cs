namespace AuthArmor.Sdk.Models.Auth
{
    using System;
    using System.Text.Json.Serialization;

    public class GetAuthInfoRequest
    {
        /// <summary>
        /// Gets or sets the AuthHistory_Id - the Unique auth History ID 
        /// </summary>
        [JsonPropertyName("auth_history_id")]
        public Guid AuthHistory_Id { get; set; }
    }
}
