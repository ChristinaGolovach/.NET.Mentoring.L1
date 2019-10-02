using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.EqualityComparers
{
    public class ObservationPointEqualityComparer : BaseEqualityComparer
    {
        public bool Equals<T>(T xObservation, T yObservation) where T : IEnergyObservation
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

        public override bool Equals(IEnergyObservation xObservation, IEnergyObservation yObservation) =>
             Equals<IEnergyObservation>(xObservation, yObservation);

        public int GetHashCode<T>(T observation) where T : IEnergyObservation
        {
            if (EqualityComparer<T>.Default.Equals(observation, default(T)))
            {
                return 0;
            }

            return observation.ObservationPoint.GetHashCode();
        }

        public override int GetHashCode(IEnergyObservation energyObservation) =>
            GetHashCode<IEnergyObservation>(energyObservation);

        private bool EqualsCoordinatesOfObservations(Coordinates firstObservationCoordinates, Coordinates secondObservationCoordinates)
        {
            double x1 = firstObservationCoordinates.X;
            double y1 = firstObservationCoordinates.Y;

            double x2 = secondObservationCoordinates.X;
            double y2 = secondObservationCoordinates.Y;

            ComparerSettings.GetCanonicalValues(ref x1, ref y1);
            ComparerSettings.GetCanonicalValues(ref x2, ref y2);

            return x1 == x2 && y1 == y2;
        }
    }
}
