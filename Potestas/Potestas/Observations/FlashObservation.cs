using Potestas.Observations.Comparers;
using System;

namespace Potestas.Observations
{
    /* TASK: Implement this structure by following requirements:
    * 1. EstimatedValue is the intensity multiple by duration
    * 2. Observations are equal if they made at the same time, 
    * the same observation point and EstimatedValue 
    * is the same by decimal presicion
    * 3. Implement custom constructors with ability to set ObservationTime by moment of creation or from constructor parameter.
    * 4. Implement == and != operators for the structure.
    * 6. Negative Intensity is a sign of invalid observation. Figure out how to process such errors. Remember you are writing a library.
    * 7. Intensity more than 2 000 000 000 is imposible and could be a sign of the invalid observation.
    * 8. Implement nice string representation of this observation.
    * QUESTIONS: 
    * How implementation of interface impacts boxing and unboxing operation for the structure?
    * Why overriding of Equals method is not enough?
    * What kind of pollymorhism does this struct contain?
    * Why immutable structure is used here?
    * TESTS: Cover this structure with unit tests
    */
    public struct FlashObservation : IEnergyObservation, IEquatable<FlashObservation>
    {
        public static readonly double PRECISION = 0.01;
        public static readonly int MININTENSITY = 0;
        public static readonly int MAXINTENSITY = 2000000000;

        public int DurationMs { get; }

        public double Intensity { get; }

        public Coordinates ObservationPoint { get; }

        public DateTime ObservationTime { get; }

        public double EstimatedValue { get => Intensity * DurationMs; }

        public FlashObservation(int durationMs, double intensity, Coordinates observationPoint) 
            : this (durationMs, intensity, observationPoint, DateTime.UtcNow) { }

        public FlashObservation(int durationMs, double intensity, Coordinates observationPoint, DateTime observationTime)
        {
            if (intensity < MININTENSITY || MAXINTENSITY < intensity)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(intensity)} must be between {MININTENSITY} and {MAXINTENSITY}.");
            }

            DurationMs = durationMs;
            Intensity = intensity;
            ObservationPoint = observationPoint;
            ObservationTime = observationTime;
        }

        public static bool operator ==(FlashObservation flashObservation1, FlashObservation flashObservation2)
        {
            return flashObservation1.Equals(flashObservation2);
        }

        public static bool operator !=(FlashObservation flashObservation1, FlashObservation flashObservation2)
        {
            return !(flashObservation1 == flashObservation2);
        }

        public bool Equals(FlashObservation other)
        {
            double thisEstimatedValue = EstimatedValue;
            double otherEstimatedValue = other.EstimatedValue;

            thisEstimatedValue = ComparerUtils.GetCanonicalValues(thisEstimatedValue, PRECISION);
            otherEstimatedValue = ComparerUtils.GetCanonicalValues(otherEstimatedValue, PRECISION);

            return ObservationTime.Equals(other.ObservationTime) &&
                   ObservationPoint.Equals(other.ObservationPoint) && 
                   thisEstimatedValue == otherEstimatedValue;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (!(other is FlashObservation))
            {
                return false;
            }

            return Equals((FlashObservation)other);
        }

        public override int GetHashCode()
        {
            int hash = 3;
            double thisEstimatedValue = EstimatedValue;

            thisEstimatedValue = ComparerUtils.GetCanonicalValues(thisEstimatedValue, PRECISION);

            hash = (hash * 7) + ObservationTime.GetHashCode();
            hash = (hash * 7) + ObservationPoint.GetHashCode();
            hash = (hash * 7) + thisEstimatedValue.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return $"FlashObservation - Duration(ms): {DurationMs},  Intensity: {Intensity}, Coordinates: {ObservationPoint}, ObservationTime: {ObservationTime}.";
        }
    }
}
