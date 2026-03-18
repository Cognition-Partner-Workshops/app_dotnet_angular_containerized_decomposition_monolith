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

        // Product details should be resolved via the Product Catalog API.
        // ProductId is retained as a plain integer foreign key (no EF navigation).
        public int ProductId { get; set; }

        public int OrderId { get; set; }
        public required Order Order { get; set; }
    }
}
