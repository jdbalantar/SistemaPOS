using ApplicationLayer.DTOs.Cart;
using ApplicationLayer.Interfaces;
using Domain.Entities;

namespace ApplicationLayer.Services
{
    public class InMemoryCartService : ICartService
    {
        private readonly List<CartItem> _cart = [];

        public Task<List<CartItem>> GetCartAsync()
        {
            return Task.FromResult(_cart.ToList());
        }

        public Task AddToCartAsync(Product product, int quantity = 1)
        {
            var existing = _cart.FirstOrDefault(c => c.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }

            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(int productId)
        {
            var item = _cart.FirstOrDefault(c => c.Product.Id == productId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    _cart.Remove(item);
                }
            }

            return Task.CompletedTask;
        }


        public Task ClearCartAsync()
        {
            _cart.Clear();
            return Task.CompletedTask;
        }
    }
}
