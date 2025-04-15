namespace ApplicationLayer.Interfaces.Infrastructure
{
    public interface IAuthenticatedUserService
    {
        Task SaveAuthDataAsync(string token, string userId, string email);
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetUserIdAsync();
        Task<string?> GetEmailAsync();
        Task<string?> GetTokenAsync();
        Task ClearAuthDataAsync();

    }
}
