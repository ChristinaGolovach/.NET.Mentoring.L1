using Potestas.Sources;

namespace Potestas.Factories.second_attempt
{
    public class RandomEnergySourceFactory : ISourceFactory<IEnergyObservation>
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
