namespace AuthArmor.Samples.AspNetCore.WebApi.Configuration;

public class AuthenticationTokenConfiguration
{
    public const string ConfigurationKey = "AuthenticationToken";

    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";

    public string IssuerSigningKey { get; set; } = "";

    public int ValidityPeriod { get; set; } = 0;
}
