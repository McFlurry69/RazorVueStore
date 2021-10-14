using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Database;

namespace Shop.Application.ProductsAdmin
{
    public class DeleteProduct
    {
        private readonly ApplicationDbContext _ctx;

        public DeleteProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Do(int id)
        {
            _ctx.Products.Remove(await _ctx.Products.FirstOrDefaultAsync(x => x.Id.Equals(id)));
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}