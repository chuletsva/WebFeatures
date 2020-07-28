using System;
using Application.Models.Requests;

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
