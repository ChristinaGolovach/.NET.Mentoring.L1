using System;
using System.IO;
using Potestas.ApplicationFrame.SourceRegistration;
using Potestas.Processors;
using Potestas.Serializers;

namespace Potestas.ApplicationFrame.ProcessingGroup
{
    class RegisteredSourceProcessingGroup : IProcessingGroup
    {
        private readonly RegisteredEnergyObservationSourceWrapper _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper sourceRegistration,
                                               IProcessingFactory<IEnergyObservation> processingFactory,
                                               IStreamProcessor<IEnergyObservation> streamProcessor = null, string filePath = null,
                                               IEnergyObservationStorage<IEnergyObservation> storage = null, Stream stream = null)
        {
            _sourceRegistration = sourceRegistration;
            Processor = processingFactory.CreateProcessor(streamProcessor, filePath, storage, stream);
            _processorSubscription = _sourceRegistration.Subscribe(Processor);
        }

        public void Detach()
        {
            _processorSubscription.Dispose();
            _sourceRegistration.RemoveProcessingGroup(this);
        }
    }
}
