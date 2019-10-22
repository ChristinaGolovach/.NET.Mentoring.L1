using Potestas.Analizers;

namespace Potestas.Factories.second_attempt.AnalizerFactories
{
    public class LINQAnalizerFactory : IAnalizerFactory<IEnergyObservation>
    {
        public IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage)
        {
            return new LINQAnalizer(observationStorage);
        }
    }
}
