using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Models;
using ProductCatalogService.Services;

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController(IProductService productService, ILogger<ProductCategoriesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await productService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await productService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryCreateDto dto)
        {
            var category = new ProductCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon
            };

            var created = await productService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryCreateDto dto)
        {
            var category = new ProductCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon
            };

            var updated = await productService.UpdateCategoryAsync(id, category);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await productService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }

    public class ProductCategoryCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
}
