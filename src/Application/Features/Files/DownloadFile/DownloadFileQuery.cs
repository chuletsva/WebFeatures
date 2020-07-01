using Application.Infrastructure.Requests;
using System;

namespace Application.Features.Files.DownloadFile
{
    /// <summary>
    /// Получить файл
    /// </summary>
    public class DownloadFileQuery : IQuery<FileDownloadDto>
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public Guid Id { get; set; }
    }
}
