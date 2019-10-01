using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public abstract class BaseEqualityComparer : EqualityComparer<IEnergyObservation>
    {
        public bool? BaseEqualityCompare(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            if (xObservation == null)
            {
                return yObservation == null;
            }

            if (yObservation == null)
            {
                return xObservation == null;
            }

            if (ReferenceEquals(xObservation, yObservation))
            {
                return true;
            }

            return null;
        }
    }
}
