using AuthArmor.Samples.AspNetCore.WebApi.Dtos;
using AuthArmor.Samples.AspNetCore.WebApi.Services;
using AuthArmor.Sdk.Models.Auth;
using AuthArmor.Sdk.Models.Auth.MessageValidation;
using AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail;
using AuthArmor.Sdk.Services.Auth;
using AuthArmor.Sdk.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace AuthArmor.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationTokenService _tokenService;

    private readonly AuthService _authService;
    private readonly UserService _userService;

    public AuthController(AuthenticationTokenService tokenService, AuthService authService, UserService userService)
    {
        _tokenService = tokenService;
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LogInResponse>> LogIn([FromBody] LogInRequest request)
    {
        ValidateAuthResponseDetails? validationDetails = request.AuthenticationMethod switch
        {
            "authenticator" =>
                (await _authService.ValidateAuthenticatorAuth(new ()
                {
                    AuthRequestId = request.RequestId,
                    AuthValidationToken = request.ValidationToken
                })).ValidateAuthResponse,
            "magicLinkEmail" =>
                (await _authService.ValidateMagiclinkEmailAuth(new ()
                {
                    AuthRequestId = request.RequestId,
                    AuthValidationToken = request.ValidationToken
                })).ValidateAuthResponse,
            "webAuthn" =>
                (await _authService.ValidateWebAuthn(new ()
                {
                    AuthRequestId = request.RequestId,
                    AuthValidationToken = request.ValidationToken
                })).ValidateAuthResponse,
            _ => null
        };

        if (validationDetails is null)
            return BadRequest();

        if (!validationDetails.Authorized)
            return Forbid();

        AuthProfileDetails profileDetails =
            validationDetails.AuthDetails.AuthResponseDetails.AuthProfileDetails;

        string token = _tokenService.GetToken(profileDetails.User_Id.ToString(), profileDetails.Username);

        return new LogInResponse() { Token = token };
    }

    [HttpPost("register")]
    public RegisterResponse Register([FromBody] RegisterRequest request)
    {
        // Ensure in your database that this is a fresh registration.
        // Otherwise, this endpoint can be used by malicious actors to obtain a token on behalf of any user.
        // AuthArmor is working on a validation token for registration to remove this requirement.

        string token = _tokenService.GetToken(request.UserId, request.Username);

        return new RegisterResponse() { Token = token };
    }

    [HttpPost("register-magic-link")]
    public async Task<RegisterResponse> RegisterWithMagicLink(
        [FromBody] RegisterWithMagicLinkRequest request
    )
    {
        ValidateMagiclinkEmailRegistrationResponse validationDetails =
            await _userService.ValidateMagiclinkEmailRegistrationToken(new ()
            {
                MagiclinkEmailRegistrationValidationToken = request.ValidationToken
            });

        string token = _tokenService.GetToken(validationDetails.User_Id.ToString(), validationDetails.Username);

        return new RegisterResponse() { Token = token };
    }
}
