using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces;
using MediatR;

namespace ApplicationLayer.Features.Cart.Commands
{
    public record RemoveProductFromCartCommand(int ProductId) : IRequest<Result<bool>>;

    public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand, Result<bool>>
    {
        private readonly ICartService _cartService;

        public RemoveProductFromCartCommandHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<Result<bool>> Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(request.ProductId);
                return Result<bool>.Ok("Producto eliminado del carrito", true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Error("No se pudo eliminar el producto del carrito. Error: " + ex.Message);
            }
        }
    }

}
