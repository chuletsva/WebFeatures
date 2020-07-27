using System;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Domian.Entities.Accounts;
using FluentAssertions;
using Infrastructure.DataAccess;
using Infrastructure.Tests.Common.Attributes;
using Infrastructure.Tests.Common.Stubs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Infrastructure.Tests.Unit.DataAccess
{
    public class AppDbContextTests
	{
		private readonly Mock<IDateTime> _datetime;
		private readonly Mock<ICurrentUser> _currentUser;
		private readonly Mock<IMediator> _mediator;
		private readonly DbContextOptions<AppDbContext> _options;

		public AppDbContextTests()
		{
			_datetime = new Mock<IDateTime>();
			_currentUser = new Mock<ICurrentUser>();
			_mediator = new Mock<IMediator>();
			_options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
		}
		
		[Fact]
		public async Task ShouldSetAuditData_WhenCreateEntity()
		{
			// Arrange
			var now = DateTime.UtcNow;

			_datetime.Setup(x => x.Now).Returns(now);

			var user = new User()
			{
				Id = new Guid("6dfe1db5-282d-426d-aa49-8af67b04019f"),
				Name = "User"
			};

			_currentUser.Setup(x => x.UserId).Returns(user.Id);

			var auditable = new CustomAuditableEntity();

			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			// Act
			await sut.AddRangeAsync(user, auditable);

			// Assert
			auditable.CreatedAt.Should().Be(now);
			auditable.CreatedById.Should().Be(user.Id);
			auditable.UpdatedAt.Should().BeNull();
			auditable.UpdatedById.Should().BeNull();
		}

		[Fact]
		public async Task ShouldSetAuditData_WhenUpdateEntity()
        {
			// Arrange
			var now = DateTime.UtcNow;

			_datetime.Setup(x => x.Now).Returns(now);

			var user = new User()
			{
				Id = new Guid("6dfe1db5-282d-426d-aa49-8af67b04019f"),
				Name = "User"
			};

			_currentUser.Setup(x => x.UserId).Returns(user.Id);

			var auditable = new CustomAuditableEntity();

			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			// Act
			await sut.AddRangeAsync(user, auditable);
			await sut.SaveChangesAsync();

			sut.Update(auditable);

			await sut.SaveChangesAsync();

			// Assert
			auditable.CreatedAt.Should().Be(now);
			auditable.CreatedById.Should().Be(user.Id);
			auditable.UpdatedAt.Should().Be(now);
			auditable.UpdatedById.Should().Be(user.Id);
		}

		[Fact]
		public async Task ShouldSupportSoftDelete()
        {
			// Arrange
			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			var softdel = new CustomSoftDeleteEntity();

			// Act
			await sut.AddAsync(softdel);

			await sut.SaveChangesAsync();

			sut.Remove(softdel);

			await sut.SaveChangesAsync();

			// Assert
			softdel.IsDeleted.Should().BeTrue();
		}
		
		
		[Fact]
		public void ShouldNotThrow_WhenConfiguredWithDefaultConnectionString()
		{	
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
			   .UseNpgsql(
					"server=localhost;port=5432;database=webfeatures_db;username=postgres;password=postgres",
					opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
			   .Options;
			
			// Act
			Action act = () => { using var sut = new AppDbContext(options, _currentUser.Object, _datetime.Object, _mediator.Object); };
			
			// Assert
			act.Should().NotThrow();
		}
	}
}