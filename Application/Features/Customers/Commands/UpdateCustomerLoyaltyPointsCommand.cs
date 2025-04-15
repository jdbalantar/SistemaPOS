using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;

namespace ApplicationLayer.Features.Customers.Commands
{
    public class UpdateCustomerLoyaltyPointsCommand : IRequest<Result<bool>>
    {
        public int CustomerId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }

        public UpdateCustomerLoyaltyPointsCommand(int customerId, int pointsEarned, int pointsRedeemed)
        {
            CustomerId = customerId;
            PointsEarned = pointsEarned;
            PointsRedeemed = pointsRedeemed;
        }
    }

    public class UpdateCustomerLoyaltyPointsCommandHandler : IRequestHandler<UpdateCustomerLoyaltyPointsCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerLoyaltyPointsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateCustomerLoyaltyPointsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId, cancellationToken: cancellationToken);
                if (customer == null)
                    return Result<bool>.NotFound("Cliente no encontrado");

                customer.LoyaltyPoints += request.PointsEarned;
                customer.LoyaltyPoints -= request.PointsRedeemed;

                if (customer.LoyaltyPoints < 0)
                    customer.LoyaltyPoints = 0;

                await _unitOfWork.CustomerRepository.Update(customer, cancellationToken: cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Error($"No se pudieron actualizar los puntos de lealtad del cliente. Error: {ex.Message}");
            }
        }
    }
}
