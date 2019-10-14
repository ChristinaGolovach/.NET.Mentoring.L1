using System;

namespace Potestas.Observations.Comparers
{
    internal static class ComparerUtils
    {
        internal static readonly double comparePrecision;

        static ComparerUtils()
        {
            //TODO ConfigurationManager.AppSettings does not work 
            //try it later
            //IConfiguration configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();

            comparePrecision = 0.001;

            //try
            //{
            //    compareEpsilon = double.Parse(System.Configuration.ConfigurationManager.AppSettings["compareEpsilon"]);
            //}
            //catch (Exception)
            //{
            //    compareEpsilon = 0.001;
            //}
        }
        
        public static double GetCanonicalValues(double x, double precision)
        {
            return x = Math.Floor(x / precision) * precision;
        }

        public static bool? IsNaNPointComparer(Coordinates poin1, Coordinates point2, Func<double, double, bool> EqualsCoordinates)
        {
            if (double.IsNaN(poin1.X) && double.IsNaN(poin1.Y))
            {
                return double.IsNaN(point2.X) && double.IsNaN(point2.Y);
            }

            if (double.IsNaN(poin1.X) && !double.IsNaN(poin1.Y))
            {
                return double.IsNaN(point2.X) && !double.IsNaN(point2.Y) && EqualsCoordinates(poin1.Y, point2.Y);
            }

            if (!double.IsNaN(poin1.X) && double.IsNaN(point2.Y))
            {
                return !double.IsNaN(point2.X) && double.IsNaN(point2.Y) && EqualsCoordinates(poin1.X, point2.X);
            }

            return null;
        }
    }

    // https://stackoverflow.com/questions/12580981/overriding-equals-and-gethashcode-and-double-comparison
}
