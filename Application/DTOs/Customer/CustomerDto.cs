namespace ApplicationLayer.DTOs.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new();
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public CompanyDto Company { get; set; } = new();
        public int LoyaltyPoints { get; set; }

    }
}
