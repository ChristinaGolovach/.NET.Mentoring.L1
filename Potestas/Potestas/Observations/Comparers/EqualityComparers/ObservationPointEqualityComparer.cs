using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationPointEqualityComparer<T> : BaseEqualityComparer<T> where T : IEnergyObservation
    {
        public override bool Equals(T xObservation, T yObservation)
        {
            var baseEqualityCompareResult = DefaulValueEquals(xObservation, yObservation);

            if (baseEqualityCompareResult.HasValue)
            {
                return baseEqualityCompareResult.Value;
            }

            var resultOfNaNCheck = ComparerUtils.IsNaNPointComparer(xObservation.ObservationPoint, yObservation.ObservationPoint, EqualsCoordinatesOfObservations);

            if (resultOfNaNCheck.HasValue)
            {
                return resultOfNaNCheck.Value;
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

            x = ComparerUtils.GetCanonicalValues(x, ComparerUtils.comparePrecision);
            y = ComparerUtils.GetCanonicalValues(y, ComparerUtils.comparePrecision);


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

            x1 = ComparerUtils.GetCanonicalValues(x1, ComparerUtils.comparePrecision);
            y1 = ComparerUtils.GetCanonicalValues(y1, ComparerUtils.comparePrecision);

            x2 = ComparerUtils.GetCanonicalValues(x2, ComparerUtils.comparePrecision);
            y2 = ComparerUtils.GetCanonicalValues(y2, ComparerUtils.comparePrecision);

            return x1 == x2 && y1 == y2;
        }

        private bool EqualsCoordinatesOfObservations(double point1XorY, double point2XorY)
        {
            return ComparerUtils.GetCanonicalValues(point1XorY, ComparerUtils.comparePrecision) 
                   == ComparerUtils.GetCanonicalValues(point2XorY, ComparerUtils.comparePrecision);
        }
    }

    // An equality comparer switches on non standard equality and hashing behavior
    // and it need not to call logic from ObservationPoint.
    // Here the logic is implemented as in the structure just for example
}
