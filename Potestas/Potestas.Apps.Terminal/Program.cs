using Potestas.Analizers;
using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.SourceRegistration;
using Potestas.Factories;
using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Storages;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Potestas.Apps.Terminal
{
    static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;
        private static int startPluginProcessorsIndexInMenu = 3;
        private static string selectedPluginDllName;

        static Program()
        {
            _app = new ApplicationCoreFrame();
        }

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            MainMenu();
        }

        private static void MainMenu()
        {
            LoadPlugin();
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
                        switch (processorKey.Key) //Processors not from plugins
                        {
                            case ConsoleKey.D1:
                                ConsoleProcessingMenu();
                                break;
                            case ConsoleKey.D2:
                                SaveToFileProcessingMenu();
                                break;
                        }
                        if (char.IsNumber(processorKey.KeyChar)) //Processors from plugins
                        {

                            Console.Clear();
                            StartFinishMenu();
                            var consoleProcessingKey = Console.ReadKey();

                            int processorIndexInMenu = int.Parse(processorKey.KeyChar.ToString());
                            var plaginProcessingFactory =  _app.ProcessingFactories.ElementAt(processorIndexInMenu + 1 - startPluginProcessorsIndexInMenu);
                            var plaginSourceFactory = _app.SourceFactories.Where(sourceFactory => sourceFactory.GetType().ToString().StartsWith(selectedPluginDllName)).First();

                            _testRegistration = _app.CreateAndRegisterSource(plaginSourceFactory);
                            var attachedProcessingGroup =  _testRegistration.AttachProcessingGroup(plaginProcessingFactory);

                            _testRegistration.Start().Wait();

                            ShowAnalizerResult(attachedProcessingGroup.Analizer);
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
        }

        private static void LoadPlugin()
        {
            Console.WriteLine("would you like to load a palgin? - Y/N");

            var userChoice = Console.ReadKey().KeyChar.ToString().ToUpper();

            if (userChoice == "Y")
            {
                Console.Clear();
                Console.WriteLine("Please, choise a plugin or go to main menu.");
                Console.WriteLine("0. Go to Main menu.");
                Console.WriteLine("1. Potestas.ADO.Plugin.dll");

                bool pluginIsSelected = false;

                while (!pluginIsSelected)
                {
                    var pluginKey = Console.ReadKey();                   
                    if (pluginKey.Key == ConsoleKey.D0)
                    {
                        break;
                    }
                    switch (pluginKey.Key)
                    {
                        case ConsoleKey.D1:
                            selectedPluginDllName = "Potestas.ADO.Plugin";
                            _app.LoadPlugin(Assembly.LoadFrom(selectedPluginDllName + ".dll"));  
                            //_app.LoadPlugin(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("Potestas.ADO.Pluginl.dll")).First());
                            break;
                    }

                    pluginIsSelected = true;
                }
            }           
        }

        private static void ShowAnalizerResult(IEnergyObservationAnalizer analizer)
        {
            Console.Clear();
            Console.WriteLine("Select:");
            Console.WriteLine("1 - GetAverageEnergy");
            //Console.WriteLine("2 - GetDistributionByCoordinates");
            //Console.WriteLine("3 - GetDistributionByEnergyValue");
            //Console.WriteLine("4 - GetDistributionByObservationTime");
            Console.WriteLine("5 - GetMaxEnergy");
            Console.WriteLine("6 - GetMaxEnergyPosition");
            Console.WriteLine("7 - GetMaxEnergyTime");
            Console.WriteLine("8 - GetMinEnergy");
            Console.WriteLine("9 - GetMinEnergyPosition");
            Console.WriteLine("10 - GetMinEnergyTime");
            Console.WriteLine("something another - exit");

            var exit = false;
            while (!exit)
            {
                int menuIndex = 0;
                var userChoice = Console.ReadKey().Key.ToString();
                if (int.TryParse(userChoice, out menuIndex))
                {
                    switch (menuIndex)
                    {
                        case 1:
                            Console.WriteLine(analizer.GetAverageEnergy());
                            break;
                        //case 2:
                        //    analizer.GetDistributionByCoordinates();
                        //    break;
                        //case 3:
                        //    (analizer.GetDistributionByEnergyValue();
                        //    break;
                        //case 4:
                        //    analizer.GetDistributionByObservationTime();
                        //    break;
                        case 5:
                            Console.WriteLine(analizer.GetMaxEnergy());
                            break;
                        case 6:
                            Console.WriteLine(analizer.GetMaxEnergyPosition());
                            break;
                        case 7:
                            Console.WriteLine(analizer.GetMaxEnergyTime());
                            break;
                        case 8:
                            Console.WriteLine(analizer.GetMinEnergy());
                            break;
                        case 9:
                            Console.WriteLine(analizer.GetMinEnergyPosition());
                            break;
                        case 10:
                            Console.WriteLine(analizer.GetMinEnergyTime());
                            break;
                    }
                }
                else
                {
                    exit = true;
                    Console.Clear();
                    ShowMainMenu();

                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Please select:");
            Console.WriteLine("1: Choise Processor");
            Console.WriteLine("0: Exit");
        }

        private static void ShowProcessorMenu()
        {
            Console.WriteLine("1: Console Processor");
            Console.WriteLine("2: SaveToFileProcessor Processor");
            ShowPluginProcessors();
            Console.WriteLine("0: Go to main menu");
        }

        private static void StartFinishMenu()
        {
            Console.WriteLine("Press any button for start observation process.");
            Console.WriteLine("Press Ctrl+C for finish observation process.");
        }

        private static void ShowPluginProcessors()
        {
            if (_app.ProcessingFactories.Count > 0)
            {                
                foreach(var processinFactory in _app.ProcessingFactories)
                {
                    Console.WriteLine($"{startPluginProcessorsIndexInMenu}: " + processinFactory.GetType());
                    startPluginProcessorsIndexInMenu++;
                }
            }
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
            return new ConsoleProcessor();
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
