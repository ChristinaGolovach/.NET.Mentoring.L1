using System;
using System.IO;
using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Sources;
using Potestas.Storages;
using Potestas.Serializers;

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
            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory());
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

    class ConsoleProcessingFactory : IProcessingFactory
    {
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new LINQAnalizer();
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            //return new ConsoleProcessor();

            return new SaveToFileProcessor<IEnergyObservation>(new SerializeProcessor<IEnergyObservation>(), @"D:\task4.txt"); //for testing task 4 (SaveToFileProcessor + SerializeProcessor)

            //return new SaveToStorageProcessor<IEnergyObservation>(CreateStorage()); // for testing task 4 (SaveToStorageProcessor) + 5 (FileStorage)
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return new ListStorage();

            //return new FileStorage<IEnergyObservation>(@"D:\task5.txt", new JsonSerializer<IEnergyObservation>()); // for testing task 5
        }
    }
}
