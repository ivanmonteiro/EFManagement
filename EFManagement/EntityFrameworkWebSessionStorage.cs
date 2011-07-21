using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Reflection;
using System.Diagnostics;

namespace EFManagement
{
    public class EntityFrameworkWebSessionStorage
    {
        private EntityFrameworkWebSessionStorage()
        {

        }

        private static EntityFrameworkWebSessionStorage _instance;

        public static EntityFrameworkWebSessionStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EntityFrameworkWebSessionStorage();
                }

                return _instance;
            }
        }

        public ObjectContext CurrentObjectContext()
        {
            ObjectContext session = GetSessionForKey("");

            if (session == null)// !session.IsOpen)
            {
                session = EntityFrameworkContextFactory.Instance.Create();
                SetSessionForKey("", session);
            }

            return session;
        }



        /// <summary>
        /// Initializes the entity framework session storage
        /// </summary>
        /// <param name="httpApplication"></param>
        /// <param name="entitiesObjectContextType">The ObjectContext type, use typeof(YourEntities)</param>
        /// <param name="connectionString">Optional connection string</param>
        public static void Configure(HttpApplication httpApplication, Type entitiesObjectContextType, string connectionString = null)
        {
            if (httpApplication == null)
                throw new ArgumentNullException("httpApplication");

            EntityFrameworkContextFactory.Configure(entitiesObjectContextType, connectionString);

            httpApplication.EndRequest += Application_EndRequest;

            Debug.WriteLine("Initializing Entity Framework Web Sesssion Storage");
        }

        private const string HttpContextSessionStorageKey = "HttpContextSessionStorageKey";

        public IEnumerable<ObjectContext> GetAllSessions()
        {
            var storage = GetSimpleSessionStorage();
            return storage.GetAllSessions();
        }

        public ObjectContext GetSessionForKey(string factoryKey)
        {
            var storage = GetSimpleSessionStorage();
            return storage.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ObjectContext session)
        {
            var storage = GetSimpleSessionStorage();
            storage.SetSessionForKey(factoryKey, session);
        }

        private static void Application_EndRequest(object sender, EventArgs e)
        {            
            foreach (var entityFrameworkContext in GetSimpleSessionStorage().GetAllSessions())
            {
                Debug.WriteLine("Request ended - Disposing Object Context");
                entityFrameworkContext.Dispose();
            }

            var context = HttpContext.Current;
            context.Items.Remove(HttpContextSessionStorageKey);
        }

        private static SimpleSessionStorage<ObjectContext> GetSimpleSessionStorage()
        {
            var context = HttpContext.Current;
            var storage = context.Items[HttpContextSessionStorageKey] as SimpleSessionStorage<ObjectContext>;
            if (storage == null)
            {
                storage = new SimpleSessionStorage<ObjectContext>();
                context.Items[HttpContextSessionStorageKey] = storage;
            }

            return storage;
        }
    }
}