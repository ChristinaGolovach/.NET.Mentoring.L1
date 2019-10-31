using System;

namespace Potestas.Exceptions.ProcessorExceptions
{
    public class SerializeProcessorException : Exception
    {
        public SerializeProcessorException() : base() { }

        public SerializeProcessorException(string message) : base(message) { }

        public SerializeProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
