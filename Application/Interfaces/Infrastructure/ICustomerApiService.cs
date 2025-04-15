using ApplicationLayer.DTOs.Customer;

namespace ApplicationLayer.Interfaces.Infrastructure
{
    public interface ICustomerApiService
    {
        Task<List<CustomerDto>> GetAllAsync();
    }
}
