using System;
using Potestas.ApplicationFrame.SourceRegistration;

namespace Potestas.ApplicationFrame.ProcessingGroup
{
    class RegisteredSourceProcessingGroup : IProcessingGroup
    {
        private readonly RegisteredEnergyObservationSourceWrapper _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        public IEnergyObservationStorage<IEnergyObservation> Storage { get; }

        public IEnergyObservationAnalizer Analizer { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper sourceRegistration,
                                               IProcessingFactory<IEnergyObservation> processingFactory)
        {
            _sourceRegistration = sourceRegistration;
            Processor = processingFactory.CreateProcessor();
            Storage = processingFactory.CreateStorage();
            Analizer = processingFactory.CreateAnalizer();

            _processorSubscription = _sourceRegistration.Subscribe(Processor);
        }

        public void Detach()
        {
            _processorSubscription.Dispose();
            _sourceRegistration.RemoveProcessingGroup(this);
        }
    }
}
