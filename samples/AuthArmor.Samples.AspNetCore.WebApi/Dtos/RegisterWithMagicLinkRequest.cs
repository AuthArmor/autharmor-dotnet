using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class RegisterWithMagicLinkRequest
{
    [JsonPropertyName("validationToken")]
    public required string ValidationToken { get; set; }
}
