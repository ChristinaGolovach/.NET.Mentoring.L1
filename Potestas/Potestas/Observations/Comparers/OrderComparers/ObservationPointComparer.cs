using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationPointComparer : BaseOrderComparer
    {
        public override int Compare(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            var baseOrderCompareResult = BaseOrderCompare(xObservation, yObservation);

            if (baseOrderCompareResult != null)
            {
                return (int)baseOrderCompareResult;
            }

            int xComparingResult = Comparer<double>.Default.Compare(xObservation.ObservationPoint.X, yObservation.ObservationPoint.X);
            int yComparingResult = Comparer<double>.Default.Compare(xObservation.ObservationPoint.Y, yObservation.ObservationPoint.Y);

            return (xComparingResult == 0) ? yComparingResult : (xComparingResult == -1) ? -1 : 1;
        }
    }
}
