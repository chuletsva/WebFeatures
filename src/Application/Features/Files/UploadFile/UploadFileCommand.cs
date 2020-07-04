using System;
using Application.Infrastructure.Requests;
using Application.Interfaces.Files;

namespace Application.Features.Files.UploadFile
{
    /// <summary>
    /// Загрузить файл
    /// </summary>
    public class UploadFileCommand : CommandBase<Guid>
    {
        /// <summary>
        /// Файл
        /// </summary>
        public IFile File { get; set; }
    }
}