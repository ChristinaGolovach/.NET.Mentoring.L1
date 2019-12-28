using Potestas.Utils;
using System;

namespace Potestas.Processors
{
    public class ProcessorLoggerDecorator<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationProcessor<T> _processor;
        private readonly ILoggerManager _loggerManager;

        public ProcessorLoggerDecorator(IEnergyObservationProcessor<T> processor, ILoggerManager loggerManager)
        {
            _processor = processor ?? throw new ArgumentNullException();
            _loggerManager = loggerManager ?? throw new ArgumentNullException();
        }

        public string Description => _processor.Description;

        public void OnCompleted() => Helper.Run(() => _processor.OnCompleted(), _loggerManager, string.Intern("OnCompleted"));

        public void OnError(Exception error) => Helper.Run(() => _processor.OnError(error), _loggerManager, string.Intern("OnError"));

        public void OnNext(T value) => Helper.Run(() => _processor.OnNext(value), _loggerManager, $"OnNext with value {value}");
    }
}
