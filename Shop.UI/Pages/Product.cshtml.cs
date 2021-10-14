using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.UI.Pages
{
    public class Product : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public Product(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [BindProperty] public AddToCart.Request CartViewModel { get; set; }
        
        public GetProduct.ProductViewModel ProductModel { get; set; }

        public async Task<IActionResult> OnGet(string name)
        {
            ProductModel = await new GetProduct(_ctx).Do(name.Replace("-"," "));
            if (ProductModel == null)
                return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var stockAdded = await new AddToCart(HttpContext.Session, _ctx).Do(CartViewModel);

            if (stockAdded)
                return RedirectToPage("Cart");
            
            //TODO Add warning
            return Page();
        }
    }
}