using Application.Behaviours;
using Application.Tests.Common.Stubs.Requests;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Tests.Unit.Behaviours
{
    public class ValidationBehaviourTests
    {
        [Fact]
        public async Task ShouldValidateRequestBeforeCallNextDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var validator = fixture.Freeze<Mock<IValidator<CustomCommand<int>>>>();

            var messages = new List<string>();

            var request = new CustomCommand<int>();
            var token = new CancellationToken();
            var validationResult = new ValidationResult();

            validator.Setup(x => x.ValidateAsync(request, token))
                .ReturnsAsync(validationResult)
                .Callback(() => messages.Add("validation"));

            RequestHandlerDelegate<int> next = () =>
            {
                messages.Add("next");

                return Task.FromResult(0);
            };

            var sut = new ValidationBehaviour<CustomCommand<int>, int>(new[] { validator.Object });

            // Act
            await sut.Handle(request, token, next);

            // Assert
            messages.Should().Equal(new[] { "validation", "next" });
        }

        [Fact]
        public void ShouldThrow_WhenValidationIsFailed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var validator = fixture.Freeze<Mock<IValidator<CustomCommand<int>>>>();

            var request = new CustomCommand<int>();

            var token = new CancellationToken();

            var validationFailure = fixture.Create<ValidationFailure>();

            var validationResult = new ValidationResult(new[] { validationFailure });

            validator.Setup(x => x.ValidateAsync(request, token)).ReturnsAsync(validationResult);

            var sut = new ValidationBehaviour<CustomCommand<int>, int>(new[] { validator.Object });

            // Act
            Func<Task<int>> act = () => sut.Handle(request, token, () => Task.FromResult(0));

            // Assert
            var _ = act.Should().Throw<ValidationException>();

            _.And.Error.Should().NotBeNull();
            _.And.Error.Errors.Should().NotBeNull();
            _.And.Error.Errors.Should().HaveCount(1);
            _.And.Error.Errors[validationFailure.PropertyName].Should().Equal(new[] { validationFailure.ErrorMessage });
        }
    }
}