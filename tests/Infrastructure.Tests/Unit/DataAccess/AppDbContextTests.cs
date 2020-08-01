using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.CommonServices;
using Domian.Entities.Accounts;
using FluentAssertions;
using Infrastructure.DataAccess;
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

		private static User GetDefaultUser()
		{
			return new User()
			{
				Id = new Guid("6dfe1db5-282d-426d-aa49-8af67b04019f"),
				Name = "User"
			};
		}
		
		[Fact]
		public async Task SaveChanges_ShouldSetAuditData_WhenCreateEntity()
		{
			// Arrange
			var now = DateTime.UtcNow;

			_datetime.Setup(x => x.Now).Returns(now);

			User user = GetDefaultUser();

			_currentUser.Setup(x => x.UserId).Returns(user.Id);

			var entity = new CustomAuditableEntity();
			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			// Act
			await sut.AddRangeAsync(user, entity);
			await sut.SaveChangesAsync();

			// Assert
			entity.CreatedAt.Should().Be(now);
			entity.CreatedById.Should().Be(user.Id);
		}

		[Fact]
		public async Task SaveChanges_ShouldSetAuditData_WhenUpdateEntity()
        {
			// Arrange
			var now = DateTime.UtcNow;

			_datetime.Setup(x => x.Now).Returns(now);

			User user = GetDefaultUser();

			_currentUser.Setup(x => x.UserId).Returns(user.Id);

			var entity = new CustomAuditableEntity();

			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			// Act
			await sut.AddRangeAsync(user, entity);		
			await sut.SaveChangesAsync();

			sut.Update(entity);

			await sut.SaveChangesAsync();

			// Assert
			entity.UpdatedAt.Should().Be(now);
			entity.UpdatedById.Should().Be(user.Id);
		}

		[Fact]
		public async Task SaveChanges_ShouldSupportSoftDelete()
        {
			// Arrange
			var entity = new CustomSoftDeleteEntity();	
			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);
			
			// Act
			await sut.AddAsync(entity);
			await sut.SaveChangesAsync();

			sut.Remove(entity);

			await sut.SaveChangesAsync();

			// Assert
			entity.IsDeleted.Should().BeTrue();
		}

		[Fact]
		public async Task SaveChanges_ShouldRaiseEvent()
		{
			// Arrange
			INotification eve = new CustomEvent();
			
			User user = GetDefaultUser();
			
			user.Events.Add(eve);
			
			var sut = new CustomAppDbContext(_options, _currentUser.Object, _datetime.Object, _mediator.Object);

			// Act
			await sut.AddAsync(user);
			await sut.SaveChangesAsync();

			// Assert
			_mediator.Verify(x => x.Publish(eve, It.IsAny<CancellationToken>()), Times.Once);
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