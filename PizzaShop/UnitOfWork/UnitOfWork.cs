using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PizzaShop.UnitOfWork
{
    public abstract class UnitOfWork
    {
        readonly protected DbContext _context;

        public UnitOfWork(DbContext conetxt)
        {
            _context = conetxt;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}