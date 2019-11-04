using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace Potestas.Storages
{
    public class SqlStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private string _connectionString;
        private DataSet _dataSet;
        public string Description => "SQL storage of energy observations";

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => false;
        public SqlStorage(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"The {nameof(connectionString)} can not be null.");

            _dataSet = new DataSet("Observations");

            FillDataSet();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void FillDataSet()
        {
            string getCoordinateCommand = "SELECT * FROM Coordinates";
            string getObservationCommand = "SELECT * FROM FlashObservations";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var coordinatesAdapter = new SqlDataAdapter(getCoordinateCommand, connection);
                var observationsAdapter = new SqlDataAdapter(getObservationCommand, connection);

                coordinatesAdapter.Fill(_dataSet, "Coordinates");
                observationsAdapter.Fill(_dataSet, "FlashObservations");

                DataRelation relation = _dataSet.Relations.Add("CoordinatesFlasObservations",
                    _dataSet.Tables["Coordinates"].Columns["Id"],
                    _dataSet.Tables["FlashObservations"].Columns["CoordinateId"]);
            }
        }
    }
}
