using System;
using System.Collections.Generic;

namespace Potestas.Validators
{
    public static class GenericValidator
    {
        public static void CheckInitialization<T>(T value, string name)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                throw new ArgumentException($"The {name} must be initialized.");
            }
        }
    }
}
