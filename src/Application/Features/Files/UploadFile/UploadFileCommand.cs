using System;
using Application.Interfaces.Files;
using Application.Models.Requests;

namespace Application.Features.Files.UploadFile
{
    /// <summary>
    /// Загрузить файл
    /// </summary>
    public class UploadFileCommand : CommandBase<Guid>, IRequireAuthorization
    {
        /// <summary>
        /// Файл
        /// </summary>
        public IFile File { get; set; }
    }
}