using Application.Interfaces.DataAccess;
using AutoMapper;
using Domian.Entities.Products;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductComments.CreateProductComment
{
    internal class CreateProductCommentCommandHandler : IRequestHandler<CreateProductCommentCommand, Guid>
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public CreateProductCommentCommandHandler(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateProductCommentCommand request, CancellationToken cancellationToken)
        {
            ProductComment comment = _mapper.Map<ProductComment>(request);

            await _db.ProductComments.AddAsync(comment, cancellationToken);

            comment.Events.Add(new ProductCommentCreated(comment.Id));

            return comment.Id;
        }
    }
}