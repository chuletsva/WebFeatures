using AutoFixture.Xunit2;
using FluentAssertions;
using Infrastructure.Security;
using System;
using Xunit;

namespace Infrastructure.Tests.Unit.Security
{
    public class PasswordHasherTests
    {
        [Fact]
        public void ComputeHash_ShouldComputeHash()
        {
            // Arrange
            var sut = new PasswordHasher();

            // Act
            string hash = sut.ComputeHash("12345");

            // Assert
            hash.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        public void ComputeHash_ShouldThrowWhenInvalidPassword(string password)
        {
            // Arrange
            var sut = new PasswordHasher();

            // Act
            Func<string> act = () => sut.ComputeHash(password);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Verify_ShouldVerifyComputedHash()
        {
            // Arrange
            string password = "12345";

            var sut = new PasswordHasher();

            // Act
            string hash = sut.ComputeHash(password);

            bool verified = sut.Verify(hash, password);

            // Assert
            verified.Should().BeTrue();
        }

        [Fact]
        public void Verify_ShouldNotVerifyInvalidPassword()
        {
            // Arrange
            var sut = new PasswordHasher();

            // Act
            string hash = sut.ComputeHash("12345");

            bool verified = sut.Verify(hash, "54321");

            // Assert
            verified.Should().BeFalse();
        }
    }
}
