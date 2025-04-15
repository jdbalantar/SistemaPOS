using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Customer;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;

namespace ApplicationLayer.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<Result<List<CustomerDto>>>
    {
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCustomersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var customers = await unitOfWork.CustomerRepository.GetAllAsync(cancellationToken);
                var customerDtos = customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Username = c.Name,
                    LoyaltyPoints = c.LoyaltyPoints
                }).ToList();

                return Result<List<CustomerDto>>.Ok(customerDtos);
            }
            catch (Exception ex)
            {
                return Result<List<CustomerDto>>.Error($"No se pudieron cargar los clientes. Error: {ex.Message}");
            }
        }
    }
}
