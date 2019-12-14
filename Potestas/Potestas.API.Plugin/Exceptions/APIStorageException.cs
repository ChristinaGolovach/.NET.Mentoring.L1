using System;

namespace Potestas.API.Plugin.Exceptions
{
    public class APIStorageException : Exception
    {
        public APIStorageException() : base() { }

        public APIStorageException(string message) : base(message) { }

        public APIStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
