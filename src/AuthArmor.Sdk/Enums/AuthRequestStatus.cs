namespace AuthArmor.Sdk.Enums
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthRequestStatus
    {
        Unknown = 0,
        Completed = 2,
        PendingApproval = 3,
        PendingValidation = 4
    }
}
