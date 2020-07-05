using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Infrastructure.Tests.Common.Attributes
{
    public class AutoMoqAttribute : AutoDataAttribute
    {
        public AutoMoqAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}