using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudDataMar.IBLL;
using CloudDataMar.Model;

namespace CloudDataMar.BLL
{
    
    public partial class user_infoService : BaseService<user_info>, Iuser_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.user_infoDao;
       }
    }
}