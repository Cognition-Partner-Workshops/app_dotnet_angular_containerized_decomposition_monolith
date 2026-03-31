using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Core.Models;
using ProductService.Core.Services;
using ProductService.Server.ViewModels;

namespace ProductService.Server.Controllers
{
    [Authorize]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IMapper mapper, IProductService productService)
            : base(logger, mapper)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(_mapper.Map<IEnumerable<ProductVM>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductVM>(product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductVM productVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productVm);
            var created = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, _mapper.Map<ProductVM>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductVM productVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _productService.GetProductByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(productVm, existing);
            await _productService.UpdateProductAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existing = await _productService.GetProductByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
