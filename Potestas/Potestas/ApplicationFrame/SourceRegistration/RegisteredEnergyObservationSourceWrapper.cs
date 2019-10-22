using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Potestas.ApplicationFrame.ProcessingGroup;
using Potestas.Processors;

namespace Potestas.ApplicationFrame.SourceRegistration
{
    class RegisteredEnergyObservationSourceWrapper : ISourceRegistration, IEnergyObservationProcessor<IEnergyObservation>
    {
        private readonly ApplicationCoreFrame _app;
        private readonly IEnergyObservationSource<IEnergyObservation> _inner;
        private readonly IDisposable _internalSubscription;
        private readonly List<IProcessingGroup> _processingGroups;
        private CancellationTokenSource _cts;

        public RegisteredEnergyObservationSourceWrapper(ApplicationCoreFrame app, IEnergyObservationSource<IEnergyObservation> inner)
        {
            _app = app;
            _inner = inner;
            _processingGroups = new List<IProcessingGroup>();
            Subscribe(this);
        }

        public SourceStatus Status { get; private set; }

        public IReadOnlyCollection<IProcessingGroup> ProcessingUnits => _processingGroups.AsReadOnly();

        public string Description => "Internal application listener to track Sources State";

        internal IDisposable Subscribe(IEnergyObservationProcessor<IEnergyObservation> processor)
        {
            return _inner.Subscribe(processor);
        }

        public IProcessingGroup AttachProcessingGroup(IProcessingFactory<IEnergyObservation> processingFactory, IStreamProcessor<IEnergyObservation> streamProcessor = null, 
                                                      string filePath = null, IEnergyObservationStorage<IEnergyObservation> storage = null, Stream stream = null)
        {
            var processingGroup = new RegisteredSourceProcessingGroup(this, processingFactory, streamProcessor, filePath, storage, stream);
            _processingGroups.Add(processingGroup);
            return processingGroup;
        }

        internal void RemoveProcessingGroup(IProcessingGroup processingGroup)
        {
            _processingGroups.Remove(processingGroup);
        }

        public void OnCompleted() => Status = SourceStatus.Completed;

        public void OnError(Exception error) => Status = SourceStatus.Failed;

        public void OnNext(IEnergyObservation value) => Status = SourceStatus.Running;

        public Task Start()
        {
            // TODO: add SemaphoreSlim to prevent multiple runs
            _cts = new CancellationTokenSource();
            return _inner.Run(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void Unregister()
        {
            _internalSubscription.Dispose();
            _app.RemoveRegistration(this);
        }


    }
}
