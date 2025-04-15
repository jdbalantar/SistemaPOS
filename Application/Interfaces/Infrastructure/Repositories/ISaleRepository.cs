using ApplicationLayer.DTOs.Invoice;
using Domain.Entities;

namespace ApplicationLayer.Interfaces.Infrastructure.Repositories
{
    public interface ISaleRepository : IBaseRepository<Sale>
    {
        Task<ICollection<InvoiceDto>> GetInvoices();
        Task<Sale?> GetSaleWithDetailsAsync(int id);
    }
}
