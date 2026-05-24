using Backend.Application.Common.Interfaces;
using Backend_Saas.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "AdminTenant")]
public class UsersController : ControllerBase
{
    private readonly IAdminUserService _userService;

    public UsersController(IAdminUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetUsersAsync();
        var response = users.Select(u => new UserResponse(u.Id, u.Email!, u.FirstName, u.LastName, !u.IsDeleted));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName, request.RoleIds);
        return Ok(new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, true));
    }
}
