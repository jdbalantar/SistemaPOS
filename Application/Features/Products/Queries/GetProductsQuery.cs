using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Product;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;

namespace ApplicationLayer.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<Result<List<ProductApiResponseDto>>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductApiResponseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        public async Task<Result<List<ProductApiResponseDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await unitOfWork.ProductRepository.GetAllAsync(cancellationToken);
                var productDtos = products.Select(p => new ProductApiResponseDto
                {
                    Id = p.Id,
                    Title = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.Image
                }).ToList();

                return Result<List<ProductApiResponseDto>>.Ok(productDtos);
            }
            catch (Exception ex)
            {
                return Result<List<ProductApiResponseDto>>.Error($"No se pudieron cargar los productos. Error: {ex.Message}");
            }
        }
    }

}
