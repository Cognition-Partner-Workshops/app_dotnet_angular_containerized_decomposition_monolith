namespace OrderService.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
        public int CustomerId { get; set; }
        public string? CashierId { get; set; }
        public List<OrderDetailVM> OrderDetails { get; set; } = [];
    }
}
