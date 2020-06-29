using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.CreateProduct
{
    class CreateProductCommandProfile : Profile
    {
        public CreateProductCommandProfile()
        {
            CreateMap<CreateProductCommand, Product>(MemberList.Source);
        }
    }
}