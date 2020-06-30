using Application.Features.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ProductsController : BaseController
    {
        public async Task<IActionResult> Create(CreateProductCommand request)
        {

        }
    }
}