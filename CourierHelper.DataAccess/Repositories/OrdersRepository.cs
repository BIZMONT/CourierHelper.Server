using CourierHelper.DataAccess.Abstract;
using CourierHelper.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CourierHelper.DataAccess.Repositories
{
	public class OrdersRepository : IRepository<Order>
	{
		private CourierHelperDbContext _dbContext;

		public OrdersRepository(CourierHelperDbContext context)
		{
			_dbContext = context;
		}

		public IQueryable<Order> Query
		{
			get
			{
				return _dbContext.Orders.AsQueryable();
			}
		}

		public void Create(Order entity)
		{
			entity.Created = DateTime.Now;
			entity.Edited = DateTime.Now;
			_dbContext.Orders.Add(entity);
		}

		public void Delete(Order entity)
		{
			_dbContext.Orders.Remove(entity);
		}

		public void Delete(object key)
		{
			var entity = _dbContext.Orders.Find(key);

			if (entity != null)
			{
				Delete(entity);
			}
		}

		public Order Get(object key)
		{
			Order order = _dbContext.Orders.Find(key);
			return order.Deleted != null ? null : order;
		}

		public IEnumerable<Order> GetAll()
		{
			return _dbContext.Orders.AsEnumerable();
		}

		public void Update(Order entity)
		{
			entity.Edited = DateTime.Now;
			_dbContext.Entry(entity).State = EntityState.Modified;
		}
	}
}
