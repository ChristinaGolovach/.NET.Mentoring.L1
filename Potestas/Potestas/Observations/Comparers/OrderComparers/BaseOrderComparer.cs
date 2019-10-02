using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public abstract class BaseOrderComparer : Comparer<IEnergyObservation>
    {
        public int? BaseOrderCompare<T>(T xObservation, T yObservation) where T: IEnergyObservation
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
