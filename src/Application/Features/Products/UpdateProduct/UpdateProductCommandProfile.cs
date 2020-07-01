using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.UpdateProduct
{
    class UpdateProductCommandProfile : Profile
    {
        public UpdateProductCommandProfile()
        {
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
        }
    }
}