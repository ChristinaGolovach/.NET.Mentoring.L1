using Potestas.Analizers;
using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.SourceRegistration;
using Potestas.Factories;
using Potestas.Observations;
using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Storages;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

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
            //_app.LoadPlugin(Assembly.LoadFrom("Potestas.dll")); //for loading plugin module_3 task. now it anly check that it works

            MainMenu();
        }

        private static void MainMenu()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString;


            var sqlStorage = new SqlStorage<IEnergyObservation>(connectionString);
            sqlStorage.Add(new FlashObservation(11, 12, new Coordinates(11,22)));
            var testArray = new IEnergyObservation[100];
            sqlStorage.CopyTo(testArray, 2);


            var sqlAnalizer = new SqlAnalizer(connectionString);
            var average = sqlAnalizer.GetAverageEnergy();
            var distribution = sqlAnalizer.GetDistributionByCoordinates();

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

            _testRegistration = _app.CreateAndRegisterSource(new ObservationSourceFactory());
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

            _testRegistration = _app.CreateAndRegisterSource(new ObservationSourceFactory());
            _testRegistration.AttachProcessingGroup(new SaveToFileProcessingFactory(filePath, new JsonSerializer<IEnergyObservation>()));
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
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new LINQAnalizer(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToSqlProcessor<IEnergyObservation>(ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString);//ConsoleProcessor();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
            {
                _storage = new ListStorage();
            }

            return _storage;
        }
    }

    #endregion console factories
}
