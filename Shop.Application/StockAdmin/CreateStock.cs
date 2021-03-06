using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin
{
    public class CreateStock
    {
        private readonly ApplicationDbContext _ctx;

        public CreateStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Responce> Do(Request request)
        {
            var stock = new Stock()
            {
                Description = request.Description,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _ctx.Stocks.AddAsync(stock);
            await _ctx.SaveChangesAsync();
            return new Responce()
            {
                Id = stock.Id,
                Description = stock.Description,
                Quantity = stock.Quantity
            };
        }

        public class Request
        {
            public string Description { get; set; }
            public int Quantity { get; set; }
            public int ProductId { get; set; }
        }
        
        public class Responce
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }
    }
}