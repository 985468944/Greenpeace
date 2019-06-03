using CloudDataMar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudDataMar.BLL;
using CloudDataMar.IBLL;
using CloudDataMar.Model;
using CloudDataMar.Web.ViewModel;
using System.IO;
using CloudDataMar.Web.App_Start;

namespace CloudDataMar.Web.Controllers
{
    public class DataManageController : Controller
    {
        private data_specialService dataspecialService = new data_specialService();
        private data_applicationService dataapplicationService = new data_applicationService();
        #region 数据应用

       

        //后台数据应用管理
        public ActionResult DataSpecialManage()
        {
            return View();
        }   

        //Home数据应用列表
        public ActionResult DataSpecialList(string txtaname, string startTime, string endTime)
        {
            bool o = false;
            if(!string.IsNullOrEmpty(txtaname))
            {
                ViewData["dataSpecialList"] = dataspecialService.LoadEntites(a => a.Data_Special_Name.Contains(txtaname)).ToList();
                o = true;
            }
            if ( !string.IsNullOrEmpty(startTime) &&!string.IsNullOrEmpty(endTime))
            {
                ViewData["dataSpecialList"] = dataspecialService.LoadEntites(m => m.CreateDateTime > DateTime.Parse(startTime) && m.CreateDateTime < DateTime.Parse(endTime)).ToList();
                o = true;
            }
            if ( (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime)) && !string.IsNullOrEmpty(txtaname))
            {
                ViewData["dataSpecialList"] = dataspecialService.LoadEntites(m => m.CreateDateTime > DateTime.Parse(startTime) && m.CreateDateTime < DateTime.Parse(endTime) && m.Data_Special_Name.Contains(txtaname)).ToList();
                o = true;
            }
          else
            {
                if (o == false) {
                    ViewData["dataSpecialList"] = dataspecialService.LoadEntites().ToList();
                }
            }

            return View();
        }

        //编辑数据应用
        [IsLogin]
        public ActionResult EditDataSpecial(int ID = 0)
        {
            DataSpecialViewModel viewmodel;
            if (ID != 0)
            {
                var item = dataspecialService.LoadEntites(o => o.ID == ID).First();
                viewmodel = new DataSpecialViewModel
                {
                    ID = item.ID,
                    Data_Special_Name = item.Data_Special_Name,
                    Data_Special_Url = item.Data_Special_Url,
                    Data_Special_Desc = item.Data_Special_Desc,
                    CreateDateTime = item.CreateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                };
            }
            else { viewmodel = new DataSpecialViewModel(); }
            ViewData["IsPublish"] = viewmodel.Data_IsPublish;
            ViewData.Model = viewmodel;
            return View();
        }
        [HttpPost]
        public JsonResult GetSpecialList()
        {
            var list = dataspecialService.LoadEntites().ToList();
            List<DataSpecialViewModel> detaillist = new List<DataSpecialViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataSpecialViewModel
                {
                    ID = item.ID,
                    Data_Special_Name = item.Data_Special_Name,
                    Data_Special_Url = item.Data_Special_Url,
                    Data_Special_Desc = item.Data_Special_Desc,
                    CreateDateTime = item.CreateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                    Action = string.Format("<a class='btn-link' href='/DataManage/EditDataSpecial?ID={0}' data-toggle='mainFrame'>Edit</a><span> | </span><a href='/DataManage/DeleteDataSpecial?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            if (detaillist != null && detaillist.Count() > 0)
            {
                return Json(new
                {
                    Results = detaillist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }
        //获取已发布的应用列表
        public JsonResult GetSpecialListByPublish()
        {
            var list = dataspecialService.LoadEntites(a => a.Data_IsPublish == 1).ToList();
            List<DataSpecialViewModel> detaillist = new List<DataSpecialViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataSpecialViewModel
                {
                    ID = item.ID,
                    Data_Special_Name = item.Data_Special_Name,
                    Data_Special_Url = item.Data_Special_Url,
                    Data_Special_Desc = item.Data_Special_Desc,
                    CreateDateTime = item.CreateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                });
            }
            if (detaillist != null && detaillist.Count() > 0)
            {
                return Json(new
                {
                    Results = detaillist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        //创建数据应用
        [IsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateDataSpecial(DataSpecialViewModel item)
        {
            try
            {
                data_special dinfo = new data_special
                {

                    ID = item.ID,
                    Data_Special_Name = item.Data_Special_Name,
                    Data_Special_Url = item.Data_Special_Url,
                    Data_Special_Desc = item.Data_Special_Desc,
                    CreateDateTime = DateTime.Now,
                    Data_Image = item.Data_Image,
                    DownloadCount = 0,
                    PageViewCount = 0,
                    Data_IsPublish = item.Data_IsPublish,

                };
                if (dinfo.ID > 0)
                {
                    dinfo.UpdateDateTime = DateTime.Now;
                    dinfo.CreateDateTime = item.CreateDateTime;
                    dataspecialService.UpdateEntity(dinfo);
                }
                else
                {
                    dinfo.UpdateDateTime = item.UpdateDateTime;
                    dinfo.CreateDateTime = DateTime.Now;
                    dataspecialService.AddEntity(dinfo);
                }
                var result = new { State = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { State = "Exception", Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //删除数据应用
        [IsLogin]
        public JsonResult DeleteDataSpecial(int ID)
        {
            try
            {
                var list = dataspecialService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (dataspecialService.DelEntity(list.First()))
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

        public JsonResult DataSpecialAttachmentImage(HttpPostedFileBase fileData)
        {
            string infoname = Convert.ToString(Request["Data_Special_Name"]);
            string filename = fileData.FileName.Insert(fileData.FileName.LastIndexOf('.'), "_" + infoname + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            string localPath = Server.MapPath(@"\Content\UploadImages\") + filename;
            FileInfo file = new FileInfo(localPath);
            if (!file.Exists)
            {
                Stream stream = new FileStream(localPath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = fileData.InputStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = fileData.InputStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                var result = new { State = "Success", FileName = filename };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { State = "Exites", Message = "文件已存在！" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 数据专题
     
        //后台专题管理
        public ActionResult DataApplicationManage()
        {
            return View();
        }

        //Home数据应用列表
        public ActionResult DataApplicationList(string startTime, string endTime, string AppName)
        {
            bool b = false;
            if (!string.IsNullOrEmpty(AppName))
            {
                ViewData["dataApplicationList"] = dataapplicationService.LoadEntites(a => a.Data_IsPublish == 1 && a.Data_Application_Name.Contains(AppName)).ToList();
                b = true;
            }
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                ViewData["dataApplicationList"] = dataapplicationService.LoadEntites(m => m.CreateDateTime > DateTime.Parse(startTime) && m.CreateDateTime < DateTime.Parse(endTime)).ToList();
                b = true;
            }
            if (!string.IsNullOrEmpty(AppName) && (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime)))
            {
                ViewData["dataApplicationList"] = dataspecialService.LoadEntites(m => m.Data_Special_Name.Contains(AppName) && m.CreateDateTime > DateTime.Parse(startTime) && m.CreateDateTime < DateTime.Parse(endTime)).ToList();
                b = true;
            }
            else
            {
                if (b == false)
                {
                    ViewData["dataApplicationList"] = dataapplicationService.LoadEntites().ToList();
                }
            }

            return View();
        }
        [IsLogin]
        public ActionResult EditDataApplication(int ID = 0)
        {
            DataApplicationViewModel viewmodel;
            if (ID != 0)
            {
                var item = dataapplicationService.LoadEntites(o => o.ID == ID).First();
                viewmodel = new DataApplicationViewModel
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                };
            }
            else { viewmodel = new DataApplicationViewModel(); }
            ViewData["IsPublish"] = viewmodel.Data_IsPublish;
            ViewData.Model = viewmodel;
            return View();
        }

        [HttpPost]
        public JsonResult GetApplitionList()
        {
            var list = dataapplicationService.LoadEntites().ToList();
            List<DataApplicationViewModel> detaillist = new List<DataApplicationViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataApplicationViewModel
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                    Action = string.Format("<a class='btn-link' href='/DataManage/EditDataApplication?ID={0}' data-toggle='mainFrame'>Edit</a><span> | </span><a href='/DataManage/DeleteDataApplition?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            if (detaillist != null && detaillist.Count() > 0)
            {
                return Json(new
                {
                    Results = detaillist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        //获取已发布的应用列表
        public JsonResult GetApplitionListByPublish(string starttime, string endtime)
        {
            List<data_application> list = null;
            if (!string.IsNullOrEmpty(starttime) || !string.IsNullOrEmpty(endtime))
            {
                var startTime = Convert.ToDateTime(starttime);
                var endTime = Convert.ToDateTime(endtime);
                list = dataapplicationService.LoadEntites(a => a.Data_IsPublish == 1 && (a.CreateDateTime >= startTime && a.CreateDateTime <= endTime)).ToList();
            }
            else
            {
                list = dataapplicationService.LoadEntites(a => a.Data_IsPublish == 1).ToList();
            }

            List<DataApplicationViewModel> detaillist = new List<DataApplicationViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataApplicationViewModel
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                });
            }
            if (detaillist != null && detaillist.Count() > 0)
            {
                return Json(new
                {
                    Results = detaillist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        //创建
        [IsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateDataApplition(DataApplicationViewModel item)
        {
            try
            {
                data_application dinfo = new data_application
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    Data_Image = item.Data_Image,
                    DownloadCount = 0,
                    PageViewCount = 0,
                    Data_IsPublish = item.Data_IsPublish,

                };
                if (dinfo.ID > 0)
                {
                    dinfo.CreateDateTime = item.CreateDateTime;
                    dinfo.UpdateDateTime = DateTime.Now;
                    dataapplicationService.UpdateEntity(dinfo);
                }
                else
                {
                    dinfo.UpdateDateTime = DateTime.Now;
                    dinfo.CreateDateTime = DateTime.Now;
                    dataapplicationService.AddEntity(dinfo);
                }
                var result = new { State = "Success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { State = "Exception", Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //删除
        [IsLogin]
        public JsonResult DeleteDataApplition(int ID)
        {
            try
            {
                var list = dataapplicationService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (dataapplicationService.DelEntity(list.First()))
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

        //上传图片
        public JsonResult DataApplicationAttachmentImage(HttpPostedFileBase fileData)
        {
            string infoname = Convert.ToString(Request["Data_Application_Name"]);
            string filename = fileData.FileName.Insert(fileData.FileName.LastIndexOf('.'), "_" + infoname + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            string localPath = Server.MapPath(@"\Content\UploadImages\") + filename;
            FileInfo file = new FileInfo(localPath);
            if (!file.Exists)
            {
                Stream stream = new FileStream(localPath, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = fileData.InputStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = fileData.InputStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                var result = new { State = "Success", FileName = filename };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { State = "Exites", Message = "文件已存在！" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //前台Application内容
        public ActionResult DataApplicationDetail(int ID = 0)
        {

            DataApplicationViewModel viewmodel;
            if (ID != 0)
            {
                var item = dataapplicationService.LoadEntites(o => o.ID == ID).First();
                viewmodel = new DataApplicationViewModel
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount,
                    Data_IsPublish = item.Data_IsPublish,
                };

                //增加访问量start
                data_application dinfo = new data_application
                {
                    ID = item.ID,
                    Data_Application_Name = item.Data_Application_Name,
                    Data_Application_Content = item.Data_Application_Content,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_Image = item.Data_Image,
                    DownloadCount = item.DownloadCount,
                    PageViewCount = item.PageViewCount + 1,
                    Data_IsPublish = item.Data_IsPublish,
                };
                if (dinfo.ID > 0)
                {
                    dataapplicationService.UpdateEntity(dinfo);
                }
               
                //增加访问量end
            }
            else { viewmodel = new DataApplicationViewModel(); }
            ViewData.Model = viewmodel;
            return View();
        }

        #endregion
    }
}
