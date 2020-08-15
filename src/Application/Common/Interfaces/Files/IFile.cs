using System.IO;

namespace Application.Common.Interfaces.Files
{
    public interface IFile
    {
        string Name { get; }
        string ContentType { get; }
        Stream OpenReadStream();
    }
}
