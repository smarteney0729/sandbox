using Acrylic;
using Acrylic.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AcrylicUnitTests.Extensions.DependencyInjection
{
    public class AcrylicServiceProviderTests
    {
        [Fact]
        public void GetService_returns_singleton_instance_for_each_invocation_when_SingletonLifetime()
        {
            IContainer container = new AcrylicContainer();
            container.Register<ICalculator, Calculator>(Lifetime.Singleton);

            IServiceProvider servieProvider = new AcrylicServiceProvider(container);
            var instance1 = servieProvider.GetService(typeof(ICalculator));
            var instance2 = servieProvider.GetService(typeof(ICalculator));

            Assert.IsAssignableFrom<ICalculator>(instance1);
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void GetService_forwards_call_to_containers_Resolve_successful_for_registered_type()
        {
            var container = new Mock<IContainer>();
            container.Setup(m => m.Resolve(typeof(ICalculator))).Returns(new Calculator());            
            
            IServiceProvider serviceProvider = new AcrylicServiceProvider(container.Object);
            var instance = serviceProvider.GetService(typeof(ICalculator));

            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ICalculator>(instance);
            container.Verify(m => m.Resolve(typeof(ICalculator)), Times.Once);
        }

        [Fact]
        public void GetService_returns_null_service_type_is_not_registered()
        {
            var container = new Mock<IContainer>();
            container.Setup(m => m.Resolve(typeof(ICalculator))).Throws<TypeMappingUnavailableException>();

            IServiceProvider serviceProvider = new AcrylicServiceProvider(container.Object);
            var instance = serviceProvider.GetService(typeof(ICalculator));

            Assert.Null(instance);
        }
    }
}
