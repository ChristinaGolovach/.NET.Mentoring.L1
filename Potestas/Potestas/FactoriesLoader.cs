using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Potestas
{
    /* TASK. Implement method Load to load factories interfaces from assembly provided.
     * 1. Consider some classes could be private.
     * 2. Consider using special attribute to exclude some factories from creation.
     * 3. Consider refactoring of factory interfaces.
     * 4. Consider making an extension for Assembly class.
     */
    // class FactoriesLoader
    //{
    //    public (ISourceFactory<IEnergyObservation>[],
    //            IProcessingFactory<IEnergyObservation>[],
    //            IStorageFactory<IEnergyObservation>[],
    //            IAnalizerFactory<IEnergyObservation>[],
    //            ISerializerFactory<IEnergyObservation>[],
    //            IStreamProcessingFactory<IEnergyObservation>[]) Load(Assembly assembly)
    //    {

    //    }

    //}

    #region when task 7 was implemented in first attempt
    class FactoriesLoader
    {
        public (ISourceFactory<IEnergyObservation>[],
                IProcessingFactory<IEnergyObservation>[],
                IStorageFactory<IEnergyObservation>[],
                IAnalizerFactory<IEnergyObservation>[],
                ISerializerFactory<IEnergyObservation>[],
                IStreamProcessingFactory<IEnergyObservation>[]) Load(Assembly assembly)
        {
            assembly = assembly ?? throw new ArgumentNullException($"The {nameof(assembly)} can not be null.");

            var sourceFactoryInstances = new List<ISourceFactory<IEnergyObservation>>();
            var processingFactoryInstances = new List<IProcessingFactory<IEnergyObservation>>();
            var storageFactoryInstances = new List<IStorageFactory<IEnergyObservation>>();
            var analizerFactoryInstances = new List<IAnalizerFactory<IEnergyObservation>>();
            var serializerFactoryInstances = new List<ISerializerFactory<IEnergyObservation>>();
            var streamProcessingFactoryInstances = new List<IStreamProcessingFactory<IEnergyObservation>>();

            var allowedTypes = assembly.GetTypes().Where(t => IsAllowedType(t)).ToList();

            var sourceFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                              .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISourceFactory<>)).ToList().Count > 0).ToList();

            var processingFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                                  .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProcessingFactory<>)).ToList().Count > 0).ToList();

            var storageFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                               .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStorageFactory<>)).ToList().Count > 0).ToList();

            var analizerFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAnalizerFactory<>)).ToList().Count > 0).ToList();

            var serializerFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                                  .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISerializerFactory<>)).ToList().Count > 0).ToList();

            var streamProcessingFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStreamProcessingFactory<>)).ToList().Count > 0).ToList();

            sourceFactoryTypes.ForEach(t => sourceFactoryInstances.Add((ISourceFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            processingFactoryTypes.ForEach(t => processingFactoryInstances.Add((IProcessingFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            storageFactoryTypes.ForEach(t => storageFactoryInstances.Add((IStorageFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            analizerFactoryTypes.ForEach(t => analizerFactoryInstances.Add((IAnalizerFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            serializerFactoryTypes.ForEach(t => serializerFactoryInstances.Add((ISerializerFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            streamProcessingFactoryTypes.ForEach(t => streamProcessingFactoryInstances.Add((IStreamProcessingFactory<IEnergyObservation>)Activator.CreateInstance(t)));

            return (sourceFactoryInstances.ToArray(), processingFactoryInstances.ToArray(),
                    storageFactoryInstances.ToArray(), analizerFactoryInstances.ToArray(),
                    serializerFactoryInstances.ToArray(), streamProcessingFactoryInstances.ToArray());
        }

        private bool IsAllowedType(Type type)
        {
            return type.IsClass && type.IsPublic && !type.IsAbstract;
        }
    }
    #endregion when task 7 was implemented in first attempt
}
