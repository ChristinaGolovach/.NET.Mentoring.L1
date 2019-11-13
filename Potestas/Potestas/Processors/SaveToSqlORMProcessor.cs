using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Potestas.Observations.Comparers;
using Potestas.Models;

namespace Potestas.Processors
{
    public class SaveToSqlORMProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly DbContext _dbContext;

        public string Description => "Saves observations to the provided DB using EF Core.";

        public SaveToSqlORMProcessor(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"the {nameof(dbContext)} can not be null.");
        }

        public void OnCompleted()
        {
            //TODO add info in loger
        }

        public void OnError(Exception error)
        {
            //TODO add Exception
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                throw new ArgumentException($"The {nameof(value)} must be initialized.");
            }

           var coordinate = _dbContext.Set<Models.Coordinates>().FirstOrDefault(c => new Coordinates(c.X, c.Y).Equals(new Coordinates(value.ObservationPoint.X, value.ObservationPoint.Y)));

            if (coordinate != null)
            {
                _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    CoordinateId = coordinate.Id,
                    EstimatedValue = value.EstimatedValue,
                    ObservationTime = value.ObservationTime
                });
            }

            else
            {
                coordinate = new Models.Coordinates() { X = value.ObservationPoint.X, Y = value.ObservationPoint.Y };

                _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    Coordinate = coordinate,
                    EstimatedValue = value.EstimatedValue,
                    ObservationTime = value.ObservationTime
                });

                //as we have navigation property in EnergyObservations points to coordinates, the last will be added in DB automaticly
            }

            _dbContext.SaveChanges();

        }

        private double GetCanonicalDoubleValue(double value)
        {
            return ComparerUtils.GetCanonicalValues(value, ComparerUtils.comparePrecision);
        }
    }
}
