using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFManagement
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork Start()
        {
            throw new NotImplementedException();
        }
    }
}
