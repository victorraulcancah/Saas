using Backend.Application.PIM.Models;
using Backend.Application.PIM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/pim/product-contents")]
    [Authorize]
    [RequireSaasAccess("pim", "contenido-producto", "ficha-tecnica")]
    public class ProductContentsController : ControllerBase
    {
        private readonly IPimProductContentService _contents;

        public ProductContentsController(IPimProductContentService contents)
        {
            _contents = contents;
        }

        [HttpGet]
        [RequirePermission("pim.contenido-producto.ficha-tecnica.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contents.GetContentsAsync());
        }

        [HttpPost]
        [RequirePermission("pim.contenido-producto.ficha-tecnica.administrar")]
        public async Task<IActionResult> Create([FromBody] ProductContentRequest request)
        {
            return Ok(await _contents.CreateContentAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("pim.contenido-producto.ficha-tecnica.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductContentRequest request)
        {
            var content = await _contents.UpdateContentAsync(id, request);
            return content is null ? NotFound() : Ok(content);
        }
    }
}
