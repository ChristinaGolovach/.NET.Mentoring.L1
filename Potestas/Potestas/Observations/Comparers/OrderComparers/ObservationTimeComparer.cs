using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationTimeComparer<T> : BaseOrderComparer<T> where T : IEnergyObservation
    {
        public override int Compare(T xObservation, T yObservation)
        {
            return BaseOrderCompare(xObservation, yObservation) ??
                   Comparer<DateTime>.Default.Compare(xObservation.ObservationTime, yObservation.ObservationTime);
        }
    }
}
