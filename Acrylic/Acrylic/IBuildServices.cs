using System;

namespace Acrylic
{
    public interface IBuildServices
    {
        object BuildUpService(Type service,IContainer container);
    }
}