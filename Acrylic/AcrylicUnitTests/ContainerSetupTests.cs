using Acrylic;
using System;
using Xunit;

namespace AcrylicUnitTests
{
    public class ContainerSetupTests
    {
        [Fact]
        public void Container_allows_type_registration()
        {
            var container = new AcrylicContainer();
            container.Register<ITextStream, TextStream>();

            Assert.True(container.IsRegistered(typeof(ITextStream)));
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
            //Need to implement Resolve first.
        }

    }
}
