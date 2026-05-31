using Backend.Application.PIM.Models;
using Backend.Application.PIM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.PIM
{
    [ApiController]
    [Route("api/pim/product-media")]
    [Authorize]
    [RequireSaasAccess("pim", "medios")]
    public class ProductMediaController : ControllerBase
    {
        private readonly IPimMediaService _media;

        public ProductMediaController(IPimMediaService media)
        {
            _media = media;
        }

        [HttpGet("product/{productId:guid}")]
        [RequirePermission("pim.medios.ver")]
        public async Task<IActionResult> GetByProduct(Guid productId)
        {
            return Ok(await _media.GetProductMediaAsync(productId));
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("pim.medios.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var media = await _media.GetMediaByIdAsync(id);
            return media is null ? NotFound() : Ok(media);
        }

        [HttpPost]
        [RequirePermission("pim.medios.administrar")]
        public async Task<IActionResult> Create([FromBody] ProductMediaRequest request)
        {
            return Ok(await _media.CreateProductMediaAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("pim.medios.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductMediaRequest request)
        {
            var media = await _media.UpdateProductMediaAsync(id, request);
            return media is null ? NotFound() : Ok(media);
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("pim.medios.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _media.DeleteProductMediaAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
