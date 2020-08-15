using Domian.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces.DataAccess;

namespace Application.Features.Files.DownloadFile
{
    internal class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, FileDownloadDto>
    {
        private readonly IDbContext _db;

        public DownloadFileQueryHandler(IDbContext db)
        {
            _db = db;
        }

        public async Task<FileDownloadDto> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            File file = await _db.Files.FindAsync(new object[] { request.Id }, cancellationToken) ?? 
                        throw new ValidationException("File doesn't exist");

            return new FileDownloadDto()
            {
                Name = file.Name,
                Content = file.Content,
                ContentType = file.ContentType
            };
        }
    }
}