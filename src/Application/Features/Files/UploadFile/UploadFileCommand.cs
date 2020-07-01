using Application.Infrastructure.Requests;
using Application.Interfaces.Files;
using System;

namespace WebFeatures.Application.Features.Files.UploadFile
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