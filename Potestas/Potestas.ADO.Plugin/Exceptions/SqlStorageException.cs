using System;

namespace Potestas.ADO.Plugin.Exceptions
{
    public class SqlStorageException : Exception
    {
        public SqlStorageException() : base() { }

        public SqlStorageException(string message) : base(message) { }

        public SqlStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
