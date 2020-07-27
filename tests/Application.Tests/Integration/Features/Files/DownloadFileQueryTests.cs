using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.Files.DownloadFile;
using Application.Tests.Common.Base;
using AutoFixture.Xunit2;
using Domian.Entities;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration.Features.Files
{
	public class DownloadFileQueryTests : RequestTestBase
	{
		[Theory]
		[AutoData]
		public async Task ShouldReturnExistingFile(File file)
		{
			// Act
			await AddAsync(file);

			FileDownloadDto fileDto = await SendAsync(new DownloadFileQuery { Id = file.Id });

			// Assert
			fileDto.Should().NotBeNull();
			fileDto.Name.Should().Be(file.Name);
			fileDto.ContentType.Should().Be(file.ContentType);
			fileDto.Content.Should().Equal(file.Content);
		}

		[Theory]
		[AutoData]
		public void ShouldThrow_WhenFileDoesntExist(DownloadFileQuery query)
		{
			FluentActions.Awaiting(() => SendAsync(query))
			   .Should()
			   .Throw<ValidationException>()
			   .And.Error.Message.Should()
			   .Be("File doesn't exist");
		}
	}
}
