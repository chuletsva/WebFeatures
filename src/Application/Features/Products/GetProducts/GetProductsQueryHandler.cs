using Application.Interfaces.DataAccess;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.GetProducts
{
    internal class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IQueryable<ProductListDto>>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IQueryable<ProductListDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return _db.Products.ProjectTo<ProductListDto>(_mapper.ConfigurationProvider);
        }
    }
}