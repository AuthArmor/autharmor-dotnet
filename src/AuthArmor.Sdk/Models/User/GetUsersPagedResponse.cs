namespace AuthArmor.Sdk.Models.User
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class GetUsersPagedResponse
    {
        [JsonPropertyName("user_records")]
        public List<User> UserRecords { get; set; }

        [JsonPropertyName("page_info")]
        public PagingInfo PagingInfo { get; set; }
    }
}
