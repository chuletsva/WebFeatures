using Application.Infrastructure.Results;
using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Linq;
using Xunit;

namespace Application.Tests.Unit.Infrastructure.Results
{
    public class ValidationErrorTests
    {
        [Fact]
        public void ShouldSetErrors()
        {
            var fixture = new Fixture();

            string propertyName = fixture.Create<string>();

            int errorsCount = fixture.Create<int>();

            string[] errors = fixture.CreateMany<string>(errorsCount).ToArray();

            // Arrange
            ValidationFailure[] failures = errors
                .Select(x => new ValidationFailure(propertyName, x))
                .ToArray();

            // Act
            var sut = new ValidationError(failures);

            // Assert
            sut.Errors.Should().NotBeNull();
            sut.Errors.Should().HaveCount(1);
            sut.Errors.Should().ContainKey(propertyName);
            sut.Errors[propertyName].Should().Equal(errors);
        }
    }
}