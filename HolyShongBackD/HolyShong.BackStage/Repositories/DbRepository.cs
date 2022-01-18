using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Repositories
{
    public class DbRepository : IDbRepository
    {
        public HolyShongContext Context { get; set; }
        public DbRepository(HolyShongContext context)
        {
            Context = context;
        }

        public void Create<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Added;
        }
        public void Update<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return Context.Set<T>();
        }

        public void Save()
        {
            Context.SaveChanges();
        }
        
        public void CreateRange<T>(IEnumerable<T> value) where T: class
        {
            Context.Set<T>().AddRange(value);
        }
    }
}
