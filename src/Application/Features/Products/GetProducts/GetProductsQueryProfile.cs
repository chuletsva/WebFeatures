using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.GetProducts
{
    internal class GetProductsQueryProfile : Profile
    {
        public GetProductsQueryProfile()
        {
            CreateMap<Product, ProductListDto>(MemberList.Destination)
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Price.CurrencyId))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Price.Currency.Code));
        }
    }
}
