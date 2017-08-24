using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Acrylic.Extensions.DependencyInjection
{
    public class AcrylicServiceProvider : IServiceProvider
    {
        private static Dictionary<ServiceLifetime, Lifetime> map = new Dictionary<ServiceLifetime, Lifetime>();
        static AcrylicServiceProvider() {
            map.Add(ServiceLifetime.Scoped, Lifetime.Singleton);
            map.Add(ServiceLifetime.Singleton, Lifetime.Singleton);
            map.Add(ServiceLifetime.Transient, Lifetime.Transient);
        }

        private IContainer _container;
        /// <summary>
        /// Adapter for the ASP.Net Core Dependency Injection Service Provider
        /// </summary>
        /// <param name="container">Reference to an Acrylic IOC Container</param>
        public AcrylicServiceProvider(IContainer container)
        {
            this._container = container;
        }
        public object GetService(Type serviceType)
        {
            object instance;
            try
            {
                instance = _container.Resolve(serviceType);
            }
            catch(TypeMappingUnavailableException)
            {
                //Interface specification of IServiceProvider says that GetService returns null
                //when service is not registered.
                instance = null;
            }
            return instance;
        }

        public void Configure(IServiceCollection services)
        {
            if (services == null) return;
            
            foreach(var s in services)
            {
                var lifetime = MapToAcrylicLifetime(s.Lifetime);

                if (s.ImplementationFactory != null)
                {
                    _container.Register(s.ServiceType, new FactoryMethod(s.ImplementationFactory));
                }
                else
                {
                    _container.Register(s.ServiceType, s.ImplementationType, s.ImplementationInstance, lifetime);
                }
                
            }
        }

        private Lifetime MapToAcrylicLifetime(ServiceLifetime lifetime)
        {
            return map[lifetime];
        }
    }
}
