using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FDB.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public SchoolContext _db { get; set; }
        protected DbSet<T> _table = null;

        public GenericRepository()
        {
            _db = new SchoolContext();
            _table = _db.Set<T>();
        }

        public GenericRepository(SchoolContext _db)
        {
            this._db = _db;
            _table = _db.Set<T>();
        }

        // select all
        public IEnumerable<T> SelectAll()
        {
            return _table.ToList();
        }

        // select by id
        public T SelectById(object id)
        {
            return _table.Find(id);
        }

        // insert
        public void Insert(T obj)
        {
            _table.Add(obj);
        }

        // update
        public void Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
        }

        // delete
        public void Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        // save
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}