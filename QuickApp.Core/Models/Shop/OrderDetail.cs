// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace QuickApp.Core.Models.Shop
{
    public class OrderDetail : BaseEntity
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }

        public int ProductId { get; set; }
        // Product navigation property removed — Product now lives in the Product Catalog microservice.
        // Fetch product details from the Product Catalog API if needed.

        public int OrderId { get; set; }
        public required Order Order { get; set; }
    }
}
