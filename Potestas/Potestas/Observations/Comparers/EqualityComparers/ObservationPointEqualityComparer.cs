using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationPointEqualityComparer<T> : BaseEqualityComparer<T> where T : IEnergyObservation
    {
        public override bool Equals(T xObservation, T yObservation)
        {
            var baseEqualityCompareResult = BaseEqualityCompare(xObservation, yObservation);

            if (baseEqualityCompareResult.HasValue)
            {
                return baseEqualityCompareResult.Value;
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

            return EqualsCoordinatesOfObservations(xObservation.ObservationPoint, yObservation.ObservationPoint);
        }

        public override int GetHashCode(T observation)
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            int hash = 3;
            double x = observation.ObservationPoint.X;
            double y = observation.ObservationPoint.Y;

            ComparerUtils.GetCanonicalValues(ref x, ref y, ComparerUtils.comparePrecision);

                hash = (hash * 7) + x.GetHashCode();
                hash = (hash * 7) + y.GetHashCode();

            return hash;
        }

        private bool EqualsCoordinatesOfObservations(Coordinates firstObservationCoordinates, Coordinates secondObservationCoordinates)
        {
            double x1 = firstObservationCoordinates.X;
            double y1 = firstObservationCoordinates.Y;

            double x2 = secondObservationCoordinates.X;
            double y2 = secondObservationCoordinates.Y;

            ComparerUtils.GetCanonicalValues(ref x1, ref y1, ComparerUtils.comparePrecision);
            ComparerUtils.GetCanonicalValues(ref x2, ref y2, ComparerUtils.comparePrecision);

            return x1 == x2 && y1 == y2;
        }
    }

    // An equality comparer switches on non standard equality and hashing behavior
    // and it need not to call logic from ObservationPoint.
    // Here the logic is implemented as in the structure just for example
}
