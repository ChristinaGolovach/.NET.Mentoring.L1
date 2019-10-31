using System.Collections.Generic;
using System.Reflection;
using Potestas.ApplicationFrame.SourceRegistration;

namespace Potestas.ApplicationFrame
{
    public sealed class ApplicationCoreFrame : IEnergyObservationApplicationModel
    {
        private readonly static FactoriesLoader _factoriesLoader = new FactoriesLoader();

        private readonly List<ISourceFactory<IEnergyObservation>> _sourceFactories;
        private readonly List<IProcessingFactory<IEnergyObservation>> _processingFactories;
        private readonly List<RegisteredEnergyObservationSourceWrapper> _registeredSources;

        public IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories => _sourceFactories.AsReadOnly();
        public IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories => _processingFactories.AsReadOnly();
        public IReadOnlyCollection<ISourceRegistration> RegisteredSources => _registeredSources.AsReadOnly();

        public ApplicationCoreFrame()
        {
            _registeredSources = new List<RegisteredEnergyObservationSourceWrapper>();
            _processingFactories = new List<IProcessingFactory<IEnergyObservation>>();
            _sourceFactories = new List<ISourceFactory<IEnergyObservation>>();
        }

        public void LoadPlugin(Assembly assembly)
        {
            var (sourceFactories, processingFactories) = _factoriesLoader.Load(assembly);
            _processingFactories.AddRange(processingFactories);
            _sourceFactories.AddRange(sourceFactories);
        }

        public ISourceRegistration CreateAndRegisterSource(ISourceFactory<IEnergyObservation> factory)
        {
            var source = factory.CreateSource();
            var registration = new RegisteredEnergyObservationSourceWrapper(this, source);
            _registeredSources.Add(registration);
            return registration;
        }

        internal void RemoveRegistration(RegisteredEnergyObservationSourceWrapper registration)
        {
            _registeredSources.Remove(registration);
        }
    }
}
