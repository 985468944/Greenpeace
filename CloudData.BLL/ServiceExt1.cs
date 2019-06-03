


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudDataMar.IBLL;
using CloudDataMar.Model;

namespace CloudDataMar.BLL
{
    
    public partial class data_accessory_infoService : BaseService<data_accessory_info>, Idata_accessory_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_accessory_infoDao;
       }
    }
    
    public partial class data_announce_infoService : BaseService<data_announce_info>, Idata_announce_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_announce_infoDao;
       }
    }
    
    public partial class data_applicationService : BaseService<data_application>, Idata_applicationService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_applicationDao;
       }
    }
    
    public partial class data_detail_infoService : BaseService<data_detail_info>, Idata_detail_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_detail_infoDao;
       }
    }
    
    public partial class data_infoService : BaseService<data_info>, Idata_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_infoDao;
       }
    }
    
    public partial class data_specialService : BaseService<data_special>, Idata_specialService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_specialDao;
       }
    }
    
    public partial class data_statisticsService : BaseService<data_statistics>, Idata_statisticsService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_statisticsDao;
       }
    }
    
    public partial class data_type_infoService : BaseService<data_type_info>, Idata_type_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.data_type_infoDao;
       }
    }
    
    public partial class user_infoService : BaseService<user_info>, Iuser_infoService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.user_infoDao;
       }
    }
    
    public partial class user_roleService : BaseService<user_role>, Iuser_roleService
    {
       public override void SetCurrentDao()
       {
           this.CurrentDao = this.DbSession.user_roleDao;
       }
    }

}