using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudDataMar.BLL;
using CloudDataMar.IBLL;
using CloudDataMar.Model;
using CloudDataMar.Web.ViewModel;
using System.Web.Mvc;

namespace CloudDataMar.Web.App_Start
{
    public class AppCommon : Controller
    {
         public static List<SelectListItem> GetDataTypes()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            data_type_infoService datatypeService = new data_type_infoService();
            var types = datatypeService.LoadEntites();
            list.Add(new SelectListItem { Text ="---请选择---", Value = "0"});
            if (types != null && types.Count() > 0)
            {
                foreach (var item in types)
                {
                    list.Add(new SelectListItem { Text = item.Data_Type_Name, Value = item.ID.ToString() });
                }
            }
            return list;
        }

        public static List<SelectListItem> GetDataInfos()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            data_infoService datainfoService = new data_infoService();
            var types = datainfoService.LoadEntites();
            if (types != null && types.Count() > 0)
            {
                foreach (var item in types)
                {
                    list.Add(new SelectListItem { Text = item.Data_Info_Name, Value = item.ID.ToString() });
                }
            }
            return list;
        }

        public static List<data_type_info> GetMenuList() {
            data_type_infoService datatypeService = new data_type_infoService();
            data_infoService datainfoService = new data_infoService();
            var list = datatypeService.LoadEntites().OrderBy(o => o.Data_Type_OrderNum).ToList();
            foreach (var item in list)
            {
                item.data_info = datainfoService.LoadEntites(o => o.Data_Type_ID == item.ID).ToList();
            }
            return list;
        }
    }

    //登录、角色、权限过滤器
    public class IsLogin : ActionFilterAttribute
    {
        public string Role { get; set; }
        //当方法执行时
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var lastController = ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName;
                var lastAction = (filterContext.ActionDescriptor).ActionName;
                var lastParams = "?";
                var lastParam = "";
                IDictionary<string, object> lastDicParams = filterContext.ActionParameters;
                if (lastDicParams.Count > 0)
                {
                    foreach (var param in lastDicParams)
                    {
                        lastParams += param.Key + "=" + param.Value+"&";
                    }
                    lastParams=lastParams.TrimEnd('&');
                    lastParam = lastParams.Equals("?") ? "" : lastParams;
                }
                var ReturnUrl = "?ReturnUrl=" + lastController + "/" + lastAction + lastParam;
                var sess = filterContext.RequestContext.HttpContext.Session["UserName"];
                if (sess == null || string.IsNullOrEmpty(sess.ToString()))
                {
                    filterContext.Result = new RedirectResult("/Account/login" + ReturnUrl);
                }
                else
                {
                    var level = filterContext.RequestContext.HttpContext.Session["UserLevel"];
                    if (level.ToString() != "1" && level.ToString() != "2")
                    {
                        if (Role != "member" && (filterContext.ActionDescriptor).ActionName != "UpdatePssWord" && (filterContext.ActionDescriptor).ActionName != "UpdatePssWordMethod")
                        {
                            filterContext.Result = new RedirectResult("/AdminAccount/Home");
                        }
                    }
                }
            }
            catch
            {
                filterContext.Result = new RedirectResult("/Account/login");
            }
        }

        //当方法执行完毕
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}