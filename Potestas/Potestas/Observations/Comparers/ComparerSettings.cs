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
    }
}
