using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class ProductRepository(PosDbContext context) : BaseRepository<Product>(context), IProductRepository
    {

    }
}
