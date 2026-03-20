namespace ShopService.API.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
    }
}
