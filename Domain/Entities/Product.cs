using Domain.Interfaces;

namespace Domain.Entities
{
    public class Product : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }

}
