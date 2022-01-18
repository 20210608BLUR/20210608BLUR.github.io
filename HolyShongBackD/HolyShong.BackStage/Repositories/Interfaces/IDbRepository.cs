using HolyShong.BackStage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Repositories.Interfaces
{
    public interface IDbRepository
    {
        public HolyShongContext Context { get; set; }
        IQueryable<T> GetAll<T>() where T : class;
        void Create<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void CreateRange<T>(IEnumerable<T> value) where T : class;
        void Save();
    }
}
