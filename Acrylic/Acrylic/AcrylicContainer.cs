using System;
using System.Collections.Concurrent;
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
            var registration = new TypeRegistration(typeof(TAbstract), typeof(T), Lifetime.Transient);
            _registry.AddOrUpdate(typeof(TAbstract), registration, (k, v) => registration);
        }

        public TAbstract Resolve<TAbstract>()
        {
            return (TAbstract)Resolve(typeof(TAbstract));
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
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