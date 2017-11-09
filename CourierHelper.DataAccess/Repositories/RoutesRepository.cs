using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class RoutesRepository : IRepository<Route>
    {
        private CourierHelperDbContext _dbContext;

        public RoutesRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Route> Query
        {
            get
            {
                return _dbContext.Routes.AsQueryable();
            }
        }

        public void Create(Route entity)
        {
			entity.Created = DateTime.Now;
			entity.Edited = DateTime.Now;
			_dbContext.Routes.Add(entity);
        }

        public void Delete(Route entity)
        {
            _dbContext.Routes.Remove(entity);
        }

        public void Delete(object key)
        {
            var entity = _dbContext.Routes.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

		public Route Get(object key)
		{
			return _dbContext.Routes.Find(key);
		}

		public IEnumerable<Route> GetAll()
        {
            return _dbContext.Routes.AsEnumerable();
        }

        public void Update(Route entity)
        {
			entity.Edited = DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
