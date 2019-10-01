using System;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationPointEqualityComparer : BaseEqualityComparer
    {
        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult != null)
            {
                return (bool)baseEqualityCompareResult;
            }

            if (double.IsNaN(xObservation.ObservationPoint.X) && double.IsNaN(xObservation.ObservationPoint.Y))
            {
                return double.IsNaN(yObservation.ObservationPoint.X) && double.IsNaN(yObservation.ObservationPoint.Y);
            }

            if (double.IsNaN(xObservation.ObservationPoint.X) && !double.IsNaN(xObservation.ObservationPoint.Y))
            {
                return double.IsNaN(yObservation.ObservationPoint.X) && !double.IsNaN(yObservation.ObservationPoint.Y);
            }

            if (!double.IsNaN(xObservation.ObservationPoint.X) && double.IsNaN(xObservation.ObservationPoint.Y))
            {
                return !double.IsNaN(yObservation.ObservationPoint.X) && double.IsNaN(yObservation.ObservationPoint.Y);
            }

            return Math.Abs(xObservation.ObservationPoint.X - yObservation.ObservationPoint.X) + 
                   Math.Abs(xObservation.ObservationPoint.Y - yObservation.ObservationPoint.Y) < ComparerSettings.epsilon;

        }

        public override int GetHashCode(IEnergyObservation energyObservation)
        {
            if (energyObservation==null)
            {
                return 0;
            }

            return energyObservation.ObservationPoint.X.GetHashCode() + energyObservation.ObservationPoint.Y.GetHashCode();
        }
    }
}
