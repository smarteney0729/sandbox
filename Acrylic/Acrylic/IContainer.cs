using System;

namespace Acrylic
{
    public interface IContainer
    {
        /// <summary>
        /// Registers the interface or type <typeparamref name="TAbstract"/> that is implemented by 
        /// <typeparamref name="T"/> in the container.
        /// </summary>
        /// <typeparam name="TAbstract">An abstract type that the container uses to satisfy dependencies.</typeparam>
        /// <typeparam name="T">A constructable concrete type that provides the abstraction <typeparamref name="TAbstract"/></typeparam>
        void Register<TAbstract, T>() where T : TAbstract;

        void Register<TAbstract, T>(Lifetime lifetimeManager) where T : TAbstract;
        void Register(Type abstractType, Type implementationType);
        void Register(Type abstractType, Type implementationType, Lifetime lifetime);
        void Register<T>(T instance);
        void Register(Type abstractType, object instance);
        void Register(Type abstractType, Type implementationType, object instance, Lifetime lifetime);

        void Register(Type abstractType, IBuildServices factory);

        TAbstract Resolve<TAbstract>();
        object Resolve(Type type);

        bool IsRegistered(Type @type);
    }
}