using System;
using System.Collections.Generic;
using System.Reflection;

namespace Acrylic
{
    public class SingletonFactory: IBuildServices
    {
        private object _sync = new object();
        private object _singleton = default;
        private ServiceFactory _factory;

        public SingletonFactory():this(null)
        {

        }

        public SingletonFactory(object implementation)
        {
            _factory = new ServiceFactory();
            _singleton = implementation;
        }

        public object BuildUpService(Type service, IContainer container)
        {
            if (_singleton != null) return _singleton;
            lock (_sync)
            {
                if (_singleton == null)
                {
                    _singleton = _factory.BuildUpService(service, container);
                }
            }
            return _singleton;
        }
    }
}