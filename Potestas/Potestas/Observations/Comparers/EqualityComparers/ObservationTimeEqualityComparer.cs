using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationTimeEqualityComparer<T> : BaseEqualityComparer<T> where T : IEnergyObservation
    {
        public override bool Equals(T xObservation, T yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult != null)
            {
                return (bool)baseEqualityCompareResult;
            }

            return xObservation.ObservationTime == yObservation.ObservationTime;
        }

        public override int GetHashCode(T observation)
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            return observation.ObservationTime.GetHashCode();
        }

    }
}
