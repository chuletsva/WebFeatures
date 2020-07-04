using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.GetProductComments
{
    class GetProductCommentsQueryProfile : Profile
    {
        public GetProductCommentsQueryProfile()
        {
            CreateMap<ProductComment, ProductCommentInfoDto>(MemberList.Destination);
        }
    }
}
