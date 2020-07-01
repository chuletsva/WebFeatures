namespace Application.Features.Files.DownloadFile
{
    /// <summary>
    /// Файл для загрузки
    /// </summary>
    public class FileDownloadDto
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}