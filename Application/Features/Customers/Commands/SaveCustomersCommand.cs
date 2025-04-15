using ApplicationLayer.DTOs;
using ApplicationLayer.Interfaces.Infrastructure;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using MediatR;

namespace ApplicationLayer.Features.Customers.Commands
{
    public class SaveCustomersCommand : IRequest<Result<bool>> { }

    public class SaveCustomersCommandHandler(IUnitOfWork unitOfWork, ICustomerApiService customerApiService) : IRequestHandler<SaveCustomersCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ICustomerApiService customerApiService = customerApiService;

        public async Task<Result<bool>> Handle(SaveCustomersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = unitOfWork.CustomerRepository.GetAllAsync(cancellationToken).Result;

                if (existing.Any())
                {
                    return Result<bool>.Ok("Ya existen clientes en la base de datos.");
                }

                var customers = await customerApiService.GetAllAsync();

                var customersToSave = customers.Select(c => new Client
                {
                    Name = c.Name!,
                    Email = c.Email!,
                }).ToList();

                await unitOfWork.CustomerRepository.AddRangeAsync(customersToSave, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Ok();
            }
            catch (Exception ex)
            {
                return Result<bool>.Error($"No se pudieron guardar los clientes. Error: {ex.Message}");
            }
        }
    }
}
