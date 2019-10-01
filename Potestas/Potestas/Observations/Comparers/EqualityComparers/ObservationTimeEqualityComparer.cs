using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationTimeEqualityComparer : BaseEqualityComparer
    {
        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult != null)
            {
                return (bool)baseEqualityCompareResult;
            }

            return xObservation.ObservationTime == yObservation.ObservationTime;
        }

        public override int GetHashCode(IEnergyObservation observation)
        {
            if (observation == null)
            {
                return 0;
            }

            return observation.ObservationTime.GetHashCode();
        }
    }
}
