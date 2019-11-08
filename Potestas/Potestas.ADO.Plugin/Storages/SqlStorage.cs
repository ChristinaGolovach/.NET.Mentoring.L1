using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ImpromptuInterface;
using Potestas.ADO.Plugin.Exceptions;

namespace Potestas.ADO.Plugin
{
    public class SqlStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly string _srcCoordinatesTblName = "Coordinates";
        private readonly string _srcObservationsTblName = "FlashObservations";

        private string _connectionString;
        private SqlConnection _connection;
        private DataSet _dataSet;
        private Dictionary<string, KeyValuePair<SqlDataAdapter, DataTable>> _adapterTablePairs;

        public string Description => "SQL storage of energy observations";

        public int Count => _adapterTablePairs[_srcObservationsTblName].Value.Rows.Count;

        public bool IsReadOnly => false;

        public SqlStorage(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"The {nameof(connectionString)} can not be null.");

            _dataSet = new DataSet("ObservationSet");

            _connection = new SqlConnection(_connectionString);

            _adapterTablePairs = new Dictionary<string, KeyValuePair<SqlDataAdapter, DataTable>>();

            FillDataSet(_srcCoordinatesTblName);
            FillDataSet(_srcObservationsTblName);
        }

        public void Add(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default))
            {
                throw new ArgumentException($"The {nameof(item)} must be initialized.");
            }

            var newCoordinateRow = _adapterTablePairs[_srcCoordinatesTblName].Value.NewRow();
            var newObservationRow = _adapterTablePairs[_srcObservationsTblName].Value.NewRow();

            newCoordinateRow["X"] = item.ObservationPoint.X;
            newCoordinateRow["Y"] = item.ObservationPoint.Y;

            _adapterTablePairs[_srcCoordinatesTblName].Value.Rows.Add(newCoordinateRow);

            newObservationRow["CoordinateId"] = (int)newCoordinateRow["Id"];
            newObservationRow["EstimatedValue"] = item.EstimatedValue;
            newObservationRow["ObservationTime"] = item.ObservationTime;

            _adapterTablePairs[_srcObservationsTblName].Value.Rows.Add(newObservationRow);

            SendChangesToDB();
        }

        public void Clear()
        {
            _dataSet.Clear();

            SendChangesToDB();
        }

        public bool Contains(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default))
            {
                throw new ArgumentException($"The {nameof(item)} must be initialized.");
            }

            return _adapterTablePairs[_srcObservationsTblName].Value.Rows.Contains(item.Id);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = array ?? throw new ArgumentNullException($"The {nameof(array)} can not be null.");

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(arrayIndex)} can not be less than 0.");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException($"The available space in {nameof(array)} is not enough.");
            }

            try
            {
                var joinResult = JoinCoordinateAndObservation(_adapterTablePairs[_srcCoordinatesTblName].Value,
                                                              _adapterTablePairs[_srcObservationsTblName].Value);

                var genericColection = ConvertTypedCollectionToGeneric(joinResult);

                genericColection.CopyTo(array, arrayIndex);
            }
            catch (Exception exception)
            {
                throw new SqlStorageException($"Exception occurred during copy data from the sql storage to array.", exception);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var joinResult = JoinCoordinateAndObservation(_adapterTablePairs[_srcCoordinatesTblName].Value,
                                                          _adapterTablePairs[_srcObservationsTblName].Value);

            return ConvertTypedCollectionToGeneric(joinResult).GetEnumerator();
        }

        public bool Remove(T item)
        {
            if (_adapterTablePairs[_srcObservationsTblName].Value.Rows.Contains(item.Id) &&
                _adapterTablePairs[_srcCoordinatesTblName].Value.Rows.Contains(item.ObservationPoint.Id))
            {
                try
                {
                    _adapterTablePairs[_srcObservationsTblName].Value.Rows.Find(item.Id).Delete();
                    _adapterTablePairs[_srcCoordinatesTblName].Value.Rows.Find(item.ObservationPoint.Id).Delete();

                    SendChangesToDB();

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void FillDataSet(string srcTableName)
        {
            string command = $"SELECT * FROM {srcTableName};";

            using (_connection)
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                var dataAdapter = new SqlDataAdapter(command, _connection);

                dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                dataAdapter.Fill(_dataSet, srcTableName);

                var dataTable = _dataSet.Tables[srcTableName];

                _adapterTablePairs.Add(srcTableName, new KeyValuePair<SqlDataAdapter, DataTable>(dataAdapter, dataTable));
            }
        }

        private void setRelationship(DataTable parent, string primaryKeyName, DataTable child, string foreignKeyName, string relationshipName)
        {
            DataRelation relation = _dataSet.Relations.Add(relationshipName, parent.Columns[primaryKeyName], child.Columns[foreignKeyName]);
        }

        private IEnumerable<IEnergyObservation> JoinCoordinateAndObservation(DataTable coordinates, DataTable observations)
        {
            return from t1 in coordinates.AsEnumerable()
                   join t2 in observations.AsEnumerable()
                      on (int)t1["Id"] equals (int)t2["CoordinateId"]
                   select new
                   {
                       Id = (int)t2["Id"],
                       ObservationPoint = new Coordinates((int)t1["Id"], (double)t1["X"], (double)t1["Y"]),
                       EstimatedValue = (double)t2["EstimatedValue"],
                       ObservationTime = (DateTime)t2["ObservationTime"]
                   }.ActLike<IEnergyObservation>();
            // https://stackoverflow.com/questions/612689/a-generic-list-of-anonymous-class  - not suitable 
            //https://github.com/ekonbenefits/impromptu-interface - I have used it to convert anonymous type to Interface type
        }

        private List<T> ConvertTypedCollectionToGeneric(IEnumerable<IEnergyObservation> typedCollection)
        {
            var genericCollection = new List<T>();

            foreach (var result in typedCollection)
            {
                genericCollection.Add((T)result);
            }

            return genericCollection;
        }

        private void SendChangesToDB()
        {
            foreach (var pair in _adapterTablePairs)
            {
                SendChangesToDB(pair.Value.Key, pair.Value.Value);
            }
        }

        private void SendChangesToDB(SqlDataAdapter dataAdapter, DataTable dataTable)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                SqlCommandBuilder observationsCommand = new SqlCommandBuilder(dataAdapter);

                dataAdapter.AcceptChangesDuringUpdate = true;
                dataAdapter.Update(dataTable);
            }
            catch (Exception ex)
            {
                var newEx = ex;

                _dataSet.RejectChanges();
            }
            finally
            {
                _connection.Close();
            }

            // It is why I used several adapters

            // If your dataset contains multiple tables, you have to update them individually
            // by calling the Update method of each data adapter separately
            //https://docs.microsoft.com/en-us/previous-versions/aa983594(v=vs.71)?redirectedfrom=MSDN
        }
    }
}
