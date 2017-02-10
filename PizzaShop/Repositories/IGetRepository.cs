using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories
{
    public interface IGetRepository<T> where T : class
    {
        List<T> All();
        T GetByID(int? id);
    }
}
