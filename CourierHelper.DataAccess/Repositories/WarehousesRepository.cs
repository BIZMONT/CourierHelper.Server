using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class WarehousesRepository : IRepository<Warehouse>
    {
        private CourierHelperDbContext _dbContext;

        public WarehousesRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Warehouse> Query
        {
            get { return _dbContext.Warehouses.Where(w => w.Deleted == null).AsQueryable(); }
        }

        public void Create(Warehouse entity)
        {
            _dbContext.Warehouses.Add(entity);
        }

        public void Delete(Warehouse entity)
        {
			entity.Deleted = DateTime.Now;
			Update(entity);
		}

        public void Delete(object key)
        {
            var entity = _dbContext.Warehouses.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _dbContext.Warehouses.Where(w => w.Deleted == null).AsEnumerable();
		}

        public void Update(Warehouse entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
