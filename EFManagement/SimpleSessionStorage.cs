using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace EFManagement
{
    public class SimpleSessionStorage<T> : ISessionStorage<T> where T : ObjectContext
    {
        private readonly Dictionary<string, T> storage = new Dictionary<string, T>();

        /// <summary>
        ///     Returns all the values of the internal dictionary of sessions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAllSessions()
        {
            return this.storage.Values;
        }

        /// <summary>
        ///     Returns the session associated with the specified factoryKey or
        ///     null if the specified factoryKey is not found.
        /// </summary>
        /// <param name = "factoryKey"></param>
        /// <returns></returns>
        public T GetSessionForKey(string factoryKey)
        {
            T session;

            if (!this.storage.TryGetValue(factoryKey, out session))
            {
                return null;
            }

            return session;
        }

        /// <summary>
        ///     Stores the session into a dictionary using the specified factoryKey.
        ///     If a session already exists by the specified factoryKey, 
        ///     it gets overwritten by the new session passed in.
        /// </summary>
        /// <param name = "factoryKey"></param>
        /// <param name = "session"></param>
        public void SetSessionForKey(string factoryKey, T session)
        {
            this.storage[factoryKey] = session;
        }
    }
}
