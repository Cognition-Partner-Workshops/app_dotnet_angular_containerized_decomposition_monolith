// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace QuickApp.Server.ViewModels.Shop
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CashierId { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IReadOnlyList<OrderDetailVM> OrderDetails { get; set; } = [];
    }

    public class OrderDetailVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

    public class CreateOrderVM
    {
        public int CustomerId { get; set; }
        public string? CashierId { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
        public IReadOnlyList<CreateOrderDetailVM> OrderDetails { get; set; } = [];
    }

    public class CreateOrderDetailVM
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

    public class UpdateOrderVM
    {
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
    }
}
