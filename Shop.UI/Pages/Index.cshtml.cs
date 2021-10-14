using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Products;
using Shop.Application.ProductsAdmin;
using Shop.Application.ViewModels;
using Shop.Application.ViewModels.AdminModels;
using Shop.Database;
using GetProducts = Shop.Application.ProductsAdmin.GetProducts;

namespace Shop.UI.Pages
{
    public class Index : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public Index(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public IEnumerable<ProductViewModelAdmin> Products { get; set; }

        public void OnGet()
        {
            Products = new GetProducts(_ctx).Do();
        }
    }
}