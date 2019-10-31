using System;

namespace Potestas.Exceptions.ProcessorExceptions
{
    public class SaveToStorageProcessorException : Exception
    {
        public SaveToStorageProcessorException() : base() { }

        public SaveToStorageProcessorException(string message) : base(message) { }

        public SaveToStorageProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
