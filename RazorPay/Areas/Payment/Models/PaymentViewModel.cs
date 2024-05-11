namespace RazorPay.Areas.Payment.Models
{
    public class PaymentViewModel
    {
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public decimal Amount { get; set; }
    }
}
