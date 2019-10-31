using Potestas.Exceptions.ProcessorExceptions;
using System;

namespace Potestas.Processors
{
    public class SaveToStorageProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _storage;

        public string Description => "Saves observations to the provided storage.";

        /// <summary>
        /// Create instance of SaveToStorageProcessor with the given storage.
        /// </summary>
        /// <param name="storage" cref="IEnergyObservationStorage">Storage for the saving observable data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="storage"/> is null.
        /// </exception>
        public SaveToStorageProcessor(IEnergyObservationStorage<T> storage)
        {
            _storage = storage ?? throw new ArgumentNullException($"The {nameof(storage)} can not be null.");
        }

        public void OnCompleted() { }

        public void OnError(Exception error)
        {
            throw new SaveToStorageProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            _storage.Add(value);
        }
    }
}
