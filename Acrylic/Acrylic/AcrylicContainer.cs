using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Acrylic
{
    public class AcrylicContainer : IDisposable
    {
        private ConcurrentDictionary<Type, TypeRegistration> _registry;
        public AcrylicContainer()
        {
            _registry = new ConcurrentDictionary<Type, TypeRegistration>(Environment.ProcessorCount, 101);
        }
        /// <summary>
        /// Registers the interface or type <typeparamref name="I"/> that is implemented by 
        /// <typeparamref name="T"/> in the container.
        /// </summary>
        /// <typeparam name="I">An abstract type that the container uses to satisfy dependencies.</typeparam>
        /// <typeparam name="T">A constructable concrete type that provides the abstraction <typeparamref name="I"/></typeparam>
        public void Register<I, T>() where T:I,new()
        {
            Register<I, T>(Lifetime.Transient);
        }

        public void Register<I, T>(Lifetime lifetimeManager) where T : I, new()
        {
            var registration = new TypeRegistration(typeof(I), typeof(T), Lifetime.Transient);
            _registry.AddOrUpdate(typeof(I), registration, (k, v) => registration);
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