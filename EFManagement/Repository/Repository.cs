using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace EFManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IObjectSet<T> _objectSet;

        private IObjectSet<T> ObjectSet
        {
            get
            {
                if(_objectSet == null)
                    _objectSet = Context.CreateObjectSet<T>();

                return _objectSet;
            }
        }

        private ObjectContext _context;

        private ObjectContext Context
        {
            get
            {
                if(_context == null)
                    _context = EntityFrameworkWebSessionStorage.Instance.CurrentObjectContext();

                return _context;
            }
        }

        //public static ObjectContext CurrentObjectContext()
        //{
        //    ObjectContext session = EntityFrameworkWebSessionStorage.Instance.GetSessionForKey("");

        //    if (session == null)// !session.IsOpen)
        //    {
        //        session = EntityFrameworkContextFactory.Instance.Create();

        //        EntityFrameworkWebSessionStorage.Instance.SetSessionForKey("", session);
        //    }

        //    return session;
        //}

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<T> AsQueryable()
        {
            return ObjectSet;
        }

        /// <summary>
        /// Gets all records as an IEnumberable
        /// </summary>
        /// <returns>An IEnumberable object containing the results of the query</returns>
        public IEnumerable<T> GetAll()
        {
            return AsQueryable().AsEnumerable();
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A collection containing the results of the query</returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return ObjectSet.Where<T>(predicate);
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public T Single(Func<T, bool> predicate)
        {
            return ObjectSet.Single<T>(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public T First(Func<T, bool> predicate)
        {
            return ObjectSet.First<T>(predicate);
        }

        /// <summary>
        /// Deletes the specified entitiy
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            ObjectSet.DeleteObject(entity);
        }

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        public void Delete(Func<T, bool> predicate)
        {
            IEnumerable<T> records = from x in ObjectSet.Where<T>(predicate) select x;

            foreach (T record in records)
            {
                ObjectSet.DeleteObject(record);
            }
        }

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            ObjectSet.AddObject(entity);
        }

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="entity">Entity to attach</param>
        public void Attach(T entity)
        {
            ObjectSet.Attach(entity);
        }

        /// <summary>
        /// Saves all context changes
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Saves all context changes with the specified SaveOptions
        /// </summary>
        /// <param name="options">Options for saving the context</param>
        public void SaveChanges(SaveOptions options)
        {
            Context.SaveChanges(options);
        }
    }
}
