using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class Cart : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public Cart(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<GetCart.Response> CartList { get; set; }
        
        public IActionResult OnGet()
        {
            CartList = new GetCart(HttpContext.Session, _ctx).Do();
            return Page();
        }
    }
}