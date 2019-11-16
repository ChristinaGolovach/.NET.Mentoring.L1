using System.Collections.Generic;

namespace Potestas.Extensions
{
    public static class EnergyObservationExtension
    {
        public static List<T> ConvertObservationCollectionToGeneric<T, U>(this IEnumerable<U> typedCollection)
        {
            var genericCollection = new List<T>();

            foreach (var result in typedCollection)
            {
                genericCollection.Add((T)(object)result);
            }

            return genericCollection;
        }
    }
}
