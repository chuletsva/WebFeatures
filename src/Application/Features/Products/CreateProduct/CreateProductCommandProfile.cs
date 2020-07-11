using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.CreateProduct
{
    internal class CreateProductCommandProfile : Profile
    {
        public CreateProductCommandProfile()
        {
            CreateMap<Product, CreateProductCommand>().ReverseMap();
        }
    }
}