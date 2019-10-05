using System;

namespace Potestas.Observations.Comparers
{
    internal static class ComparerSettings
    {
        internal static readonly double compareEpsilon;

        static ComparerSettings()
        {
            //TODO ConfigurationManager.AppSettings does not work 
            //try it later
            //IConfiguration configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();

            compareEpsilon = 0.001;

            //try
            //{
            //    compareEpsilon = double.Parse(System.Configuration.ConfigurationManager.AppSettings["compareEpsilon"]);
            //}
            //catch (Exception)
            //{
            //    compareEpsilon = 0.001;
            //}
        }

        public static void GetCanonicalValues(ref double x, ref double y, double precision)
        {
            GetCanonicalValues(ref x, precision);
            GetCanonicalValues(ref y, precision);
        }
        
        public static void GetCanonicalValues(ref double x, double precision)
        {
            x = Math.Floor(x / precision) * precision;
        }
    }

    // https://stackoverflow.com/questions/12580981/overriding-equals-and-gethashcode-and-double-comparison
}
