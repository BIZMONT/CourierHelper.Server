using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
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
                return _dbContext.Customers.AsQueryable();
            }
        }

        public void Create(Customer entity)
        {
            _dbContext.Customers.Add(entity);
        }

        public void Delete(Customer entity)
        {
            _dbContext.Customers.Remove(entity);
        }

        public void Delete(object key)
        {
            var entity = _dbContext.Customers.Find(key);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            return _dbContext.Customers.AsEnumerable();
        }

        public void Update(Customer entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
