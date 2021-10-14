using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class GetCart
    {
        private readonly ISession _session;
        private readonly ApplicationDbContext _ctx;

        public GetCart(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Response
        {
            public string Name { get; set; }
            public string Value { get; set; }
            
            public int StockId { get; set; }
            public int Quantity { get; set; }
        }
        
        public IEnumerable<Response> Do()
        {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
            {
                return new List<Response>();
            }

            var cartList = JsonSerializer.Deserialize<List<CartProduct>>(stringObject);
            
            var response = _ctx.Stocks.Include(x => x.Product).AsEnumerable().
                Where(x => cartList.Any(y => y.StockId == x.Id)).
                Select(x =>
                new Response()
                {
                    Name = x.Product.Name,
                    Value = x.Product.Value.ToString(CultureInfo.InvariantCulture),
                    StockId = x.Id,
                    Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id).Quantity
                }).ToList();
            
            _session.SetString("cart", stringObject);

            return response;
        }
    }
}