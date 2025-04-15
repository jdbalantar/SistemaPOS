using ApplicationLayer.DTOs.Invoice;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SaleRepository(PosDbContext context) : BaseRepository<Sale>(context), ISaleRepository
    {
        public async Task<ICollection<InvoiceDto>> GetInvoices()
        {
            return await _DbSet
                .Include(x => x.Client)
                .Select(s => new InvoiceDto
                {
                    InvoiceId = s.Id,
                    ClientName = s.Client!.Name ?? "N/A",
                    Date = s.Date,
                    Total = s.Total,
                    PaymentMethod = s.PaymentMethod,
                    PointsUsed = s.PointsUsed,
                    PointsEarned = s.PointsEarned
                }).ToListAsync();
        }

        public async Task<Sale?> GetSaleWithDetailsAsync(int id)
        {
            return await _DbSet
                .Include(x => x.Client)
                .Include(x => x.SaleDetails)!
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }

}
