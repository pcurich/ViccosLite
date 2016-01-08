using System;
using System.Runtime.Serialization;

namespace ViccosLite.Core
{
    [Serializable]
    public class KsException : Exception
    {
        public KsException()
        {
        }
        public KsException(string message)
            : base(message)
        {
        }

        public KsException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {

        }

        public KsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public KsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}