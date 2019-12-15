using System;

namespace Potestas.API.Plugin.Exceptions
{
    public class APIAnalizerException : Exception
    {
        public APIAnalizerException() : base() { }

        public APIAnalizerException(string message) : base(message) { }

        public APIAnalizerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
