using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Potestas.ApplicationFrame.ProcessingGroup;
using Potestas.Processors;

namespace Potestas.ApplicationFrame.SourceRegistration
{
    public interface ISourceRegistration
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup AttachProcessingGroup(IProcessingFactory<IEnergyObservation> processingFactory, 
                                               IStreamProcessor<IEnergyObservation> streamProcessor = null, string filePath = null,
                                               IEnergyObservationStorage<IEnergyObservation> storage = null, Stream stream = null);
    }
}
