using System;

namespace Potestas.Processors
{
    /* TASK. Implement Processor which saves IEnergyObservation to the provided file.
     * 1. Try to decorate SerializeProcessor.
     * QUESTIONS:
     * Which bonuses does decoration have?
     * TEST: Which kind of tests should be written for this class?
     */
    public class SaveToFileProcessor<T> : IEnergyObservationProcessor<T> where T: IEnergyObservation
    {
        public string Description => throw new NotImplementedException();

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }
    }
}
