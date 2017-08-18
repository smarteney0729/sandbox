using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Acrylic
{
    public class TypeMappingUnavailableException : Exception
    {
        public TypeMappingUnavailableException()
        {
        }

        public TypeMappingUnavailableException(string message) : base(message)
        {
        }

        public TypeMappingUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeMappingUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
