using Application.Interfaces.DataAccess;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using File = Domian.Entities.File;

namespace Application.Features.Files.UploadFile
{
    internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Guid>
    {
        private readonly IDbContext _db;

        public UploadFileCommandHandler(IDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();

            using Stream fs = request.File.OpenReadStream();

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