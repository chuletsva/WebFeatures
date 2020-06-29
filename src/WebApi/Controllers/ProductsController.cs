using Application.Features.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ProductsController : BaseODataController
    {
        public async Task<IActionResult> Create(CreateProductCommand request)
        {

        }
    }
}