




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudDataMar.Model;//引用Domain的命名空间

namespace CloudDataMar.IDAO //实体类接口所在的命名空间
{
    
    public interface Idata_accessory_infoDao:IBaseDao<data_accessory_info> //生成实体对象接口
    {
    }
    
    public interface Idata_announce_infoDao:IBaseDao<data_announce_info> //生成实体对象接口
    {
    }
    
    public interface Idata_applicationDao:IBaseDao<data_application> //生成实体对象接口
    {
    }
    
    public interface Idata_detail_infoDao:IBaseDao<data_detail_info> //生成实体对象接口
    {
    }
    
    public interface Idata_infoDao:IBaseDao<data_info> //生成实体对象接口
    {
    }
    
    public interface Idata_specialDao:IBaseDao<data_special> //生成实体对象接口
    {
    }
    
    public interface Idata_statisticsDao:IBaseDao<data_statistics> //生成实体对象接口
    {
    }
    
    public interface Idata_type_infoDao:IBaseDao<data_type_info> //生成实体对象接口
    {
    }
    
    public interface Iuser_infoDao:IBaseDao<user_info> //生成实体对象接口
    {
    }
    
    public interface Iuser_roleDao:IBaseDao<user_role> //生成实体对象接口
    {
    }

}