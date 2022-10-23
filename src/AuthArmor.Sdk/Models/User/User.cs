namespace AuthArmor.Sdk.Models.User
{
    using System;
    using System.Text.Json.Serialization;

    public class User
    {        
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }
    }
}
