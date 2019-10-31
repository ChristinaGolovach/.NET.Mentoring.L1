using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Potestas.Extensions;
using Potestas.Attributes;

namespace Potestas
{
    /* TASK. Implement method Load to load factories interfaces from assembly provided.
     * 1. Consider some classes could be private.
     * 2. Consider using special attribute to exclude some factories from creation.
     * 3. Consider refactoring of factory interfaces.
     * 4. Consider making an extension for Assembly class.
     */

    class FactoriesLoader
    {
        public (ISourceFactory<IEnergyObservation>[], IProcessingFactory<IEnergyObservation>[]) Load(Assembly assembly)
        {
            assembly = assembly ?? throw new ArgumentNullException($"The {nameof(assembly)} can not be null.");

            var sourceFactoryInstances = new List<ISourceFactory<IEnergyObservation>>();
            var processingFactoryInstances = new List<IProcessingFactory<IEnergyObservation>>();

            var allowedTypes = assembly.GetTypes(t => IsAllowedType(t));

            var sourceFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                              .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISourceFactory<>)).ToList().Count > 0).ToList();

            var processingFactoryTypes = allowedTypes.Where(t => t.GetInterfaces()
                                                                  .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProcessingFactory<>)).ToList().Count > 0).ToList();

            sourceFactoryTypes.ForEach(t => sourceFactoryInstances.Add((ISourceFactory<IEnergyObservation>)Activator.CreateInstance(t)));
            processingFactoryTypes.ForEach(t => processingFactoryInstances.Add((IProcessingFactory<IEnergyObservation>)Activator.CreateInstance(t)));


            return (sourceFactoryInstances.ToArray(), processingFactoryInstances.ToArray());
        }

        private bool IsAllowedType(Type type)
        {
            return type.IsClass && type.IsPublic && !type.IsAbstract && !type.GetCustomAttributes(typeof(ExcludeFromFactoryLoaderAttribute)).Any();
        }
    }
}
