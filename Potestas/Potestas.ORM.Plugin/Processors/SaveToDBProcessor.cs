﻿using System;
using Potestas.ORM.Plugin.Exceptions;
using Potestas.Validators;

namespace Potestas.ORM.Plugin.Processors
{
    public class SaveToDBProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _dbStorage;

        public string Description => "Saves observations to the provided DB using EF Core.";

        public SaveToDBProcessor(IEnergyObservationStorage<T> dbStorage)
        {
            _dbStorage = dbStorage ?? throw new ArgumentNullException($"The {nameof(dbStorage)} can not be null.");
        }

        public void OnCompleted()
        {
            //TODO add info in loger
        }

        public void OnError(Exception error)
        {
            throw new DBProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            GenericValidator.CheckInitialization(value, nameof(value));

            _dbStorage.Add(value);
        }
    }
}
