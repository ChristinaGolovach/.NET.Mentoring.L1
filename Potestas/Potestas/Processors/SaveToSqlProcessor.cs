using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Potestas.Exceptions.ProcessorExceptions;

namespace Potestas.Processors
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
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentException($"The {nameof(value)} must be initialized.");
            }

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

            }
        }
    }
}
