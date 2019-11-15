using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Potestas.Models;
using Potestas.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Potestas.Storages
{
    public class SqlORMStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly DbContext _dbContext;
        private readonly ObservationContext observationContext;

        public string Description => "SQL (EF Core ORM) storage of energy observations";

        public int Count => _dbContext.Set<EnergyObservations>().Count();

        public bool IsReadOnly => false;

        public SqlORMStorage(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"the {nameof(dbContext)} can not be null.");
        }

        public void Add(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            //TODO move to help method. since it is copy-past from OnNext of SaveToSqlORMProcessor 
            var coordinate = _dbContext.Set<Models.Coordinates>().FirstOrDefault(c => new Coordinates(c.X, c.Y).Equals(new Coordinates(item.ObservationPoint.X, item.ObservationPoint.Y)));
            EntityEntry<EnergyObservations> newEntity = null;
            if (coordinate != null)
            {
                newEntity = _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    CoordinateId = coordinate.Id,
                    EstimatedValue = item.EstimatedValue,
                    ObservationTime = item.ObservationTime
                });
            }

            else
            {
                coordinate = new Models.Coordinates() { X = item.ObservationPoint.X, Y = item.ObservationPoint.Y };

                newEntity = _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    Coordinate = coordinate,
                    EstimatedValue = item.EstimatedValue,
                    ObservationTime = item.ObservationTime
                });
            }

            // TODO MApper
            //item = newEntity.Entity;

            _dbContext.SaveChanges();
        }

        public void Clear()
        {
            //not beautiful
            _dbContext.Set<EnergyObservations>().RemoveRange(_dbContext.Set<EnergyObservations>());
            _dbContext.Set<Models.Coordinates>().RemoveRange(_dbContext.Set<Models.Coordinates>());

            _dbContext.SaveChanges();
        }

        public bool Contains(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            return _dbContext.Set<EnergyObservations>().FirstOrDefault(e => e.Id == item.Id) != null ? true : false;

            // it's for myself (from SQL Profiler)
            // exec sp_executesql N'SELECT TOP(1) [e].[Id], [e].[CoordinateId], [e].[EstimatedValue], [e].[ObservationTime]
            // FROM[EnergyObservations] AS[e]
            // WHERE[e].[Id] = @__Id_0',N'@__Id_0 int',@__Id_0=5010
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            // https://riptutorial.com/csharp/example/15938/creating-an-instance-of-a-type
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            //TODO make mapper
            var energyEntity = new EnergyObservations()
            {
                Id = item.Id
            };

            try
            {
                var deletedEntity = _dbContext.Remove<EnergyObservations>(energyEntity);
                //or
                //var deletedEntity  = _dbContext.Set<EnergyObservations>().Remove(energyEntity);

                _dbContext.SaveChanges();

                return true;
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return false;
            }         
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    //https://stackoverflow.com/questions/32308095/entity-framework-performance-in-count
    //https://softwareengineering.stackexchange.com/questions/359667/is-it-ok-to-create-an-entity-framework-datacontext-object-and-dispose-it-in-a-us
}
