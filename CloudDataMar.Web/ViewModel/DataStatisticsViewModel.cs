using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudDataMar.Model;
namespace CloudDataMar.Web.ViewModel
{
    public class DataStatisticsViewModel 
    {
        public int id { get; set; }
        public string name { get; set; }
        public long browse_quantity { get; set; }
        public long download_quantity { get; set; }
    }
}