﻿using Potestas.Observations;
using Potestas.Sources.ObservationCreators;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Sources
{
    /* TASK. Implement random observation source.
     * 1. This class should generate observations by random periods of time.
     * 1. Implement both IEnergyObservationSource and IEnergyObservationSourceEventSource interfaces.
     * 2. Try to implement it with abstract class or delegate parameters to make it universal.
     */
    // TODO ask refactoring RandomEnergySource (ConsoleSource has the similar implementation. template method?)  
    public class RandomEnergySource : IEnergyObservationSource<IEnergyObservation>, IEnergyObservationEventSource<IEnergyObservation>
    {
        private readonly List<IObserver<IEnergyObservation>> _observers;

        public event EventHandler<IEnergyObservation> NewValueObserved = delegate { };
        public event EventHandler<Exception> ObservationError = delegate { };
        public event EventHandler ObservationEnd = delegate { };

        public string Description => "Random energy observation source.";

        public RandomEnergySource()
        {
            _observers = new List<IObserver<IEnergyObservation>>();
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await SimulateNewObservationDataAsync(cancellationToken);
        }

        public IDisposable Subscribe(IObserver<IEnergyObservation> observer)
        {
            observer = observer ?? throw new ArgumentNullException($"The {nameof(observer)} can not be null.");

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }                 

            return new RandomEnergySourceSubscription(this, observer);
        }

        internal void Unsubscribe(IObserver<IEnergyObservation> observer)
        {
            _observers.Remove(observer);
        }

        private async Task SimulateNewObservationDataAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        GenerateRandomObservation();
                    }
                    catch (Exception exception)
                    {
                        PublishException(exception);
                        throw;
                    }
                }

                Done();

            });
        }

        private void Done()
        {
            _observers.ForEach(observer => { observer.OnCompleted(); });

            ObservationEnd(this, EventArgs.Empty);
        }

        private void PublishException(Exception exception)
        {
            _observers.ForEach(observer => { observer.OnError(exception); });

            ObservationError(this, exception);
        }

        private void GenerateRandomObservation()
        {
            var flashObservation = RandomObservationCreator.CreateObservation();

            _observers.ForEach(observer => observer.OnNext(flashObservation));

            NewValueObserved(this, flashObservation);
        }
    }

    class RandomEnergySourceSubscription : IDisposable
    {
        private readonly RandomEnergySource _randomEnergySource;
        private readonly IObserver<IEnergyObservation> _observer;

        public RandomEnergySourceSubscription(RandomEnergySource source, IObserver<IEnergyObservation> observer)
        {
            _randomEnergySource = source;
            _observer = observer;
        }

        public void Dispose()
        {
            _randomEnergySource.Unsubscribe(_observer);
        }
    }
}
