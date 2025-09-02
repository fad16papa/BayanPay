using BayanPay.UserService.Api.Interfaces;
using BayanPay.UserService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BayanPay.UserService.Api.Controller;

[ApiController]
[Route("api/[controller]")]
class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

[HttpGet("login")]
    public IActionResult Login(string? redirectUri = null)
    {
        var clientId = _configuration["Clerk:Oidc:ClientId"]!;
        var authority = _configuration["Clerk:Authority"]!.TrimEnd('/');
        var callback = _configuration["Clerk:Oidc:RedirectUri"]!; // e.g. https://api.yourapp.com/auth/callback
        var state = Guid.NewGuid().ToString("N");
        var codeChallenge = "pkce_challenge_here"; // generate and store per-session
        var url =
            $"{authority}/oauth/authorize?response_type=code&client_id={clientId}" +
            $"&redirect_uri={Uri.EscapeDataString(callback)}&scope=openid%20email%20profile" +
            $"&code_challenge={codeChallenge}&code_challenge_method=S256&state={state}";
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        var authority = _configuration["Clerk:Authority"]!.TrimEnd('/');
        var clientId = _configuration["Clerk:Oidc:ClientId"]!;
        var clientSecret = _configuration["Clerk:Oidc:ClientSecret"]!;
        var redirectUri = _configuration["Clerk:Oidc:RedirectUri"]!;
        var codeVerifier = "pkce_verifier_here"; // lookup your stored verifier

        using var http = new HttpClient();
        var form = new FormUrlEncodedContent(new Dictionary<string,string> {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["code_verifier"] = codeVerifier
        });

        var resp = await http.PostAsync($"{authority}/oauth/token", form);
        if (!resp.IsSuccessStatusCode) return Unauthorized("Token exchange failed");

        var tokenJson = await resp.Content.ReadAsStringAsync();
        // Option A: set secure cookie for your own admin UI
        // Option B: return the access_token to the client
        return Content(tokenJson, "application/json");
    }
    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByClerkId(string userId)
    {
        try
        {
            var user = await _userService.GetUserByClerkIdAsync(userId);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] AppUser appUser)
    {
        if (appUser == null)
        {
            return BadRequest("User data is null.");
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(appUser);
            return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] AppUser appUser)
    {
        if (appUser == null)
        {
            return BadRequest("User data is null.");
        }

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(appUser);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
