using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class EstimatedValueEqualityComparer : BaseEqualityComparer
    {
        public bool Equals<T>(T xObservation, T yObservation) where T: IEnergyObservation
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
            ComparerSettings.GetCanonicalValues(ref x, ref y);

            return (x == y);
        }

        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation) =>
            Equals<IEnergyObservation>(xObservation, yObservation);

        public int GetHashCode<T>(T observation) where T: IEnergyObservation
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            double x = observation.EstimatedValue;
            ComparerSettings.GetCanonicalValues(ref x);

            return x.GetHashCode();
        }

        public override int GetHashCode(IEnergyObservation observation) =>
            GetHashCode<IEnergyObservation>(observation);
    }
}
