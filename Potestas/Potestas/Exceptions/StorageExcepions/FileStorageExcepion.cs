using System;

namespace Potestas.Exceptions
{
    public class FileStorageExcepion : Exception
    {
        public FileStorageExcepion() : base() { }

        public FileStorageExcepion(string message) : base(message) { }

        public FileStorageExcepion(string message, Exception innerException) : base(message, innerException) { }
    }
}
