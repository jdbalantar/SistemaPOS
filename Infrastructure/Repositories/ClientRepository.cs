using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class ClientRepository(PosDbContext context) : BaseRepository<Client>(context), IClientRepository
    {
    }
}
