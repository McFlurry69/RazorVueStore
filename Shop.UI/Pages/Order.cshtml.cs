using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Orders;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class  Order : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public Order(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public GetOrder.Responce OrderModel { get; set; }
        
        public void OnGet(string reference)
        {
            OrderModel = new GetOrder(_ctx).Do(reference);
        }
    }
}