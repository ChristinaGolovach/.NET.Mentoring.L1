using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public abstract class BaseEqualityComparer<T> : EqualityComparer<T> where T: IEnergyObservation
    {
        public bool? BaseEqualityCompare<T>(T xObservation, T yObservation) where T : IEnergyObservation
        {
            if (EqualityComparer<T>.Default.Equals(xObservation, default(T)))
            {
                return EqualityComparer<T>.Default.Equals(yObservation, default(T));
            }

            if (EqualityComparer<T>.Default.Equals(yObservation, default(T)))
            {
                return EqualityComparer<T>.Default.Equals(xObservation, default(T));
            }

            if (EqualityComparer<T>.Default.Equals(xObservation, yObservation))
            {
                return true;
            }

            return null;
        }
    }
}
