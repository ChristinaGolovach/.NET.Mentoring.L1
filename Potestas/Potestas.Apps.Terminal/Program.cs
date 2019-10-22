using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.SourceRegistration;
using Potestas.Factories.second_attempt;
using Potestas.Factories.second_attempt.ProcessingFactories;
using Potestas.Factories.second_attempt.SerializeStreamProcessorFactory;
using Potestas.Processors;
using System;
using System.IO;

namespace Potestas.Apps.Terminal
{
    static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;

        static Program()
        {
            _app = new ApplicationCoreFrame();
        }

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            // _app.LoadPlugin(Assembly.LoadFrom("Potestas.dll")); //for loading plugin module_3 task. now it anly check that it works

            // for testing that it also work 
            //_testRegistration.AttachProcessingGroup(new SaveToStorageProcessorFactory(), null, null, storageFactory.CreateStorage(@"D:\test.txt", new JsonSerializer<IEnergyObservation>()));
            //_testRegistration.Start().Wait();

            MainMenu();
        }

        private static void MainMenu()
        {
            ShowMainMenu();

            var key = Console.ReadKey();

            while (key.Key != ConsoleKey.D0)
            {
                if (key.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    ShowProcessorMenu();
                    do
                    {
                        var processorKey = Console.ReadKey();
                        if (processorKey.Key == ConsoleKey.D0)
                        {
                            break;
                        }
                        switch (processorKey.Key)
                        {
                            case ConsoleKey.D1:
                                ConsoleProcessingMenu();
                                break;
                            case ConsoleKey.D2:
                                SaveToFileProcessingMenu();
                                break;
                        }

                    } while (true);
                }
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _testRegistration.Stop();
            Console.Clear();
            MainMenu();
        }

        private static void ShowMainMenu()
        {
            Console.WriteLine("Please select:");
            Console.WriteLine("1: Choise Processor");
            Console.WriteLine("0: Exit");
        }

        private static void ShowProcessorMenu()
        {
            Console.WriteLine("1: Console Processor");
            Console.WriteLine("2: SaveToFileProcessor Processor");
            Console.WriteLine("0: Go to main menu");
        }

        private static void StartFinishMenu()
        {
            Console.WriteLine("Press any button for start observation process.");
            Console.WriteLine("Press Ctrl+C for finish observation process.");
        }

        private static void ConsoleProcessingMenu()
        {
            Console.Clear();
            StartFinishMenu();
            var consoleProcessingKey = Console.ReadKey();

            _testRegistration = _app.CreateAndRegisterSource(new RandomEnergySourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory());
            _testRegistration.Start().Wait();
        }

        private static void SaveToFileProcessingMenu()
        {
            string filePath = null;

            while (string.IsNullOrEmpty(filePath))
            {
                Console.Clear();
                Console.WriteLine("Please enter file path. For example -  D:\\test.txt");
                filePath = Console.ReadLine();
            }

            StartFinishMenu();
            var saveToFileProcessingKey = Console.ReadKey();

            _testRegistration = _app.CreateAndRegisterSource(new RandomEnergySourceFactory());
            var streamProcessor = new SerializeStreamProcessorFactory().CreateStreamProcessor(); // for demo only hardcoded
            _testRegistration.AttachProcessingGroup(new SaveToFileProcessorFactory(), streamProcessor, filePath);
            _testRegistration.Start().Wait();
        }
    }

    #region console factories
    class ConsoleSourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource( )
        {
            return new ConsoleSource();
        }
    }

    class ConsoleProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor(IStreamProcessor<IEnergyObservation> streamProcessor = null, string filePath = null, 
                                                                               IEnergyObservationStorage<IEnergyObservation> storage = null, Stream stream = null)
        {
            return new ConsoleProcessor();
        }
    }

    #endregion console factories
}
