using System;
using System.Collections.Generic;
using System.Text;

namespace Potestas.ORM.Plugin.Exceptions
{
    public class DBStorageException : Exception
    {
        public DBStorageException() : base() { }

        public DBStorageException(string message) : base(message) { }

        public DBStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
