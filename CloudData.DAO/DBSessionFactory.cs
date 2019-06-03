using CloudDataMar.IDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.DAO
{
    public class DBSessionFactory : IDBSessionFactory
    {
        public IDBSession GetCurrentDBSession()
        {
            IDBSession dbSession = CallContext.GetData(typeof(DBSessionFactory).FullName) as DBSession;
            if (dbSession == null)
            {
                dbSession = new DBSession();
                CallContext.SetData(typeof(DBSessionFactory).FullName, dbSession);
            }
            return dbSession;
        }
    }
}
