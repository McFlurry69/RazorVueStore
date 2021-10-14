using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Application.ViewModels;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ProductViewModel> Do(string name)
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0)
            {
                var stockToReturn = _ctx.Stocks.AsEnumerable()
                    .Where(x => stocksOnHold.Any(y => y.StockId == x.Id)).ToList();

                foreach (var stock in stockToReturn)
                {
                    stock.Quantity = stock.Quantity + stocksOnHold.FirstOrDefault(x => x.StockId.Equals(stock.Id)).Quantity;
                }
                
                _ctx.StocksOnHold.RemoveRange(stocksOnHold);

                await _ctx.SaveChangesAsync();
            }

            return await _ctx.Products.Include(x => x.Stock)
                .Where(x => x.Name == name)
                .Select(vm => new ProductViewModel()
                {
                    Description = vm.Description,
                    Name = vm.Name,
                    Value = vm.Value,
                    Stock = vm.Stock.Select(y => new StockViewModel()
                    {
                        Description = y.Description,
                        Id = y.Id,
                        InStock = y.Quantity > 0
                    })
                }).FirstOrDefaultAsync();
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public bool InStock { get; set; }
        }
        
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}