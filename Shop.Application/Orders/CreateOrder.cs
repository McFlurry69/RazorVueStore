using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Orders
{
    public class CreateOrder
    {
        private readonly ApplicationDbContext _ctx;
        private static readonly Random random = new Random();

        public CreateOrder(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public class Request : CustomerInformation
        {
            public string StripeReference { get; set; }
            public List<Stock> Stocks { get; set; }
            public string SessionId { get; set; }
        }

        public class Stock
        {
            public int StockId { get; set; }
            public int Quantity { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var stocksOnHold = _ctx.StocksOnHold
                .AsEnumerable()
                .Where(x => x.SessionId == request.SessionId)
                .ToList();
            
            _ctx.StocksOnHold.RemoveRange(stocksOnHold);

            var order = new Order()
            {
                StripeReference = request.StripeReference,
                OrderRef = CreateOrderReference(),

                Address1 = request.Address1,
                Address2 = request.Address2,
                Address3 = request.Address3,
                City = request.City,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                PostCode = request.PostCode,

                OrderStocks = request.Stocks.Select(x => new OrderStock()
                {
                    StockId = x.StockId,
                    Quantity = x.Quantity,
                }).ToList()
            };
            _ctx.Orders.Add(order);

            return await _ctx.SaveChangesAsync() > 0;
        }

        private string CreateOrderReference()
        {
            const string chars = "ABCDEFJHIGKLMNOPQRSTYVDXWZabcdefjhigklmnopqrstyvwxwz0123456789";
            var result = new char[12];
            do
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = chars[random.Next(chars.Length)];
            } while (_ctx.Orders.Any(x => x.OrderRef == new string(result)));


            return new string(result);
        }
    }
}