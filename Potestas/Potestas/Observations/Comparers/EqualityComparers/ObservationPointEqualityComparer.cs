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

            return observation.ObservationPoint.GetHashCode();
        }

        private bool EqualsCoordinatesOfObservations(Coordinates firstObservationCoordinates, Coordinates secondObservationCoordinates)
        {
            double x1 = firstObservationCoordinates.X;
            double y1 = firstObservationCoordinates.Y;

            double x2 = secondObservationCoordinates.X;
            double y2 = secondObservationCoordinates.Y;

            ComparerSettings.GetCanonicalValues(ref x1, ref y1, ComparerSettings.epsilon);
            ComparerSettings.GetCanonicalValues(ref x2, ref y2, ComparerSettings.epsilon);

            return x1 == x2 && y1 == y2;
        }
    }
}
