using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudDataMar.BLL;
using CloudDataMar.IBLL;
using CloudDataMar.Model;
using CloudDataMar.Web.ViewModel;
using System.Linq.Expressions;
using CloudDataMar.Web.Util;
using CloudDataMar.Web.App_Start;

namespace CloudDataMar.Web.Controllers
{
    [IsLogin]
    public class BaseMarController : BaseController
    {
        //
        // GET: /BaseManage/
        public user_infoService userService = new user_infoService();
        user_roleService rolwService = new user_roleService();
        public ActionResult UpdatePssWord()
        {
            return View();
        }

        public JsonResult UpdatePssWordMethod(string oldpass, string newpass)
        {
            if (Session["UserName"] == null)
            {
                return Json(new { State = "TimeOut" }, JsonRequestBehavior.AllowGet);
            }
            var model = userService.LoadEntites(o => o.UserName == Session["UserName"].ToString()).FirstOrDefault();
            if (model != null)
            {
                if (model.UserPassword != oldpass)
                {
                    return Json(new { State = "Failure" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.UserPassword = newpass;
                    userService.UpdateEntity(model);
                }
            }
            return Json(new { State = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownSetting()
        {
            var roleList = rolwService.LoadEntites().ToList();
            if (roleList.Count > 0)
            {
               var guanli= roleList.Where(w=>w.RoleCode==1).Select(u => u.RoleDownTime).FirstOrDefault().ToString();
               var yuangong = roleList.Where(w => w.RoleCode == 2).Select(u => u.RoleDownTime).FirstOrDefault().ToString();
               var gaoji = roleList.Where(w => w.RoleCode == 3).Select(u => u.RoleDownTime).FirstOrDefault().ToString();
               var zhongji = roleList.Where(w => w.RoleCode == 4).Select(u => u.RoleDownTime).FirstOrDefault().ToString();
               var chuji = roleList.Where(w => w.RoleCode == 5).Select(u => u.RoleDownTime).FirstOrDefault().ToString();
               ViewBag.Guanli = GetDripIndexByDownTime(guanli);
               ViewBag.Yuangong = GetDripIndexByDownTime(yuangong);
               ViewBag.Gaoji = GetDripIndexByDownTime(gaoji);
               ViewBag.Zhongji = GetDripIndexByDownTime(zhongji);
               ViewBag.Chuji = GetDripIndexByDownTime(chuji);
            }
            return View();
        }

        [HttpPost]
        public string UpdateDownSetting(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string[] arr = id.Split(',');
                    //管理员
                    var itemGuanli = rolwService.LoadEntites(o => o.RoleID==1).FirstOrDefault();
                    itemGuanli.RoleDownTime = Convert.ToInt32(arr[0]);
                    rolwService.UpdateEntity(itemGuanli);
                    //用户
                    var itemYonghu = rolwService.LoadEntites(o => o.RoleID == 2).FirstOrDefault();
                    itemYonghu.RoleDownTime = Convert.ToInt32(arr[1]);
                    rolwService.UpdateEntity(itemYonghu);
                    //高级
                    var itemGaoji = rolwService.LoadEntites(o => o.RoleID == 3).FirstOrDefault();
                    itemGaoji.RoleDownTime = Convert.ToInt32(arr[2]);
                    rolwService.UpdateEntity(itemGaoji);
                    //中级
                    var itemZhongji = rolwService.LoadEntites(o => o.RoleID == 4).FirstOrDefault();
                    itemZhongji.RoleDownTime = Convert.ToInt32(arr[3]);
                    rolwService.UpdateEntity(itemZhongji);
                    //初级
                    var itemChuji = rolwService.LoadEntites(o => o.RoleID == 5).FirstOrDefault();
                    itemChuji.RoleDownTime = Convert.ToInt32(arr[4]);
                    rolwService.UpdateEntity(itemChuji);
                }
            }
            catch (Exception e)
            {
                return "err";
            }
            return "suc";
        }
        public string GetDripIndexByDownTime(string t)
        {
            switch (t)
            {
                case "0":
                    return "4";
                case "3":
                    return "1";
                case "6":
                    return "2";
                case "12":
                    return "3";
                default:
                    return "0";
            }
        }
        public ActionResult UserList()
        {
            return View();
        }

        public JsonResult GetUserList(string user_name, string tele_phone, string e_mail, string work_units, string user_level)
        {
            Func<user_info, bool> whereLambda = null;
           
            whereLambda += user => (user.UserName == null || user.UserName.Contains(user_name)
                && (user.TelePhone == null || user.TelePhone.Contains(tele_phone))
                && (user.EMail == null || user.EMail.Contains(e_mail))
                && (user.WorkUnits == null || user.WorkUnits.Contains(work_units))
                && (user_level == "-1" || user.UserLevel == Convert.ToInt32(user_level))
              );
            var userlist = userService.LoadEntites(whereLambda).ToList();
            List<UserViewModel> results = new List<UserViewModel>();
            foreach (var item in userlist)
            {
                results.Add(new UserViewModel
                {
                    ID = item.ID,
                    UserLevel = item.UserLevel,
                    UserID = item.UserID,
                    UserName = item.UserName,
                    UserPassword = item.UserPassword,
                    EMail = item.EMail,
                    TelePhone = item.TelePhone,
                    WorkUnits = item.WorkUnits,
                    Role = Enum.GetName(typeof(RoleType), item.UserLevel),
                    Action = string.Format("<a class='btn-link' href='#portlet-config' data-toggle='modal'>角色调整</a><span> | </span><a href='/BaseMar/DeleteUser?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            if (results != null && results.Count() > 0)
            {
                if (!string.IsNullOrEmpty(user_name))
                {
                    results.RemoveAll(u=>u.UserName == null);
                }
                if (!string.IsNullOrEmpty(tele_phone))
                {
                    results.RemoveAll(u => u.TelePhone == null);
                }
                if (!string.IsNullOrEmpty(e_mail))
                {
                    results.RemoveAll(u => u.EMail == null);
                }
                if (!string.IsNullOrEmpty(work_units))
                {
                    results.RemoveAll(u => u.WorkUnits == null);
                }
                return Json(new
                {
                    Results = results
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        public JsonResult ChangeRoleById(string ID, string userlevel)
        {
            try
            {
                var item = userService.LoadEntites(o => o.ID == Convert.ToInt32(ID)).FirstOrDefault();
                item.UserLevel = Convert.ToInt32(userlevel);
                userService.UpdateEntity(item);
                var result = new { State = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { State = "Exception", Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteUser(int ID)
        {
            try
            {
                var list = userService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (userService.DelEntity(list.First()))
                    {
                        var result = new { State = "Success" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = new { State = "Failure" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var result = new { State = "non-existent" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var result = new { State = "Exception", Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserUpdate()
        {
            return View();
        }
    }
 
    public static class PredicateBuilder
    {
        /// <summary>
        /// 机关函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 机关函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
