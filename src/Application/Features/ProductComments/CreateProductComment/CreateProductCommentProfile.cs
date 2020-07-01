using AutoMapper;
using Domian.Entities.Products;

namespace Application.Features.ProductComments.CreateProductComment
{
    class CreateProductCommentProfile : Profile
    {
        public CreateProductCommentProfile()
        {
            CreateMap<CreateProductCommentCommand, ProductComment>(MemberList.Source)
                .ForSourceMember(src => src.CommandId, opt => opt.DoNotValidate());
        }
    }
}
