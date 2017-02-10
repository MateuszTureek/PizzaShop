using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.Repositories
{
    public class Repository<T>: IGetRepository<T>, IChangeRepository<T> where T : class
    {
        protected DbContext _context;
        protected DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public List<T> All()
        {
            var result = _dbSet.ToList();
            return result;
        }

        public T GetByID(int? id)
        {
            var result = _dbSet.Find(id);
            return result;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}