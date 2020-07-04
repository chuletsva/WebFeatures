using Application.Interfaces.DataAccess;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.GetProductReviews
{
    class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, IQueryable<ProductReviewInfoDto>>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public GetProductReviewsQueryHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IQueryable<ProductReviewInfoDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            return _db.ProductReviews
                .Where(x => x.ProductId == request.ProductId)
                .ProjectTo<ProductReviewInfoDto>(_mapper.ConfigurationProvider);
        }
    }
}