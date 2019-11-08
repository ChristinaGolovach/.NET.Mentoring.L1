using System;
using System.Collections.Generic;
using Potestas.DBUtils;
using Potestas.Exceptions.StorageExcepions;
using Potestas.Observations.Comparers.EqualityComparers;

namespace Potestas.Analizers
{
    public class SqlAnalizer : IEnergyObservationAnalizer
    {
        private string _connectionString;

        public SqlAnalizer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"The {nameof(connectionString)} can not be null.");
        }

        public double GetAverageEnergy()
        {
            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Average_Energy", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select average Energy.");
            }

            return (double)result;
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@StartFrom", startFrom);
            parameters.Add("@EndBy", endBy);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Average_Energy_Between_Dates", parameters);

            if (result == null)
            {
                throw new SqlStorageException("Can not select average Energy.");
            }

            return (double)result;
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@TopLeftX", rectTopLeft.X);
            parameters.Add("@TopLeftY", rectTopLeft.Y);
            parameters.Add("@BottomRightX", rectBottomRight.X);
            parameters.Add("@BottomRightY", rectBottomRight.Y);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Average_Energy_Between_Coordinates", parameters);

            if (result == null)
            {
                throw new SqlStorageException("Can not select average Energy.");
            }

            return (double)result;
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {           
            var resultDistribution = new Dictionary<Coordinates, int>(new CoordinateEqualityComparer());
            int fieldCountInSelectedItem = 4;

            var selectedItems = ADOUtils.ExecuteReaderRows(_connectionString, "Select_Distribution_By_Coordinates", null, fieldCountInSelectedItem);

            if (selectedItems == null)
            {
                throw new SqlStorageException("Can not select distribution by coordinates");
            }

            foreach (var item in selectedItems)
            {
                var coordinate = new Coordinates((int)item["Id"], (double)item["X"], (double)item["Y"]);

                resultDistribution.Add(coordinate, (int)item["Count"]);
            }

            return resultDistribution;
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            var resultDistribution = new Dictionary<double, int>();
            int fieldCountInSelectedItem = 2;

            var selectedItems = ADOUtils.ExecuteReaderRows(_connectionString, "Select_Distribution_By_EnergyValue", null, fieldCountInSelectedItem);

            if (selectedItems == null)
            {
                throw new SqlStorageException("Can not select distribution by estimated value.");
            }

            foreach (var item in selectedItems)
            {              
                resultDistribution.Add((double)item["EstimatedValue"], (int)item["Count"]);
            }

            return resultDistribution;
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            var resultDistribution = new Dictionary<DateTime, int>();
            int fieldCountInSelectedItem = 2;

            var selectedItems = ADOUtils.ExecuteReaderRows(_connectionString, "Select_Distribution_By_ObservationTime", null, fieldCountInSelectedItem);

            if (selectedItems == null)
            {
                throw new SqlStorageException("Can not select distribution by observation time.");
            }

            foreach (var item in selectedItems)
            {
                resultDistribution.Add((DateTime)item["ObservationTime"], (int)item["Count"]);
            }

            return resultDistribution;
        }

        public double GetMaxEnergy()
        {
            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Max_Energy", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select max Energy.");
            }

            return (double)result;
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@X", coordinates);
            parameters.Add("@Y", coordinates);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Max_Energy_By_Coordinate", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select max Energy.");
            }

            return (double)result;
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Date", dateTime);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Max_Energy_By_Date", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select max Energy.");
            }

            return (double)result;
        }

        public Coordinates GetMaxEnergyPosition()
        {
            int fieldCountInCoordinateInstanse = 3;
            var result = ADOUtils.ExecuteReaderSingleRow(_connectionString, "Select_Max_Energy_Position", null, fieldCountInCoordinateInstanse);

            if (result == null)
            {
                throw new ArgumentNullException("Can not select position of max energy.");
            }

            return new Coordinates((int)result["Id"], (double)result["X"], (double)result["Y"]);
        }

        public DateTime GetMaxEnergyTime()
        {
            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Max_Energy_Time", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select time of max energy.");
            }

            return (DateTime)result;
        }

        public double GetMinEnergy()
        {
            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Min_Energy", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select min Energy.");
            }

            return (double)result;
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@X", coordinates);
            parameters.Add("@Y", coordinates);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Min_Energy_By_Coordinate", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select min Energy.");
            }

            return (double)result;
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Date", dateTime);

            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Min_Energy_By_Date", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select min Energy.");
            }

            return (double)result;
        }

        public Coordinates GetMinEnergyPosition()
        {
            int fieldsCountInCoordinateInstanse = 3;
            var result = ADOUtils.ExecuteReaderSingleRow(_connectionString, "Select_Min_Energy_Position", null, fieldsCountInCoordinateInstanse);

            if (result == null)
            {
                throw new ArgumentNullException("Can not select position of min energy.");
            }

            return new Coordinates((int)result["Id"], (double)result["X"], (double)result["Y"]);
        }

        public DateTime GetMinEnergyTime()
        {
            object result = ADOUtils.ExecuteScalar(_connectionString, "Select_Min_Energy_Time", null);

            if (result == null)
            {
                throw new SqlStorageException("Can not select time of min energy.");
            }

            return (DateTime)result;
        }

        private class CoordinateEqualityComparer : EqualityComparer<Coordinates>
        {
            public override bool Equals(Coordinates x, Coordinates y)
            {
                return x.Id == y.Id;
            }

            public override int GetHashCode(Coordinates obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
