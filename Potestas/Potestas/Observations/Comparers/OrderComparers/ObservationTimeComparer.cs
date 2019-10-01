using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationTimeComparer : BaseOrderComparer 
    {
        public override int Compare(IEnergyObservation xObservation, IEnergyObservation yObservation)
        {
            return BaseOrderCompare(xObservation, yObservation) ?? 
                   Comparer<DateTime>.Default.Compare(xObservation.ObservationTime, yObservation.ObservationTime);
        }
    }
}
