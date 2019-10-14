using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public abstract class BaseOrderComparer<T> : Comparer<T> where T: IEnergyObservation
    {
        public int? DefaultValueOrderCompare(T xObservation, T yObservation)
        {
            if (EqualityComparer<T>.Default.Equals(xObservation, default(T)))
            {
                return -1;
            }

            if (EqualityComparer<T>.Default.Equals(yObservation, default(T)))
            {
                return 1;
            }

            if (EqualityComparer<T>.Default.Equals(xObservation, yObservation))
            {
                return 0;
            }

            return null;
        }
    }
}
