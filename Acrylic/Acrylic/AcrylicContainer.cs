using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Acrylic
{
    public class AcrylicContainer : IContainer, IDisposable
    {
        private ConcurrentDictionary<Type, TypeRegistration> _registry;
       
        public AcrylicContainer()
        {
            _registry = new ConcurrentDictionary<Type, TypeRegistration>(Environment.ProcessorCount, 101);
        }
        /// <summary>
        /// Registers the interface or type <typeparamref name="TAbstract"/> that is implemented by 
        /// <typeparamref name="T"/> in the container.
        /// </summary>
        /// <typeparam name="TAbstract">An abstract type that the container uses to satisfy dependencies.</typeparam>
        /// <typeparam name="T">A constructable concrete type that provides the abstraction <typeparamref name="TAbstract"/></typeparam>
        public void Register<TAbstract, T>() where T : TAbstract => Register<TAbstract, T>(Lifetime.Transient);

        public void Register<TAbstract, T>(Lifetime lifetimeManager) where T : TAbstract
        {
            var registration = new TypeRegistration(typeof(TAbstract), typeof(T), lifetimeManager);
            //Need to remove the template parameters from the factories but for now
            //let's get things working
            switch (lifetimeManager)
            {
                case Lifetime.Singleton:
                    registration.Factory = new SingletonFactory<TAbstract, T>();
                    break;
                case Lifetime.Transient:
                default:
                    registration.Factory = new DefaultFactory<TAbstract, T>();
                    break;
            }
            _registry.AddOrUpdate(typeof(TAbstract), registration, (k, v) => registration);
        }

        public TAbstract Resolve<TAbstract>()
        {
            return (TAbstract)Resolve(typeof(TAbstract));
        }

        public object Resolve(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            object instance = null;
            if (_registry.ContainsKey(type))
            {
                var r = _registry[type];
                var factory = r.Factory;
                instance = factory.BuildUpService(r.ConcreteType,this);
            }
            else if(!type.IsAbstract)
            {
                instance = Activator.CreateInstance(type);
            }
            else
            {
                throw new TypeMappingUnavailableException($"{type.Name} is not mapped to a concrete implementation.");
            }
            return instance;
        }

        public bool IsRegistered(Type @type)
        {
            if (type == null) throw new ArgumentNullException(nameof(@type));
            return _registry.ContainsKey(@type);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}