using System.Collections.Generic;
using System.Reflection;
using Potestas.ApplicationFrame.SourceRegistration;

namespace Potestas.ApplicationFrame
{
    public interface IEnergyObservationApplicationModel
    {
        IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration CreateAndRegisterSource(ISourceFactory<IEnergyObservation> factory);
    }
}
