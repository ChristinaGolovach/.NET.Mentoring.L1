using System;
using System.IO;
using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Sources;
using Potestas.Storages;
using Potestas.Serializers;
using Potestas.Factories;
using Potestas.Factories.AnalizerFactories;
using System.Reflection;

namespace Potestas.Apps.Terminal
{
    static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;

        static Program()
        {
            _app = new ApplicationFrame();
        }

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            //_app.LoadPlugin(Assembly.Load("Potestas.dll"));
            _app.LoadPlugin(Assembly.LoadFrom("Potestas.dll"));
            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory(), new StorageFactory(), new LINQAnalizerFactory());
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }
    }

    class ConsoleSourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource()
        {
            //return new ConsoleSource();
            return new RandomEnergySource(); // for the testing of task 3 
        }
    }

    class ConsoleProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToFileProcessor(IStreamProcessor<IEnergyObservation> streamProcessor, string filePath)
        {
            return new SaveToFileProcessor<IEnergyObservation>(new SerializeProcessor<IEnergyObservation>(), @"D:\task4.txt");
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateSaveToStorageProcessor(IEnergyObservationStorage<IEnergyObservation> storage)
        {
            return new SaveToStorageProcessor<IEnergyObservation>(new FileStorage<IEnergyObservation>(@"D:\task5.txt", new JsonSerializer<IEnergyObservation>()));
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateSerializeProcessor(Stream stream)
        {
            return new SerializeProcessor<IEnergyObservation>(stream);
        }
    }
}
