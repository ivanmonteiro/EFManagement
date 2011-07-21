using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFManagement
{
    public interface ISessionStorage<T>
    {
        IEnumerable<T> GetAllSessions();

        T GetSessionForKey(string factoryKey);

        void SetSessionForKey(string factoryKey, T session);
    }
}
