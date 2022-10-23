using AuthArmor.Sdk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AuthArmor.Sdk.Models.User.AuthHistory
{
    public class GetAuthHistoryPagedResponse
    {
        [JsonPropertyName("auth_history_records")]
        public List<GetAuthInfoResponse> UserAuthHistory { get; set; }

        [JsonPropertyName("page_info")]
        public PagingInfo PagingInfo { get; set; }
    }
}
