using System;

namespace Potestas.Exceptions.SerializerExceptions
{
    public class JsonSerializerException : Exception
    {
        public JsonSerializerException() : base() { }

        public JsonSerializerException(string message) : base(message) { }

        public JsonSerializerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
