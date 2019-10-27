using Potestas.Sources;

namespace Potestas.Factories
{
    public class ObservationSourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            return new RandomEnergySource();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource()
        {
            return new RandomEnergySource();
        }
    }
}
