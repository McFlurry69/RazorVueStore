using System.Collections.Generic;
using System.Linq;
using Shop.Application.ViewModels.AdminModels;
using Shop.Database;

namespace Shop.Application.ProductsAdmin
{
    public class GetProducts
    {
        private readonly ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModelAdmin> Do() =>
            _ctx.Products.ToList().Select(x => new ProductViewModelAdmin()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Value = x.Value
            });
    }
}