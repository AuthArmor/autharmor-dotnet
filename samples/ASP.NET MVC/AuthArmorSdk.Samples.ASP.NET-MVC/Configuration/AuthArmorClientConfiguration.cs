namespace AuthArmorSdk.Samples.ASP.NET_MVC.Configuration;

public class AuthArmorClientConfiguration
{
    public string ClientSdkApiKey { get; set; } = "";
    public string WebAuthnClientId { get; set; } = "";

    public string RedirectBaseUrl { get; set; } = "";
}
