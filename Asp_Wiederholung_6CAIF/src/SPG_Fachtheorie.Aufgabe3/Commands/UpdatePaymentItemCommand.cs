public class UpdatePaymentItemCommand
{
    public int Id { get; set; }
    public string ArticleName { get; set; } = string.Empty;
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public int PaymentId { get; set; }
    public DateTime? LastUpdated { get; set; }
}
