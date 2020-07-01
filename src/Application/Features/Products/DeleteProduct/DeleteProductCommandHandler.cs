using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Domian.Entities.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.DeleteProduct
{
    class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IDbContext _db;

        public DeleteProductCommandHandler(IDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _db.Products.FindAsync(request.Id, cancellationToken)
                ?? throw new ValidationException("Product doesn't exist");

            _db.Products.Remove(product);

            product.Events.Add(new ProductDeleted(request.Id));

            return Unit.Value;
        }
    }
}