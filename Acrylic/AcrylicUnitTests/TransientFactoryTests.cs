using Acrylic;
using Moq;
using System;
using System.Text;
using Xunit;

namespace AcrylicUnitTests
{
    public class TransientFactoryTests
    {
        [Fact]
        public void Build_returns_unique_instance_for_each_invocation()
        {
            var builder = new ServiceFactory();
            var container = Mock.Of<IContainer>();

            var instance1 = builder.BuildUpService(typeof(Calculator), container);
            var instance2 = builder.BuildUpService(typeof(Calculator), container);

            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
        }

        [Fact]
        public void Build_throws_when_it_cannot_statisfy_dependencies_for_all_constructor_parameters()
        {
            var builder = new ServiceFactory();
            var container = new Mock<IContainer>();
            container.Setup(c => c.Resolve(typeof(IEmail))).Returns(new EmailService(new StringBuilder()) as IEmail);
            container.Setup(c => c.Resolve(typeof(ICalculator))).Throws<InvalidOperationException>();
            Assert.Throws<DepencencyNotSatisfiedException>(() => builder.BuildUpService(typeof(ViewController), container.Object));
        }

        [Fact]
        public void Build_throws_when_concrete_class_does_not_have_public_constructor()
        {
            var builder = new ServiceFactory();
            var container = Mock.Of<IContainer>();

            Assert.Throws<InvalidOperationException>(() => builder.BuildUpService(typeof(InternalConstructorClass), container));
        }

        [Fact]
        public void Build_provides_dependencies_for_registered_constructor_parameters()
        {
            var builder = new ServiceFactory();
            var container = new Mock<IContainer>();
            container.Setup(c => c.Resolve(typeof(IEmail))).Returns(new EmailService(new StringBuilder()) as IEmail);
            container.Setup(c => c.Resolve(typeof(ICalculator))).Returns(new Calculator() as ICalculator);

            ViewController instance = (ViewController)builder.BuildUpService(typeof(ViewController), container.Object);

            Assert.NotNull(instance);
            Assert.IsType<ViewController>(instance);
            Assert.NotNull(instance.Calculator);
            Assert.NotNull(instance.EmailService);
        }



        [Theory]
        [InlineData(typeof(ConcreteClass), 2)]
        [InlineData(typeof(TextStream), 0)]
        [InlineData(typeof(ViewController), 2)]

        public void GetGreedyConstructor_returns_constructor_that_has_the_most_arguments(Type t, int args)
        {
            var constructor = ServiceFactory.GetGreedyConstructor(t);

            Assert.Equal<int>(args, constructor.GetParameters().Length);
        }

    }
}
