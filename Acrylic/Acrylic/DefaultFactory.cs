using System;
using System.Collections.Generic;
using System.Reflection;

namespace Acrylic
{
    public class ServiceFactory : IBuildServices
    {
        public object BuildUpService(Type service, IContainer container)
        {
            ConstructorInfo constructor;
            constructor = GetGreedyConstructor(service);

            //TODO: concreteClass does not have a public constructor.
            if (constructor == null) throw new InvalidOperationException($"No public constructors for implementation of {service.Name}");

            var instance = CreateInstance(container, constructor);

            return instance;
        }

        public static object CreateInstance(IContainer container, ConstructorInfo constructor)
        {
            object instance;
            List<object> parameters = new List<object>();

            foreach (var p in constructor.GetParameters())
            {
                try
                {
                    //TODO: Change to a chain of responsibility, very simple one.
                    //Just builds.
                    object value;
                    if (p.ParameterType.IsValueType)
                    {
                        value = Activator.CreateInstance(p.ParameterType);
                    }
                    else
                    {
                        value = container.Resolve(p.ParameterType);
                    }
                    parameters.Add(value);
                }
                catch (MissingMethodException ex)
                {
                    throw new DepencencyNotSatisfiedException($"Unable to satisfy dependency on {p.ParameterType.Name} while building {constructor.ReflectedType}", ex);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DepencencyNotSatisfiedException($"Unable to satisfy dependency on {p.ParameterType.Name} while building {constructor.ReflectedType}", ex);
                }
            }

            //Intentionally outside of try catch.  If this throws.  It's different.
            //Obviously a seperate concern.
            instance = constructor.Invoke(parameters.ToArray());

            return instance;
        }
        public static ConstructorInfo GetGreedyConstructor(Type type)
        {
            ConstructorInfo greedyConstructor = null;

            var constructors = type.GetConstructors();

            int greedyConstructorArgCount = -1;
            foreach (var c in constructors)
            {
                int argCount = c.GetParameters().Length;
                greedyConstructorArgCount = Math.Max(argCount, greedyConstructorArgCount);
                if (greedyConstructorArgCount == argCount)
                {
                    greedyConstructor = c;
                }
            }

            return greedyConstructor;
        }
    } 
}