using System;
using System.Diagnostics;
using System.Reflection;

namespace Acrylic
{
    internal class TypeRegistration
    {
        private object abstractType;

        public TypeRegistration(Type @abstract, object instance):
            this(@abstract, null, instance, Lifetime.Singleton)
        {
            Debug.Assert(@abstract != null);
            Debug.Assert(instance != null);
            Debug.Assert(@abstract.IsAssignableFrom(instance.GetType()));
        }

        public TypeRegistration(Type @abstract, Type concrete, Lifetime lifetimeManager)
            :this(@abstract, concrete, null, lifetimeManager)
        {
            Debug.Assert(@abstract != null);
            Debug.Assert(concrete != null);
            Debug.Assert(@abstract.IsAssignableFrom(concrete));
        }

        public TypeRegistration(Type @abstract, Type concrete, object instance, Lifetime lifetime)
        {
            Debug.Assert(@abstract != null);
            Debug.Assert(concrete != null || instance != null,"Implementation type or instance must be provided.");
            //Debug.Assert(concrete == null || (concrete != null && @abstract.IsAssignableFrom(concrete)));
            Debug.Assert(instance == null || (instance != null && @abstract.IsAssignableFrom(instance.GetType())));
            AbstractType = @abstract;
            ConcreteType = concrete;
            Instance = instance;
            LifetimeStrategy = lifetime;
            CreateFactory();
        }

        public Type AbstractType { get; private set; }
        public Type ConcreteType { get; private set; }
        public object Instance { get; private set; }
        public Lifetime LifetimeStrategy { get; private set; }
        public IBuildServices Factory { get;  set; }
        
        private IBuildServices CreateFactory()
        {
            switch (LifetimeStrategy)
            {
                case Lifetime.Singleton:
                    Factory = new SingletonFactory(Instance);
                    break;
                case Lifetime.Transient:
                default:
                    Factory = new ServiceFactory();
                    break;
            }
            return Factory;
        }
    }
}