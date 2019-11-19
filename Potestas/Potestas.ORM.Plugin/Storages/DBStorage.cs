using Microsoft.EntityFrameworkCore;
using Potestas.Extensions;
using Potestas.ORM.Plugin.Exceptions;
using Potestas.ORM.Plugin.Mappers;
using Potestas.ORM.Plugin.Models;
using Potestas.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Potestas.ORM.Plugin.Storages
{
    public class DBStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly DbContext _dbContext;

        public string Description => "SQL (EF Core ORM) storage of energy observations";

        public int Count => _dbContext.Set<EnergyObservations>().Count();

        public bool IsReadOnly => false;

        public DBStorage(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"the {nameof(dbContext)} can not be null.");
        }

        public void Add(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            var coordinate = _dbContext.Set<Models.Coordinates>().FirstOrDefault(c => new Coordinates(c.X, c.Y).Equals(new Coordinates(item.ObservationPoint.X, item.ObservationPoint.Y)));

            if (coordinate != null)
            {
                _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    CoordinateId = coordinate.Id,
                    EstimatedValue = item.EstimatedValue,
                    ObservationTime = item.ObservationTime
                });
            }

            else
            {
                coordinate = new Models.Coordinates() { X = item.ObservationPoint.X, Y = item.ObservationPoint.Y };

                _dbContext.Set<EnergyObservations>().Add(new EnergyObservations()
                {
                    Coordinate = coordinate,
                    EstimatedValue = item.EstimatedValue,
                    ObservationTime = item.ObservationTime
                });
            }

            _dbContext.SaveChanges();

            //as we have navigation property in EnergyObservations points to coordinates, the last will be added in DB automaticly
        }

        public void Clear()
        {
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

                var genericColection = _dbContext.Set<EnergyObservations>()
                                                 .Include(x => x.Coordinate)
                                                 .AsEnumerable()
                                                 .ConvertObservationCollectionToGeneric<T, EnergyObservations>();

                genericColection.CopyTo(array, arrayIndex);
            }
            catch (Exception exception)
            {
                throw new DBStorageException($"Exception occurred during copy data from the ORM storage to array.", exception);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var setCollection = _dbContext.Set<EnergyObservations>().Include(x => x.Coordinate).AsEnumerable();

            return setCollection.ConvertObservationCollectionToGeneric<T, EnergyObservations>().GetEnumerator();
        }

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            var energyEntity = item.ToORMEntity();   

            try
            {
                var deletedEntity = _dbContext.Remove<EnergyObservations>(energyEntity);
                //or
                //var deletedEntity  = _dbContext.Set<EnergyObservations>().Remove(energyEntity);

                _dbContext.SaveChanges();

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
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
