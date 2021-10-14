using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Enums;

namespace Shop.Application.OrdersAdmin
{
    public class GetOrders
    {
        private readonly ApplicationDbContext _ctx;

        public GetOrders(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        public async Task<IEnumerable<Response>> Do(int status) =>
            await _ctx.Orders.Where(x => x.Status == (OrderStatus) status)
                .Select(x => new Response()
                {
                    Id = x.Id,
                    OrderRef = x.OrderRef,
                    Email = x.Email
                }).ToListAsync();
    }
}