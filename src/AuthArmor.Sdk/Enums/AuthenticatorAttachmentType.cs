namespace AuthArmor.Sdk.Enums
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthenticatorAttachmentType
    {
        Any = 1,
        CrossPlatform = 2,
        Platform = 3
    }
}
