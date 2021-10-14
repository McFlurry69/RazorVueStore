using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.OrdersAdmin;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize (Policy = "Manager")]
    public class OrdersController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetOrders(int status, [FromServices] GetOrders getOrders) => Ok(await getOrders.Do(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id,[FromServices] GetOrder getOrder) => Ok(getOrder.Do(id));
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromServices] UpdateOrder updateOrder) => Ok(await updateOrder.Do(id));
    }
}