using System;
using Application.Interfaces.Services;
using FluentAssertions;
using Infrastructure.DataAccess;
using Infrastructure.Tests.Common.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Integration.DataAccess
{
	public class AppDbContextTests
	{
		[Theory][AutoMoq]
		public void ShouldNotThrow_WhenConfiguredWithDefaultConnectionString(
			ICurrentUser currentUser, 
			IDateTime dateTime, 
			IMediator mediator)
		{	
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
			   .UseNpgsql(
					"server=localhost;port=5432;database=webfeatures_db;username=postgres;password=postgres",
					opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
			   .Options;
			
			// Act
			Action act = () => { using var ctx = new AppDbContext(options, currentUser, dateTime, mediator); };
			
			// Assert
			act.Should().NotThrow();
		}
	}
}