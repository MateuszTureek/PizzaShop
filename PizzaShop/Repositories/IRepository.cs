using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repository
{
    public interface IRepository<PKey,TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        TEntity Get(PKey id);
        IEnumerable<TEntity> GetAll();
        void Save();
    }
}
