namespace AuthArmor.Sdk.Models.User
{    
    using System.Text.Json.Serialization;

    public class EnrolledAuthMethod
    {
        [JsonPropertyName("auth_method_name")]
        public string AuthMethodName { get; set; }

        [JsonPropertyName("auth_method_id")]
        public int AuthMethod_Id { get; set; }

        [JsonPropertyName("auth_method_masked_info")]
        public string AuthMethodMaskedInfo { get; set; }
    }
}
