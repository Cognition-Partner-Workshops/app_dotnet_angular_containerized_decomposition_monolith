using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Data;
using ProductCatalog.API.Models;

namespace ProductCatalog.API.Services
{
    public class ProductService(ProductCatalogDbContext dbContext) : IProductService
    {
        public IEnumerable<Product> GetAllProducts()
        {
            return dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Parent)
                .Include(p => p.Children)
                .OrderBy(p => p.Name)
                .ToList();
        }

        public Product? GetProductById(int id)
        {
            return dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Parent)
                .Include(p => p.Children)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            return dbContext.ProductCategories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Product CreateProduct(Product product)
        {
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return product;
        }

        public Product UpdateProduct(Product product)
        {
            dbContext.Products.Update(product);
            dbContext.SaveChanges();
            return product;
        }

        public void DeleteProduct(int id)
        {
            var product = dbContext.Products.Find(id);
            if (product != null)
            {
                dbContext.Products.Remove(product);
                dbContext.SaveChanges();
            }
        }
    }
}
