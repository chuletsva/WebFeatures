using Application.Interfaces.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ModelBinders
{
    class FileModelBinderProvider : IModelBinderProvider
    {
        private static HashSet<Type> FileTypes = new HashSet<Type>()
        {
            typeof(IFile),
            typeof(IFile[])
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new NullReferenceException(nameof(context));
            }

            return FileTypes.Contains(context.Metadata.ModelType) ? new FileModelBinder() : null;
        }
    }

    class FileModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new NullReferenceException(nameof(bindingContext));
            }

            HttpRequest request = bindingContext.HttpContext.Request;

            if (!request.HasFormContentType)
            {
                return;
            }

            string modelName = bindingContext.ModelName;

            IFormCollection form = await request.ReadFormAsync();

            var postedFiles = new List<IFormFile>();

            foreach (IFormFile file in form.Files)
            {
                if (file.Length != 0 && string.Equals(file.Name, modelName, StringComparison.OrdinalIgnoreCase))
                {
                    postedFiles.Add(file);
                }
            }

            if (postedFiles.Count == 0)
            {
                return;
            }

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

    class FileAdapter : IFile
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