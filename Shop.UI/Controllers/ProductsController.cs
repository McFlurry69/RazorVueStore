using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Application.ViewModels.AdminModels;
using Shop.Database;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize (Policy = "Manager")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;

        public ProductsController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id) => Ok(await new GetProduct(_ctx).Do(id));

        [HttpGet("")]
        public IActionResult GetProducts() => Ok(new GetProducts(_ctx).Do());

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductViewModelAdmin vm) => Ok(await new CreateProduct(_ctx).Do(vm));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok(await new DeleteProduct(_ctx).Do(id));
        
        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductViewModelAdmin vm) => Ok(await new UpdateProduct(_ctx).Do(vm));
    }
}