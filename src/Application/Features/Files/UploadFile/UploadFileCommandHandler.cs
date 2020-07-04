using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.DataAccess;
using MediatR;
using File = Domian.Entities.File;

namespace Application.Features.Files.UploadFile
{
    class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Guid>
    {
        private readonly IDbContext _db;

        public UploadFileCommandHandler(IDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            await using var ms = new MemoryStream();

            await using var fs = request.File.OpenReadStream();

            await fs.CopyToAsync(ms, cancellationToken);

            File file = new File()
            {
                Name = request.File.Name,
                ContentType = request.File.ContentType,
                Content = ms.ToArray()
            };

            await _db.Files.AddAsync(file, cancellationToken);

            return file.Id;
        }
    }
}