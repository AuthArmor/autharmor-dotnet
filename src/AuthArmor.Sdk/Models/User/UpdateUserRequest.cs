namespace AuthArmor.Sdk.Models.User
{
    using System;
    using System.Text.Json.Serialization;

    public class UpdateUserRequest
    {
        [JsonPropertyName("new_username")]
        public string NewUsername { get; set; }
    }
}
