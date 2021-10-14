using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shop.Application.Cart;
using Stripe;

namespace Shop.UI.Pages.Checkout
{
    public class Payment : PageModel
    {
        public Payment(IConfiguration configuration)
        {
            PublicKey = configuration["Stripe:PublicKey"].ToString();
        }

        public string PublicKey { get; }
        
        public IActionResult OnGet() => new GetCustomerInformation(HttpContext.Session).Do() == null ? RedirectToPage("/Checkout/CustomerInformation") : Page();
    }
} 