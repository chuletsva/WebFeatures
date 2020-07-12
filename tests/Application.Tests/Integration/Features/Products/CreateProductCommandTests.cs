using System.Threading.Tasks;
using Application.Tests.Common.Base;
using AutoFixture.Xunit2;
using Domian.Entities.Products;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	public class CreateProductCommandTests : RequestTestBase
	{
		[Theory][AutoData]
		public async Task ShouldCreateProduct(Product product)
		{
			// Arrange & act

			// Act
			
			// Assert
		}
	}
}
