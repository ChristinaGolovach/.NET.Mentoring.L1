using System;
using System.Collections.Generic;
using System.Text;

namespace Potestas.ORM.Plugin.Exceptions
{
    public class ORMStorageException : Exception
    {
        public ORMStorageException() : base() { }

        public ORMStorageException(string message) : base(message) { }

        public ORMStorageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
