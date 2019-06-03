using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.IDAO
{
    public interface IDBSessionFactory
    {
        IDBSession GetCurrentDBSession();
    }
}
