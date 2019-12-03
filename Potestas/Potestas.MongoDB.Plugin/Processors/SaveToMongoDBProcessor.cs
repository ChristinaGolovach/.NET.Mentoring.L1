using Potestas.MongoDB.Plugin.Exceptions;
using Potestas.Validators;
using System;

namespace Potestas.MongoDB.Plugin.Processors
{
    public class SaveToMongoDBProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _dbStorage;

        public string Description => "Saves observations to the provided MongoDB.";

        public SaveToMongoDBProcessor(IEnergyObservationStorage<T> dbStorage)
        {
            _dbStorage = dbStorage ?? throw new ArgumentNullException($"The {nameof(dbStorage)} can not be null.");
        }

        public void OnCompleted()
        {
            //TODO add info in loger
        }

        public void OnError(Exception error)
        {
            throw new MongoDBProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            GenericValidator.CheckInitialization(value, nameof(value));

            _dbStorage.Add(value);
        }
    }
}
