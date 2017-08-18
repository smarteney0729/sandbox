using System;
using System.Collections.Generic;
using System.Reflection;

namespace Acrylic
{
    public class SingletonFactory<T1, T2> where T2 : T1
    {
        private object _sync = new object();
        private T1 _singleton = default;

        public T1 buildUpInstance(IContainer container)
        {
            //TODO: These are 2 seperate concerns, setting singleton and buildup
            if (_singleton != null) return _singleton;
            lock (_sync)
            {
                if (_singleton == null)
                {
                    ConstructorInfo constructor;
                    var type = typeof(T2);
                    constructor = GetGreedyConstructor(type);
                    //TODO: concreteClass does not have a public constructor.
                    if (constructor == null) throw new InvalidOperationException($"No public constructors for implementation of {typeof(T1).Name}");

                    _singleton = CreateInstance(container, constructor);
                }
            }
            return _singleton;
        }

        public static T1 CreateInstance(IContainer container, ConstructorInfo constructor)
        {
            T1 instance;
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
            instance = (T1)constructor.Invoke(parameters.ToArray());

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