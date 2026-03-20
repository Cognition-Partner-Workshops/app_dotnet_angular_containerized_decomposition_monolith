namespace ShopService.Core.Models
{
    public class Order : BaseEntity
    {
        public decimal Discount { get; set; }
        public string? Comments { get; set; }

        public string? CashierId { get; set; }

        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; } = [];
    }
}
