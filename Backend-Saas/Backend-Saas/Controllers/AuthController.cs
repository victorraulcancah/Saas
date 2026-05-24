using Backend.Application.Common.Interfaces;
using Backend_Saas.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _auth.LoginAsync(request.Email, request.Password);
        return Ok(new LoginResponse(result.Token, result.Email, result.Role, result.TenantId));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _auth.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName, request.TenantId);
        return Ok(new LoginResponse(result.Token, result.Email, result.Role, result.TenantId));
    }
}
