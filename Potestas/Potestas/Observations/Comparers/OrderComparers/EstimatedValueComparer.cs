using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class EstimatedValueComparer<T> : BaseOrderComparer<T> where T: IEnergyObservation
    {
        public override int Compare(T xObservation, T yObservation)
        {
             return DefaultValueOrderCompare(xObservation, yObservation) ?? 
                    Comparer<double>.Default.Compare(xObservation.EstimatedValue, yObservation.EstimatedValue);
        }
    }
}
