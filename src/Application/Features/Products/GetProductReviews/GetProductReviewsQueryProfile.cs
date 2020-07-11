using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.GetProductReviews
{
    internal class GetProductReviewsQueryProfile : Profile
    {
        public GetProductReviewsQueryProfile()
        {
            CreateMap<ProductReview, ProductReviewInfoDto>(MemberList.Destination);
        }
    }
}
