using Acrylic;
using System;
using Xunit;

namespace AcrylicUnitTests
{
    public class ContainerSetupTests
    {
        [Fact]
        public void When_container_is_empty_IsRegistered_returns_false()
        {
            var container = new AcrylicContainer();
            Assert.False(container.IsRegistered(typeof(ITextStream)));
        }

        [Fact]
        public void When_type_not_registerd_IsRegistered_returns_false()
        {
            var container = new AcrylicContainer();

            container.Register<BaseClass, ConcreteClass>();
            container.Register<ITextStream, TextStream>();

            Assert.False(container.IsRegistered(typeof(ICalculator)));
        }

        [Fact]
        public void Container_allows_type_registration()
        {
            var container = new AcrylicContainer();
            container.Register<ITextStream, TextStream>();

            Assert.True(container.IsRegistered(typeof(ITextStream)));
        }

        [Fact]
        public void Container_allows_type_registration_of_abstract_class()
        {
            var container = new AcrylicContainer();
            container.Register<BaseClass, ConcreteClass>();

            Assert.True(container.IsRegistered(typeof(BaseClass)));
        }

        [Fact]
        public void Container_allows_registration_an_instance()
        {
            var instance = new ConcreteClass();
            var container = new AcrylicContainer();
            container.Register<BaseClass>(instance);

            Assert.True(container.IsRegistered(typeof(BaseClass)));
        }


        [Fact]
        public void Container_allows_type_registration_multiple_times_last_is_winner()
        {
            //There is a choice here to throw or to overwrite existing registration
            //Need to pick one way of the other.  This test is just a declaration of 
            //that choice and since it is somewhat passive agressive behavior
            //if the something changes the test will fail.

            var container = new AcrylicContainer();
            container.Register<ITextStream, TextStream>();
            container.Register<ITextStream, AsciiStream>();

            var instance = container.Resolve<ITextStream>();
            Assert.IsType<AsciiStream>(instance);
        }
    }
}
