using System;

namespace Potestas.Observations.Comparers
{
    internal static class ComparerSettings
    {
        internal static readonly double epsilon;

        static ComparerSettings()
        {
            try
            {
                epsilon = double.Parse(System.Configuration.ConfigurationManager.AppSettings["epsilon"]);
            }
            catch (Exception)
            {
                epsilon = 0.001;
            }
        }

        public static void GetCanonicalValues(ref double x, ref double y, double precision)
        {
            GetCanonicalValues(ref x, precision);
            GetCanonicalValues(ref y, precision);
        }

        // https://stackoverflow.com/questions/12580981/overriding-equals-and-gethashcode-and-double-comparison
        public static void GetCanonicalValues(ref double x, double precision)
        {
            x = Math.Floor(x / precision) * precision;
        }
    }
}
