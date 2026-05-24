using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Backend_Saas.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Saas.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "AdminTenant")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UsersController(UserManager<ApplicationUser> userManager, AppDbContext db, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _db = db;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .Where(u => u.TenantId == _currentUser.TenantId && !u.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var response = users.Select(u => new UserResponse(u.Id, u.Email!, u.FirstName, u.LastName, !u.IsDeleted));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            TenantId = _currentUser.TenantId
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (request.RoleIds?.Any() == true)
        {
            var roles = await _db.Roles
                .Where(r => request.RoleIds.Contains(r.Id) && r.TenantId == _currentUser.TenantId)
                .Select(r => r.Name!)
                .ToListAsync();

            if (roles.Any())
                await _userManager.AddToRolesAsync(user, roles);
        }

        return Ok(new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, true));
    }
}
