using Domain.Interfaces;

namespace Domain.Entities
{
    public class Sale : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int PointsUsed { get; set; }
        public int PointsEarned { get; set; }
        public ICollection<SaleDetail>? SaleDetails { get; set; } = [];
    }
}
