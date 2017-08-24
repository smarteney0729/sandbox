using Acrylic;
using Moq;
using System;
using System.Text;
using Xunit;

namespace AcrylicUnitTests
{
    public class SingletonFactoryTests
    {
        [Fact]
        public void SingltonFactory_uses_provided_implementation_instance()
        {
            var implementation = new Calculator();
            var builder = new SingletonFactory(implementation);
            var container = Mock.Of<IContainer>();

            var instance = builder.BuildUpService(typeof(Calculator), container);
            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ICalculator>(instance);
            Assert.Same(implementation, instance);
        }

        [Fact]
        public void Build_returns_instance_that_has_default_constructor()
        {
            var builder = new SingletonFactory();
            var container = Mock.Of<IContainer>();

            var instance = builder.BuildUpService(typeof(Calculator), container);
            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ICalculator>(instance);
        }

        [Fact]
        public void Build_returns_instance_that_has_primative_parameters_only()
        {
            var builder = new SingletonFactory();
            var container = Mock.Of<IContainer>();

            var instance = builder.BuildUpService(typeof(ConcreteClass), container);
            Assert.NotNull(instance);
            Assert.IsAssignableFrom<BaseClass>(instance);
        }

        [Fact]
        public void Build_returns_singleton_instance_for_each_invocation()
        {
            var builder = new SingletonFactory();
            var container = Mock.Of<IContainer>();

            var instance1 = builder.BuildUpService(typeof(Calculator), container);
            var instance2 = builder.BuildUpService(typeof(Calculator), container);

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }

        

    }

    
}
