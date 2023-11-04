using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AuthArmor.Sdk.Models.User.Credentials
{
    public class GetCredentialsPagedResponse
    {
        [JsonPropertyName("credential_records")]
        public List<GetCredentialResponse> Credentials { get; set; }

        [JsonPropertyName("page_info")]
        public PagingInfo PagingInfo { get; set; }
    }
}
