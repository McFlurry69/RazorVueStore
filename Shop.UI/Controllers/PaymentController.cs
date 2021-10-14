using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;
using Stripe;

namespace Shop.UI.Controllers
{
    [Route("create-payment-intent")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;

        public PaymentController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(PaymentIntentCreateRequest request)
        {
            var cartOrder = new GetOrderFromCart(HttpContext.Session, _ctx).Do();
            
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(),
                Currency = "usd"
            });

            var sessionId = HttpContext.Session.Id;
            
            await new CreateOrder(_ctx).Do(new CreateOrder.Request()
            {
                StripeReference = paymentIntent.Id,
                
                SessionId = sessionId,
                
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                Address3 = cartOrder.CustomerInformation.Address3,
                City = cartOrder.CustomerInformation.City,
                Email = cartOrder.CustomerInformation.Email,
                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                PostCode = cartOrder.CustomerInformation.PostCode,
                
                Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock
                {
                    StockId = x.StockId,
                    Quantity = x.Quantity
                }).ToList()
            });
            
            return new JsonResult(new { clientSecret = paymentIntent.ClientSecret });
        }
        
        
        
        private int CalculateOrderAmount() => new GetOrderFromCart(HttpContext.Session, _ctx).Do().GetTotalCharge();
        
        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }
    }
}