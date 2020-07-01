using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Domian.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Files.DownloadFile
{
    class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, FileDownloadDto>
    {
        private readonly IDbContext _db;

        public DownloadFileQueryHandler(IDbContext db)
        {
            _db = db;
        }

        public async Task<FileDownloadDto> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            File file = await _db.Files.FindAsync(request.Id, cancellationToken)
                ?? throw new ValidationException("File doesn't exist");

            return new FileDownloadDto()
            {
                Name = file.Name,
                Content = file.Content,
                ContentType = file.ContentType
            };
        }
    }
}