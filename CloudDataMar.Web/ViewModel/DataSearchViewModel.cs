using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudDataMar.Model;

namespace CloudDataMar.Web.ViewModel
{
    public class DataSearchViewModel
    {
        public List<data_info> DatainfoList { get; set; }
        public List<SearchDetailViewModel> SearchDetailList { get; set; }
    }

    public class SearchDetailViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string detail { get; set; }

        public int? isoutside { get; set; }
        public DateTime? updatedatetime { get; set; }
    }
}