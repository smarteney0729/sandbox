using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Acrylic
{
    public class DepencencyNotSatisfiedException : Exception
    {
        //TODO: Provide better message strategy, use a couple of type arguments
        //target Type, dependency Type
        public DepencencyNotSatisfiedException()
            : base("Unable to statisfy one or more constructor dependencies resolving the requested type.")
        {
        }

        public DepencencyNotSatisfiedException(string message) : base(message)
        {
        }

        public DepencencyNotSatisfiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DepencencyNotSatisfiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
