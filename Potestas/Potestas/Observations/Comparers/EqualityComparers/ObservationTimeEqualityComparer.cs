using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationTimeEqualityComparer : BaseEqualityComparer
    {
        public bool Equals<T>(T xObservation, T yObservation) where T : IEnergyObservation
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult != null)
            {
                return (bool)baseEqualityCompareResult;
            }

            return xObservation.ObservationTime == yObservation.ObservationTime;
        }

        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation) =>
            Equals<IEnergyObservation>(xObservation, yObservation);

        public int GetHashCode<T>(T observation) where T : IEnergyObservation
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            return observation.ObservationTime.GetHashCode();
        }

        public override int GetHashCode(IEnergyObservation observation) =>
            GetHashCode<IEnergyObservation>(observation);
    }
}
