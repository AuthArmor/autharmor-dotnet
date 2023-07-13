using System.Text.Json.Serialization;

namespace AuthArmor.Samples.AspNetCore.WebApi.Dtos;

public class LogInRequest
{
    [JsonPropertyName("requestId")]
    public required string RequestId { get; set; }

    [JsonPropertyName("authenticationMethod")]
    public required string AuthenticationMethod { get; set; }

    [JsonPropertyName("validationToken")]
    public required string ValidationToken { get; set; }    
}

