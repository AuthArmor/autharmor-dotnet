# Project Name

AuthArmor.Sdk

## Installation

`PM> NuGet\Install-Package AuthArmor.Sdk`

## Usage

You will need an API key to use the SDK. Please visit https://dashboard.autharmor.com and select your project, then create an API key.

Next, you need to set the API key info in your appSettings.json file:

```json
"AuthArmorConfig": {
    "ClientId": "your_client_id_goes_here",
    "ClientSecret": "your_client_secret_goes_here"
  },
```
Next, setup Auth Armor Services in your startup.cs or program.cs:

```csharp
builder.Services.Configure<AuthArmor.Sdk.Infrastructure.AuthArmorConfiguration>(builder.Configuration.GetSection("AuthArmorConfig"));

builder.Services.AddAuthArmorSdkServices();
```
Now you can use Auth Armor with DI.

```csharp
public class HomeController : Controller
{
    //logger
    private readonly ILogger<HomeController> _logger;
    //auth armor auth service
    private readonly AuthArmor.Sdk.Services.Auth.AuthService _authService;
    //auth armor user service
    private readonly AuthArmor.Sdk.Services.User.UserService _userService;

    public HomeController(ILogger<HomeController> logger,
                          AuthArmor.Sdk.Services.Auth.AuthService authService,
                          AuthArmor.Sdk.Services.User.UserService userService)

    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }
}
```

Validate Auth token for the Auth Armor Authenticator:

```csharp
var validateRequest = new AuthArmor.Sdk.Models.Auth.Authenticator.ValidateAuthenticatorAuthRequest
{
    AuthRequestId = request.AuthRequest_Id.ToString(),
    AuthValidationToken = request.AuthValidationToken
};

var validationResult = await _authService.ValidateAuthenticatorAuth(validateRequest);
if (validationResult.ValidateAuthResponse.Authorized)
{
    return await CreateSessionAndLogin(validationResult.ValidateAuthResponse.AuthDetails.AuthResponseDetails.AuthProfileDetails.Username);
}

```

## License

MIT