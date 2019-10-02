using System;
using System.Collections.Generic;

namespace Potestas.Observations.Comparers.OrderComparers
{
    public class ObservationTimeComparer : BaseOrderComparer 
    {
        public int Compare<T>(T xObservation, T yObservation) where T: IEnergyObservation
        {
            return BaseOrderCompare(xObservation, yObservation) ??
                   Comparer<DateTime>.Default.Compare(xObservation.ObservationTime, yObservation.ObservationTime);
        }

        public override int Compare(IEnergyObservation xObservation, IEnergyObservation yObservation) =>
            Compare<IEnergyObservation>(xObservation, yObservation);
    }
}
