using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Application.ViewModels;
using Shop.Database;

namespace Shop.Application.Products
{
    public class GetProducts
    {
        private readonly ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _ctx.Products.
                Include(x => x.Stock).
                Select(x => new ProductViewModel()
            {
                Description = x.Description,
                Name = x.Name,
                Value = x.Value.ToString("N2"),
                StockCount = x.Stock.Sum(y => y.Quantity)
            }).ToList();
    }
}