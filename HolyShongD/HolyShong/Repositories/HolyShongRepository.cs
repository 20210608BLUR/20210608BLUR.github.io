using System.Data.Entity;
using HolyShong.Models.HolyShongModel;
using HolyShong.Services;
using HolyShong.ViewModels;
using System.Linq;

namespace HolyShong.Repositories
{
    public class HolyShongRepository
    {
        public readonly HolyShongContext Context;


        public HolyShongRepository()
        {
            //if (context == null)
            //{
            //    throw new ArgumentNullException();
            //}
            Context = new HolyShongContext();
        }


        public void Create<T>(T value) where T : class
        {
            Context.Entry(value).State = EntityState.Added;
        }
        public void Update<T>(T value) where T : class
        {
            Context.Entry(value).State = EntityState.Modified;
        }
        public void Delete<T>(T value) where T : class
        {
            Context.Entry(value).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return Context.Set<T>();
        }

        public void SaveChange()
        {
            Context.SaveChanges();
        }
    }
}