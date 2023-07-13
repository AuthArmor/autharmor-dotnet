using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class RegisterRequest
{
    [JsonPropertyName("userId")]
    public required string UserId { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }
}
