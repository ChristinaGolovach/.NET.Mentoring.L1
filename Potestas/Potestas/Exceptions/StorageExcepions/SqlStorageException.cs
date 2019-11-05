using System;
using System.Collections.Generic;
using System.Text;

namespace Potestas.Exceptions.StorageExcepions
{
    public class SqlStorageException : Exception
    {
        public SqlStorageException() : base() { }

        public SqlStorageException(string message) : base(message) { }

        public SqlStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
