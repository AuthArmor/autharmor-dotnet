namespace AuthArmor.Sdk.Enums
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthMethodUseType
    {
        fido2 = 30,
        fido2_mobiledevice = 35,
        fido2_crossplatform = 40,
        magiclink_email = 50,
    }
}
