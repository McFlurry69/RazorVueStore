using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin
{
    public class UpdateStock
    {
        private readonly ApplicationDbContext _ctx;

        public UpdateStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Responce> Do(Request request)
        {
            var stocks = new List<Stock>();

            foreach (var stock in request.Stocks)
            {
                stocks.Add(new Stock
                {
                    Id = stock.Id,
                    Description = stock.Description,
                    ProductId = stock.ProductId,
                    Quantity = stock.Quantity
                });
            }
            
            _ctx.Stocks.UpdateRange(stocks);
            
            await _ctx.SaveChangesAsync();
            
            return new Responce
            {
                Stocks = request.Stocks
            };
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public int ProductId { get; set; }
        }
        
        public class Request
        {
            public IEnumerable<StockViewModel> Stocks { get; set; }
        }
        
        public class Responce
        {
            public IEnumerable<StockViewModel> Stocks { get; set; }
        }
    }
}