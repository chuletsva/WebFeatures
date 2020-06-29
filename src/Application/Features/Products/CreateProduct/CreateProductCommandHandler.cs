using Application.Interfaces.DataAccess;
using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.CreateProduct
{
    class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateProductCommandHandler(IDbContext db, IMapper mapper, IMediator mediator)
        {
            _db = db;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = _mapper.Map<Product>(request);

            await _db.Products.AddAsync(product, cancellationToken);

            await _mediator.Send(new ProductCreated(product.Id), cancellationToken);

            return product.Id;
        }
    }
}