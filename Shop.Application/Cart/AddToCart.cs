using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private readonly ISession _session;
        private readonly ApplicationDbContext _ctx;

        public AddToCart(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Quantity { get; set; }
        }
        
        public async Task<bool> Do(Request request)
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _session.Id).ToList();
            var stockToHold = _ctx.Stocks.FirstOrDefault(x => x.Id == request.StockId);

            if (stockToHold != null && stockToHold.Quantity < request.Quantity)
                return false;

            foreach (var stockOnHold in stocksOnHold)
            {
                stockOnHold.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            if (stockToHold != null)
            {
                _ctx.StocksOnHold.Add(new StockOnHold()
                {
                    StockId = stockToHold.Id,
                    SessionId = _session.Id,
                    Quantity = request.Quantity,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });

                stockToHold.Quantity = stockToHold.Quantity - request.Quantity;
            }

            await _ctx.SaveChangesAsync();


            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonSerializer.Deserialize<List<CartProduct>>(stringObject);
            }

            if (cartList!.Any(x => x.StockId == request.StockId))
            {
                cartList.Find(x => x.StockId == request.StockId).Quantity += request.Quantity;
            }
            else
            {
                cartList.Add(new CartProduct()
                {
                    StockId = request.StockId,
                    Quantity = request.Quantity
                });
            }
            
            stringObject = JsonSerializer.Serialize(cartList);
            
            _session.SetString("cart", stringObject);

            return true;
        }
    }
}