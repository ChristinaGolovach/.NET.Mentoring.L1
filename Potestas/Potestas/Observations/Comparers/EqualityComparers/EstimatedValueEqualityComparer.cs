using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class EstimatedValueEqualityComparer<T> : BaseEqualityComparer<T> where T : IEnergyObservation
    {
        public override bool Equals(T xObservation, T yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult.HasValue)
            {
                return baseEqualityCompareResult.Value;
            }

            if (double.IsNaN(xObservation.EstimatedValue))
            {
                return double.IsNaN(yObservation.EstimatedValue);
            }

            double x = xObservation.EstimatedValue;
            double y = yObservation.EstimatedValue;
            ComparerSettings.GetCanonicalValues(ref x, ref y, ComparerSettings.epsilon);

            return (x == y);
        }

        public override int GetHashCode(T observation)
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            double x = observation.EstimatedValue;
            ComparerSettings.GetCanonicalValues(ref x, ComparerSettings.epsilon);

            return x.GetHashCode();
        }

    }
}
