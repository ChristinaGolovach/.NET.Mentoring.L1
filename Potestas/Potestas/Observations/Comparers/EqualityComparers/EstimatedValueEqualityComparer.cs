using System;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class EstimatedValueEqualityComparer : BaseEqualityComparer
    {
        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult != null)
            {
                return (bool)baseEqualityCompareResult;
            }

            if (double.IsNaN(xObservation.EstimatedValue))
            {
                return double.IsNaN(yObservation.EstimatedValue);
            }

            if (double.IsNaN(yObservation.EstimatedValue))
            {
                return double.IsNaN(xObservation.EstimatedValue);
            }

            return Math.Abs(xObservation.EstimatedValue - yObservation.EstimatedValue) < ComparerSettings.epsilon;

        }

        public override int GetHashCode(IEnergyObservation observation)
        {
            if (observation == null)
            {
                return 0;
            }

            return observation.EstimatedValue.GetHashCode();
        }
    }
}
