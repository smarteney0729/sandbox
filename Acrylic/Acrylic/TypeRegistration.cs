using System;
using System.Diagnostics;
using System.Reflection;

namespace Acrylic
{
    internal class TypeRegistration
    {
        internal TypeRegistration(Type @abstract, Type concrete, Lifetime lifetimeManager)
        {
            Debug.Assert(@abstract != null);
            Debug.Assert(concrete != null);
            Debug.Assert(@abstract.IsAssignableFrom(concrete));
            AbstractType = @abstract;
            ConcreteType = concrete;
            LifetimeStrategy = lifetimeManager;
        }
        public Type AbstractType { get; private set; }
        public Type ConcreteType { get; private set; }
        public Lifetime LifetimeStrategy { get; private set; }

    }
}