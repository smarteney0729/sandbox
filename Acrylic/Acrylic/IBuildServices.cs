using System;

namespace Acrylic
{
    public interface IBuildServices<T> : IBuildServices
    {
        T BuildUpService(IContainer container);
    }

    public interface IBuildServices
    {
        object BuildUpService(Type service,IContainer container);
    }
}