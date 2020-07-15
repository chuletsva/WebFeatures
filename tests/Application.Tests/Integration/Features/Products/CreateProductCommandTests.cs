using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Tests.Common.Base;
using AutoFixture;
using Domian.Entities;
using Domian.Entities.Accounts;
using Domian.Entities.Products;
using Domian.ValueObjects;
using Xunit;

namespace Application.Tests.Integration.Features.Products
{
	public class CreateProductCommandTests : RequestTestBase
	{	
		[Fact]
		public async Task ShouldCreateProduct()
		{
		}
	}
}
