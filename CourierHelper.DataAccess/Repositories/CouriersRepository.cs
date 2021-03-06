﻿using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class CouriersRepository : IRepository<Courier>
    {
        private CourierHelperDbContext _dbContext;

        public CouriersRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Courier> Query
        {
            get
            {
                return _dbContext.Couriers.Where(c => c.Deleted == null).AsQueryable();
			}
        }

        public void Create(Courier entity)
        {
			entity.Created = DateTime.Now;
			entity.Edited = DateTime.Now;
			_dbContext.Couriers.Add(entity);
        }

        public void Delete(Courier entity)
        {
			entity.Deleted = DateTime.Now;
			Update(entity);
		}

        public void Delete(object key)
        {
            var entity = _dbContext.Couriers.Find(key);

            if(entity != null)
            {
                Delete(entity);
            }
        }

		public Courier Get(object key)
		{
			Courier courier = _dbContext.Couriers.Find(key);
			return courier.Deleted != null ? null : courier;
		}

		public IEnumerable<Courier> GetAll()
        {
            return _dbContext.Couriers.Where(c => c.Deleted == null).AsEnumerable();
		}

        public void Update(Courier entity)
        {
			entity.Edited = DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
