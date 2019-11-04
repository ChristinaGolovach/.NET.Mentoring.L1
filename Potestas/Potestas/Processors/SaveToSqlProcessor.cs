using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                throw new ArgumentException($"The {nameof(value)} must be initialized.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("Insert_FlasObservation", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@X", value.ObservationPoint.X);
                command.Parameters.AddWithValue("@Y", value.ObservationPoint.Y);
                command.Parameters.AddWithValue("@EstimatedValue", value.EstimatedValue);
                command.Parameters.AddWithValue("@ObservationTime", value.ObservationTime);

                connection.Open();
                int affected = command.ExecuteNonQuery();
            }
        }
    }
}
