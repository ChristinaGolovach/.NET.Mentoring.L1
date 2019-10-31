using Potestas.Observations;
using System;

namespace Potestas.Sources.ObservationCreators
{
    internal static class RandomObservationCreator
    {
        public static IEnergyObservation CreateObservation()
        {
            var random = new Random();

            int durationMs = random.Next(1, 20);
            double intensity = random.Next(0, 1000000000);
            Coordinates observationPoint = new Coordinates(random.Next((int)Coordinates.xMinValue, (int)Coordinates.xMaxValue), 
                                                           random.Next((int)Coordinates.yMinValue, (int)Coordinates.yMaxValue));

            return new FlashObservation(durationMs, intensity, observationPoint);
        }
    }
}
