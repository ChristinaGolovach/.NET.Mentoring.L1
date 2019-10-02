using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class EstimatedValueComparer : BaseOrderComparer
    {
        public int Compare<T>(T xObservation, T yObservation) where T: IEnergyObservation
        {
             return BaseOrderCompare(xObservation, yObservation) ?? 
                    Comparer<double>.Default.Compare(xObservation.EstimatedValue, yObservation.EstimatedValue);
        }

        public override int Compare(IEnergyObservation x, IEnergyObservation y) =>
            Compare<IEnergyObservation>(x, y);
    }
}
