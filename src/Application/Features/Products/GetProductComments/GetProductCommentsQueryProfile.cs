using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.Products.GetProductComments
{
    internal class GetProductCommentsQueryProfile : Profile
    {
        public GetProductCommentsQueryProfile()
        {
            CreateMap<ProductComment, ProductCommentInfoDto>(MemberList.Destination);
        }
    }
}
