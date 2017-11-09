using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
    public class CustomersRepository : IRepository<Customer>
    {
        private CourierHelperDbContext _dbContext;

        public CustomersRepository(CourierHelperDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Customer> Query
        {
            get
            {
                return _dbContext.Customers.Where(c=> c.Deleted == null).AsQueryable();
            }
        }

        public void Create(Customer entity)
        {
			entity.Created = DateTime.Now;
			entity.Edited = DateTime.Now;
			_dbContext.Customers.Add(entity);
        }

        public void Delete(Customer entity)
        {
			entity.Deleted = DateTime.Now;
			Update(entity);
        }

        public void Delete(object key)
        {
            var entity = _dbContext.Customers.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

		public Customer Get(object key)
		{
			Customer customer = _dbContext.Customers.Find(key);
			return customer.Deleted != null ? null : customer;
		}

		public IEnumerable<Customer> GetAll()
        {
            return _dbContext.Customers.Where(c=>c.Deleted == null).AsEnumerable();
        }

        public void Update(Customer entity)
        {
			entity.Edited = DateTime.Now;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
