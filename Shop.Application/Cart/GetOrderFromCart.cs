using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class GetOrderFromCart
    {
        private readonly ISession _session;
        private readonly ApplicationDbContext _ctx;

        public GetOrderFromCart(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Response
        {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }

            public int GetTotalCharge() => Products.Sum(x => int.Parse(x.Value) * x.Quantity);
        }
        
        public class Product
        {
            public int Quantity { get; set; }
            public string Value { get; set; }
            public int StockId { get; set; }
            public int ProductId { get; set; }
        }

        public Response Do()
        {
            var cart = _session.GetString("cart");

            var cartList = JsonSerializer.Deserialize<List<CartProduct>>(cart);

            var listOfProducts = _ctx.Stocks.Include(x => x.Product).AsEnumerable()
                .Where(y => cartList.Any(z => z.StockId == y.Id))
                .Select(x => new Product()
                {
                    ProductId = x.ProductId,
                    StockId = x.Id,
                    Value = (x.Product.Value*100).ToString(CultureInfo.InvariantCulture),
                    Quantity = cartList.FirstOrDefault(z => z.StockId == x.Id).Quantity
                }).ToList();

            var customerInfoString = _session.GetString("customer-info");

            var customerInformation = JsonSerializer.Deserialize<CustomerInformation>(customerInfoString);

            return new Response()
            {
                Products = listOfProducts,
                CustomerInformation = customerInformation
            };
        }
    }
}