using System;
using System.Collections.Generic;
using Potestas.ADO.Plugin.Exceptions;

namespace Potestas.ADO.Plugin
{
    public class SaveToSqlProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly string _connectionString;

        public string Description => "Saves observations to the provided Data Base.";

        public SaveToSqlProcessor(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"The {nameof(connectionString)} can not be null.");
        }

        public void OnCompleted()
        {
            //TODO add info in loger
        }

        public void OnError(Exception error)
        {
            throw new SaveToSqlProcessorException($"Error in {Description}", error);
        }

        public void OnNext(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                throw new ArgumentException($"The {nameof(value)} must be initialized.");
            }

            var parameters = new Dictionary<string, object>();

            parameters.Add("@X", value.ObservationPoint.X);
            parameters.Add("@Y", value.ObservationPoint.Y);
            parameters.Add("@EstimatedValue", value.EstimatedValue);
            parameters.Add("@ObservationTime", value.ObservationTime);

            ADOUtils.ExecuteNonQuery(_connectionString, "Insert_FlasObservation", parameters);
        }
    }
}
