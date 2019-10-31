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
        IEnergyObservationProcessor<T> CreateProcessor();

        IEnergyObservationStorage<T> CreateStorage();

        IEnergyObservationAnalizer CreateAnalizer();
    }
}