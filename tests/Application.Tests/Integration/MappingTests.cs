using Application.Features.ProductComments.CreateProductComment;
using Application.Features.ProductReviews.CreateProductReview;
using Application.Features.Products.CreateProduct;
using Application.Features.Products.GetProduct;
using Application.Features.Products.GetProductComments;
using Application.Features.Products.GetProductReviews;
using Application.Features.Products.GetProducts;
using Application.Features.Products.UpdateProduct;
using AutoMapper;
using Domian.Entities.Products;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Application.Tests.Integration
{
    public class MappingTests
    {
        private static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(x => x.AddMaps(typeof(DI).Assembly));
        }

        [Fact]
        public void ShouldAssertValidConfiguration()
        {
            // Arrange
            MapperConfiguration configuration = CreateConfiguration();

            // Act
            Action act = () => configuration.AssertConfigurationIsValid();

            // Assert
            act.Should().NotThrow();
        }

        [Theory][MemberData(nameof(Mappings))]
        public void ShouldSupportMap(Type sourceType, Type destinationType)
        {
            // Arrange
            MapperConfiguration configuration = CreateConfiguration();

            // Act
            Action act = () =>
            {
                object instance = Activator.CreateInstance(sourceType);

                IMapper mapper = configuration.CreateMapper();

                mapper.Map(instance, sourceType, destinationType);
            };

            // Assert
            act.Should().NotThrow();
        }

        public static IEnumerable<object[]> Mappings
        {
            get
            {
                yield return new object[] { typeof(CreateProductCommentCommand), typeof(ProductComment) };
                yield return new object[] { typeof(CreateProductReviewCommand), typeof(ProductReview) };
                yield return new object[] { typeof(Product), typeof(CreateProductCommand) };
                yield return new object[] { typeof(Product), typeof(ProductInfoDto) };
                yield return new object[] { typeof(ProductComment), typeof(ProductCommentInfoDto) };
                yield return new object[] { typeof(ProductReview), typeof(ProductReviewInfoDto) };
                yield return new object[] { typeof(Product), typeof(ProductListDto) };
                yield return new object[] { typeof(UpdateProductCommand), typeof(Product) };
            }
        }
    }
}