using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.UpdateProduct
{
    internal class UpdateProductCommandProfile : Profile
    {
        public UpdateProductCommandProfile()
        {
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
        }
    }
}