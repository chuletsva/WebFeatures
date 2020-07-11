using AutoFixture.Xunit2;
using FluentAssertions;
using Infrastructure.Security;
using Infrastructure.Tests.Common.Attributes;
using System;
using Xunit;

namespace Infrastructure.Tests.Unit.Security
{
    public class PasswordHasherTests
    {
        [Theory, AutoMoq]
        public void ComputeHash_ReturnsNonEmptyHash(string password, PasswordHasher sut)
        {
            // Act
            string hash = sut.ComputeHash(password);

            // Assert
            hash.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        public void ComputeHash_Throws_WhenEmptyPassword(string password, PasswordHasher sut)
        {
            // Act
            Func<string> act = () => sut.ComputeHash(password);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoMoq]
        public void Verify_ShouldVerifyComputedHash(string password, PasswordHasher sut)
        {
            // Act
            string hash = sut.ComputeHash(password);

            bool isVerified = sut.Verify(hash, password);

            // Assert
            isVerified.Should().BeTrue();
        }

        [Theory, AutoMoq]
        public void Verify_ShouldNotVerify_WhenPassedWrongPassword(
            string password,
            string wrongPassword,
            PasswordHasher sut)
        {
            // Act
            string hash = sut.ComputeHash(password);

            bool isVerified = sut.Verify(hash, wrongPassword);

            // Assert
            isVerified.Should().BeFalse();
        }
    }
}
