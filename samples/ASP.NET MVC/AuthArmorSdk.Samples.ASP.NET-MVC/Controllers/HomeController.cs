using AuthArmorSdk.Samples.ASP.NET_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthArmorSdk.Samples.ASP.NET_MVC.Controllers
{

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

        //authenticated route        

        [Authorize]
        [Route("LoggedIn")]
        public IActionResult LoggedIn()
        {
            return View();
        }

        [Authorize]
        [Route("Welcome")]
        public IActionResult Welcome()
        {
            return View();
        }

        //anon methods/routes

        //show login view
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        //show logged out view
        [Route("loggedout")]
        public IActionResult LoggedOut()
        {
            return View();
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //logout url is anon - even if session expired already, just logout to prevent asking for login then logout again.
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            //try logout - if the user is not authenticated, just suppress errors and take to logged out page
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("LoggedOut");
        }

        //validate auth armor auth for magiclink or webauthn
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValidateAuthArmorAuth(Models.ValidateAuthArmorAuthRequest request)
        {
            switch (request.AuthMethod.ToLower())
            {
                case "autharmorauthenticator":
                    {
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
                        break;
                    }
                case "webauthn":
                    {
                        var validateRequest = new AuthArmor.Sdk.Models.Auth.WebAuthn.ValidateWebAuthnAuthRequest
                        {
                            AuthRequestId = request.AuthRequest_Id.ToString(),
                            AuthValidationToken = request.AuthValidationToken
                        };

                        var validationResult = await _authService.ValidateWebAuthn(validateRequest);
                        if (validationResult.ValidateAuthResponse.Authorized)
                        {
                            return await CreateSessionAndLogin(validationResult.ValidateAuthResponse.AuthDetails.AuthResponseDetails.AuthProfileDetails.Username);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }

            }

            return RedirectToAction("Error");

        }

        //get page for validating magiclink email auth
        [HttpGet("magiclinkemail-auth")]
        public async Task<ActionResult> ValidateAuthArmorMagiclinkEmailAuth([FromQuery] string auth_validation_token, [FromQuery] string auth_request_id)
        {
            var validateRequest = new AuthArmor.Sdk.Models.Auth.MagiclinkEmail.ValidateMagiclinkEmailAuthRequest
            {
                AuthRequestId = auth_request_id,
                AuthValidationToken = auth_validation_token
            };

            var validationResult = await _authService.ValidateMagiclinkEmailAuth(validateRequest);
            if (validationResult.ValidateAuthResponse.Authorized)
            {
                return await CreateSessionAndLogin(validationResult.ValidateAuthResponse.AuthDetails.AuthResponseDetails.AuthProfileDetails.Username);
            }

            return RedirectToAction("Error");

        }

        //get page for validating magiclink email registration
        [HttpGet("magiclinkemail-register")]
        public async Task<ActionResult> ValidateAuthArmorMagiclinkEmailRegistration([FromQuery] string registration_validation_token)
        {
            var validateRequest = new AuthArmor.Sdk.Models.User.Registration.MagiclinkEmail.ValidateMagiclinkEmailRegistrationRequest
            {
                MagiclinkEmailRegistrationValidationToken = registration_validation_token
            };


            var validationResult = await _userService.ValidateMagiclinkEmailRegistrationToken(validateRequest);

            //create user in database

            //create session and log user in after registration
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, validationResult.Username));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Welcome", "Home");

        }

        //user registration route for webauthn and authenticator
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRegisterSuccess(Models.UserRegistration request)
        {
            //create the user in your database
            //modify the request as needed - check reCaptcha data on registration
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, request.Username));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Welcome", "Home");
        }

        
        

        

        private async Task<ActionResult> CreateSessionAndLogin(string username)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, username));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("LoggedIn", "Home");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}