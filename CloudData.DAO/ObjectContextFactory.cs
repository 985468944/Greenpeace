using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataMar.DAO
{
    public class ObjectContextFactory
    {
        public static System.Data.Objects.ObjectContext GetCurrentObjectContext()
        {
            //从CallContext数据槽中获取EF上下文
            ObjectContext objectContext = CallContext.GetData(typeof(ObjectContextFactory).FullName) as ObjectContext;
            if (objectContext == null)
            {
                //如果CallContext数据槽中没有EF上下文，则创建EF上下文，并保存到CallContext数据槽中
                objectContext = new ObjectContext("name=CloudDataContainer");
                CallContext.SetData(typeof(ObjectContextFactory).FullName, objectContext);
            }
            return objectContext;
        }
    }
}
