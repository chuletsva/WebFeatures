using Application.Exceptions;
using Application.Interfaces.DataAccess;
using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.GetProduct
{
    internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductInfoDto>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductInfoDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            Product product = await _db.Products.FindAsync(new object[] { request.Id }, cancellationToken) ?? 
                              throw new ValidationException("Product doesn't exist");

            return _mapper.Map<ProductInfoDto>(product);
        }
    }
}