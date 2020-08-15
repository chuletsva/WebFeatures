using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Linq;
using Application.Common.Models.Results;
using Xunit;

namespace Application.Tests.Unit.Infrastructure.Results
{
    public class ValidationErrorTests
    {
        [Fact]
        public void ShouldSetErrors()
        {
            // Arrange
            var fixture = new Fixture();

            string propertyName = fixture.Create<string>();

            string[] errors = fixture.CreateMany<string>().ToArray();

            ValidationFailure[] failures = errors.Select(error => new ValidationFailure(propertyName, error)).ToArray();

            // Act
            var sut = new ValidationError(failures);

            // Assert
            sut.Errors.Should().NotBeNull();
            sut.Errors.Should().HaveCount(1);
            sut.Errors.Should().ContainKey(propertyName);
            sut.Errors[propertyName].Should().Equal(failures.Select(x => x.ErrorMessage));
        }
    }
}