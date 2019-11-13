﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Potestas.Models;
using Potestas.Validators;

namespace Potestas.Storages
{
    public class SqlORMStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly DbContext _dbContext;

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
        }

        public void Clear()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            return false; 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    //https://stackoverflow.com/questions/32308095/entity-framework-performance-in-count
    //https://softwareengineering.stackexchange.com/questions/359667/is-it-ok-to-create-an-entity-framework-datacontext-object-and-dispose-it-in-a-us
}
