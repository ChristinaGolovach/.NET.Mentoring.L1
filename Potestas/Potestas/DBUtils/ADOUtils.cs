﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Potestas.DBUtils
{
    public static class ADOUtils
    {
        public static void ExecuteNonQuery(string connectionString, string sprocName, Dictionary<string, object> parameters)
        {
            CheckArguments(connectionString, sprocName);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string connectionString, string sprocName, Dictionary<string, object> parameters)
        {
            CheckArguments(connectionString, sprocName);

            object result = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                result = command.ExecuteScalar();
            }

            return result;
        }

        public static Dictionary<string, object> ExecuteReaderSingleRow(string connectionString, 
                                                                        string sprocName, 
                                                                        Dictionary<string, object> parameters, 
                                                                        int filedCountOfSelectedEntity)
        {
            CheckArguments(connectionString, sprocName);

            var fieldsOfRow = new Dictionary<string, object>(); //fieldsOfRow : key(name in DB) - value(data in DB)

            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                var reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (!reader.HasRows) return null;

                reader.Read();

                for (int i = 0; i < filedCountOfSelectedEntity; i++)
                {
                    fieldsOfRow.Add(reader.GetName(i), reader[i]);
                }

                reader.Close();
            }

            return fieldsOfRow;
        }


        public static IEnumerable<Dictionary<string, object>> ExecuteReaderRows(string connectionString, 
                                                                                string sprocName, 
                                                                                Dictionary<string, object> parameters, 
                                                                                int filedCountOfSelectedEntity)
        {
            CheckArguments(connectionString, sprocName);

            var resultRows = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                var reader = command.ExecuteReader();

                if (!reader.HasRows) return null;

                while (reader.Read())
                {
                    var fieldsOfRow = new Dictionary<string, object>(); //fieldsOfRow : key(name in DB) - value(data in DB)

                    for (int i = 0; i < filedCountOfSelectedEntity; i++)
                    {
                        fieldsOfRow.Add(reader.GetName(i), reader[i]);
                    }
                    resultRows.Add(fieldsOfRow);
                }
                reader.Close();
            }

            return resultRows;
        }

        private static SqlCommand CreateStoredProcedureCommand(SqlConnection connection, string sprocName, Dictionary<string, object> parameters)
        {
            var command = new SqlCommand(sprocName, connection) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            return command;
        }

        private static void CheckArguments(string connectionString, string sprocName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"The {nameof(connectionString)} can not be null or empty.");
            }

            if (string.IsNullOrEmpty(sprocName))
            {
                throw new ArgumentException($"The {nameof(sprocName)} can not be null or empty.");
            }
        }
    }
}
