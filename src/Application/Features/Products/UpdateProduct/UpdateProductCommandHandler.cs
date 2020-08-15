using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.DataAccess;

namespace Application.Features.Products.UpdateProduct
{
    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _db.Products.FindAsync(new object[]{request.Id}, cancellationToken);

            _mapper.Map(request, product);

            product.Events.Add(new ProductUpdated(product.Id));

            return Unit.Value;
        }
    }
}