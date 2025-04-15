using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces.Infrastructure;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using MediatR;

namespace ApplicationLayer.Features.Products.Commands
{
    public class SaveProductsCommand : IRequest<Result<bool>> { }

    public class SaveProductsCommandHandler(IUnitOfWork unitOfWork, IProductApiService productApiService) : IRequestHandler<SaveProductsCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IProductApiService productApiService = productApiService;

        public async Task<Result<bool>> Handle(SaveProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await unitOfWork.ProductRepository.GetAllAsync(cancellationToken);
                if (existing.Any())
                {
                    return Result<bool>.Ok("Ya existen productos en la base de datos.");
                }

                var products = await productApiService.GetAllAsync();
                var productsToSave = products.Select(p => new Product
                {
                    Name = p.Title!,
                    Description = p.Description!,
                    Price = p.Price,
                    Image = p.Image!
                }).ToList();

                await unitOfWork.ProductRepository.AddRangeAsync(productsToSave, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<bool>.Ok();
            }
            catch (Exception ex)
            {
                return Result<bool>.Error("No se pudieron guardar los productos. Error: " + ex.Message);
            }

        }
    }
}
