using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using ImpromptuInterface;
using Dynamitey;
using Potestas.Exceptions.StorageExcepions;

namespace Potestas.Storages
{
    public class SqlStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private string _connectionString;
        private SqlConnection _connection;
        private DataSet _dataSet;
        private DataTable _coordinates;
        private DataTable _observations;
        private SqlDataAdapter _coordinatesAdapter;
        private SqlDataAdapter _observationsAdapter;

        private readonly string coordinateTableName = "Coordinates";
        private readonly string observationTableName = "Observations";

        public string Description => "SQL storage of energy observations";

        public int Count => _observations.Rows.Count;

        public bool IsReadOnly => false;
        public SqlStorage(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"The {nameof(connectionString)} can not be null.");

            _dataSet = new DataSet("ObservationSet");

            FillDataSet();            
        }

        public void Add(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default))
            {
                throw new ArgumentException($"The {nameof(item)} must be initialized.");
            }

            var newCoordinateRow = _coordinates.NewRow();
            var newObservationRow = _observations.NewRow();

            newCoordinateRow["X"] = item.ObservationPoint.X;
            newCoordinateRow["Y"] = item.ObservationPoint.Y;

            _coordinates.Rows.Add(newCoordinateRow);

            newObservationRow["CoordinateId"] = (int)newCoordinateRow["Id"];
            newObservationRow["EstimatedValue"] = item.EstimatedValue;
            newObservationRow["ObservationTime"] = item.ObservationTime;

            _observations.Rows.Add(newObservationRow);

            _dataSet.AcceptChanges();

            // SendChangesToDB();
        }

        public void Clear()
        {
            _dataSet.Clear();
            _dataSet.AcceptChanges();
        }

        public bool Contains(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default))
            {
                throw new ArgumentException($"The {nameof(item)} must be initialized.");
            }

            return _observations.Rows.Contains(item.Id);
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
                var joinResult = JoinCoordinateAndObservation(_coordinates, _observations);

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
            var joinResult = JoinCoordinateAndObservation(_coordinates, _observations);

            return ConvertTypedCollectionToGeneric(joinResult).GetEnumerator();
        }

        public bool Remove(T item)
        {
            if (_observations.Rows.Contains(item.Id) && _coordinates.Rows.Contains(item.ObservationPoint.Id))
            {
                try
                {
                    _observations.Rows.Find(item.Id).Delete();
                    _coordinates.Rows.Find(item.ObservationPoint.Id).Delete();
                    _dataSet.AcceptChanges();

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

        private void FillDataSet()
        {
            string getCoordinateCommand = "SELECT * FROM Coordinates;";
            string getObservationCommand = "SELECT * FROM FlashObservations;";

            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                _coordinatesAdapter = new SqlDataAdapter(getCoordinateCommand, _connection);
                _observationsAdapter = new SqlDataAdapter(getObservationCommand, _connection);

                _coordinatesAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                _observationsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                _coordinatesAdapter.Fill(_dataSet, coordinateTableName);
                _observationsAdapter.Fill(_dataSet, observationTableName);

                _coordinates = _dataSet.Tables[coordinateTableName];
                _observations = _dataSet.Tables[observationTableName];

                DataRelation relation = _dataSet.Relations.Add("CoordinatesObservations",
                    _dataSet.Tables[coordinateTableName].Columns["Id"],
                    _dataSet.Tables[observationTableName].Columns["CoordinateId"]);
            }
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

        //private void SendChangesToDB()
        //{
        //    _connection = new SqlConnection(_connectionString);

        //    try
        //    {
        //        _connection.Open();

        //        SqlCommandBuilder coordinatesCommand = new SqlCommandBuilder(_coordinatesAdapter);
        //        SqlCommandBuilder observationsCommand = new SqlCommandBuilder(_observationsAdapter);

        //        coordinatesCommand.DataAdapter.Update(_coordinates);
        //        observationsCommand.DataAdapter.Update(_observations);

        //        _dataSet.AcceptChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        var newEx = ex;
        //        _dataSet.RejectChanges();
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //}

    }
}
