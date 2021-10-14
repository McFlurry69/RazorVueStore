using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Application.ViewModels.AdminModels;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<ProductViewModelAdmin> Do(int id)
        {
            return _ctx.Products.Where(x => x.Id.Equals(id)).Select(x => new ProductViewModelAdmin
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value
            }).FirstOrDefaultAsync();
        }
    }
}