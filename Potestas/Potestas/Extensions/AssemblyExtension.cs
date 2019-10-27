using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Potestas.Extensions
{
    public static class AssemblyExtension
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly, Func<Type, bool> predicate)
        {
            return assembly.GetTypes().Where(predicate);
        }
    }
}
