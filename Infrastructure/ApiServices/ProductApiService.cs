using ApplicationLayer.DTOs.Product;
using ApplicationLayer.Interfaces.Infrastructure;
using System.Text.Json;

namespace Infrastructure.ApiServices
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _http;

        public ProductApiService(IHttpClientFactory factory) => _http = factory.CreateClient("ProductsApiClient");

        public async Task<List<ProductApiResponseDto>> GetAllAsync()
        {
            var response = await _http.GetAsync("products");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductApiResponseDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }

}
