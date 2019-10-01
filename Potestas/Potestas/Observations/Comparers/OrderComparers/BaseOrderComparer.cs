using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public abstract class BaseOrderComparer : Comparer<IEnergyObservation>
    {

        public int? BaseOrderCompare(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            if (xObservation == null)
            {
                return -1;
            }

            if (yObservation == null)
            {
                return 1;
            }

            if (ReferenceEquals(xObservation, yObservation))
            {
                return 0;
            }

            return null;
        }
    }
}
