using ApplicationLayer.DTOs.Customer;
using ApplicationLayer.Interfaces.Infrastructure;
using System.Text.Json;

namespace Infrastructure.ApiServices
{
    public class CustomerApiService : ICustomerApiService
    {
        private readonly HttpClient _http;

        public CustomerApiService(IHttpClientFactory factory) => _http = factory.CreateClient("CustomersApiClient");

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var response = await _http.GetAsync("users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CustomerDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
