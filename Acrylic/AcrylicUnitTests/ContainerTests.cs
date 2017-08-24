using Acrylic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AcrylicUnitTests
{
    public class ContainerTests
    {
        [Fact]
        public void Resolve_throws_when_abstract_type_is_not_registered()
        {
            IContainer container = new AcrylicContainer();

            Assert.Throws<TypeMappingUnavailableException>(() => container.Resolve<ICalculator>());
        }

        [Fact]
        public void Resolve_provides_unregistered_types_with_default_constructor()
        { 
            IContainer container = new AcrylicContainer();

            var instance = container.Resolve<StringBuilder>();
            Assert.NotNull(instance);
            Assert.IsType<StringBuilder>(instance);
        }

        [Fact]
        public void Resolve_returns_singleton_instance_for_each_invocation_when_SingletonLifetime()
        {
            IContainer container = new AcrylicContainer();            
            container.Register<ICalculator, Calculator>(Lifetime.Singleton);
            
            var instance1 = container.Resolve<ICalculator>();
            var instance2 = container.Resolve<ICalculator>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Resolve_returns_provided_singleton_instance_for_each_invocation()
        {
            var calculator = new Calculator();
            IContainer container = new AcrylicContainer();
            container.Register<ICalculator>(calculator);

            var instance1 = container.Resolve<ICalculator>();
            var instance2 = container.Resolve<ICalculator>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
            Assert.Same(calculator, instance1);
            Assert.Same(calculator, instance2);
        }

        [Fact]
        public void Resolve_returns_different_instance_for_each_invocation_when_TransientLifetime()
        {
            IContainer container = new AcrylicContainer();
            container.Register<ICalculator, Calculator>(Lifetime.Transient);

            var instance1 = container.Resolve<ICalculator>();
            var instance2 = container.Resolve<ICalculator>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
        }

        [Fact]
        public void Resolve_returns_different_instance_for_each_invocation_when_DefaultLifetime()
        {
            IContainer container = new AcrylicContainer();
            container.Register<ICalculator, Calculator>();

            var instance1 = container.Resolve<ICalculator>();
            var instance2 = container.Resolve<ICalculator>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
        }

        [Fact]
        public void Resolve_provides_dependencies_for_registered_constructor_parameters()
        {
            IContainer container = new AcrylicContainer();
            container.Register<ViewController, ViewController>(Lifetime.Transient);
            container.Register<ICalculator, Calculator>();
            container.Register<IEmail, EmailService>(Lifetime.Singleton);

            ViewController instance = container.Resolve<ViewController>();

            Assert.NotNull(instance);
            Assert.IsType<ViewController>(instance);
            Assert.NotNull(instance.Calculator);
            Assert.NotNull(instance.EmailService);
        }

    }

   
}
