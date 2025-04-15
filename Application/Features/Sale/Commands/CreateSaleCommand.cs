using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;

namespace ApplicationLayer.Features.Sale.Commands
{
    public class CreateSaleCommand(Domain.Entities.Sale sale) : IRequest<Result<int>>
    {
        public Domain.Entities.Sale Sale { get; set; } = sale;
    }

    public class CreateSaleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateSaleCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.SaleRepository.AddAsync(request.Sale, cancellationToken: cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<int>.Ok(request.Sale.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Error($"No se pudo completar la venta. Error: ${ex.Message}");
            }
        }
    }

}
