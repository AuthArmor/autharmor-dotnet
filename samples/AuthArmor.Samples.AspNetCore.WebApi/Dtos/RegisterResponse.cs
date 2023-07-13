using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class RegisterResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
