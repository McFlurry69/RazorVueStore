using System.Threading.Tasks;
using Shop.Application.ViewModels;
using Shop.Application.ViewModels.AdminModels;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class CreateProduct
    {
        private readonly ApplicationDbContext _ctx;

        public CreateProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ProductViewModelAdmin> Do(ProductViewModelAdmin vm)
        {
            var productToAdd = new Product()
            {
                Description = vm.Description,
                Name = vm.Name,
                Value = vm.Value
            };
            _ctx.Products.Add(productToAdd);
            await _ctx.SaveChangesAsync();

            return new ProductViewModelAdmin()
            {
                Id = productToAdd.Id,
                Description = productToAdd.Description,
                Name = productToAdd.Name,
                Value = productToAdd.Value
            };
        }
    }
}