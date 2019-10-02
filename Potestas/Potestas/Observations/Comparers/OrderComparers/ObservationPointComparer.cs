using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationPointComparer : BaseOrderComparer
    {
        public int Compare<T>(T xObservation, T yObservation) where T: IEnergyObservation
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

        public override int Compare(IEnergyObservation xObservation, IEnergyObservation yObservation) =>
            Compare<IEnergyObservation>(xObservation, yObservation);
    }
}
