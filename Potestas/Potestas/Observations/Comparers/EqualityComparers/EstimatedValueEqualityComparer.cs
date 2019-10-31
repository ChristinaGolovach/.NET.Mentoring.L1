using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class EstimatedValueEqualityComparer<T> : BaseEqualityComparer<T> where T : IEnergyObservation
    {
        public override bool Equals(T xObservation, T yObservation)
        {
            var baseEqualityCompareResult = DefaulValueEquals(xObservation, yObservation);

            if (baseEqualityCompareResult.HasValue)
            {
                return baseEqualityCompareResult.Value;
            }

            if (double.IsNaN(xObservation.EstimatedValue))
            {
                return double.IsNaN(yObservation.EstimatedValue);
            }

            double x = ComparerUtils.GetCanonicalValues(xObservation.EstimatedValue, ComparerUtils.comparePrecision);
            double y = ComparerUtils.GetCanonicalValues(yObservation.EstimatedValue, ComparerUtils.comparePrecision);

            return (x == y);
        }

        public override int GetHashCode(T observation)
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }
            
            double x = ComparerUtils.GetCanonicalValues(observation.EstimatedValue, ComparerUtils.comparePrecision);

            return x.GetHashCode();
        }

    }
}
