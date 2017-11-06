using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class ActivePointsRepository : IRepository<ActivePoint>
    {
        private CourierHelperDbContext _dbContext;

        public ActivePointsRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<ActivePoint> Query
        {
            get
            {
                return _dbContext.ActivePoints.AsQueryable();
            }
        }

        public void Create(ActivePoint entity)
        {
            _dbContext.ActivePoints.Add(entity);
        }

        public void Delete(ActivePoint entity)
        {
            _dbContext.ActivePoints.Remove(entity);
        }

        public void Delete(object key)
        {
            var entity = _dbContext.ActivePoints.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

		public ActivePoint Get(object key)
		{
			return _dbContext.ActivePoints.Find(key);
		}

		public IEnumerable<ActivePoint> GetAll()
        {
            return _dbContext.ActivePoints.AsEnumerable();
        }

        public void Update(ActivePoint entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
