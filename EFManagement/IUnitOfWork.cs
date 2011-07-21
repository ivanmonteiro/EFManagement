using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFManagement
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork Start();
    }
}
