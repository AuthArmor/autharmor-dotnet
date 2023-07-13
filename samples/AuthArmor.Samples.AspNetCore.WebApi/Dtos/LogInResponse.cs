using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class LogInResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
