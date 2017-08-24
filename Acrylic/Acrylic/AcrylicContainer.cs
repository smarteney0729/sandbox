using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Acrylic
{
    public class AcrylicContainer : IContainer, IDisposable
    {
        private ConcurrentDictionary<Guid, TypeRegistration> _registry;
       
        public AcrylicContainer()
        {
            _registry = new ConcurrentDictionary<Guid, TypeRegistration>(Environment.ProcessorCount, 101);
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
            Register(typeof(TAbstract), typeof(T), lifetimeManager);
        }

        public void Register(Type abstractType, Type implementationType)
        {
            Register(abstractType, implementationType, Lifetime.Transient);
        }

        public void Register(Type abstractType, Type implementationType, Lifetime lifetime)
        {
            var registration = new TypeRegistration(abstractType, implementationType, lifetime);
            _registry.AddOrUpdate(abstractType.GUID, registration, (k, v) => registration);
        }

        public void Register<T>(T instance)
        {
            Register(typeof(T), instance);
        }

        public void Register(Type abstractType, object instance)
        {
            var registration = new TypeRegistration(abstractType, instance);
            _registry.AddOrUpdate(abstractType.GUID, registration, (k, v) => registration);
        }
        public void Register(Type abstractType, Type implementationType, object instance, Lifetime lifetime)
        {
            var registration = new TypeRegistration(abstractType, implementationType, instance, lifetime);
            _registry.AddOrUpdate(abstractType.GUID, registration, (k, v) => registration);
        }

        public void Register(Type abstractType, IBuildServices factory)
        {
            var registration = new TypeRegistration(abstractType, factory);
            _registry.AddOrUpdate(abstractType.GUID, registration, (k, v) => registration);
        }


        public TAbstract Resolve<TAbstract>()
        {
            return (TAbstract)Resolve(typeof(TAbstract));
        }

        public object Resolve(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            object instance = null;
            if (_registry.ContainsKey(type.GUID))
            {
                var r = _registry[type.GUID];
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
            return _registry.ContainsKey(@type.GUID);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

       
    }
}