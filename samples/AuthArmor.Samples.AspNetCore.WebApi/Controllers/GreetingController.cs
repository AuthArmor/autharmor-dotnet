using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthArmor.Samples.AspNetCore.WebApi.Controllers;

[ApiController]
[Route("greeting")]
[Authorize]
public class GreetingController : ControllerBase
{
    [HttpGet]
    public string GetGreeting()
    {
        string username = User!.Claims.First(c => c.Type == JwtRegisteredClaimNames.NameId).Value;
        
        return $"Hello, {username}!";
    }
}
