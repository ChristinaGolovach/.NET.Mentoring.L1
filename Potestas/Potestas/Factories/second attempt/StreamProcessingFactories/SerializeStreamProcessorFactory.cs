using Potestas.Processors;

namespace Potestas.Factories.second_attempt.SerializeStreamProcessorFactory
{
    public class SerializeStreamProcessorFactory : IStreamProcessingFactory<IEnergyObservation>
    {
        public IStreamProcessor<IEnergyObservation> CreateStreamProcessor()
        {
            return new SerializeProcessor<IEnergyObservation>();
        }
    }
}
