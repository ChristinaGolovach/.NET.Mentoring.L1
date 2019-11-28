using System;

namespace Potestas.MongoDB.Plugin.Exceptions
{
    class MongoDBStorageException : Exception
    {
        public MongoDBStorageException() : base() { }

        public MongoDBStorageException(string message) : base(message) { }

        public MongoDBStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
