using CloudDataMar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudDataMar.Web.Controllers
{
    public class HomeController : BaseController
    {
        private data_type_infoService datatypeService = new data_type_infoService();
        private data_infoService datainfoService = new data_infoService();
        public ActionResult Index()
        {
            var list = datatypeService.LoadEntites().OrderBy(o => o.Data_Type_OrderNum).ToList();
            foreach (var item in list)
            {
                item.data_info = datainfoService.LoadEntites(o => o.Data_Type_ID == item.ID).ToList();
            }
            ViewData["DataTypeList"] = list;
            return View();
        }
        
        public ActionResult FLSM()
        {
            return View();
        }

        public ActionResult GYWM()
        {
            return View();
        }

        public ActionResult LXWM()
        {
            return View();
        }

        public ActionResult HelpCenter() {
            return View();
        }
    }
}
