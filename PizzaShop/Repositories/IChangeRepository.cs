using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories
{
    public interface IChangeRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
