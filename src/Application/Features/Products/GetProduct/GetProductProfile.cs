using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.GetProduct
{
    class GetProductProfile : Profile
    {
        public GetProductProfile()
        {
            CreateMap<Product, ProductInfoDto>(MemberList.Destination)
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Price.CurrencyId));
        }
    }
}
