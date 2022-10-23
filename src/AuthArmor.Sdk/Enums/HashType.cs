namespace AuthArmor.Sdk.Enums
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HashType
    {
        Sha256 = 1
    }
}
