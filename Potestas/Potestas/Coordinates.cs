using Potestas.Observations.Comparers;
using System;

namespace Potestas
{
    /* TASK. Implement this structure: 
     * 1. Implement custom constructor
     * 2. The valid range for X is [-90; 90], for Y [0; 180]
     * 3. Take into account boxing and unboxing issues
     * 4. Implement + and - operators for this structure.
     * 5. Implement a way to represent coordinates in string.
     * 6. Coordinates are equal each other when each dimension values are equal with thousand precision
     * 7. Implement == and != operators for this structure.
     * 8. 
     */
    public struct Coordinates : IEquatable<Coordinates>
    {
        private static readonly double precision = 0.001;

        private static readonly double xMinValue = -90;
        private static readonly double xMaxValue = 90;

        private static readonly double yMinValue = 0;
        private static readonly double yMaxValue = 180;  


        public double X { get; }

        public double Y { get; }

        public Coordinates(double xCoordinate, double yCoordinate)
        {
            //TODO ask move it to the separate validator
            if (xCoordinate < xMinValue || xCoordinate > xMaxValue)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(xCoordinate)} must be in range between {xMinValue} and {xMaxValue} inclusively.");
            }

            if (yCoordinate < yMinValue || yCoordinate > yMaxValue)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(yCoordinate)} must be in range between {yMinValue} and {yMaxValue} inclusively.");
            }

            X = xCoordinate;
            Y = yCoordinate;

        }

        public static Coordinates operator +(Coordinates coordinates1, Coordinates coordinates2)
        {
            return new Coordinates(coordinates1.X + coordinates2.X, coordinates1.Y + coordinates2.Y);
        }

        public static Coordinates operator -(Coordinates coordinates1, Coordinates coordinates2)
        {
            return new Coordinates(coordinates1.X - coordinates2.X, coordinates1.Y - coordinates2.Y);
        }

        public static bool operator ==(Coordinates coordinates1, Coordinates coordinates2)
        {
            return coordinates1.Equals(coordinates2);
        }

        public static bool operator !=(Coordinates coordinates1, Coordinates coordinates2)
        {
            return !(coordinates1 == coordinates2);
        }

        public bool Equals(Coordinates other)
        {
            if (double.IsNaN(X) && double.IsNaN(Y))
            {
                return double.IsNaN(other.X) && double.IsNaN(other.Y);
            }

            if (double.IsNaN(X) && !double.IsNaN(Y))
            {
                return double.IsNaN(other.X) && !double.IsNaN(other.Y);
            }

            if (!double.IsNaN(X) && double.IsNaN(Y))
            {
                return !double.IsNaN(other.X) && double.IsNaN(other.Y);
            }

            return EqualsCoordinates(other);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.GetType() != typeof(Coordinates))
            {
                return false;
            }

            return Equals((Coordinates)other);
        }

        public override int GetHashCode()
        {
            int hash = 3;
            double x = X;
            double y = Y;

            ComparerUtils.GetCanonicalValues(ref x, ref y, precision);

            hash = (hash * 7) + x.GetHashCode();
            hash = (hash * 7) + y.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}";
        }

        private bool EqualsCoordinates(Coordinates other)
        {
            double x1 = X;
            double y1 = Y;

            double x2 = other.X;
            double y2 = other.Y;

            ComparerUtils.GetCanonicalValues(ref x1, ref y1, precision);
            ComparerUtils.GetCanonicalValues(ref x2, ref y2, precision);

            return x1 == x2 && y1 == y2;
        }
    }

    // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
}
