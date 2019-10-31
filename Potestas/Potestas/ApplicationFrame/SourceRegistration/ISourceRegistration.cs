using System.Collections.Generic;
using System.Threading.Tasks;
using Potestas.ApplicationFrame.ProcessingGroup;

namespace Potestas.ApplicationFrame.SourceRegistration
{
    public interface ISourceRegistration
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup AttachProcessingGroup(IProcessingFactory<IEnergyObservation> processingFactory);
    }
}
