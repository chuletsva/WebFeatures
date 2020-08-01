using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces.Files;

namespace WebApi.ModelBinders
{
    internal class FileModelBinderProvider : IModelBinderProvider
    {
        private static readonly HashSet<Type> FileTypes = new HashSet<Type>()
        {
            typeof(IFile),
            typeof(IFile[])
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new NullReferenceException(nameof(context));

            return FileTypes.Contains(context.Metadata.ModelType) ? new FileModelBinder() : null;
        }
    }

    internal class FileModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new NullReferenceException(nameof(bindingContext));

            HttpRequest request = bindingContext.HttpContext.Request;

            if (!request.HasFormContentType) return;

            string modelName = bindingContext.ModelName;

            IFormCollection form = await request.ReadFormAsync();

            List<IFormFile> postedFiles = form.Files
               .Where(file => file.Length != 0 && string.Equals(file.Name, modelName, StringComparison.OrdinalIgnoreCase))
               .ToList();

            if (postedFiles.Count == 0) return;

            if (bindingContext.ModelType == typeof(IFile))
            {
                bindingContext.Result = ModelBindingResult.Success(
                    new FileAdapter(postedFiles.First()));

                return;
            }

            if (bindingContext.ModelType == typeof(IFile[]))
            {
                bindingContext.Result = ModelBindingResult.Success(
                    postedFiles.Select(x => new FileAdapter(x)).ToArray());

                return;
            }
        }
    }

    internal class FileAdapter : IFile
    {
        private readonly IFormFile _file;

        public FileAdapter(IFormFile file)
        {
            _file = file;
        }

        public string Name => _file.FileName;
        public string ContentType => _file.ContentType;
        public Stream OpenReadStream() => _file.OpenReadStream();
    }
}