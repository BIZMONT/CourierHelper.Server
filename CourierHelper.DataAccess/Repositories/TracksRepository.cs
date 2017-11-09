using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class TracksRepository : IRepository<Track>
    {
        private CourierHelperDbContext _dbContext;

        public TracksRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Track> Query
        {
            get
            {
                return _dbContext.Tracks.AsQueryable();
            }
        }

        public void Create(Track entity)
        {
			entity.Created = DateTime.Now;
			entity.Edited = DateTime.Now;
			_dbContext.Tracks.Add(entity);
        }

        public void Delete(Track entity)
        {
			entity.Deleted = DateTime.Now;
			Update(entity);
		}

        public void Delete(object key)
        {
            var entity = _dbContext.Tracks.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

		public Track Get(object key)
		{
			return _dbContext.Tracks.Find(key);
		}

		public IEnumerable<Track> GetAll()
        {
            return _dbContext.Tracks.AsEnumerable();
        }

        public void Update(Track entity)
        {
			entity.Edited = DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
