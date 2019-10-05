using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationPointComparer<T> : BaseOrderComparer<T> where T : IEnergyObservation
    {
        public override int Compare(T xObservation, T yObservation)
        {
            var baseOrderCompareResult = BaseOrderCompare(xObservation, yObservation);

            if (baseOrderCompareResult.HasValue) 
            {
                return baseOrderCompareResult.Value;
            }

            int xComparingResult = Comparer<double>.Default.Compare(xObservation.ObservationPoint.X, yObservation.ObservationPoint.X);
            int yComparingResult = Comparer<double>.Default.Compare(xObservation.ObservationPoint.Y, yObservation.ObservationPoint.Y);

            return (xComparingResult == 0) ? yComparingResult : (xComparingResult == -1) ? -1 : 1;
        }
    }
}
