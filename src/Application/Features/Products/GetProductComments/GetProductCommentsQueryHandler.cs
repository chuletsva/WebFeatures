using Application.Interfaces.DataAccess;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.GetProductComments
{
    internal class GetProductCommentsQueryHandler : IRequestHandler<GetProductCommentsQuery, IQueryable<ProductCommentInfoDto>>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public GetProductCommentsQueryHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IQueryable<ProductCommentInfoDto>> Handle(GetProductCommentsQuery request, CancellationToken cancellationToken)
        {
            return _db.ProductComments
                .Where(x => x.ProductId == request.ProductId)
                .ProjectTo<ProductCommentInfoDto>(_mapper.ConfigurationProvider);
        }
    }
}