namespace AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail
{
    using System;
    using System.Text.Json.Serialization;

    public class StartMagiclinkEmailRegistrationResponse
    {
        [JsonPropertyName("timeout_in_seconds")]
        public int TimeoutInSeconds { get; set; }

        [JsonPropertyName("timeout_utc_datetime")]
        public DateTime TimeoutUTCDateTime { get; set; }

    }
}
