namespace AuthArmor.Sdk.Models.Auth.MagiclinkEmail
{
    using System;
    using System.Text.Json.Serialization;

    public class StartMagiclinkEmailAuthResponse
    {
        [JsonPropertyName("auth_request_id")]
        public Guid AuthRequest_Id { get; set; }

        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("timeout_in_seconds")]
        public int TimeoutInSeconds { get; set; }

        [JsonPropertyName("timeout_utc_datetime")]
        public DateTime TimeoutUTCDateTime { get; set; }
    }
}
