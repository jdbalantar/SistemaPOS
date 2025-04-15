using ApplicationLayer.DTOs.Product;

namespace ApplicationLayer.Interfaces.Infrastructure
{
    public interface IProductApiService
    {
        Task<List<ProductApiResponseDto>> GetAllAsync();
    }
}
