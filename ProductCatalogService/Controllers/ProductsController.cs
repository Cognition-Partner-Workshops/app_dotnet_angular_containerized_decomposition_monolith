using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Models;
using ProductCatalogService.Services;

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var category = await productService.GetCategoryByIdAsync(dto.ProductCategoryId);
            if (category == null)
                return BadRequest("Invalid ProductCategoryId.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon,
                BuyingPrice = dto.BuyingPrice,
                SellingPrice = dto.SellingPrice,
                UnitsInStock = dto.UnitsInStock,
                IsActive = dto.IsActive,
                IsDiscontinued = dto.IsDiscontinued,
                ProductCategoryId = dto.ProductCategoryId,
                ProductCategory = category,
                ParentId = dto.ParentId
            };

            var created = await productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto dto)
        {
            var category = await productService.GetCategoryByIdAsync(dto.ProductCategoryId);
            if (category == null)
                return BadRequest("Invalid ProductCategoryId.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon,
                BuyingPrice = dto.BuyingPrice,
                SellingPrice = dto.SellingPrice,
                UnitsInStock = dto.UnitsInStock,
                IsActive = dto.IsActive,
                IsDiscontinued = dto.IsDiscontinued,
                ProductCategoryId = dto.ProductCategoryId,
                ProductCategory = category,
                ParentId = dto.ParentId
            };

            var updated = await productService.UpdateProductAsync(id, product);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }

    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiscontinued { get; set; }
        public int ProductCategoryId { get; set; }
        public int? ParentId { get; set; }
    }
}
