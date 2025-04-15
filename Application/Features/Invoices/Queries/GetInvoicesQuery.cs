using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Invoice;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;

namespace ApplicationLayer.Features.Invoices.Queries
{
    public record GetInvoicesQuery() : IRequest<Result<ICollection<InvoiceDto>>>;
    public class GetInvoicesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetInvoicesQuery, Result<ICollection<InvoiceDto>>>
    {
        public async Task<Result<ICollection<InvoiceDto>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken) =>
            Result<ICollection<InvoiceDto>>.Ok(await unitOfWork.SaleRepository.GetInvoices());
    }

}
