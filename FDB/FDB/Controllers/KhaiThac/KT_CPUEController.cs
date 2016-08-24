using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Common;
using FDB.DataAccessLayer;
using FDB.Models;
using PagedList;
using System.Data.Entity.Validation;
using Excel;
using System.Text;
using System.IO;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;
using Aron.Sinoai.OfficeHelper;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace FDB.Controllers
{
    public class KT_CPUEController : FDBController
    {
        FDBContext db = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }

        #region "Tim kiem"
        public ActionResult Index(ViewModelSearchKT_CPUE t)
        {
            string ma_TinhTP = string.Empty;

            if (t.THANG == null) t.THANG = DateTime.Today.Month;
            if (t.NAM == null) t.NAM = DateTime.Today.Year;


            initialCategorySearchAction(ref ma_TinhTP);

            var cpue = db.KT_CPUE.Where(o => (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                && (t.DM_NhomNgheID == null || o.DM_NhomNgheID == t.DM_NhomNgheID)
                                                && (t.NAM == null || o.NGAY_CAP_BEN.Value.Year == t.NAM)
                                                && (t.THANG == null || o.NGAY_CAP_BEN.Value.Month == t.THANG)
                                                //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                            ).OrderByDescending(m => m.ID);

            ViewBag.TotalRow = cpue.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = t.Page ?? 1;

            t.SearchResults = cpue.ToPagedList(pageNumber, pageSize);


            return View(t);
        }


        #endregion

        #region "them moi"
        public ActionResult Create()
        {
            KT_CPUE n = new KT_CPUE();

            n.NGAY_NM = DateTime.Today;
            n.NGUOI_NM = User.Identity.Name;

            initialCategoryCreateAction();

            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_CPUE t)
        {
            if (ModelState.IsValid)
            {
                var cpue = db.KT_CPUE.Where(c => c.MA_TINHTP == t.MA_TINHTP 
                                                && c.DM_NhomNgheID == t.DM_NhomNgheID 
                                                && c.DNHOM_TAUID == t.DNHOM_TAUID 
                                                && c.SO_DK.ToLower().Equals(t.SO_DK.ToLower())
                                                && c.NGAY_CAP_BEN.Value.Month == t.NGAY_CAP_BEN.Value.Month
                                                && c.NGAY_CAP_BEN.Value.Year == t.NGAY_CAP_BEN.Value.Year
                                                );

                // validate server code
                string msgErrs = string.Empty;
                if (t.NGAY_THU_PHIEU.HasValue && t.NGAY_THU_PHIEU.Value > DateTime.Today)
                {
                    msgErrs += string.Format("Ngày thu phiếu [{0}] không được lớn hơn ngày hiện tại", t.NGAY_THU_PHIEU.Value.ToShortDateString()) + "<br />";
                }

                if (t.NGAY_XUAT_BEN.Value > DateTime.Today)
                {
                    msgErrs += string.Format("Ngày xuất bến [{0}] không được lớn hơn ngày hiện tại", t.NGAY_XUAT_BEN.Value.ToShortDateString()) + "<br />";
                }

                if (t.NGAY_CAP_BEN.Value > DateTime.Today)
                {
                    msgErrs += string.Format("Ngày cập bến [{0}] không được lớn hơn ngày hiện tại", t.NGAY_CAP_BEN.Value.ToShortDateString()) + "<br />";
                }

                if (cpue.Count() > 0)
                {
                    KT_CPUE cp = cpue.FirstOrDefault();

                    msgErrs += String.Format("Đã tồn tại thông tin phỏng vấn sản lượng của tàu có số đăng ký [{0}]", cp.SO_DK) + "<br />";
                    //Inline_Danger(String.Format("Đã tồn tại thông tin phỏng vấn sản lượng của tàu có số đăng ký [{0}]",cp.SO_DK), true);
                }

                if (string.IsNullOrEmpty(msgErrs))
                {
                    t.NGAY_NM = DateTime.Today;
                    t.NGUOI_NM = User.Identity.Name;

                    db.KT_CPUE.Add(t);
                    db.SaveChanges();

                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Phỏng vấn sản lượng"));

                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
            }
            initialCategoryCreateAction();

            return View(t);
        }

        #endregion

        #region "Sua"
        public ActionResult Edit(int ID)
        {
            KT_CPUE t = db.KT_CPUE.Find(ID);
            if (t == null)
            {
                return HttpNotFound();
            }
            initialCategoryEditAction();

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_CPUE t)
        {
            if (ModelState.IsValid)
            {
                t.NGAY_NM = DateTime.Today;
                t.NGUOI_NM = User.Identity.Name;

                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Phỏng vấn sản lượng"));


                return RedirectToAction("Index");
            }
            initialCategoryEditAction();



            return View(t);
        }
        #endregion

        #region "Xoa"
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return PartialView("_Delete");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KT_CPUE t = await db.KT_CPUE.FindAsync(id);


            db.KT_CPUE.Remove(t);

            await db.SaveChangesAsync();

            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Phỏng vấn sản lượng"));

            return Json(new { success = true });
        }

        #endregion

        public void initialCategoryCreateAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NHOMNGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }

        public void initialCategoryEditAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NHOMNGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
        }

        public void initialCategorySearchAction(ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NHOMNGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ma_Tinh = curUser.MA_TINHTP;
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }


        public JsonResult Load_Tau_By_SoDK(string so_Dang_Ky)
        {
            ApplicationUser curUser = getCurrentUser();

            var dk = db.KT_TAUTHUYEN.Where(m => m.SO_DK.ToLower().Equals(so_Dang_Ky.ToLower()) && m.MA_TINHTP == curUser.MA_TINHTP).FirstOrDefault();

            if (dk != null)
            {
                return Json(
                    new
                    {
                        SO_THUYEN_VIEN = dk.SO_THUYEN_VIEN
                    ,
                        CHU_PHUONG_TIEN = dk.CHU_PHUONG_TIEN
                    ,
                        KT_CHIEU_DAI = dk.KT_CHIEU_DAI
                    ,
                        KT_TONG_CONG_SUAT = dk.KT_TONG_CONG_SUAT
                    ,
                        DNHOM_TAUID = dk.DNHOM_TAUID
                    ,
                        NGHE_CHINH_ID = dk.NGHE_CHINH_ID
                    }
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KT_TAUTHUYEN(), JsonRequestBehavior.AllowGet);
            }
        }


        #region"Import excel"

        public FileResult KT_CPUEDownLoadFile()
        {
            this.CreateExcelFile();

            String FullPathFileName = GENERATED_FILE_NAME;
            string contentType = "application/vnd.ms-excel";
            string extension = new FileInfo(FullPathFileName).Extension;
            switch (extension)
            {
                case ".xls":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }
            return FDB.Common.Helpers.DownLoadFile(FullPathFileName, "FDB_Template_Import_CPUE" + extension, contentType);
        }


        //Create excel file:

        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_CPUE_FileDownload.xlsx");// System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_CPUE_FileDownload.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_CPUE.xlsx");

        private void CreateExcelFile()
        {

            using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_NAME, GENERATED_FILE_NAME))
            {
                helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                helper.CurrentSheetName = "DanhMuc";

                //helper.CurrentPosition = new CellRef("A2");
                //helper.InsertRange("row_empty");

              
                //fill danh mục chuẩn:
                Dictionary<string, IEnumerable<List<object>>> dictDanhMuc = this.getAllDanhMucChuan();

                CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("TinhThanhPho", new List<string> { "tinhthanhpho1" });
                helper.CurrentPosition = new CellRef("A3");
                helper.InsertRange(row_tinhthanhpho, dictDanhMuc["DTINHTP"]);

                CellRangeTemplate row_nhomtau = helper.CreateCellRangeTemplate("NhomTau", new List<string> { "nhomtau1" });
                helper.CurrentPosition = new CellRef("B4");
                helper.InsertRange(row_nhomtau, dictDanhMuc["DNHOM_TAU"]);


                CellRangeTemplate row_nghe = helper.CreateCellRangeTemplate("Nghe", new List<string> { "nghe1" });
                helper.CurrentPosition = new CellRef("C4");
                helper.InsertRange(row_nghe, dictDanhMuc["DM_NHOMNGHE"]);

               
                helper.DeleteSheet("Sheet2");
                helper.CurrentSheetName = "KT_CPUE";

            }
        }

        //get all danh mục chuẩn:
        private Dictionary<string, IEnumerable<List<object>>> getAllDanhMucChuan()
        {
            //tạo Dictionary để lưu IEnumerable<List<object>> các danh mục, mỗi Key,value tương ứng với 1 bảng danh mục
            Dictionary<string, IEnumerable<List<object>>> dictItems = new Dictionary<string, IEnumerable<List<object>>>();

            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj;

            //get Tỉnh TP
            var selects = db.DTINHTP.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects[loop].TEN_TINHTP });

            }
            dictItems.Add("DTINHTP", lstobj.AsEnumerable<List<object>>());

            //get nhóm tàu
            var selects1 = db.DNHOM_TAU.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects1.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects1[loop].Name });

            }
            dictItems.Add("DNHOM_TAU", lstobj.AsEnumerable<List<object>>());

          
            //get nhóm nghề
            var selects4 = db.DM_NHOMNGHE.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects4.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects4[loop].TenNhomNghe });

            }
            dictItems.Add("DM_NHOMNGHE", lstobj.AsEnumerable<List<object>>());

           
            return dictItems;
        }

        public ActionResult ImportExcel()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase fileUpload)
        {
            StringBuilder strValidationError = new StringBuilder(string.Empty);
            string strValiteDataTable = "";
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                if (extension.StartsWith(".xls") || extension.StartsWith(".xlsx"))
                {
                    string _filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/" + Path.GetFileNameWithoutExtension(fileUpload.FileName) + DateTime.Now.ToString("ddMMyyyyhhMMss") + extension);
                    fileUpload.SaveAs(_filePath);
                    DataTable dt = FDB.Common.Helpers.ConvertExcelToDataTable(_filePath);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //step1 : đang kiểm tra dữ liệu

                        DataTable dtData = this.CreateNewDataTableSecondRowIsColumnName(FDB.Common.Helpers.ConvertExcelToDataTable(_filePath));
                        strValiteDataTable = this.ValidateDataTable(dtData);
                        //if (dtData.Columns.Contains("SO_DK"))
                        if (string.IsNullOrEmpty(strValiteDataTable))
                        {
                            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._KT_CPUEmapPropertyNameToText);

                            this.InitDictDanhMuc();
                            DateTimeFormatInfo dateFormat = new CultureInfo("vi-VN", false).DateTimeFormat;

                            for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
                            {
                                DataRow r = dtData.Rows[rowIndex];
                                for (int i = 0; i < dtData.Columns.Count; i++)
                                {
                                    switch (dtData.Columns[i].ColumnName)
                                    {

                                        case "MA_TINHTP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Mã tỉnh tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictTThanhPho.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictTThanhPho[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột tỉnh/TP tại dòng " + (rowIndex + 4).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }


                                            }
                                            break;

                                        case "DNHOM_TAUID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nhóm tàu tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictNhomTau.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictNhomTau[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột nhóm tàu tại dòng " + (rowIndex + 4).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;


                                        //Validate Nghề :
                                        case "DM_NhomNgheID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nghề khai thác tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictNghe.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictNghe[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Nghề chính tại dòng " + (rowIndex + 4).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;

                                        case "SO_DK":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số ĐK tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            
                                            }
                                            break;

                                        case "KT_TONG_CONG_SUAT":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tổng công suất tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;

                                        case "NGAY_XUAT_BEN":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày xuất bến tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString(), dateFormat);
                                            }
                                            break;
                                        case "NGAY_CAP_BEN":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày cập bến tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            else
                                            {
                                                
                                               // r[i] = DateTime.Parse(r[i].ToString(), new CultureInfo("vi-VN"));
                                                r[i] = Convert.ToDateTime(r[i].ToString(), dateFormat);
                                            }
                                            break;

                                        case "NOI_CAP_BEN":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nơi cập bến tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "SO_NGAY_DANH_CA":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số ngày đánh cá tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "TONG_SAN_LUONG":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tổng sản lượng tại dòng " + (rowIndex + 4).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "NGAY_THU_PHIEU":
                                            if (r[i].Equals(DBNull.Value))
                                            {

                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString(), dateFormat);
                                            }
                                            break;

                                    }
                                }
                            }
                        ExitForRow: ;
                            //đây là Nhãn: khi validate dữ liệu phát hiện lỗi sẽ nhảy ngay đến nhãn này, thoát khỏi vòng for ngay, tránh việc check tất cả các row của table, gây chậm hệ thống.

                            if (strValidationError != null && strValidationError.Length > 0 && !strValidationError.Equals(string.Empty))
                            {
                                ViewBag.ThongBao = strValidationError.ToString();
                                ViewBag.success = "0";

                            }
                            else
                            {

                                //Remove row duplicate SO_DK :
                                DataTable dtDataDistinct = dtData; //FDB.Common.Helpers.RemoveDuplicateRows(dtData, "SO_DK");

                                //Insert row by row:
                                string _count = (dtDataDistinct.Rows.Count).ToString();
                                int i = 0;
                                using (db.Database.Connection)
                                {
                                    try
                                    {
                                        using (var txt = new TransactionScope())
                                        {
                                            try
                                            {
                                                foreach (DataRow r in dtDataDistinct.Rows)
                                                {
                                                    //string _SO_DK = r["SO_DK"].ToString();
                                                    bool isExists = false; //db.KT_CPUE.Any(o => o.SO_DK == _SO_DK);
                                                    if (!isExists)
                                                    {
                                                        KT_CPUE _ktCPUE = FDB.Common.Helpers.MapDataRowToObject<KT_CPUE>(r);
                                                        _ktCPUE.NGAY_NM = DateTime.Now;
                                                        _ktCPUE.NGUOI_NHAP_PHIEU = User.Identity.Name;
                                                        db.KT_CPUE.Add(_ktCPUE);
                                                        db.SaveChanges();
                                                        i++;
                                                    }
                                                }

                                                txt.Complete();
                                            }
                                            catch (Exception ex)
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Có lỗi xảy ra trong quá trình insert data! <br/>" + ex.ToString() + "</span> <br/>");
                                                txt.Dispose();
                                            }
                                            finally
                                            {
                                                txt.Dispose();
                                            }
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        strValidationError.Append("<span style=\"color:red\">Có lỗi xảy ra trong quá trình mở Connection! <br/>" + ex.ToString() + "</span> <br/>");
                                        if (db.Database.Connection.State != ConnectionState.Closed)
                                            db.Database.Connection.Close();
                                    }

                                }

                                if (strValidationError != null && strValidationError.Length > 0 && !strValidationError.Equals(string.Empty))
                                {
                                    TempData["_SUCCESS"] = "";
                                    ViewBag.success = "0";
                                    ViewBag.ThongBao = strValidationError.ToString();
                                }
                                else
                                {
                                    strValidationError.Clear();
                                    strValidationError.Append("<span style=\"color:blue\">Import file thành công !</span><br/>");
                                    ViewBag.ThongBao = strValidationError.ToString();
                                    ViewBag.success = "1";
                                    TempData["_SUCCESS"] = "Import Excel điều tra sản lượng thành công " + dtData.Rows.Count.ToString()+" bản ghi!";
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                        else
                        {
                            TempData["_SUCCESS"] = "";
                            strValidationError.Clear();
                            strValidationError.Append("<span style=\"color:red\">Mẫu file không đúng, vui lòng tải file mẫu excel!</span><br/>" + strValiteDataTable);
                            ViewBag.ThongBao = strValidationError.ToString();

                        }
                    }
                    else
                    {
                        TempData["_SUCCESS"] = "";
                        strValidationError.Clear();
                        strValidationError.Append("<span style=\"color:red\">File không có dữ liệu!</span><br/>");
                        ViewBag.ThongBao = strValidationError.ToString();

                    }
                    if (System.IO.File.Exists(_filePath))
                    {
                        System.IO.File.Delete(_filePath);
                    }
                }
            }

            return View();
        }

        //Create new DataTable:hàm này dùng để tạo 1 DataTable mới, với columnname sẽ là row thứ 2 của DataTable truyền vào
        // Add giá trị từ row thứ 3 của datatable cũ  vào DataTable mới
        public DataTable CreateNewDataTableSecondRowIsColumnName(DataTable dt)
        {
            DataTable dtNew = new DataTable("KT_CPUE");
            //Tạo column với column name từ row thứ 2 của dt cũ:
            DataRow rHeader = dt.Rows[1];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dtNew.Columns.Add(rHeader[i].ToString(), typeof(string));
            }

            //Add các row từ row thứ 3 của dt cũ:
            DataRow row;
            for (int i = 2; i < dt.Rows.Count; i++)
            {
                row = dtNew.NewRow();
                for (int j = 0; j < dtNew.Columns.Count; j++)
                {
                    row[dtNew.Columns[j].ColumnName] = dt.Rows[i][j];
                }
                dtNew.Rows.Add(row);
            }
            return dtNew;
        }


        private Dictionary<string, string> _KT_CPUEmapPropertyNameToText = new Dictionary<string, string>()
        {

                {"Tỉnh/TP", "MA_TINHTP"},
                {"Người nhập phiếu", "NGUOI_NHAP_PHIEU"},
                {"Người thu phiếu", "NGUOI_THU_PHIEU"},
                {"Ngày thu phiếu", "NGAY_THU_PHIEU"},
                {"Số ĐK", "SO_DK"},
                {"Chủ phương tiện", "CHU_PHUONG_TIEN"},
                {"Số thuyền viên", "SO_THUYEN_VIEN"},
                {"Chiều dài", "KT_CHIEU_DAI"},
                {"Tổng công suất", "KT_TONG_CONG_SUAT"},
                {"Nhóm tàu", "DNHOM_TAUID"},
                {"Nghề khai thác", "DM_NhomNgheID"},
                {"Chiều dài vàng lưới rê", "RE_CHIEU_DAI_VANG_LUOI"},
                {"Chiều cao lưới rê", "RE_CHIEU_CAO_LUOI"},
                {"Kích thước mặt lưới rê 2A", "RE_KICH_THUOC_MAT_LUOI_2A"},
                {"Chiều dài giềng phao kéo", "KEO_CHIEU_DAI_GIENG_PHAO"},
                {"Chiều dài giềng chì kéo", "KEO_CHIEU_DAI_GIENG_CHI"},
                {"Kích thước mắt lưới ở đụt lưới 2a", "KEO_KICH_THUOC_MAT_LUOI_DUT_2A"},
                {"Chiều dài lưới vây", "VAY_CHIEU_DAI_LUOI"},
                {"Chiều cao lưới vây", "VAY_CHIEU_CAO_LUOI"},
                {"Kích thước mặt lưới tùng 2a", "VAY_KICH_THUOC_MAT_LUOI_TUNG_2A"},
                {"Số bóng đèn vây", "VAY_SO_BONG_DEN"},
                {"Công suất bóng đèn vây", "VAY_CONG_SUAT_BONG_DEN"},
                {"Chiều dài vàng câu", "CAU_CHIEU_DAI_VANG_CAU"},
                {"Tổng số lưới câu", "CAU_TONG_SO_LUOI_CAU"},
                {"Kích thước chủ yếu của loại nghề", "KHAC_KICH_THUOC_CHU_YEU"},
                {"Ngày xuất bến", "NGAY_XUAT_BEN"},
                {"Nơi cập bến", "NOI_CAP_BEN"},
                {"Ngày cập bến", "NGAY_CAP_BEN"},
                {"Ngư trường", "NGU_TRUONG"},
                {"Tổng số mẻ lưới", "TONG_SO_ME_LUOI"},
                {"Số ngày đánh cá", "SO_NGAY_DANH_CA"},
                {"Số ngày đánh cá tháng trước", "SO_NGAY_DANH_CA_THANG_TRUOC"},
                {"Sản lượng chuyển tải", "SAN_LUONG_CHUYEN_TAI"},
                {"Tổng sản lượng", "TONG_SAN_LUONG"},
                {"Sản lượng tôm", "SAN_LUONG_TOM"},
                {"Giá bán tôm", "GIA_BAN_TOM"},
                {"Thành tiền tôm", "THANH_TIEN_TOM"},
                {"Sản lượng cá chọn", "SAN_LUONG_CA_CHON"},
                {"Giá bán cá chọn", "GIA_BAN_CA_CHON"},
                {"Thành tiền cá chọn", "THANH_TIEN_CA_CHON"},
                {"Sản lượng cá xô", "SAN_LUONG_CA_XO"},
                {"Giá bán cá xô", "GIA_BAN_CA_XO"},
                {"Thành tiền cá xô", "THANH_TIEN_CA_XO"},
				
				{"Sản lượng cá tạp", "SAN_LUONG_CA_TAP"},
                {"Giá bán cá tạp", "GIA_BAN_CA_TAP"},
                {"Thành tiền cá tạp", "THANH_TIEN_CA_TAP"},

                {"Sản lượng cá ngừ đại dương", "SAN_LUONG_CA_NGU_DD"},
                {"Giá bán cá ngừ đại dương", "GIA_BAN_CA_NGU_DD"},
                {"Thành tiền cá ngừ đại dương", "THANH_TIEN_CA_NGU_DD"},
				
				{"Sản lượng mực ống", "SAN_LUONG_MUC_ONG"},
                {"Giá bán mực ống", "GIA_BAN_MUC_ONG"},
                {"Thành tiền mực ống", "THANH_TIEN_MUC_ONG"},
				
				{"Sản lượng mực nang", "SAN_LUONG_MUC_NANG"},
                {"Giá bán mực nang", "GIA_BAN_MUC_NANG"},
                {"Thành tiền mực nang", "THANH_TIEN_MUC_NANG"},
				
				{"Sản lượng khác", "SAN_LUONG_KHAC"},
                {"Giá bán khác", "GIA_BAN_KHAC"},
                {"Thành tiền khác", "THANH_TIEN_KHAC"}
                
              
        };

        //Hàm dùng để validate file excel import có đầy đủ số cột qui định không:
        public string ValidateDataTable(DataTable dt)
        {
            string sWarining = "";
            foreach (string s in this._KT_CPUEmapPropertyNameToText.Keys)
            {
                if (dt.Columns.IndexOf(s) < 0)
                    sWarining += String.Format("<span style=\"color: #b94a48;\">&nbsp;&nbsp +) Thiếu cột: {0}</span> <br/>", s);
            }
            return sWarining;
        }
        //Lists dictionary dùng để mapping Text to Value các danh mục:
        private Dictionary<string, string> _dictTThanhPho = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictNhomTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictNghe = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok

        //Chưa tối ưu, sẽ tối ưu sau
        private void InitDictDanhMuc()
        {
            var dbQuanTri = new ApplicationDbContext();
            string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
            var objTThanhPho = db.DTINHTP.Where(o => (string.IsNullOrEmpty(Ma_TinhTP)||o.MA_TINHTP == Ma_TinhTP)).ToList();
            objTThanhPho.ForEach(o => _dictTThanhPho.Add(o.TEN_TINHTP, o.MA_TINHTP));

            //tạo Dictionary nhóm tàu
            var objNhomTau = db.DNHOM_TAU.ToList();
            objNhomTau.ForEach(o => _dictNhomTau.Add(o.Name, o.ID));

            //tạo Dictionary nhóm nghề
            var objNghe = db.DM_NHOMNGHE.ToList();
            objNghe.ForEach(o => _dictNghe.Add(o.TenNhomNghe, o.DM_NhomNgheID));


        }

        #endregion
    }
}