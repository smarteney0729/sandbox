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
        public void Container_allows_type_registration_multiple_times_last_is_winner()
        {
            var container = new AcrylicContainer();
            container.Register<ITextStream, TextStream>();
            container.Register<ITextStream, AsciiStream>();

            Assert.True(container.IsRegistered(typeof(ITextStream)));
            //need to verify the container indeed holds registration for
            //AsciiStream as the implementation of ITextStream 
            //Need to implement Resolve first, or a Spy that can
            //look at the internals of the container.
        }

    }

   
}
