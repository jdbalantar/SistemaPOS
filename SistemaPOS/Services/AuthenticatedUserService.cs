using ApplicationLayer.Interfaces.Infrastructure;
using System.Text.Json;

namespace SistemaPOS.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private const string AuthStorageKey = "auth_token_data";

        public async Task SaveAuthDataAsync(string token, string userId, string email)
        {
            var data = new AuthData
            {
                Token = token,
                UserId = userId,
                Email = email
            };

            var json = JsonSerializer.Serialize(data);
            await SecureStorage.SetAsync(AuthStorageKey, json);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var json = await SecureStorage.GetAsync(AuthStorageKey);
            return !string.IsNullOrWhiteSpace(json);
        }

        public async Task<string?> GetUserIdAsync()
        {
            var data = await LoadAuthDataAsync();
            return data?.UserId;
        }

        public async Task<string?> GetEmailAsync()
        {
            var data = await LoadAuthDataAsync();
            return data?.Email;
        }

        public async Task<string?> GetTokenAsync()
        {
            var data = await LoadAuthDataAsync();
            return data?.Token;
        }

        public async Task ClearAuthDataAsync()
        {
            SecureStorage.Remove(AuthStorageKey);
            await Task.CompletedTask;
        }

        private async Task<AuthData?> LoadAuthDataAsync()
        {
            var json = await SecureStorage.GetAsync(AuthStorageKey);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonSerializer.Deserialize<AuthData>(json);
        }

        private sealed class AuthData
        {
            public required string Token { get; set; } = string.Empty;
            public required string UserId { get; set; } = string.Empty;
            public required string Email { get; set; } = string.Empty;
        }
    }
}