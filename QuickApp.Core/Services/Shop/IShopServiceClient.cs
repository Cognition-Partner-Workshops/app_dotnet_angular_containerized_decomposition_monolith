namespace QuickApp.Core.Services.Shop
{
    public interface IShopServiceClient
    {
        Task<bool> HasOrdersByCashierAsync(string cashierId);
    }
}
