using Domain.Interfaces;

namespace Domain.Entities
{
    public class Client : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
        public ICollection<Sale> Sales { get; set; } = [];
    }

}
