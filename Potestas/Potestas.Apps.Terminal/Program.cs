﻿using Potestas.Analizers;
using Potestas.API.Plugin.Analizers;
using Potestas.API.Plugin.Services.Implementations;
using Potestas.API.Plugin.Storages;
using Potestas.ApplicationFrame;
using Potestas.ApplicationFrame.ProcessingGroup;
using Potestas.ApplicationFrame.SourceRegistration;
using Potestas.Factories;
using Potestas.Observations;
using Potestas.Serializers;
using Potestas.Sources;
using Potestas.Storages;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly string pathForSeekDLL;
        private static readonly string solutionName = "Potestas";
        private static IProcessingGroup attachedProcessingGroup;

        static Program()
        {
            _app = new ApplicationCoreFrame();
            var curentAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            pathForSeekDLL = curentAssemblyPath.Substring(0, curentAssemblyPath.IndexOf(solutionName) + solutionName.Length + 1);
        }

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;            

            MainMenu();
        }

        private static void MainMenu()
        {
            //------------------------------------------Enumerator Check
            //var enumerator = sqlORM.GetEnumerator();
            //enumerator.MoveNext();
            //var current = enumerator.Current;
            //------------------------------------------

            //------------------------------------------MongoDB Check
            //var connectopn = ConfigurationManager.ConnectionStrings["MongoDBObservationConnection"].ConnectionString;
            //var newItem = new FlashObservation( 11, 23.3, new Coordinates(12, 12), DateTime.Now);

            //var mongoStorage = new MongoDBStorage<IEnergyObservation>(connectopn, "observation", "energyObservations");
            //mongoStorage.Add(newItem);

            //var analizer = new MongoDBAnalizer(mongoStorage.ObservationDBCollection);
            ////mongoStorage.Clear();
            ////mongoStorage.Remove(newItem);
            //Console.WriteLine(analizer.GetMinEnergy(new Coordinates(12, 12)));
            //var countItems = mongoStorage.Count;

            //-------------------------------------------------------

            //--------------------------------------API Plugin Check
            //var httpClient = new HttpClientService();
            //var apiStorage = new APIStorage<IEnergyObservation>(httpClient);
            //var analizer = new APIAnalizer(httpClient);
            //var newItem = new FlashObservation(5989, 11, 23.3, new Coordinates(5976, 12, 12), DateTime.Now);
            ////apiStorage.Remove(newItem);
            //IEnergyObservation[] array = new IEnergyObservation[45];
            //array[0] = newItem;
            //apiStorage.CopyTo(array, 1);

            //var result = analizer.GetAverageEnergy(DateTime.Now.AddDays(-2), DateTime.Now);
            //var result = analizer.GetAverageEnergy(new Coordinates(5979, 45, 19), new Coordinates(5980, 14, 14));
            //var result2 = analizer.GetDistributionByEnergyValue();
            //var result3 = analizer.GetDistributionByObservationTime();
            //var result4 = analizer.GetDistributionByCoordinates();
            //var result5 = analizer.GetMinEnergyPosition();
            //------------------------------------------------------

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
                            attachedProcessingGroup =  _testRegistration.AttachProcessingGroup(plaginProcessingFactory);

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
                Console.WriteLine("2. Potestas.ORM.Plugin.dll");
                Console.WriteLine("3. Potestas.MongoDB.Plugin.dll");
                Console.WriteLine("4. Potestas.API.Plugin.dll");


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
                            LoadPluginDll();
                            break;
                        case ConsoleKey.D2:
                            selectedPluginDllName = "Potestas.ORM.Plugin";
                            LoadPluginDll();
                            break;
                        case ConsoleKey.D3:
                            selectedPluginDllName = "Potestas.MongoDB.Plugin";
                            LoadPluginDll();
                            break;
                        case ConsoleKey.D4:
                            selectedPluginDllName = "Potestas.API.Plugin";
                            LoadPluginDll();
                            break;
                    }

                    pluginIsSelected = true;
                }
            }           
        }

        private static void ShowAnalizerResult(IEnergyObservationAnalizer analizer)
        {
            Console.Clear();
            Console.WriteLine("Select and press 'Enter' button:");
            Console.WriteLine("1 - GetAverageEnergy");
            Console.WriteLine("2 - GetDistributionByCoordinates");
            Console.WriteLine("3 - GetDistributionByEnergyValue");
            Console.WriteLine("4 - GetDistributionByObservationTime");
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
                var userChoice = Console.ReadLine();
                var isParsed = int.TryParse(userChoice, out menuIndex);
                if (isParsed)
                {
                    switch (menuIndex)
                    {
                        case 1:
                            Console.WriteLine(" Average energy is: " + analizer.GetAverageEnergy());
                            break;
                        case 2:
                            Console.WriteLine("Distribution by coordinates is:");
                            analizer.GetDistributionByCoordinates().PrintDistribution("Coordinates");                           
                            break;
                        case 3:
                            Console.WriteLine("Distribution by energy value is:");
                            analizer.GetDistributionByEnergyValue().PrintDistribution("Energy value");
                            break;
                        case 4:
                            Console.WriteLine("Distribution by observation time is:");
                            analizer.GetDistributionByObservationTime().PrintDistribution("Observation time");
                            break;
                        case 5:
                            Console.WriteLine(" Max energy is: " + analizer.GetMaxEnergy());
                            break;
                        case 6:
                            Console.WriteLine(" Max energy position is: " + analizer.GetMaxEnergyPosition());
                            break;
                        case 7:
                            Console.WriteLine(" Max energyTime is: " + analizer.GetMaxEnergyTime());
                            break;
                        case 8:
                            Console.WriteLine(" Min energy is: " + analizer.GetMinEnergy());
                            break;
                        case 9:
                            Console.WriteLine(" Position of min energy is: " + analizer.GetMinEnergyPosition());
                            break;
                        case 10:
                            Console.WriteLine(" Time of min energy is: " + analizer.GetMinEnergyTime());
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

        private static void LoadPluginDll()
        {
            var extensions = new List<string> { ".dll", ".DLL" };

            var pathOfDll = Directory.GetFiles(pathForSeekDLL, "*.*", SearchOption.AllDirectories)
                                     .Where(fileName => extensions.IndexOf(Path.GetExtension(fileName)) >= 0 && 
                                            fileName.EndsWith(selectedPluginDllName + ".dll", StringComparison.OrdinalIgnoreCase))
                                     .FirstOrDefault();

           _app.LoadPlugin(Assembly.LoadFrom(pathOfDll));
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

        private static void PrintDistribution<T>(this IDictionary<T, int> collection, string keyName)
        {
            foreach (var item in collection)
            {
                Console.WriteLine($"{keyName}: {item.Key.ToString()}, count: {item.Value}");
            }
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
            return new RandomEnergySource();
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
