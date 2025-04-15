namespace ApplicationLayer.DTOs.Cart
{
    public class CartItem
    {
        public Domain.Entities.Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
