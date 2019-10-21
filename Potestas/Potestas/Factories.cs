using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Sources;
using System.IO;

namespace Potestas
{
    /* TASK. Refactor these interfaces to create families of IObserver, IObservable and IObservationsRepository as a single responsibility. 
     * QUESTIONS:
     * Which pattern is used here?
     * Why factory interface is needed here?
     */

    public interface ISourceFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationSource<T> CreateSource();

        IEnergyObservationEventSource<T> CreateEventSource();
    }

    public interface IProcessingFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> CreateSaveToFileProcessor(IStreamProcessor<T> streamProcessor, string filePath);

        IEnergyObservationProcessor<T> CreateSaveToStorageProcessor(IEnergyObservationStorage<T> storage);

        IEnergyObservationProcessor<T> CreateSerializeProcessor(Stream stream);
    }

    public interface IStorageFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationStorage<T> CreateFileStorage(string filePath, ISerializer<T> serializer);

        IEnergyObservationStorage<T> CreateListStorage();
    }

    public interface IStreamProcessingFactory<T> where T : IEnergyObservation
    {
        IStreamProcessor<T> CreateStreamProcessor();
    }

    public interface IAnalizerFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<T> observationStorage);
    }

    public interface ISerializerFactory<T> where T: IEnergyObservation
    {
        ISerializer<T> CreateSerializer();
    }
}
