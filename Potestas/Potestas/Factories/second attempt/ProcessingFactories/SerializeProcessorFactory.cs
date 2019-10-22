using Potestas.Processors;
using System.IO;

namespace Potestas.Factories.second_attempt.ProcessingFactories
{
    public class SerializeProcessorFactory : IProcessingFactory<IEnergyObservation>
    {
        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor(IStreamProcessor<IEnergyObservation> streamProcessor = null, string filePath = null, 
                                                                               IEnergyObservationStorage<IEnergyObservation> storage = null, Stream stream = null)
        {
            return new SerializeProcessor<IEnergyObservation>(stream);
        }
    }
}
