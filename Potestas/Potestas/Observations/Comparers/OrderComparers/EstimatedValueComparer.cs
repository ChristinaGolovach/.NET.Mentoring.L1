using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class EstimatedValueComparer : BaseOrderComparer
    {
        public override int Compare(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
             return BaseOrderCompare(xObservation, yObservation) ?? 
                    Comparer<double>.Default.Compare(xObservation.EstimatedValue, yObservation.EstimatedValue);
        }
    }
}
