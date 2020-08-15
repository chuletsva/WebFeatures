using System;
using System.IO;
using System.Threading.Tasks;
using Application.Common.Interfaces.Files;
using Application.Features.Files.UploadFile;
using Application.Tests.Common.Attributes;
using Application.Tests.Common.Base;
using FluentAssertions;
using Moq;
using Xunit;
using File = Domian.Entities.File;

namespace Application.Tests.Integration.Features.Files
{
    public class UploadFileCommandTests : RequestTestBase
    {
        [Theory, AutoMoq]
        public async Task ShouldCreateFile(string name, string contentType, byte[] content)
        {
            // Arrange	
            var fileMock = new Mock<IFile>();

            fileMock.SetupGet(x => x.Name).Returns(name);
            fileMock.SetupGet(x => x.ContentType).Returns(contentType);
            fileMock.Setup(x => x.OpenReadStream()).Returns(() => new MemoryStream(content));

            var request = new UploadFileCommand() { File = fileMock.Object };

            // Act
            await LoginAsDefaultUserAsync();

            Guid fileId = await SendAsync(request);

            File file = await FindAsync<File>(x => x.Id == fileId);

            // Assert
            file.Should().NotBeNull();
            file.Id.Should().Be(fileId);
            file.Name.Should().Be(name);
            file.ContentType.Should().Be(contentType);
            file.Content.Should().Equal(content);
        }
    }
}