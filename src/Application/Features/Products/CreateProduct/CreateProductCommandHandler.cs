using Application.Interfaces.DataAccess;
using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.CreateProduct
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = _mapper.Map<Product>(request);

            await _db.Products.AddAsync(product, cancellationToken);

            product.Events.Add(new ProductCreated(product.Id));

            return product.Id;
        }
    }
}