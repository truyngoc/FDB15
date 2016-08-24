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
    public class KT_BACController : FDBController
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
        public ActionResult Index(ViewModelSearchKT_BAC t)
        {
            string ma_TinhTP = string.Empty;

            if (t.THANG == null) t.THANG = DateTime.Today.Month;
            if (t.NAM == null) t.NAM = DateTime.Today.Year;


            initialCategorySearchAction(ref ma_TinhTP);

            var bac = db.KT_BAC.Where(o => (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                && (t.DM_NhomNgheID == null || o.DM_NhomNgheID == t.DM_NhomNgheID)
                                                && (t.NAM == null || o.NAM == t.NAM)
                                                && (t.THANG == null || o.THANG == t.THANG)
                                                //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                            ).OrderByDescending(m => m.ID);

            ViewBag.TotalRow = bac.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = t.Page ?? 1;

            t.SearchResults = bac.ToPagedList(pageNumber, pageSize);


            return View(t);
        }


        #endregion

        #region "them moi"
        public ActionResult Create()
        {
            KT_BAC n = new KT_BAC();

            n.NGAY_NM = DateTime.Today;
            n.NGUOI_NM = User.Identity.Name;
            n.THANG = DateTime.Today.Month;
            n.NAM = DateTime.Today.Year;

            initialCategoryCreateAction();

            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_BAC t)
        {
            if (ModelState.IsValid)
            {
                var bac = db.KT_BAC.Where(b => b.MA_TINHTP == t.MA_TINHTP
                                            && b.NGAY_DIEU_TRA == t.NGAY_DIEU_TRA
                                            && b.DM_NhomNgheID == t.DM_NhomNgheID
                                            && b.DNHOM_TAUID == t.DNHOM_TAUID
                                        );

                // validate server code
                string msgErrs = string.Empty;
                if (t.NGAY_DIEU_TRA.Value  > DateTime.Today)
                {
                    msgErrs += string.Format("Ngày điều tra [{0}] không được lớn hơn ngày hiện tại", t.NGAY_DIEU_TRA.Value.ToShortDateString()) + "<br />";
                    //Inline_Danger(string.Format("Ngày điều tra [{0}] không được lớn hơn ngày hiện tại", t.NGAY_DIEU_TRA.Value.ToShortDateString()), true);
                }
                
                if (t.NGAY_DIEU_TRA.Value.Month != t.THANG)
                {
                    msgErrs += string.Format("Ngày điều tra [{0}] không thuộc tháng điều tra [{1}] đã chọn", t.NGAY_DIEU_TRA.Value.ToShortDateString(), t.THANG) + "<br />";
                    //Inline_Danger(string.Format("Ngày điều tra [{0}] không thuộc tháng điều tra [{1}] đã chọn", t.NGAY_DIEU_TRA.Value.ToShortDateString(), t.THANG), true);
                }
                
                if (t.NGAY_DIEU_TRA.Value.Year != t.NAM)
                {
                    msgErrs += string.Format("Ngày điều tra [{0}] không thuộc năm điều tra [{1}] đã chọn", t.NGAY_DIEU_TRA.Value.ToShortDateString(), t.NAM) + "<br />";
                    //Inline_Danger(string.Format("Ngày điều tra [{0}] không thuộc năm điều tra [{1}] đã chọn", t.NGAY_DIEU_TRA.Value.ToShortDateString(), t.NAM), true);
                }
                
                if (t.THANG > DateTime.Today.Month && t.NAM == DateTime.Today.Year)
                {
                    msgErrs += string.Format("Tháng điều tra {0} không được lớn hơn tháng hiện tại {1}/{2}", t.THANG, DateTime.Today.Month, DateTime.Today.Year) + "<br />";
                    //Inline_Danger(string.Format("Tháng điều tra {0} không được lớn hơn tháng hiện tại {1}/{2}", t.THANG,DateTime.Today.Month, DateTime.Today.Year));
                }
                
                if (t.NAM > DateTime.Today.Year)
                {
                    msgErrs += string.Format("Năm điều tra {0} không được lớn hơn năm hiện tại {1}", t.NAM, DateTime.Today.Year) + "<br />";
                    //Inline_Danger(string.Format("Năm điều tra {0} không được lớn hơn năm hiện tại {1}", t.NAM, DateTime.Today.Year));
                }
                
                if (t.SO_TAU_CHON_MAU_DI_BIEN > t.SO_TAU_CHON_MAU)
                {
                    msgErrs += "Số tàu chọn mẫu đi biển không thể lớn số tàu đi biển" + "<br />";
                    //Inline_Danger("Số tàu chọn mẫu đi biển không thể lớn số tàu đi biển", true);
                }

                if (bac.Count() > 0)
                {
                    KT_BAC b = bac.FirstOrDefault();

                    msgErrs += string.Format("Đã tồn tại thông tin điều tra BAC của Ngày điều tra [{0}]", t.NGAY_DIEU_TRA.Value.ToShortDateString()) + "<br />";
                    //Inline_Danger(string.Format("Đã tồn tại thông tin điều tra BAC của Ngày điều tra [{0}]", t.NGAY_DIEU_TRA.Value.ToShortDateString()), true);
                }
                // end validate

                                

                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.KT_BAC.Add(t);
                    db.SaveChanges();

                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "BAC"));

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
            KT_BAC t = db.KT_BAC.Find(ID);
            if (t == null)
            {
                return HttpNotFound();
            }
            initialCategoryEditAction();

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_BAC t)
        {
            if (ModelState.IsValid)
            {
                t.NGAY_NM = DateTime.Today;
                t.NGUOI_NM = User.Identity.Name;

                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "BAC"));


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
            KT_BAC t = await db.KT_BAC.FindAsync(id);


            db.KT_BAC.Remove(t);

            await db.SaveChangesAsync();

            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "BAC"));

            return Json(new { success = true });
        }

        #endregion


        #region "load danh muc"
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

        #endregion


        #region"Import excel"

        public FileResult KT_BACDownLoadFile()
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
            return FDB.Common.Helpers.DownLoadFile(FullPathFileName, "FDB_Template_Import_BAC" + extension, contentType);
        }


        //Create excel file:

        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_BAC_FileDownload.xlsx");// System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_CPUE_FileDownload.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_BAC.xlsx");

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
                helper.CurrentSheetName = "KT_BAC";

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
                    DataTable dtData = FDB.Common.Helpers.ConvertExcelToDataTable(_filePath);

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        //step1 : đang kiểm tra dữ liệu
                        strValiteDataTable = this.ValidateDataTable(dtData);
                        //if (dtData.Columns.Contains("SO_DK"))
                        if (string.IsNullOrEmpty(strValiteDataTable))
                        {
                            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._KT_BACmapPropertyNameToText);

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
                                                strValidationError.Append("<span style=\"color:red\">Cột Mã tỉnh tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictTThanhPho.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictTThanhPho[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột tỉnh/TP tại dòng " + (rowIndex + 2).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }


                                            }
                                            break;

                                        case "DNHOM_TAUID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nhóm tàu tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictNhomTau.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictNhomTau[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột nhóm tàu tại dòng " + (rowIndex + 2).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;


                                        //Validate Nghề :
                                        case "DM_NhomNgheID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nghề khai thác tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictNghe.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictNghe[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Nghề chính tại dòng " + (rowIndex + 2).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;


                                        case "NGAY_DIEU_TRA":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày điều tra tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString(), dateFormat);
                                            }
                                            break;
                                        case "CAN_BO_DIEU_TRA":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Cán bộ điều tra tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }

                                            break;

                                        case "NAM":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Năm tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "THANG":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tháng tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "SO_TAU_CHON_MAU":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số tàu chọn mẫu tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;

                                            }
                                            break;
                                        case "SO_TAU_CHON_MAU_DI_BIEN":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số tàu chọn mẫu đi biển tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
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
                                                        KT_BAC _ktBAC = FDB.Common.Helpers.MapDataRowToObject<KT_BAC>(r);
                                                        _ktBAC.NGAY_NM = DateTime.Now;
                                                        _ktBAC.NGUOI_NM = User.Identity.Name;
                                                        db.KT_BAC.Add(_ktBAC);
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
                                    ViewBag.success = "0";
                                    ViewBag.ThongBao = strValidationError.ToString();
                                }
                                else
                                {
                                    strValidationError.Clear();
                                    strValidationError.Append("<span style=\"color:blue\">Import file thành công !</span><br/>");
                                    ViewBag.ThongBao = strValidationError.ToString();
                                    ViewBag.success = "1";
                                }
                            }
                        }
                        else
                        {

                            strValidationError.Clear();
                            strValidationError.Append("<span style=\"color:red\">Mẫu file không đúng, vui lòng tải file mẫu excel!</span><br/>" + strValiteDataTable);
                            ViewBag.ThongBao = strValidationError.ToString();

                        }
                    }
                    else
                    {
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
            DataTable dtNew = new DataTable("KT_BAC");
            //Tạo column với column name từ row thứ 2 của dt cũ:
            DataRow rHeader = dt.Rows[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dtNew.Columns.Add(rHeader[i].ToString(), typeof(string));
            }

            //Add các row từ row thứ 3 của dt cũ:
            DataRow row;
            for (int i = 1; i < dt.Rows.Count; i++)
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


        private Dictionary<string, string> _KT_BACmapPropertyNameToText = new Dictionary<string, string>()
        {

                {"Tỉnh/TP", "MA_TINHTP"},
                {"Ngày điều tra", "NGAY_DIEU_TRA"},
                {"Cán bộ điều tra", "CAN_BO_DIEU_TRA"},
                {"Nhóm tàu", "DNHOM_TAUID"},
                {"Nghề khai thác", "DM_NhomNgheID"},
                {"Năm", "NAM"},
                {"Tháng", "THANG"},
                {"Số tàu chọn mẫu", "SO_TAU_CHON_MAU"},
                {"Số tàu chọn mẫu đi biển", "SO_TAU_CHON_MAU_DI_BIEN"}
                
              
        };

        //Hàm dùng để validate file excel import có đầy đủ số cột qui định không:
        public string ValidateDataTable(DataTable dt)
        {
            string sWarining = "";
            foreach (string s in this._KT_BACmapPropertyNameToText.Keys)
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
            var objTThanhPho = db.DTINHTP.ToList();// db.DTINHTP.First(o => o.MA_TINHTP == Ma_TinhTP);
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