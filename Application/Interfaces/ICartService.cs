using ApplicationLayer.DTOs.Cart;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartAsync();
        Task AddToCartAsync(Product product, int quantity = 1);
        Task RemoveFromCartAsync(int productId);
        Task ClearCartAsync();
    }
}
