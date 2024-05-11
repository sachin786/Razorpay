using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace RazorPay.Areas.Payment.Pages
{
    public class ResponseModel : PageModel
    {
        private const string _secret = "WgfLz9cgWvKD1F4mu4WCcwt0";
        public IActionResult OnGet(string paymentStatus, string orderId, string paymentId, string signature)
        {
            if (paymentStatus == "Fail")
                return RedirectToPage("Fail");
            var validSignature = CompareSignatures(orderId, paymentId, signature);
            if (validSignature)
            {
                return RedirectToPage("Success");
            }
            else
            {
                return RedirectToPage("Fail");
            }
        }

        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
            var text = orderId + "|" + paymentId;
            var secret = _secret;
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }

        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(secret);
            HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }

    }
}
