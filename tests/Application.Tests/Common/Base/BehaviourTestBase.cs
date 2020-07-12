using AutoFixture;
using AutoFixture.AutoMoq;

namespace Application.Tests.Common.Base
{
	public abstract class BehaviourTestBase
	{
		protected IFixture Fixture { get; }

		protected BehaviourTestBase()
		{
			Fixture = new Fixture().Customize(new AutoMoqCustomization());
		}
	}
}
