using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Cart;
using ApplicationLayer.Interfaces;
using MediatR;

namespace ApplicationLayer.Features.Cart.Queries
{
    public class GetCartQuery : IRequest<Result<List<CartItem>>>
    {
    }

    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<List<CartItem>>>
    {
        private readonly ICartService _cartService;

        public GetCartQueryHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<Result<List<CartItem>>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cartItems = await _cartService.GetCartAsync();
                return Result<List<CartItem>>.Ok(cartItems);
            }
            catch (Exception ex)
            {
                return Result<List<CartItem>>.Error($"No se pudo obtener el carrito. Error: ${ex.Message}");
                throw;
            }
        }
    }
}
