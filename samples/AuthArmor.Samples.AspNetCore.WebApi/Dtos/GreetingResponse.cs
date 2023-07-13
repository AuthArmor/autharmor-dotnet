using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class GreetingResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
