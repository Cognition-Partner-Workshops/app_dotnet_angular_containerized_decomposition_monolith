namespace ProductService.Core.Services
{
    public interface IUserIdAccessor
    {
        string? GetCurrentUserId();
    }
}
