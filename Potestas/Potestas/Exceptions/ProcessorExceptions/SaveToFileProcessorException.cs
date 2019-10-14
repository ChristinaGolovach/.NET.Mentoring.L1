using System;

namespace Potestas.Exceptions.ProcessorExceptions
{
    public class SaveToFileProcessorException : Exception
    {
        public SaveToFileProcessorException() : base() { }

        public SaveToFileProcessorException(string message) : base(message) { }

        public SaveToFileProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
