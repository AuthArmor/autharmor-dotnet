namespace AuthArmor.Sdk.Models.User
{    
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class GetUserResponse
    {
        [JsonPropertyName("enrolled_auth_methods")]
        public List<EnrolledAuthMethod> EnrolledAuthMethods { get; set; }

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
