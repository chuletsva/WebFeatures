using Application.Interfaces.DataAccess;
using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.UpdateProduct
{
    class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
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
            Product product = _mapper.Map<Product>(request);

            _db.Products.Update(product);

            product.Events.Add(new ProductUpdated(product.Id));

            return Unit.Value;
        }
    }
}