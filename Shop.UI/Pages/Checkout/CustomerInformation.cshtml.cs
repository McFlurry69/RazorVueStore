using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;

namespace Shop.UI.Pages.Checkout
{
    public class CustomerInformation : PageModel
    {
        private readonly IHostEnvironment _environment;

        public CustomerInformation(IHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Domain.Models.CustomerInformation CustomerInformationModel { get; set; }
        
        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
            {
                if (_environment.IsDevelopment())
                {
                    CustomerInformationModel = new Domain.Models.CustomerInformation()
                    {
                        FirstName = "A",
                        Address1 = "A1",
                        Address2 = "A2",
                        Address3 = "A3",
                        City = "C1",
                        Email = "E@d.d",
                        LastName = "B",
                        PhoneNumber = "11",
                        PostCode = "1234124"
                    };
                }
            }

            return information == null ? Page() : RedirectToPage("/Checkout/Payment");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            
            new AddCustomerInformation(HttpContext.Session).Do(CustomerInformationModel);
            return RedirectToPage("/Checkout/Payment");
        }
    }
}