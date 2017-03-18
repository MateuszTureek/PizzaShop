using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
        
        public void Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(r => r.Reload());
        }
    }
}