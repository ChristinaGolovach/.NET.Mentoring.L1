using System;

namespace Potestas.MongoDB.Plugin.Exceptions
{
    public class MongoDBProcessorException : Exception
    {
        public MongoDBProcessorException() : base() { }

        public MongoDBProcessorException(string message) : base(message) { }

        public MongoDBProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
