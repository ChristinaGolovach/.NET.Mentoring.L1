using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Potestas.ADO.Plugin
{
    internal static class ADOUtils
    {
        public static void ExecuteNonQuery(string connectionString, string sprocName, Dictionary<string, object> parameters)
        {
            CheckNotNullOrEmpty(connectionString, "connectionString");
            CheckNotNullOrEmpty(sprocName, "sprocName");


            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string connectionString, string sprocName, IDictionary<string, object> parameters)
        {
            CheckNotNullOrEmpty(connectionString, nameof(connectionString));
            CheckNotNullOrEmpty(sprocName, nameof(sprocName));

            object result = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = CreateStoredProcedureCommand(connection, sprocName, parameters);

                connection.Open();

                result = command.ExecuteScalar();
            }

            return result;
        }

        public static IDictionary<string, object> ExecuteReaderSingleRow(string connectionString,
                                                                        string sprocName,
                                                                        IDictionary<string, object> parameters,
                                                                        int filedCountOfSelectedEntity)
        {
            CheckNotNullOrEmpty(connectionString, nameof(connectionString));
            CheckNotNullOrEmpty(sprocName, nameof(sprocName));

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


        public static IEnumerable<IDictionary<string, object>> ExecuteReaderRows(string connectionString,
                                                                                string sprocName,
                                                                                IDictionary<string, object> parameters,
                                                                                int filedCountOfSelectedEntity)
        {
            CheckNotNullOrEmpty(connectionString, nameof(connectionString));
            CheckNotNullOrEmpty(sprocName, nameof(sprocName));

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

        private static SqlCommand CreateStoredProcedureCommand(SqlConnection connection, string sprocName, IDictionary<string, object> parameters)
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

        private static void CheckNotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"The {name} can not be null or empty.");
            }
        }
    }
}
