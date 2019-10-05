﻿using Potestas.Observations.Comparers;
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
    public struct Coordinates 
    {
        private readonly double x;

        private readonly double y;

        private static readonly double precision = 0.001;

        public double X { get => x; }

        public double Y { get => y; }

        public Coordinates(double xCoordinate, double yCoordinate)
        {
            x = xCoordinate;
            y = yCoordinate;
        }

        // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        public override int GetHashCode()
        {
            int hash = 3;
            double x = this.x;
            double y = this.y;

            ComparerSettings.GetCanonicalValues(ref x, ref y, precision);

            hash = (hash * 7) + x.GetHashCode();
            hash = (hash * 7) + y.GetHashCode();

            return hash;
        }
 
        //TODO add IEquatable
    }
}
