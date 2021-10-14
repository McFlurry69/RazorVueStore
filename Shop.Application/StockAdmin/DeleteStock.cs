using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin
{
    public class DeleteStock
    {
        private readonly ApplicationDbContext _ctx;

        public DeleteStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Do(int id)
        {
            var stock = _ctx.Stocks.FirstOrDefaultAsync(x => x.Id.Equals(id));
            _ctx.Stocks.Remove(await stock);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}