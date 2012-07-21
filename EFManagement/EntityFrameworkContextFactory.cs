using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Reflection;
using System.Configuration;

namespace EFManagement
{
    public class EntityFrameworkContextFactory
    {
        private EntityFrameworkContextFactory()
        {

        }

        private static EntityFrameworkContextFactory _instance;

        public static EntityFrameworkContextFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EntityFrameworkContextFactory();

                return _instance;
            }
        }
        private static Type _objectContextType;

        private static string _connectionString;

        public static void Configure(Type entitiesObjectContextType, string connectionString = null)
        {
            if (entitiesObjectContextType == null)
                throw new ArgumentNullException("entitiesObjectContext");

            if (entitiesObjectContextType.BaseType.Name != "ObjectContext")
                throw new ArgumentException("The entitiesObjectContext must inherit from ObjectContext");

            _objectContextType = entitiesObjectContextType;

            _connectionString = connectionString;
        }

        public ObjectContext Create()
        {
            if (String.IsNullOrEmpty(_connectionString))
            {
                return (ObjectContext)Activator.CreateInstance(_objectContextType);
            }
            else
            {
                ConstructorInfo constructor = _objectContextType.GetConstructor(new[] { typeof(string) });
                return (ObjectContext)constructor.Invoke(new[] { _connectionString });
            }
        }

    }
}
