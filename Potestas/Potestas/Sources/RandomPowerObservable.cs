using Potestas.Observations;
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
    public class RandomEnergySource : IEnergyObservationSource, IEnergyObservationEventSource
    {
        private readonly List<IObserver<IEnergyObservation>> _observers;

        public RandomEnergySource()
        {
            _observers = new List<IObserver<IEnergyObservation>>();
        }
        public string Description => "Random energy observation source.";

        public event EventHandler<IEnergyObservation> NewValueObserved = delegate { };
        public event EventHandler<Exception> ObservationError = delegate { };
        public event EventHandler ObservationEnd = delegate { };

        public async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); //ask 

            await Task.WhenAny(
                SimulateObservationNewDataAsync(cancellationToken),
                CheckCancellationAsync(cancellationToken));
        }

        public IDisposable Subscribe(IObserver<IEnergyObservation> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException($"The {nameof(observer)} can not be null.");
            }

            _observers.Add(observer);

            return new RandomEnergySourceSubscription(this, observer);
        }

        internal void Unsubscribe(IObserver<IEnergyObservation> observer)
        {
            _observers.Remove(observer);
        }

        private async Task SimulateObservationNewDataAsync(CancellationToken cancellationToken)
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
                        // ask ?? throw;
                    }
                }
            });
        }

        private async Task CheckCancellationAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);
            }

            Done();
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
            var random = new Random();

            int durationMs = random.Next(1, 20);
            double intensity = random.Next(0, 1000000000);
            Coordinates observationPoint = new Coordinates(random.Next(-90, 90), random.Next(0, 180));

            var flashObservation = new FlashObservation(durationMs, intensity, observationPoint);

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
