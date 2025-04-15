using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces;
using MediatR;

namespace ApplicationLayer.Features.Cart.Commands
{
    public class ClearCartCommand : IRequest<Result<bool>>
    {
    }

    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<bool>>
    {
        private readonly ICartService _cartService;

        public ClearCartCommandHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<Result<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.ClearCartAsync();
                return Result<bool>.Ok("Carrito limpiado con éxito");
            }
            catch (Exception ex)
            {
                return Result<bool>.Error($"No se pudo limpiar el carrito. Error: {ex.Message}");
            }
        }
    }
}
