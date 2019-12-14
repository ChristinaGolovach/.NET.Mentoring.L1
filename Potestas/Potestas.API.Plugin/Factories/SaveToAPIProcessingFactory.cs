﻿using Potestas.API.Plugin.Analizers;
using Potestas.API.Plugin.Storages;
using Potestas.Processors;
using System;

namespace Potestas.API.Plugin.Factories
{
    public class SaveToAPIProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer CreateAnalizer() 
            => new APIAnalizer();


        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
            => new SaveToStorageProcessor<IEnergyObservation>(GetStorage());


        public IEnergyObservationStorage<IEnergyObservation> CreateStorage() 
            => GetStorage();


        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if (_storage == null)
            {
                _storage = new APIStorage<IEnergyObservation>();
            }

            return _storage;
        }
    }

    //public interface IEnergyObservationOpen : IEnergyObservation
    //{
    //    int Id { get; set; }

    //    Coordinates ObservationPoint { get; set; }

    //    double EstimatedValue { get; set; }

    //    DateTime ObservationTime { get; set; }
    //}
}
