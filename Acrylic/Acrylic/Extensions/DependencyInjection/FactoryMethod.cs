using System;
using System.Collections.Generic;
using System.Text;

namespace Acrylic.Extensions.DependencyInjection
{
    public class FactoryMethod : IBuildServices
    {
        private Func<IServiceProvider,object> ImplementationFactory { get; }

        public FactoryMethod(Func<IServiceProvider,object> factoryMethod)
        {
            ImplementationFactory = factoryMethod;
        }

        public object BuildUpService(Type service, IContainer container)
        {
            var serviceProvider = new AcrylicServiceProvider(container);
            return ImplementationFactory(serviceProvider);
        }
    }
}
