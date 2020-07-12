using System;
using System.IO;
using System.Threading.Tasks;
using Application.Features.Files.UploadFile;
using Application.Interfaces.Files;
using Application.Tests.Common.Base;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using Xunit;
using File = Domian.Entities.File;

namespace Application.Tests.Integration.Features.Files
{
	public class UploadFileCommandTests : RequestTestBase
	{
		[Fact]
		public async Task ShouldCreateFile()
		{
			// Arrange
			var fixture = new Fixture().Customize(new AutoMoqCustomization());
		
			var fileMock = new Mock<IFile>();
			
			string name = fixture.Create<string>();
			string contentType = fixture.Create<string>();
			byte[] content = fixture.Create<byte[]>();
			
			fileMock.SetupGet(x => x.Name).Returns(name);
			fileMock.SetupGet(x => x.ContentType).Returns(contentType);
			fileMock.Setup(x => x.OpenReadStream()).Returns(() => new MemoryStream(content));
			
			fixture.Inject(fileMock.Object);
			
			var request = fixture.Create<UploadFileCommand>();
			
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
