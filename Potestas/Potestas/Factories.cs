using Potestas.Processors;
using Potestas.Serializers;
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

    // пыталась передавать список параметров в ctor для каждого реализации фабрики, но тогда невозможно будет
    // создать экземпляры этих фабрик FactoriesLoader.cs классе, т.к. нужно заранее передать эти параметры.
    // FactoriesLoader яв-ся всего лишь грузчиком этих фабрик и мы не можем знать какие аргументы будут предоставлены кодом клиента.
    // Поэтому в методах создания определенного инстанса класса обозначен список необязательных параметров, который обозначает какие 
    // параметры МОГУТ потребоваться. НО это очень очень очень ПЛОХО!!! так как клиенту данной библиотки не ясна картина до конца
    // какие параметры для конкретной реализации фабрики ОБЯЗАТЕЛЬНЫ. 
    // ---ПРИМЕР---: IProcessingFactory имплементируется тримя классами SaveToFileProcessorFactory, SaveToStorageProcessorFactory и SerializeProcessorFactory
    // и для создания объекта класса SaveToFileProcessor(декоратор для SerializeProcessor) фабрике требуется аргументы: streamProcessor, string filePath,
    // а для создания объекта класса SerializeProcessor фабрике требутеся значение для параметра stream
    // и как в таком случае клиенту нашей dll угадать передавать stream или нет, а может нужно имя файла только передать для SaveToFileProcessor.
    // -------------
    // Если создавать какой-то файл конфигурации, опять же, как понять клиенту какие значния нужно сконфигурировать.
    // Хардкодить значение пути файла, или какой сериалайзер использовать тоже как-то так себе.
    // Немного не понимаю КАК можно создавать объекты класов через фабрику, 
    // если список параметров для их ctor-ов НЕ унифицирован.
    //------------------------------------------------------------------------------------------------------------------------------------------
    // и другой вопрос нужно ли было разбивать  изначальный интерфейс фабрики??
    //
    //    public interface IProcessingFactory
    // {
    //    IEnergyObservationProcessor CreateProcessor(); //SaveToFile, SaveToStorage, SerializeProcessor
    //    IEnergyObservationStorage CreateStorage(); // File or List
    //    IEnergyObservationAnalizer CreateAnalizer();
    // }
    // в задании сказано: Refactor these interfaces to create families of IObserver, IObservable and IObservationsRepository as a single responsibility.
    // в тоже время в файле описании проекта  "Welcome to the mentoring program.docx"
    // везде описывается что это как unit processing, т.е состоит из трех частей, 
    // НО у нас ситуация что НЕ для всех процессоров-наблюдателей нужен какой-то storage
    // (прим. SaveToFileProcessor - по заданию требует сразу в файл сохранять или SerializeProcessor вообще со Stream работает)


    #region second attempt 

    public interface IProcessingFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> CreateProcessor(IStreamProcessor<T> streamProcessor = null, string filePath = null, 
                                                       IEnergyObservationStorage<T> storage = null, Stream stream = null);
    }

    public interface IStorageFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationStorage<T> CreateStorage(string filePath = null, ISerializer<IEnergyObservation> serializer = null);
    }

    public interface IAnalizerFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage);
    }

    public interface ISerializerFactory<T> where T : IEnergyObservation
    {
        ISerializer<T> CreateSerializer();
    }

    public interface IStreamProcessingFactory<T> where T : IEnergyObservation
    {
        IStreamProcessor<T> CreateStreamProcessor();
    }
    #endregion second attempt

    #region first attempt 
    // all work but I suppouse it not correct way 

    //public interface ISourceFactory<T> where T : IEnergyObservation
    //{
    //    IEnergyObservationSource<T> CreateSource();

    //    IEnergyObservationEventSource<T> CreateEventSource();
    //}

    //public interface IProcessingFactory<T> where T : IEnergyObservation
    //{
    //    IEnergyObservationProcessor<T> CreateSaveToFileProcessor(IStreamProcessor<T> streamProcessor, string filePath);

    //    IEnergyObservationProcessor<T> CreateSaveToStorageProcessor(IEnergyObservationStorage<T> storage);

    //    IEnergyObservationProcessor<T> CreateSerializeProcessor(Stream stream);
    //}

    //public interface IStorageFactory<T> where T : IEnergyObservation
    //{
    //    IEnergyObservationStorage<T> CreateFileStorage(string filePath, ISerializer<T> serializer);

    //    IEnergyObservationStorage<T> CreateListStorage();
    //}

    //public interface IStreamProcessingFactory<T> where T : IEnergyObservation
    //{
    //    IStreamProcessor<T> CreateStreamProcessor();
    //}

    //public interface IAnalizerFactory<T> where T : IEnergyObservation
    //{
    //    IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<T> observationStorage);
    //}

    //public interface ISerializerFactory<T> where T: IEnergyObservation
    //{
    //    ISerializer<T> CreateSerializer();
    //}
    #endregion first attempt
}
