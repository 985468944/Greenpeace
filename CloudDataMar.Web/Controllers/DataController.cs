using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudDataMar.BLL;
using CloudDataMar.IBLL;
using CloudDataMar.Model;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using CloudDataMar.Web.ViewModel;
using CloudDataMar.Web.Util;
using Microsoft.Office.Interop.Word;
using CloudDataMar.Web.App_Start;
using ICSharpCode.SharpZipLib.Zip;
using CloudSql;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CloudDataMar.Web.Controllers
{
    public class DataController : Controller
    {
        // GET: /Data/
        public static AutoResetEvent mainResetEvent = new AutoResetEvent(false); //用于线程之间的通信
        private data_type_infoService datatypeService = new data_type_infoService();
        private data_infoService datainfoService = new data_infoService();
        private data_detail_infoService detailService = new data_detail_infoService();
        private static data_accessory_infoService accessoryService = new data_accessory_infoService();
        private data_accessory_infoService accessoryService2 = new data_accessory_infoService();
        private data_announce_infoService dataAnnounceService = new data_announce_infoService();
        private user_roleService userRoleService = new user_roleService();
        private static string Script = "";
        private static string Condition1 = "";
        private static string Condition2 = "";
        private static string Condition3 = "";
        private static string Condition4 = "";
        private static string Condition5 = "";
        private static string Condition6 = "";
        CommonService commonService = new CommonService();
        public ActionResult DataInfo(int ID)
        {
            var typeinfolist = datatypeService.LoadEntites(o => o.ID == ID);
            if (typeinfolist != null && typeinfolist.Count() > 0)
            {
                var type = typeinfolist.First();
                type.data_info = datainfoService.LoadEntites(o => o.Data_Type_ID == ID).ToList();
                ViewData.Model = type;
            }
            return View();
        }
        public string strSub(string str)
        {
            if(str.Length>80)
            {
                return str.Substring(0,75)+"...";
            }else
            {
                return str;
            }
        }
        #region DataDetail

        public ActionResult DataDetailInfo(int InfoID)
        {
            ViewData["InfoID"] = InfoID;
            var dataDetailList = detailService.LoadEntites(o => o.Data_Info_ID == InfoID&&o.Data_IsPublish==1).ToList();
            if (dataDetailList != null && dataDetailList.Count>0)
            {//资料类型
                data_detail_info data = dataDetailList.First() as data_detail_info;
                 ViewData["IsOutSide"] = data.Data_IsOutSide;
            }
            else { ViewData["IsOutSide"] =0; }
            string dataInfoName = datainfoService.LoadEntites(d => d.ID == InfoID).Select(s => s.Data_Info_Name).FirstOrDefault().ToString();
            string dataInfoId= datainfoService.LoadEntites(d => d.ID == InfoID).Select(s => s.Data_Type_ID).FirstOrDefault().ToString();
            string dataTypeID = datatypeService.LoadEntites(d => d.ID == Convert.ToInt32(dataInfoId)).Select(s => s.ID).FirstOrDefault().ToString();
            string dataTypeName = datatypeService.LoadEntites(d => d.ID == Convert.ToInt32(dataInfoId)).Select(s => s.Data_Type_Name).FirstOrDefault().ToString();
            ViewData["Data_Type_Name"] = dataTypeName;
            ViewData["Data_Info_Name"] = dataInfoName;
            ViewData["Data_Type_ID"] = dataTypeID;
            return View();
        }

        public JsonResult GetDataDetailList(int InfoID, string starttime, string endtime)
        {
            if (string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(endtime))
            {
                starttime = "1899-01-01";
                endtime = "2999-01-01";
            }
            var startTime = Convert.ToDateTime(starttime);
            var endTime = Convert.ToDateTime(endtime);
            if (Session["UserLevel"] != null)
            {
                int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());
                int? roleDownTime = userRoleService.LoadEntites(r => r.RoleCode == userLevel).Select(u => u.RoleDownTime).FirstOrDefault();
                if (roleDownTime != 0)
                {//如果不是无限制下载
                    int downDays = (int.Parse(roleDownTime.ToString()) * 30);
                    DateTime sdateTime = DateTime.Now.AddDays(-downDays);//开始时间
                    DateTime edateTime = DateTime.Now;//当前时间为最大值,结束时间
                    if (DateTime.Compare(startTime, sdateTime) < 0)//如果startTime<sdateTime返回-1
                    {
                        startTime = sdateTime;
                    }
                    if (DateTime.Compare(endTime,edateTime) < 0)
                    {
                        endTime = edateTime;
                    }
                    //ViewData["roleDownTime"] = roleDownTime;
                }

            }

            var detaillist = detailService.LoadEntites(o => o.Data_Info_ID == InfoID&&o.UpdateDateTime>=startTime&&o.UpdateDateTime<=endTime.AddDays(1)&&o.Data_IsPublish==1).ToList();
            return Json(new
            {
                Results = detaillist
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataAccessorylList(int detailID)
        {
            var dataAccessory = accessoryService2.LoadEntites(a => a.Data_Detail_ID == detailID).ToList();
            return Json(new
            {
                Results = dataAccessory
            }, JsonRequestBehavior.AllowGet);
        }

        ///------zhangtao---20190509---add--------
        /// <summary>
        /// 动态获取数据及table的表头
        /// </summary>
        /// <returns></returns>
        ///------zhangtao---20190509---add--------
        /// <summary>
        /// 动态获取数据及table的表头
        /// </summary>
        /// <returns></returns>
        public ContentResult GetData(string search1 = "", string search2 = "", string search3 = "", string search4 = "", string search5 = "", string search6 = "")
        {   //1.获取列表数据
            string strSql = Script;
            string Conditions1 = Condition1;
            string Conditions2 = Condition2;
            string Conditions3 = Condition3;
            string Conditions4 = Condition4;
            string Conditions5 = Condition5;
            string Conditions6 = Condition6;
            
            if (string.IsNullOrWhiteSpace(strSql))
            {
                return Content("");
            }
            if (search1 != "")
            {
                Conditions1 = string.Format(Conditions1, search1);
                strSql = strSql + " " + Conditions1;
            }
            if (search2 != "")
            {
                Conditions2 = string.Format(Conditions2, search2);
                strSql = strSql + " " + Conditions2;
            }
            if (search3 != "")
            {
                Conditions3 = string.Format(Conditions3, search3);
                strSql = strSql + " " + Conditions3;
            }
            if (search4 != "")
            {
                Conditions4 = string.Format(Conditions4, search4);
                strSql = strSql + " " + Conditions4;
            }
            if (search5 != "")
            {
                Conditions5 = string.Format(Conditions5, search5);
                strSql = strSql + " " + Conditions5;
            }
            if (search6 != "")
            {
                Conditions6 = string.Format(Conditions6, search6);
                strSql = strSql + " " + Conditions6;
            }

            DataSet ds = commonService.QueryDataSet(strSql);
            System.Data.DataTable dt = ds.Tables[0];

            //2.拼接前台datatable columns参数需要的数据
            string s = "[";
            foreach (var item in dt.Columns)
            {
                //s = s + "{ title:" + item + "},";
                s = s + "{";
                s = s + "\"data\"";
                s = s + ":";
                s = s + "\"" + item + "\",";
                s = s + "\"title\"";
                s = s + ":";
                s = s + "\"" + item + "\"},";
            }
            string ss = s.Remove(s.Length - 1, 1);
            ss = ss + "]";
            var data = JsonConvert.SerializeObject(dt);

            //3.拼接json字符串
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"columns\"");
            sb.Append(":");
            sb.Append(ss);
            sb.Append(",\"data\"");
            sb.Append(":");
            sb.Append(data);
            sb.Append("}");
            return Content(sb.ToString());
        }


        ///------zhangtao---20190509---add--------
        /// <summary>
        /// 动态获取该列表的搜索列
        /// </summary>
        /// <returns></returns>
        public ContentResult GetDataSearchColumns()
        {
            ////1.拼接搜索需要的列-------------------版本1----------------------
            //string s = "";
            //var columnslist = datacolumns.LoadEntites(o => o.Data_Detail_Info_ID == 2).ToList();
            //foreach (var item in columnslist)
            //{
            //    s = s + item.ColumnsName + ",";
            //}
            //s = s.Remove(s.Length - 1, 1);
            //return Content(s);
            //Script = " select 8 from ddd where asdsa >= {0} and sddssd <= {1} and dsdsds = {0}";

            //------------------------版本2----------------------------------
            //string s = (Regex.Split(Script.ToLower(), "where", RegexOptions.IgnoreCase))[1].Trim();
            //string c = "";
            //string[] str = Regex.Split(s, "and", RegexOptions.IgnoreCase);
            //foreach (string item in str)
            //{

            //    c = c + item.Split('=', '>', '<')[0].Trim() + ",";
            //}
            //c.Remove(c.Length-1,1);           
            //return Content(c);
            return Content("");

        }


        public string DataTableToJson(System.Data.DataTable table)
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
        }



        [IsLogin(Role="member")]
        public void DownLoadFiles(string ids)
        {
            try
            {
                string str = Convert.ToString(ids);
                if (ids != null && !string.IsNullOrWhiteSpace(str))
                {
                    List<string> strPath = new List<string>();
                    string[] idlist = str.Substring(0, str.Length - 1).Split(',');
                    foreach (string id in idlist)
                    {
                        var list = accessoryService.LoadEntites(o => o.Data_Detail_ID == Convert.ToInt32(id));
                        if (list != null && list.Count() > 0)
                        {
                            foreach (var model in list)
                            {
                                var detail = detailService.LoadEntites(o => o.ID == Convert.ToInt32(id)).First();
                                if (detail != null)
                                {
                                    string strAddress = model.Data_Accessory_Address;
                                    //  strAddress = strAddress.Substring(0, strAddress.IndexOf("_" + id + "_")) + strAddress.Substring(strAddress.LastIndexOf("."));
                                    strPath.Add(Server.MapPath(@"\Content\Files\") + strAddress);
                                }
                            }
                        }
                    }
                    ZipFileDownload(strPath.ToArray(), "资料文件.zip");
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        //传入一个时间，返回是否允许下载该时段
        public string isAllowDown(string ids)
        {
                 string str = Convert.ToString(ids);
                 if (ids != null && !string.IsNullOrWhiteSpace(str))
                 {
                     string[] idlist = str.Substring(0, str.Length - 1).Split(',');
                     foreach (string id in idlist)
                     {
                         var stime = accessoryService.LoadEntites(o => o.Data_Detail_ID == Convert.ToInt32(id)).Select(u => u.UpdateDateTime).FirstOrDefault().ToString();
                         DateTime startTime = DateTime.Parse(stime);
                         if (Session["UserLevel"] != null)
                         {
                             int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());
                             int? roleDownTime = userRoleService.LoadEntites(r => r.RoleCode == userLevel).Select(u => u.RoleDownTime).FirstOrDefault();
                             if (roleDownTime != 0)
                             {//如果不是无限制下载
                                 int downDays = (int.Parse(roleDownTime.ToString()) * 30);
                                 DateTime sdateTime = DateTime.Now.AddDays(-downDays);//开始时间
                                 if (DateTime.Compare(startTime, sdateTime) < 0)//如果startTime<sdateTime返回-1
                                 {
                                     startTime = sdateTime;
                                     return startTime.ToShortDateString();
                                 }
                             }
                         }
                     }
                 }

            return "suc";
        }

        [IsLogin(Role = "member")]
        public void DownContentLoadFiles(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                int l3Id = 0;
                List<string> strPath = new List<string>();
                string[] idlist = ids.Substring(0, ids.Length - 1).Split(',');
                foreach (string id in idlist)
                {
                    var list = accessoryService.LoadEntites(o => o.ID == Convert.ToInt32(id));
                    if (list != null && list.Count() > 0)
                    {
                        foreach (var model in list)
                        {
                            string strAddress = model.Data_Accessory_Address;
                            strPath.Add(Server.MapPath(@"\Content\Files\") + strAddress);
                            l3Id = (int)model.Data_Detail_ID;
                        }
                    }
                }
                ZipFileDownload(strPath.ToArray(), "资料文件.zip");
                //下载计数
                if (l3Id == 0)
                    return;
                data_statisticsService dataStatisticsService = new data_statisticsService();
             
                IQueryable<data_statistics> sSet = dataStatisticsService.LoadEntites(o => o.item_id == l3Id && o.data_catagory == 3);

                if (sSet.Count() != 0)
                {
                    var sItem = sSet.First() as data_statistics;
                    data_statistics nds = new data_statistics
                    {
                        id = sItem.id,
                        data_catagory = sItem.data_catagory,
                        item_id = sItem.item_id,
                        browse_quantity = sItem.browse_quantity,
                        download_quantity = sItem.download_quantity + 1
                    };
                    dataStatisticsService.UpdateEntity(nds);
                }

                //
            }
        }
        /// <summary>  
        ///  批量进行多个文件压缩到一个文件  
        /// </summary>  
        /// <param name="files">文件列表(绝对路径)</param> 这里用的数组，你可以用list 等或者  
        /// <param name="zipFileName">生成的zip文件名称</param>  
        private void ZipFileDownload(string[] files, string zipFileName)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = null;

            using (ZipFile file = ZipFile.Create(ms))
            {
                file.BeginUpdate();
                file.NameTransform = new MyNameTransfom();
                foreach (var item in files)
                {
                    if (System.IO.File.Exists(item))
                    {
                        file.Add(item);
                    }
                }
                file.CommitUpdate();
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);   //读取文件内容(1次读ms.Length/1024M)  
                ms.Flush();
                ms.Close();
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("content-disposition", "attachment;filename=" + zipFileName);
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.End();
        }  
        /// <summary>
        /// 下载外部数据
        /// </summary>
        [IsLogin(Role = "member")]
        public FileStreamResult DownLoadOutsideFiles(string id)
        {
            //下载计数
            //data_statistics_infoService dataStatisticsService = new data_statistics_infoService();
            //int myId = int.Parse(id);
            //IQueryable<data_statistics> sSet = dataStatisticsService.LoadEntites(o => o.item_id == myId && o.data_catagory==3);

            //if (sSet.Count() != 0)
            //{
            //    var sItem = sSet.First() as data_statistics;
            //    data_statistics nds = new data_statistics
            //    {
            //        id = sItem.id,
            //        data_catagory = sItem.data_catagory,
            //        item_id = sItem.item_id,
            //        browse_quantity = sItem.browse_quantity,
            //        download_quantity = sItem.download_quantity + 1
            //    };
            //    dataStatisticsService.UpdateEntity(nds);
            //}
            
            //
            if (Session["UserLevel"] != null)
            {
                int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());
            }
            var detailInfo = detailService.LoadEntites(d => d.ID == Convert.ToInt32(id)).First();
            if (detailInfo != null)
            {
                if (!string.IsNullOrEmpty(detailInfo.Data_Detail_Script))
                {
                    string strSql = string.Format(detailInfo.Data_Detail_Script);
                    DataSet ds = commonService.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string fileName = detailInfo.Data_Detail_Name;
                        string filePath = Server.MapPath(@"\Content\Files\") + fileName + ".xlsx";
                        AsposeExcel.OutFileToDisk(ds, fileName, filePath);
                        return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", fileName + ".xlsx");
                    }
                }
            }
            return null;

        }
        #endregion

        #region 公告
        public ActionResult DataAnnounceDetail(int ID = 0)
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
            ViewData.Model = viewmodel;
            return View();
        }
        #endregion

        #region
        public ActionResult DataSearch(string searchtype, string searchtext)
        {
            DateTime sdateTime = Convert.ToDateTime("1900-01-01");
            ViewData["SearchText"] = searchtext;
            List<SearchDetailViewModel> list = new List<SearchDetailViewModel>();
            if (searchtype == "Home")
            {
                if (Session["UserLevel"] != null)
                {
                    int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());
                    int? roleDownTime = userRoleService.LoadEntites(r => r.RoleCode == userLevel).Select(u => u.RoleDownTime).FirstOrDefault();
                    if (roleDownTime != 0)
                    {//如果不是无限制下载
                        int downDays = (int.Parse(roleDownTime.ToString()) * 30);
                        sdateTime = DateTime.Now.AddDays(-downDays);//开始时间
                    }
                }

                var detaillist = detailService.LoadEntites(o => (o.Data_Detail_Name.Contains(searchtext) || o.Data_Detail_Source.Contains(searchtext)) && o.UpdateDateTime >= sdateTime&&o.Data_IsPublish==1);
                foreach (data_detail_info item in detaillist)
                {
                    var model = new SearchDetailViewModel
                    {
                        id = item.ID,
                        title = item.Data_Detail_Name,
                        detail = item.Data_Detail_Source,
                        isoutside=item.Data_IsOutSide,
                        updatedatetime = item.UpdateDateTime
                    };
                    list.Add(model);
                }
            }
            ViewData["DataCount"] = list.Count();
            return View(list);
        }

        #endregion

        #region 资料内容页
          [IsLogin(Role = "member")]
        public ActionResult DataDetailContentInfo(int ID = 0, int PageIndex = 1)
        {
            DataDetailInfoViewModel viewmodel;
            if (ID != 0)
            {
                if (ViewData["SearchStart"] == null)
                {
                    ViewData["SearchStart"] = DateTime.Now.ToShortDateString();
                }
               if (ViewData["SearchEnd"] == null)
                {
                    ViewData["SearchEnd"] = DateTime.Now.ToShortDateString();
                }
                
                    var item = detailService.LoadEntites(o => o.ID == ID).First();                  
                    var IsPublish = item.Data_IsPublish;
                    var IsOutSide = item.Data_IsOutSide;
                    //ModelState.IsValid                 
                    ViewData["IsPublish"] = IsPublish == 0 ? "未发布" : "已发布";
                    ViewData["IsOutSide"] = IsOutSide == 0 ? "内部资料" : "外部资料";
                    viewmodel = new DataDetailInfoViewModel
                    {
                        ID = item.ID,
                        Data_Info_ID = item.Data_Info_ID,
                        Data_Detail_Name = item.Data_Detail_Name,
                        Data_Detail_Source = item.Data_Detail_Source,
                        Data_Detail_Script = item.Data_Detail_Script,
                        Data_Detail_Desc = item.Data_Detail_Desc,
                        CreateDateTime = item.CreateDateTime,
                        UpdateDateTime = item.UpdateDateTime,
                        Data_IsOutSide = item.Data_IsOutSide,
                        SearchColumns1 = item.SearchColumns1,
                        SearchColumns2 = item.SearchColumns2,
                        SearchColumns3 = item.SearchColumns3,
                        SearchColumns4 = item.SearchColumns4,
                        SearchColumns5 = item.SearchColumns5,
                        SearchColumns6 = item.SearchColumns6,
                        SearchContidion1 = item.SearchContidion1,
                        SearchContidion2 = item.SearchContidion2,
                        SearchContidion3 = item.SearchContidion3,
                        SearchContidion4 = item.SearchContidion4,
                        SearchContidion5 = item.SearchContidion5,
                        SearchContidion6 = item.SearchContidion6
                 
                     };
                var dataInfoM = datainfoService.LoadEntites(u => u.ID == int.Parse(viewmodel.Data_Info_ID.ToString())).FirstOrDefault() as data_info;
                var dataTypeM = datatypeService.LoadEntites(u => u.ID == int.Parse(dataInfoM.Data_Type_ID.ToString())).FirstOrDefault() as data_type_info;

                var dataAccessory = accessoryService.LoadEntites(u => u.Data_Detail_ID == item.ID).ToList();

                var ename = dataInfoM.Data_Info_Name;
                var yname = dataTypeM.Data_Type_Name;
                var dataTypeID = dataTypeM.ID;
                ViewData["Data_Type_Name"] = yname;
                ViewData["Data_Info_Name"] = ename;
                ViewData["Data_Type_ID"] = dataTypeID;
                int i = 0;
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns1))
                {
                    i++;
                }
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns2))
                {
                    i++;
                }
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns3))
                {
                    i++;
                }
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns4))
                {
                    i++;
                }
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns5))
                {
                    i++;
                }
                if (!string.IsNullOrWhiteSpace(viewmodel.SearchColumns6))
                {
                    i++;
                }

                ViewData["SearchConditionCount"] = i;
                ViewData["Data_Accessory"] = dataAccessory;
            }
            else { viewmodel = new DataDetailInfoViewModel(); }
            ViewData.Model = viewmodel;
            Script = viewmodel.Data_Detail_Script;
            Condition1 = viewmodel.SearchContidion1;
            Condition2 = viewmodel.SearchContidion2;
            Condition3 = viewmodel.SearchContidion3;
            Condition4 = viewmodel.SearchContidion4;
            Condition5 = viewmodel.SearchContidion5;
            Condition6 = viewmodel.SearchContidion6;

            //if (!string.IsNullOrEmpty(viewmodel.Data_Detail_Script))
            //{
            //    var scriptSite = viewmodel.Data_Detail_Script;//查询外部数据的语句
            //    //-------------------------------------
            //    DataSet dsfrist = commonService.QueryDataSet(scriptSite);
            //    int RecordCount = dsfrist.Tables[0].Rows.Count;
            //    int PageSize = 10;
            //    int PageCount = RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;
            //    if (PageIndex < 1)
            //    {
            //        PageIndex = 1;//当前页码必须大于1
            //    }
            //    if (PageIndex >= PageCount)
            //    {
            //        PageIndex = PageCount;//当前页码不能大于总页码
            //    }
            //    //-------------------------------------
               
            //    scriptSite += @" Where time between '"+ ViewData["SearchStart"]+"' AND '"+ViewData["SearchEnd"]+"' ";
                
               
            //    scriptSite += string.Format(@" Limit {0},{1}", (PageIndex - 1) * PageSize, PageSize);
            //    if (scriptSite != null)
            //    {
            //        DataSet ds = commonService.QueryDataSet(scriptSite);
            //        if (ds != null && RecordCount > 0)
            //        {
            //            ViewData["data"] = ds;
            //        }
            //        else
            //        {
            //            ViewData["data"] = null;
            //        }
            //    }
            //    ViewData["ID"] = ID;
            //    ViewData["PageIndex"] = PageIndex;
            //    ViewData["PageCount"] = PageCount;
            //}
            //ViewData["Is_Outsite"] = viewmodel.Data_IsOutSide;

            //增加访问量start
           data_statisticsService dataStatisticsService = new data_statisticsService();
           IQueryable<data_statistics> sSet = dataStatisticsService.LoadEntites(o => o.item_id == ID && o.data_catagory==3);
             
           if (sSet.Count()!= 0)
            {
                var sItem = sSet.First() as data_statistics;
                data_statistics nds = new data_statistics
                {
                    id = sItem.id,
                    data_catagory = sItem.data_catagory,
                    item_id = sItem.item_id,
                    download_quantity = sItem.download_quantity,
                    browse_quantity = sItem.browse_quantity + 1,
                  
                };
                dataStatisticsService.UpdateEntity(nds);
            }
            else
            {
              
                data_statistics nds = new data_statistics
                {
                    item_id = ID,
                    data_catagory =3,
                    browse_quantity = 1,
               
                };
                dataStatisticsService.AddEntity(nds);
            }
            return View();
            
        }

        public JsonResult GetTime(string starttime, string endtime)
        {
            if (string.IsNullOrEmpty(starttime))
            {
                starttime = DateTime.Now.ToShortDateString();
            }
            if (string.IsNullOrEmpty(endtime))
            {
                endtime = DateTime.Now.ToShortDateString();
            }         
            var startTime = Convert.ToDateTime(starttime);
            var endTime = Convert.ToDateTime(endtime);
            ViewData["SearchStart"] = startTime;
            ViewData["SearchEnd"] = endTime;
            return Json(new
            {  }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public void writeTxt(string str)
        {
            FileStream fs = new FileStream("C:\\ak.txt", FileMode.Append);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();

        }
    }

    public class DownloadFile
    {
        public string title { get; set; }

        public string content { get; set; }

        public string address { get; set; }
    }

    public class MyNameTransfom : ICSharpCode.SharpZipLib.Core.INameTransform
    {

        #region INameTransform 成员

        public string TransformDirectory(string name)
        {
            return null;
        }

        public string TransformFile(string name)
        {
            return Path.GetFileName(name);
        }

        #endregion
    }
}
