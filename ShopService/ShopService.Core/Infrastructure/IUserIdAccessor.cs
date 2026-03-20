namespace ShopService.Core.Infrastructure
{
    public interface IUserIdAccessor
    {
        string? GetCurrentUserId();
    }
}
