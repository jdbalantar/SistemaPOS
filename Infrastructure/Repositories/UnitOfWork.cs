using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(PosDbContext dbContext) : IUnitOfWork
    {
        private readonly PosDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private bool disposed;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        #region Repositories

        private IProductRepository? _productRepository;
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_dbContext);

        private ISaleDetailRepositroy? _saleDetailRepositroy;
        public ISaleDetailRepositroy SaleDetailRepositroy => _saleDetailRepositroy ??= new SaleDetailRepository(_dbContext);

        private ISaleRepository? _saleRepository;
        public ISaleRepository SaleRepository => _saleRepository ??= new SaleRepository(_dbContext);

        private IClientRepository? _clientRepository;
        public IClientRepository CustomerRepository => _clientRepository ??= new ClientRepository(_dbContext);

        #endregion
    }
}
