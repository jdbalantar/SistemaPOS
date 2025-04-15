using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces;
using MediatR;

namespace ApplicationLayer.Features.Cart.Commands
{
    public class AddProductToCartCommand : IRequest<Result<bool>>
    {
        public Domain.Entities.Product Product { get; set; }
        public int Quantity { get; set; }

        public AddProductToCartCommand(Domain.Entities.Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    public class AddProductToCartCommandHandler(ICartService cartService) : IRequestHandler<AddProductToCartCommand, Result<bool>>
    {
        private readonly ICartService _cartService = cartService;

        public async Task<Result<bool>> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.AddToCartAsync(request.Product, request.Quantity);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Error($"No se pudo agregar el producto al carrito: Error: {ex.Message}");
            }
        }
    }
}
