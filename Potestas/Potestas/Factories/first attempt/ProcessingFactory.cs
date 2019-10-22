//using System.IO;
//using Potestas.Processors;

//namespace Potestas.Factories.ProcessorFactories
//{
//    public class ProcessingFactory : IProcessingFactory<IEnergyObservation>
//    {
//        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToFileProcessor(IStreamProcessor<IEnergyObservation> streamProcessor, string filePath)
//        {
//            return new SaveToFileProcessor<IEnergyObservation>(streamProcessor, filePath);
//        }

//        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToStorageProcessor(IEnergyObservationStorage<IEnergyObservation> storage)
//        {
//            return new SaveToStorageProcessor<IEnergyObservation>(storage);
//        }

//        public IEnergyObservationProcessor<IEnergyObservation> CreateSerializeProcessor(Stream stream)
//        {
//            return new SerializeProcessor<IEnergyObservation>(stream);
//        }
//    }
//}
