using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SaleDetailRepository(PosDbContext context) : BaseRepository<SaleDetail>(context), ISaleDetailRepositroy
    {
    }
}
