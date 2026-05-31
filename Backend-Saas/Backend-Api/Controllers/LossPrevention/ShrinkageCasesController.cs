using Backend.Application.LossPrevention.Models;
using Backend.Application.LossPrevention.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.LossPrevention
{
    [ApiController]
    [Route("api/loss-prevention/shrinkage-cases")]
    [Authorize]
    [RequireSaasAccess("loss-prevention", "shrinkage")]
    public class ShrinkageCasesController : ControllerBase
    {
        private readonly ILossShrinkageService _shrinkage;

        public ShrinkageCasesController(ILossShrinkageService shrinkage)
        {
            _shrinkage = shrinkage;
        }

        [HttpGet]
        [RequirePermission("loss-prevention.shrinkage.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _shrinkage.GetShrinkageCasesAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("loss-prevention.shrinkage.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shrinkageCase = await _shrinkage.GetShrinkageCaseByIdAsync(id);
            return shrinkageCase is null ? NotFound() : Ok(shrinkageCase);
        }

        [HttpPost]
        [RequirePermission("loss-prevention.shrinkage.administrar")]
        public async Task<IActionResult> Create([FromBody] ShrinkageCaseRequest request)
        {
            return Ok(await _shrinkage.CreateShrinkageCaseAsync(request));
        }

        [HttpPost("{id:guid}/resolve")]
        [RequirePermission("loss-prevention.shrinkage.administrar")]
        public async Task<IActionResult> Resolve(Guid id, [FromBody] string resolution)
        {
            var shrinkageCase = await _shrinkage.ResolveShrinkageCaseAsync(id, resolution);
            return shrinkageCase is null ? NotFound() : Ok(shrinkageCase);
        }
    }
}
