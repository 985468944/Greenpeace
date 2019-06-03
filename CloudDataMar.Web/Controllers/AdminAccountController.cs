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
    public class AdminAccountController : Controller
    {
        //
        // GET: /AdminAccount/
        private data_type_infoService datatypeService = new data_type_infoService();
        private data_infoService datainfoService = new data_infoService();
        private data_detail_infoService detailService = new data_detail_infoService();
        private data_accessory_infoService accessoryService = new data_accessory_infoService();
        private data_announce_infoService dataAnnounceService = new data_announce_infoService();
        private data_statisticsService dataStatisticsService = new data_statisticsService();

        public ActionResult Home()
        {
            var list = datatypeService.LoadEntites().OrderBy(o => o.Data_Type_OrderNum).ToList();
            foreach (var item in list)
            {
                item.data_info = datainfoService.LoadEntites(o => o.Data_Type_ID == item.ID).ToList();
            }
            List<List<CloudDataMar.Model.data_type_info>> listGroup = new List<List<data_type_info>>();
            int j = 4;
            for (int i = 0; i < list.Count; i += 4)
            {
                List<CloudDataMar.Model.data_type_info> cList = new List<CloudDataMar.Model.data_type_info>();
                cList = list.Take(j).Skip(i).ToList();
                j += 4;
                listGroup.Add(cList);
            }
            ViewData["DataTypeList"] = listGroup;
            //公告start
            bool? isAnnounce = null;//是否显示公告 null不显示
            data_announce_infoService dataAnnounceBll = new data_announce_infoService();
            List<data_announce_info> lista = dataAnnounceBll.LoadEntites(a => a.IsPublish == 1).ToList();
            if (list.Count > 0)
            {
                isAnnounce = true;
                ViewBag.Announce = lista;
            }
            ViewBag.isAnnounce = isAnnounce;
            //公告end
            return View();
        }

        public ActionResult MenuList()
        {
            return View();
        }

        #region DataType
        [IsLogin]
        public ActionResult EditDataType()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDataTypeList()
        {
            var typelist = datatypeService.LoadEntites().OrderBy(o => o.Data_Type_OrderNum).ToList();
            List<DataTypeViewModel> datatypelist = new List<DataTypeViewModel>();
            foreach (var item in typelist)
            {
                datatypelist.Add(new DataTypeViewModel
                {
                    ID = item.ID,
                    Data_Type_OrderNum = item.Data_Type_OrderNum,
                    Data_Type_Name = item.Data_Type_Name,
                    Data_Type_Describe = item.Data_Type_Describe,
                    Data_Type_Image = item.Data_Type_Image,
                    data_info = item.data_info,
                    Action = string.Format("<a class='btn-link' href='#portlet-config' data-toggle='modal'>Edit</a><span> | </span><a href='/AdminAccount/DeleteDataType?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            if (datatypelist != null && datatypelist.Count() > 0)
            {
                return Json(new
                {
                    Results = datatypelist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }
        [HttpPost]
        public JsonResult GetDetailByTypeId(string dataTypeID)
        {
            if (!string.IsNullOrEmpty(dataTypeID))
            {
                var listdataInfo= datainfoService.LoadEntites(e => e.Data_Type_ID == Convert.ToInt32(dataTypeID));
                return Json(listdataInfo, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [IsLogin]
        [HttpPost]
        public JsonResult CreateDataType(DataTypeViewModel item)
        {
            try
            {
                data_type_info dt = new data_type_info
                {
                    Data_Type_Describe = item.Data_Type_Describe,
                    Data_Type_OrderNum = item.Data_Type_OrderNum,
                    Data_Type_Image = item.Data_Type_Image,
                    Data_Type_Name = item.Data_Type_Name,
                    data_info = item.data_info,
                    ID = item.ID,
                    CreateDateTime = DateTime.Now
                };
                if (dt.ID > 0)
                {
                    datatypeService.UpdateEntity(dt);
                }
                else
                {
                    datatypeService.AddEntity(dt);
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

        //[IsLogin]
        [HttpPost]
        public JsonResult AttachmentImage(HttpPostedFileBase fileData)
        {
            string typename = Convert.ToString(Request["DataTypeName"]);
            string typeordernum = Convert.ToString(Request["DataTypeOrderNum"]);
            string filename = fileData.FileName.Insert(fileData.FileName.LastIndexOf('.'), "_" + typename + "_" + typeordernum + "" + DateTime.Now.ToString("yyyyMMddHHmmss"));
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

        [IsLogin]
        public JsonResult DeleteDataType(int ID)
        {
            try
            {
                var list = datatypeService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (datatypeService.DelEntity(list.First()))
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

        #endregion

        #region DataInfo
        [IsLogin]
        public ActionResult EditDataInfo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDataInfoList()
        {
            var typeID = Request["Data_Type_ID2"];
            var typelist = datainfoService.LoadEntites().ToList(); ;
            if (!string.IsNullOrEmpty(typeID) && typeID!="0")
            {
                typelist = datainfoService.LoadEntites(u => u.Data_Type_ID == Convert.ToInt32(typeID)).ToList();
            }
            List<DataInfoViewModel> datainfolist = new List<DataInfoViewModel>();
            foreach (var item in typelist)
            {
                datainfolist.Add(new DataInfoViewModel
                {
                    Data_Info_Describe = item.Data_Info_Describe,
                    Data_Info_Image = item.Data_Info_Image,
                    Data_Info_Name = item.Data_Info_Name,
                    data_detail_info = item.data_detail_info,
                    Data_Type_ID = item.Data_Type_ID,
                    ID = item.ID,
                    Action = string.Format("<a  class='btn-link' href='#portlet-config' data-toggle='modal'>Edit</a><span> | </span><a href='/AdminAccount/DeleteDataInfo?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            return Json(new
            {
                Results = datainfolist
            }, JsonRequestBehavior.AllowGet);
        }

        [IsLogin]
        [HttpPost]
        public JsonResult CreateDataInfo(DataInfoViewModel item)
        {
            try
            {
                data_info dinfo = new data_info
                {
                    Data_Info_Describe = item.Data_Info_Describe,
                    Data_Info_Image = item.Data_Info_Image,
                    Data_Info_Name = item.Data_Info_Name,
                    data_detail_info = item.data_detail_info,
                    ID = item.ID,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    Data_Type_ID = item.Data_Type_ID,
                    data_type_info = item.data_type_info
                };
                if (dinfo.ID > 0)
                {
                    datainfoService.UpdateEntity(dinfo);
                }
                else
                {
                    datainfoService.AddEntity(dinfo);
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

        //[IsLogin]
        public JsonResult DataInfoAttachmentImage(HttpPostedFileBase fileData)
        {
            string infoname = Convert.ToString(Request["DataInfoName"]);
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

        [IsLogin]
        public JsonResult DeleteDataInfo(int ID)
        {
            try
            {
                var list = datainfoService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (datainfoService.DelEntity(list.First()))
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

        #endregion

        #region DataDetailInfo
        [IsLogin]
        public ActionResult EditDataDetail()
        {
            return View();
        }
       
        //首页用，判断是否外部数据
        public static int? IsOutSide(int infoID)
        {
            if (infoID>0)
            {
                data_detail_infoService dat = new data_detail_infoService();
               var detailModel = dat.LoadEntites(d => d.Data_Info_ID == Convert.ToInt32(infoID)&&d.Data_IsOutSide==1);
                if (detailModel.ToList().Count>0)
                {
                    return detailModel.First().ID;
                }
            }
            return 0;
        }

        [HttpPost]
        public JsonResult GetDataDetailInfoList()
        {
            var infoID = Request["Data_Info_ID"];
            var list = detailService.LoadEntites().ToList();
            if (!string.IsNullOrEmpty(infoID) && infoID != "0")
            {
                list = detailService.LoadEntites(d => d.Data_Info_ID == Convert.ToInt32(infoID)).ToList();
            }
            List<DataDetailInfoViewModel> detaillist = new List<DataDetailInfoViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataDetailInfoViewModel
                {
                    Data_Detail_Name = item.Data_Detail_Name,
                    Data_Detail_Source = item.Data_Detail_Source,
                    Data_Info_ID = item.Data_Info_ID,
                    ID = item.ID,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_IsOutSide=item.Data_IsOutSide,
                    Action = string.Format("<a class='btn-link' href='/AdminAccount/EditDataAccessory?ID={0}' data-toggle='mainFrame'>Edit</a><span> | </span><a href='/AdminAccount/DeleteDataDetailInfo?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            return Json(new
            {
                Results = detaillist
            }, JsonRequestBehavior.AllowGet);
        }

        [IsLogin]
        public JsonResult DeleteDataDetailInfo(int ID)
        {
            try
            {
                var list = detailService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (detailService.DelEntity(list.First()))
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

        //[IsLogin]
        [HttpPost]
        public JsonResult AttachmentFile(HttpPostedFileBase fileData)
        {
            int detailid = Convert.ToInt32(Request["DetailID"]);
            string filename = fileData.FileName.Insert(fileData.FileName.LastIndexOf('.'), "_" + detailid + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));

            string localPath = Server.MapPath(@"\Content\Files\") + filename;
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
                string tr = string.Format("<tr role='row' class='odd'><td class='left sorting_1'>{0}</td><td class=' left'>{1}</td><td class=' left'>{2}</td><td class=' left'><a class='btn-delete'>Delete</a></td></tr>", 0, filename, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                var result = new { State = "Success", FileName = filename, Data = tr };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { State = "Exites", Message = "文件已存在！" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region DataStatistics
        [IsLogin]
        public ActionResult DataStatistics()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStatistics()
        {
            var infoID = Request["Data_Info_ID"];
           
           
            var list2 = dataStatisticsService.LoadEntites(o => o.data_catagory == 3).ToList();
            var list = detailService.LoadEntites().ToList();
            if (!string.IsNullOrEmpty(infoID) && infoID != "0")
            {
                list = detailService.LoadEntites(d => d.Data_Info_ID == Convert.ToInt32(infoID)).ToList();
            }
            List<DataStatisticsViewModel> detaillist = new List<DataStatisticsViewModel>();
            foreach (var item in list)
            {

                foreach (var iq in list2) { 
                    if(item.ID == iq.item_id)
                        detaillist.Add(new DataStatisticsViewModel
                        {
                            id = item.ID,
                            name = item.Data_Detail_Name,
                            browse_quantity = iq.browse_quantity,
                            download_quantity = iq.download_quantity
                   
                        });
                }
            }
            return Json(new
            {
                Results = detaillist
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AnnounceManage(公告管理)
        [IsLogin]
        public ActionResult AnnounceManage()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAnnounceInfoList()
        {
            var list = dataAnnounceService.LoadEntites().ToList();
            List<DataAnnounceViewModel> detaillist = new List<DataAnnounceViewModel>();
            foreach (var item in list)
            {
                detaillist.Add(new DataAnnounceViewModel
                {
                    ID = item.ID,
                    Title = item.Title,
                    Content = item.Content,
                    CreatedBy = item.CreatedBy,
                    CreateDate = item.CreateDate,
                    PublishBy=item.PublishBy,
                    PublishDate=item.PublishDate,
                    IsPublish = item.IsPublish,
                    Action = string.Format("<a class='btn-link' href='/AdminAccount/EditDataAnnounce?ID={0}' data-toggle='mainFrame'>Edit</a><span> | </span><a href='/AdminAccount/DeleteDataAnnounce?ID={0}' class='btn-delete'>Delete</a>", item.ID)
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
        [IsLogin]
        public ActionResult EditDataAnnounce(int ID = 0)
        {
            DataAnnounceViewModel viewmodel;
            if (ID != 0)
            {
                var item = dataAnnounceService.LoadEntites(o => o.ID == ID).First();
                viewmodel = new DataAnnounceViewModel
                {
                    ID = item.ID,
                    Title = item.Title,
                    Content = item.Content,
                    CreatedBy = item.CreatedBy,
                    CreateDate = item.CreateDate,
                    PublishBy = item.PublishBy,
                    PublishDate = item.PublishDate,
                    IsPublish = item.IsPublish,
                };
            }
            else { viewmodel = new DataAnnounceViewModel(); }
            ViewData["IsPublish"] = viewmodel.IsPublish;
            ViewData.Model = viewmodel;
            return View();
        }

        [IsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateDataAnnounce(DataAnnounceViewModel item)
        {
            try
            {
                data_announce_info dinfo = new data_announce_info
                {
                    ID=item.ID,
                    Title = item.Title,
                    Content = item.Content,
                    PublishBy = Session["UserName"].ToString(),
                    PublishDate = DateTime.Now,
                    IsPublish = item.IsPublish,
                };
                if (dinfo.ID > 0)
                {
                    dataAnnounceService.UpdateEntity(dinfo);
                }
                else
                {
                     dinfo.CreatedBy = Session["UserName"].ToString();
                     dinfo.CreateDate = DateTime.Now;
                    dataAnnounceService.AddEntity(dinfo);
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

        [IsLogin]
        public JsonResult DeleteDataAnnounce(int ID)
        {
            try
            {
                var list = dataAnnounceService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (dataAnnounceService.DelEntity(list.First()))
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
        #endregion


        #region accessory
        [IsLogin]
        public ActionResult EditDataAccessory(int ID = 0)
        {
            DataDetailInfoViewModel viewmodel;
            if (ID != 0)
            {
                var item = detailService.LoadEntites(o => o.ID == ID).First();
                viewmodel = new DataDetailInfoViewModel
                {
                    ID = item.ID,
                    Data_Info_ID = item.Data_Info_ID,
                    Data_Detail_Name = item.Data_Detail_Name,
                    Data_Detail_Source = item.Data_Detail_Source,
                    Data_Detail_Script=item.Data_Detail_Script,
                    Data_Detail_Desc = item.Data_Detail_Desc,
                    CreateDateTime = item.CreateDateTime,
                    UpdateDateTime = item.UpdateDateTime,
                    Data_IsOutSide=item.Data_IsOutSide,
                    Data_IsPublish=item.Data_IsPublish
                };
            }
            else { viewmodel = new DataDetailInfoViewModel(); }
            ViewData.Model = viewmodel;
            return View();
        }

        [IsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateDataAccessory(DataDetailInfoViewModel item,string postCon, string postStr)
        {
            var jsonData = postStr.fromJson<tmpData>();
            string state = "NotAllow";
             try
            {
                data_detail_info dinfo = new data_detail_info
                {
                    Data_Detail_Name = item.Data_Detail_Name,
                    Data_Detail_Source = item.Data_Detail_Source,
                    Data_Info_ID = item.Data_Info_ID,
                    Data_Detail_Script=item.Data_Detail_Script,
                    Data_Detail_Desc = postCon, 
                    ID = item.ID,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    Data_IsOutSide=item.Data_IsOutSide,
                    Data_IsPublish=item.Data_IsPublish
                };
                if (isAllowTogget(dinfo.Data_Info_ID, dinfo.Data_IsOutSide))
                {
                    if (dinfo.ID > 0)
                    {
                        detailService.UpdateEntity(dinfo);
                    }
                    else
                    {
                        detailService.AddEntity(dinfo);
                    }
                    for (int i = 0; i < jsonData.files.Count(); i++)
                    {
                        if (jsonData.files[i].CreateDateTime != null && Convert.ToInt32(jsonData.files[i].ID) == 0)
                        {
                            data_accessory_info dainfo = new data_accessory_info()
                            {
                                ID = Convert.ToInt32(jsonData.files[i].ID),
                                Data_Detail_ID = dinfo.ID,
                                Data_Accessory_Address = jsonData.files[i].Data_Accessory_Address,
                                CreateDateTime = jsonData.files[i].CreateDateTime,
                                UpdateDateTime = DateTime.Now,
                            };
                            accessoryService.AddEntity(dainfo);
                        }
                    }
                    state = "Success";
                }
                 var result = new { State = state }; 
                 return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { State = "Exception", Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //参数：二级目录ID，当前要添加的数据类型
        public bool isAllowTogget(int? dataInfoID,int? isOutSide)
        {
            if (isOutSide == 0)
            { //如果要添加内部数据
                var deList = detailService.LoadEntites(d => d.Data_Info_ID == dataInfoID && d.Data_IsOutSide == 1).ToList();
                if (deList != null && deList.Count() > 0)
                {
                    return false;
                }
            }
            else
            {//如果要添加外部数据
                var deList = detailService.LoadEntites(d => d.Data_Info_ID == dataInfoID && d.Data_IsOutSide == 0).ToList();
                if (deList != null && deList.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }
        public JsonResult GetDataAccessoryList(int DetailID)
        {
            var list = accessoryService.LoadEntites(o => o.Data_Detail_ID == DetailID).ToList();
            List<DataAccessoryInfoViewModel> accessorylist = new List<DataAccessoryInfoViewModel>();
            foreach (var item in list)
            {
                accessorylist.Add(new DataAccessoryInfoViewModel
                {
                    ID = item.ID,
                    Data_Detail_ID = item.Data_Detail_ID,
                    Data_Accessory_Address = item.Data_Accessory_Address,
                    Data_Accessory_Describe = item.Data_Accessory_Describe,
                    UpdateDateTime = item.UpdateDateTime,
                    CreateDateTime = item.CreateDateTime,
                    Action = string.Format("<a href='/AdminAccount/DeleteDataAccessory?ID={0}' class='btn-delete'>Delete</a>", item.ID)
                });
            }
            if (accessorylist != null && accessorylist.Count() > 0)
            {
                return Json(new
                {
                    Results = accessorylist
                }, JsonRequestBehavior.AllowGet);
            }
            else { return null; }
        }

        [IsLogin]
        public JsonResult DeleteDataAccessory(int ID)
        {
            try
            {
                var list = accessoryService.LoadEntites(o => o.ID == ID);
                if (list != null && list.Count() > 0)
                {
                    if (accessoryService.DelEntity(list.First()))
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

        #endregion

        /// <summary>
        /// 截取过滤字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        public string SubString(string str,int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            return str;
        }
    }

    public class tmpData
    {
        public tmpfilsData[] files { get; set; }
    }

    public class tmpfilsData
    {
        public string ID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public string Data_Accessory_Address { get; set; }
    }
}
