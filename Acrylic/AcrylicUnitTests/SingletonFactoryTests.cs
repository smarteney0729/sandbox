using Acrylic;
using Moq;
using System;
using Xunit;

namespace AcrylicUnitTests
{
    public class SingletonFactoryTests
    {
        [Fact]
        public void Build_returns_instance_that_has_default_constructor()
        {
            var builder = new SingletonFactory<ICalculator, Calculator>();
            var container = Mock.Of<IContainer>();

            var instance = builder.buildUpInstance(container);
            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ICalculator>(instance);
        }

        [Fact]
        public void Build_returns_instance_that_has_primative_parameters_only()
        {
            var builder = new SingletonFactory<BaseClass, ConcreteClass>();
            var container = Mock.Of<IContainer>();

            var instance = builder.buildUpInstance(container);
            Assert.NotNull(instance);
            Assert.IsAssignableFrom<BaseClass>(instance);
        }

        [Fact]
        public void Build_returns_singleton_instance_for_each_invocation()
        {
            var builder = new SingletonFactory<ICalculator, Calculator>();
            var container = Mock.Of<IContainer>();

            var instance1 = builder.buildUpInstance(container);
            var instance2 = builder.buildUpInstance(container);

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Build_provides_dependencies_for_registered_constructor_parameters()
        {
            var builder = new SingletonFactory<ViewController, ViewController>();
            var container = new Mock<IContainer>();
            container.Setup(c => c.Resolve(typeof(IEmail))).Returns(new EmailService() as IEmail);
            container.Setup(c => c.Resolve(typeof(ICalculator))).Returns(new Calculator() as ICalculator);

            ViewController instance = builder.buildUpInstance(container.Object);

            Assert.NotNull(instance);
            Assert.IsType<ViewController>(instance);
            Assert.NotNull(instance.Calculator);
            Assert.NotNull(instance.EmailService);
        }

        [Fact]
        public void Build_throws_when_it_cannot_statisfy_dependencies_for_all_constructor_parameters()
        {
            var builder = new SingletonFactory<ViewController, ViewController>();
            var container = new Mock<IContainer>();
            container.Setup(c => c.Resolve(typeof(IEmail))).Returns(new EmailService() as IEmail);
            container.Setup(c => c.Resolve(typeof(ICalculator))).Throws<InvalidOperationException>();
            Assert.Throws<DepencencyNotSatisfiedException>(() => builder.buildUpInstance(container.Object));
        }


        [Fact]
        public void Build_throws_when_concrete_class_does_not_have_public_constructor()
        {
            var builder = new SingletonFactory<BaseClass, InternalConstructorClass>();
            var container = Mock.Of<IContainer>();

            Assert.Throws<InvalidOperationException>(() => builder.buildUpInstance(container));            
        }

        [Theory]
        [InlineData(typeof(ConcreteClass), 2)]
        [InlineData(typeof(TextStream), 0)]
        [InlineData(typeof(ViewController), 2)]
        
        public void GetGreedyConstructor_returns_constructor_that_has_the_most_arguments(Type t, int args)
        {
            var constructor = SingletonFactory<BaseClass, ConcreteClass>.GetGreedyConstructor(t);

            Assert.Equal<int>(args, constructor.GetParameters().Length);
        }
    }

    
}
