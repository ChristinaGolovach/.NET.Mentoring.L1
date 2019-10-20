using Potestas.Processors;

namespace Potestas.Factories.StreamProcessingFactories
{
    public class SerializeProcessorFactory : IStreamProcessingFactory<IEnergyObservation>
    {
        public IStreamProcessor<IEnergyObservation> CreateStreamProcessor()
        {
            return new SerializeProcessor<IEnergyObservation>();
        }
    }
}
