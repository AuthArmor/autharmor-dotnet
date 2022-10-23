namespace AuthArmor.Sdk.Models.Auth.Authenticator
{
    using System;
    using System.Text.Json.Serialization;

    public class StartAuthenticatorAuthResponse
    {
        [JsonPropertyName("auth_validation_token")]
        public string ValidationToken { get; set; }

        [JsonPropertyName("auth_request_id")]
        public Guid AuthRequest_Id { get; set; }

        [JsonPropertyName("user_id")]
        public Guid User_Id { get; set; }

        [JsonPropertyName("visual_verify_value")]
        public string VisualVerifyValue { get; set; }
        
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }
        
        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }

        [JsonPropertyName("qr_code_data")]
        public string QrCodeData { get; set; }

        [JsonPropertyName("push_message_sent")]
        public bool PushMessageSent { get; set; }

        [JsonPropertyName("timeout_in_seconds")]
        public int TimeoutInSeconds { get; set; }

        [JsonPropertyName("timeout_utc_datetime")]
        public DateTime TimeoutUTCDateTime { get; set; }
    }
}
