using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudDataMar.BLL;
using CloudDataMar.IBLL;
using CloudDataMar.Model;
namespace CloudDataMar.Web.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/
        private user_infoService userInfoService = new user_infoService();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public string LoginInfo(user_info user)
        {
            var userInfos = userInfoService.LoadEntites(u => u.UserName == user.UserName && u.UserPassword == user.UserPassword);
            if (userInfos != null && userInfos.Count() > 0)
            {
                var result = userInfos.First();
                Session["UserName"] = result.UserName;
                Session["UserLevel"] = result.UserLevel.ToString();
                string UserRoleName = "";
                switch (result.UserLevel.ToString())
                {
                    case "1":
                        UserRoleName= "管理员";
                        break;
                    case "2":
                        UserRoleName= "员工";
                        break;
                    case "3":
                        UserRoleName= "高级会员";
                        break;
                    case "4":
                        UserRoleName= "中级会员";
                        break;
                    case "5":
                        UserRoleName= "初级会员";
                        break;
                }
                Session[" UserRoleName"] = UserRoleName;
                if (result.UserLevel.ToString() == "1" || result.UserLevel.ToString() == "2")
                {
                    return "suc";
                }
                else
                {
                    return "suc2";
                }
            }
            else
            {
                return "nouser";
            }
        }

        [HttpPost]
        public string RegisterInfo(user_info user)
        {
            try
            {
                var userinfos = userInfoService.LoadEntites(u => u.UserName == user.UserName);
                if (userinfos != null && userinfos.Count() > 0)
                {
                    return "exist";
                }
                user.UserLevel = 5;
                if ((!string.IsNullOrWhiteSpace(user.TelePhone) || !string.IsNullOrWhiteSpace(user.EMail)) && !string.IsNullOrWhiteSpace(user.WorkUnits))
                {
                    user.UserLevel = 3;
                }
                else if (!string.IsNullOrWhiteSpace(user.TelePhone) || !string.IsNullOrWhiteSpace(user.EMail))
                {
                    user.UserLevel = 4;
                }
                var entity = userInfoService.AddEntity(user);
                Session["UserName"] = user.UserName;
                Session["UserLevel"] = user.UserLevel.ToString();
                if (user.UserLevel.ToString() == "1" || user.UserLevel.ToString() == "2")
                    return "suc";
                else
                    return "suc2";
            }
            catch (Exception ex)
            {
                //log
                return "err";
            }
        }

        public ActionResult SignOut() {
            Session.RemoveAll();
            return RedirectToAction("Home", "AdminAccount");
        }
    }
}
