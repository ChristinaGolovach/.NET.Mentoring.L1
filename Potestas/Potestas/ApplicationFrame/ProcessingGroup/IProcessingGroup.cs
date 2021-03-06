﻿namespace Potestas.ApplicationFrame.ProcessingGroup
{
    public interface IProcessingGroup
    {
        IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        IEnergyObservationStorage<IEnergyObservation> Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }
}
