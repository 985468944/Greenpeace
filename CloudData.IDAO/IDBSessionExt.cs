


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudDataMar.Model;

namespace CloudDataMar.IDAO
{
  public partial interface IDBSession
  {
        
        Idata_accessory_infoDao data_accessory_infoDao { get; set; }
        
        Idata_announce_infoDao data_announce_infoDao { get; set; }
        
        Idata_applicationDao data_applicationDao { get; set; }
        
        Idata_detail_infoDao data_detail_infoDao { get; set; }
        
        Idata_infoDao data_infoDao { get; set; }
        
        Idata_specialDao data_specialDao { get; set; }
        
        Idata_statisticsDao data_statisticsDao { get; set; }
        
        Idata_type_infoDao data_type_infoDao { get; set; }
        
        Iuser_infoDao user_infoDao { get; set; }
        
        Iuser_roleDao user_roleDao { get; set; }
    
 }
}