using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.DataAccess.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        IQueryable<TEntity> Query { get; }

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(object key);
    }
}
