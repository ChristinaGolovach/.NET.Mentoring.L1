﻿using Potestas.Serializers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas
{
    /* TASK. Try to refactor this code and add proper exception handling
     * 
     */
    public enum SourceStatus
    {
        Pending,
        Running,
        Completed,
        Failed
    }

    public interface ISourceRegistration
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup AttachProcessingGroup(IProcessingFactory<IEnergyObservation> processingFactory,
                                               IStorageFactory<IEnergyObservation> storageFactory,
                                               IAnalizerFactory<IEnergyObservation> analizerFactory);
    }

    public interface IProcessingGroup
    {
        IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        IEnergyObservationStorage<IEnergyObservation> Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }

    public interface IEnergyObservationApplicationModel
    {
        IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration CreateAndRegisterSource(ISourceFactory<IEnergyObservation> factory);
    }

    class RegisteredSourceProcessingGroup : IProcessingGroup
    {
        private readonly RegisteredEnergyObservationSourceWrapper _sourceRegistration;
        private readonly IDisposable _processorSubscription;

        public IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        public IEnergyObservationStorage<IEnergyObservation> Storage { get; }

        public IEnergyObservationAnalizer Analizer { get; }

        public RegisteredSourceProcessingGroup(RegisteredEnergyObservationSourceWrapper sourceRegistration, 
                                               IProcessingFactory<IEnergyObservation> processingFactory,
                                               IStorageFactory<IEnergyObservation> storageFactory,
                                               IAnalizerFactory<IEnergyObservation> analizerFactory)
        {
            _sourceRegistration = sourceRegistration;
            Storage = storageFactory.CreateFileStorage(@"D:\task5.txt", new JsonSerializer<IEnergyObservation>());
            Processor = processingFactory.CreateSaveToStorageProcessor(Storage);            
            Analizer = analizerFactory.CreateAnalizer(Storage);

            _processorSubscription = _sourceRegistration.Subscribe(Processor);
        }

        public void Detach()
        {
            _processorSubscription.Dispose();
            _sourceRegistration.RemoveProcessingGroup(this);
        }
    }

    class RegisteredEnergyObservationSourceWrapper : ISourceRegistration, IEnergyObservationProcessor<IEnergyObservation>
    {
        private readonly ApplicationFrame _app;
        private readonly IEnergyObservationSource<IEnergyObservation> _inner;
        private readonly IDisposable _internalSubscription;
        private readonly List<IProcessingGroup> _processingGroups;
        private CancellationTokenSource _cts;

        public RegisteredEnergyObservationSourceWrapper(ApplicationFrame app, IEnergyObservationSource<IEnergyObservation> inner)
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

        public IProcessingGroup AttachProcessingGroup(IProcessingFactory<IEnergyObservation> processingFactory,
                                                      IStorageFactory<IEnergyObservation> storageFactory,
                                                      IAnalizerFactory<IEnergyObservation> analizerFactory)
        {
            var processingGroup = new RegisteredSourceProcessingGroup(this, processingFactory, storageFactory, analizerFactory);
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

    public sealed class ApplicationFrame : IEnergyObservationApplicationModel
    {
        private readonly static FactoriesLoader _factoriesLoader = new FactoriesLoader();

        private readonly List<ISourceFactory<IEnergyObservation>> _sourceFactories;
        private readonly List<IProcessingFactory<IEnergyObservation>> _processingFactories;
        private readonly List<RegisteredEnergyObservationSourceWrapper> _registeredSources;

        public IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories => _sourceFactories.AsReadOnly();
        public IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories => _processingFactories.AsReadOnly();
        public IReadOnlyCollection<ISourceRegistration> RegisteredSources => _registeredSources.AsReadOnly();

        public ApplicationFrame()
        {
            _registeredSources = new List<RegisteredEnergyObservationSourceWrapper>();
            _processingFactories = new List<IProcessingFactory<IEnergyObservation>>();
            _sourceFactories = new List<ISourceFactory<IEnergyObservation>>();
        }

        public void LoadPlugin(Assembly assembly)
        {
            var (sourceFactories, processingFactories, storageFactories, analizerFactories, serializerFactories, streamProcessingFactories) = _factoriesLoader.Load(assembly);
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
