using CloudDataMar.IDAO;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.DAO
{
    public partial class DBSession : IDBSession
    {
        private ObjectContext _efContext;

        //EF上下文
        public ObjectContext EfContext
        {
            get
            {
                if (_efContext == null)
                {
                    _efContext = ObjectContextFactory.GetCurrentObjectContext();
                }
                return _efContext;
            }
            set { _efContext = value; }
        }

        public int SaveChange()
        {
            return EfContext.SaveChanges();//调用SaveChanges()方法提交操作
        }


    }
}
