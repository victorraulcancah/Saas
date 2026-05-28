using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "AdminTenant")]
    public class RolesController : ControllerBase
    {
        private readonly IAdminRoleService _roleService;

        public RolesController(IAdminRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var response = roles.Select(r => new DTOs.Role.RoleResponse(r.Id, r.Name!, r.Description, r.CreatedAt));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DTOs.Role.CreateRoleRequest request)
        {
            var role = await _roleService.CreateRoleAsync(request.Name, request.Description, request.PermissionIds);
            return Ok(new DTOs.Role.RoleResponse(role.Id, role.Name!, role.Description, role.CreatedAt));
        }
    }
}