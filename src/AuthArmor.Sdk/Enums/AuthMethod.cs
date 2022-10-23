namespace AuthArmor.Sdk.Enums
{
    using System.Text.Json.Serialization;
    using System.ComponentModel.DataAnnotations;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthMethod
    {
        [Display(Name = "Auth Armor Authenticator")]
        AuthArmorAuthenticator = 4,
        [Display(Name = "Magiclink Email")]
        Magiclink_Email = 20,
        WebAuthN = 30
    }
}
