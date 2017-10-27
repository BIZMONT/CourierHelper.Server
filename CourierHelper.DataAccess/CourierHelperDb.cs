using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using CourierHelper.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace CourierHelper.DataAccess
{
    public class CourierHelperDb : IDisposable
    {
        private CourierHelperDbContext _context;

        public CourierHelperDb(string sqlServerConnectionString)
        {
            _context = new CourierHelperDbContext(sqlServerConnectionString);

            OrdersRepo = new OrdersRepository(_context);
            CouriersRepo = new CouriersRepository(_context);
            WarehousesRepo = new WarehousesRepository(_context);
            CustomersRepo = new CustomersRepository(_context);
            RoutesRepo = new RoutesRepository(_context);
            ActivePointsRepo = new ActivePointsRepository(_context);
        }

        #region Repositories
        public IRepository<Order> OrdersRepo { get; private set; }
        public IRepository<Courier> CouriersRepo { get; private set; }
        public IRepository<Warehouse> WarehousesRepo { get; private set; }
        public IRepository<Customer> CustomersRepo { get; private set; }
        public IRepository<Route> RoutesRepo { get; private set; }
        public IRepository<ActivePoint> ActivePointsRepo { get; private set; }
        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region IDisposable implementation
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
