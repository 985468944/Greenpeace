using CloudDataMar.BLL;
using CloudDataMar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CloudDataMar.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            //bool ?isAnnounce = null;//是否显示公告 null不显示
            //data_announce_infoService dataAnnounceBll = new data_announce_infoService();
            //List<data_announce_info> list= dataAnnounceBll.LoadEntites(a=>a.IsPublish==1).ToList();
            //if (list.Count > 0)
            //{
            //    isAnnounce = true;
            //    ViewBag.Announce = list;
            //}
            //ViewBag.isAnnounce = isAnnounce;
        }
    }
}
