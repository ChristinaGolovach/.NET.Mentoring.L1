using System;
using Potestas.Sources;

namespace Potestas.Factories
{
    public class RandomEnergySourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            //return new RandomEnergySource();
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource()
        {
            return new RandomEnergySource();
        }
    }
}
