namespace ApplicationLayer.Interfaces.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task Rollback();
        IProductRepository ProductRepository { get; }
        ISaleDetailRepositroy SaleDetailRepositroy { get; }
        ISaleRepository SaleRepository { get; }
        IClientRepository CustomerRepository { get; }
    }
}
