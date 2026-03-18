using Microsoft.AspNetCore.Mvc;
using ProductCatalog.API.Models;
using ProductCatalog.API.Services;

namespace ProductCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            var created = productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch.");

            var existing = productService.GetProductById(id);
            if (existing == null)
                return NotFound();

            var updated = productService.UpdateProduct(product);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = productService.GetProductById(id);
            if (existing == null)
                return NotFound();

            productService.DeleteProduct(id);
            return NoContent();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ProductCategory>> GetAll()
        {
            return Ok(productService.GetAllCategories());
        }
    }
}
