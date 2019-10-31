using System.IO;

namespace Potestas.Processors
{
    public interface IStreamProcessor<in T> : IEnergyObservationProcessor<T> where T: IEnergyObservation
    {
        void OnNext(Stream stream, T value);
    }
}
