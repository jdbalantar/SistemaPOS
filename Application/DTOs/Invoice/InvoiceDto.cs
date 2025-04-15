namespace ApplicationLayer.DTOs.Invoice
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int PointsUsed { get; set; }
        public int PointsEarned { get; set; }
    }

}
