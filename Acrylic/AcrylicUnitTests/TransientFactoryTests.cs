using Acrylic;
using Moq;
using Xunit;

namespace AcrylicUnitTests
{
    public class TransientFactoryTests
    {
        [Fact]
        public void Build_returns_unique_instance_for_each_invocation()
        {
            var builder = new DefaultFactory<ICalculator, Calculator>();
            var container = Mock.Of<IContainer>();

            var instance1 = builder.BuildUpService(container);
            var instance2 = builder.BuildUpService(container);

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
        }
    }
}
