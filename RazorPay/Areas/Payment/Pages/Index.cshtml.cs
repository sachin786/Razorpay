using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPay.Areas.Payment.Models;
using Razorpay.Api;

namespace RazorPay.Areas.Payment.Pages
{
    public class IndexModel : PageModel
    {
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public decimal Amount { get; set; }
        private const string _key = "rzp_test_U8wEbylePqOUAD";
        private const string _secret = "WgfLz9cgWvKD1F4mu4WCcwt0";
        public void OnGet()
        {
            Amount = 1;
        }
        public JsonResult OnPostPaymentInitialization([FromBody] PaymentViewModel model)
        {
            OrderViewModel order = new()
            {
                OrderAmount = model.Amount,
                Currency = "INR",
                Payment_Capture = 1,    // 0 - Manual capture, 1 - Auto capture
                Notes = new Dictionary<string, string>()
                {
                    { "Note One", "Hi Kapil" }, { "Note Two", "Hi Sachin" }
                }
            };
            var orderId = CreateOrder(order);

            OptionsViewModel razorPayOptions = new()
            {
                Key = _key,
                AmountInSubUnits = order.OrderAmountInSubUnits,
                Currency = order.Currency,
                Name = "Sachin Sharma",
                Description = "This is test payment",
                ImageLogUrl = "",
                OrderId = orderId,
                ProfileName = model.Name,
                ProfileContact = model.Mobile,
                ProfileEmail = model.Email,
                Notes = new Dictionary<string, string>()
                {
                    { "Note One", "Hi Kapil" }, { "Note Two", "Hi Sachin" }
                }
            };
            return new JsonResult(razorPayOptions);
        }
        private string CreateOrder(OrderViewModel order)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", order.OrderAmountInSubUnits },
                    { "currency", order.Currency! },
                    { "payment_capture", order.Payment_Capture },
                    { "notes", order.Notes! }
                };

                Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
