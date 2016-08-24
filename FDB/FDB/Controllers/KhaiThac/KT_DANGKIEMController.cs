using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
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
using System.Data.Entity;

using Microsoft.AspNet.Identity;

namespace FDB.Controllers
{
    public class KT_DANGKIEMController : FDBController
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
        //
        // GET: /KT_DANGKIEM/
        public ActionResult Index(ViewModelSearchKT_DANGKIEM d)
        {

            if (d.SearchButton == "Xuất Excel")
            {
                ExportExcel(d);
                //Dowload file:

                return File(GENERATED_FILE_EXPORT_NAME, "application/vnd.ms-excel", "KT_DANGKIEMExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");

            }
            else
            {
                string ma_TinhTP = string.Empty;

                initialCategorySearchAction(ref ma_TinhTP);

                var dangkiem = db.KT_DANGKIEM.Where(o => (string.IsNullOrEmpty(d.SO_SO_DANG_KIEM) || o.SO_SO_DANG_KIEM.ToLower().Contains(d.SO_SO_DANG_KIEM.ToLower()))
                                                    && (d.DLOAI_KIEM_TRA_KTID == null || o.DLOAI_KIEM_TRA_KTID == d.DLOAI_KIEM_TRA_KTID)
                                                    && (d.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == d.DTINH_TRANG_DANG_KIEMID)
                                                    && (d.DCAP_TAU_DANG_KIEMID == null || o.DCAP_TAU_DANG_KIEMID == d.DCAP_TAU_DANG_KIEMID)
                                                    && (d.NGAY_HH_DK_TU == null || o.HAN_DANG_KIEM >= d.NGAY_HH_DK_TU)
                                                    && (d.NGAY_HH_DK_DEN == null || o.HAN_DANG_KIEM <= d.NGAY_HH_DK_DEN)
                                                    && (string.IsNullOrEmpty(d.SO_DK) || o.SO_DK.ToLower().Contains(d.SO_DK.ToLower()))
                                                    && (string.IsNullOrEmpty(d.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(d.CHU_PHUONG_TIEN.ToLower()))
                    //&& ((string.IsNullOrEmpty(d.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(d.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == d.MA_TINHTP)
                                                    && ((string.IsNullOrEmpty(d.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(d.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == d.MA_TINHTP)
                                                    && (d.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == d.DCONG_DUNG_TAUID)
                                                ).OrderByDescending(m => m.ID);

                ViewBag.TotalRow = dangkiem.Count();
                int pageSize = FDB.Common.Constants.PageSize;
                int pageNumber = d.Page ?? 1;

                d.SearchResults = dangkiem.ToPagedList(pageNumber, pageSize);

                return View(d);
            }
        }

        #endregion

        #region "Them moi"
        public ActionResult Create()
        {
            initialCategoryCreateAction();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_DANGKIEM d)
        {
            if (ModelState.IsValid)
            {
                db.KT_DANGKIEM.Add(d);
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Đăng kiểm"));

                return RedirectToAction("Index");
            }

            initialCategoryCreateAction();

            return View();
        }
        #endregion

        #region "Sua"
        public ActionResult Edit(int ID)
        {

            KT_DANGKIEM t = db.KT_DANGKIEM.Find(ID);

            if (t == null)
            {
                return HttpNotFound();
            }

            initialCategoryEditAction();

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_DANGKIEM t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Đăng kiểm"));


                return RedirectToAction("Index");
            }
            initialCategoryEditAction();

            return View(t);
        }

        #endregion

        #region "xoa"
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return PartialView("_Delete");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> DeleteConfirmed(int ID)
        {
            KT_DANGKIEM t = db.KT_DANGKIEM.Find(ID);

            // xoa tau thuyen
            db.KT_DANGKIEM.Remove(t);

            await db.SaveChangesAsync();

            return Json(new { success = true });
        }

        #endregion




        public ActionResult RegisterShip(RegisterShip r)
        {
            this.Success(r.SO_SO_DANG_KIEM + " - " + r.TAU_PHAI_DANG_KIEM);
            return View();
        }



        #region "load danh muc"
        public void initialCategoryCreateAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }

        public void initialCategoryEditAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        public void initialCategorySearchAction(ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;

            ma_Tinh = curUser.MA_TINHTP;
        }


        public void initialCategoryStatisticsAction(ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DCONG_DUNG_TAUs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");
            //ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            //ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            //ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;

            ma_Tinh = curUser.MA_TINHTP;
        }


        [HttpPost]
        public JsonResult check_SO_SO_DANG_KIEM_Exist(string SO_SO_DANG_KIEM, int isEdit, int HinhThucKiemTra, string MA_TINHTP)
        {
            var dk = db.KT_DANGKIEM.FirstOrDefault(m => m.SO_SO_DANG_KIEM == SO_SO_DANG_KIEM && m.MA_TINHTP == MA_TINHTP && m.DLOAI_KIEM_TRA_KTID == 1);

            if (isEdit == 0 && HinhThucKiemTra == 1)
            {
                if (dk != null)
                {
                    return Json("Số sổ đăng kiểm đã tồn tại");
                }
            }
            else if (isEdit == 0 && HinhThucKiemTra != 1)
            {
                if (dk == null)
                {
                    return Json("Số sổ đăng kiểm không tồn tại trên hệ thống");
                }
            }

            return Json(true);
        }


        public JsonResult Load_Data_By_ID(string so_Dang_Kiem)
        {
            ApplicationUser curUser = getCurrentUser();

            var dk = db.KT_DANGKIEM.Where(m => m.SO_SO_DANG_KIEM.ToLower().Equals(so_Dang_Kiem.ToLower()) && m.MA_TINHTP == curUser.MA_TINHTP).FirstOrDefault();

            if (dk != null)
            {
                return Json(dk, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KT_DANGKIEM(), JsonRequestBehavior.AllowGet);
            }
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
                        TEN_TAU = dk.TEN_TAU
                    ,
                        CHU_PHUONG_TIEN = dk.CHU_PHUONG_TIEN
                    ,
                        KT_NAM_DONG = dk.KT_NAM_DONG
                    ,
                        KT_NOI_DONG = dk.KT_NOI_DONG
                    ,
                        DCONG_DUNG_TAUID = dk.DCONG_DUNG_TAUID
                    }
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KT_TAUTHUYEN(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Thong ke"

        public ActionResult Search(ViewModelSearchKT_DANGKIEM_ThongKe tk)
        {
            //string ma_TinhTP = string.Empty;

            //initialCategoryStatisticsAction(ref ma_TinhTP);
         

          //  // neu ko chon ma tinh tren dropdownlist thi se lay ma tinh cua user (ko chon thi se ko ra ket qua)
          //// if (tk.MA_TINHTP == null && !ma_TinhTP.StartsWith("Z")) tk.MA_TINHTP = ma_TinhTP;

            // lay thong tin bao cao
            tk.BaoCaoDangKiem = this.ThongKe(tk);

            return View(tk);
        }


        private Dictionary<string, int> ThongKe(ViewModelSearchKT_DANGKIEM_ThongKe tk)
        {
            Dictionary<string, int> _dictDangKiem = new Dictionary<string, int>();

            string ma_TinhTP = string.Empty;

            initialCategoryStatisticsAction(ref ma_TinhTP);
            // rows
            List<DNHOM_TAU> _lstDMNhomTau = db.DNHOM_TAU.ToList();

            // cols
            List<string> Columns_BaoCaoDangKiem = new List<string> { 
                { "TongSoTauDangKiem" },
                { "SoTauConHanDangKiem" },
                { "SoTauHetHanDangKiem" },
                { "KiemTra_HangNam" },
                { "KiemTra_DongMoi" },
                { "KiemTra_CaiHoan" },
                { "KiemTra_Khac" }
            };

            // du lieu
            var _lstDangKiem_MaxID = db.KT_DANGKIEM.GroupBy(m => m.SO_SO_DANG_KIEM)
                                                   .Select(s => s.Max(x => x.ID)).ToList();
            //thanhnc5:sua 30/05/2016 de khong chon dropdown tinh/tp thi hien thi het 
            var _lstDangKiem = db.KT_DANGKIEM.Where(d => 
                                                        //(d.MA_TINHTP == tk.MA_TINHTP)
                                                         ((string.IsNullOrEmpty(tk.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(tk.MA_TINHTP) && d.MA_TINHTP == ma_TinhTP) || d.MA_TINHTP == tk.MA_TINHTP)
                                                        && ((tk.DVAT_LIEU_VOID == null) || (d.DVAT_LIEU_VOID == tk.DVAT_LIEU_VOID))
                                                        && ((tk.DCONG_DUNG_TAUID == null) || (d.DCONG_DUNG_TAUID == tk.DCONG_DUNG_TAUID))
                                                        && (tk.NGAY_KIEM_TRA_TU == null || d.NGAY_KIEM_TRA >= tk.NGAY_KIEM_TRA_TU)
                                                        && (tk.NGAY_KIEM_TRA_DEN == null || d.NGAY_KIEM_TRA <= tk.NGAY_KIEM_TRA_DEN)
                                                        && _lstDangKiem_MaxID.Contains(d.ID)
                );


            var _lstDangKiem_TT = db.KT_DANGKIEM.Where(d =>
                //(d.MA_TINHTP == tk.MA_TINHTP)
                                                        ((string.IsNullOrEmpty(tk.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(tk.MA_TINHTP) && d.MA_TINHTP == ma_TinhTP) || d.MA_TINHTP == tk.MA_TINHTP)
                                                       && ((tk.DVAT_LIEU_VOID == null) || (d.DVAT_LIEU_VOID == tk.DVAT_LIEU_VOID))
                                                       && ((tk.DCONG_DUNG_TAUID == null) || (d.DCONG_DUNG_TAUID == tk.DCONG_DUNG_TAUID))
                                                       && _lstDangKiem_MaxID.Contains(d.ID)
               );


            // calc
            foreach (var col in Columns_BaoCaoDangKiem)
            {
                foreach (var nhomtau in _lstDMNhomTau)
                {
                    var iTK = 0;

                    switch (col)
                    {
                        case "TongSoTauDangKiem":
                            iTK = _lstDangKiem_TT.Where(d => d.DNHOM_TAUID == nhomtau.ID).Select(d => d.SO_SO_DANG_KIEM).Distinct().Count();
                            break;
                        case "SoTauConHanDangKiem":
                            iTK = _lstDangKiem_TT.Where(d => d.HAN_DANG_KIEM >= DateTime.Today && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                        case "SoTauHetHanDangKiem":
                            iTK = _lstDangKiem_TT.Where(d => d.HAN_DANG_KIEM < DateTime.Today && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                        case "KiemTra_HangNam":
                            // hang nam
                            iTK = _lstDangKiem.Where(d => d.DLOAI_KIEM_TRA_KTID == 2 && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                        case "KiemTra_DongMoi":
                            // dong moi (lan dau)
                            iTK = _lstDangKiem.Where(d => d.DLOAI_KIEM_TRA_KTID == 1 && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                        case "KiemTra_CaiHoan":
                            // cai hoan (bat thuong)
                            iTK = _lstDangKiem.Where(d => d.DLOAI_KIEM_TRA_KTID == 5 && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                        case "KiemTra_Khac":
                            // khac (trung gian, dinh ky)
                            iTK = _lstDangKiem.Where(d => (d.DLOAI_KIEM_TRA_KTID == 3 || d.DLOAI_KIEM_TRA_KTID == 4) && d.DNHOM_TAUID == nhomtau.ID).Count();
                            break;
                    }

                    _dictDangKiem.Add(tk.MA_TINHTP + "#" + nhomtau.ID + "#" + col.ToString(), iTK);
                }
            }


            return _dictDangKiem;
        }

        #endregion

        #region"Import excel"
        //Cấp phép tàu thuyền bằng file excel:
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
                        DataTable dtData = this.CreateNewDataTableSecondRowIsColumnName(FDB.Common.Helpers.ConvertExcelToDataTable(_filePath));
                        strValiteDataTable = this.ValidateDataTable(dtData);

                        if (string.IsNullOrEmpty(strValiteDataTable))
                        {
                            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._KT_DANGKIEMmapPropertyNameToText);

                            this.InitDictDanhMuc();
                            DateTimeFormatInfo dateFormat = new CultureInfo("en-AU", false).DateTimeFormat;

                            for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
                            {
                                DataRow r = dtData.Rows[rowIndex];
                                for (int i = 0; i < dtData.Columns.Count; i++)
                                {
                                    switch (dtData.Columns[i].ColumnName)
                                    {
                                        case "DLOAI_KIEM_TRA_KTID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Hình thức kiểm tra tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictDMHinhThucKiemTra.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictDMHinhThucKiemTra[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Hình thức kiểm tra tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }
                                            break;
                                        case "MA_TINHTP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tỉnh/TP tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictTThanhPho.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictTThanhPho[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Tỉnh/TP tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }


                                            }
                                            break;
                                        case "NGAY_CAP_SO_DANG_KIEM":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày cấp sổ đăng kiểm tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "SO_SO_DANG_KIEM":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số sổ đăng kiểm tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            break;

                                        case "NGAY_KIEM_TRA":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày biên bản KTKT tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;

                                        case "KT_SO_MAY_TAU":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số lượng máy tàu tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            break;
                                        case "DNHOM_TAUID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nhóm tàu tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictDMNhomTau.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictDMNhomTau[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Nhóm tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }


                                            }
                                            break;
                                        case "KT_TONG_CONG_SUAT":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tổng công suất (CV) tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }

                                            break;
                                        case "DTINH_TRANG_DANG_KIEMID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Kết luận chung tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictDMKetLuanChung.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictDMKetLuanChung[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Kết luận chung tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;
                                        case "DCAP_TAU_DANG_KIEMID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Cấp tàu tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictDMCapTau.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictDMCapTau[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Cấp tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;
                                        case "HAN_DANG_KIEM":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày hết hạn đăng kiểm tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;

                                        case "DVAT_LIEU_VOID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictDMVatLieuVo.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictDMVatLieuVo[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Vật liệu vỏ tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }
                                            break;

                                        case "NGAY_CAP_ATKT_TCA":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "NGAY_HH_ATKT_TCA":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;


                                    }
                                }
                            }
                        ExitForRow: ;

                            if (strValidationError != null && strValidationError.Length > 0 && !strValidationError.Equals(string.Empty))
                            {
                                ViewBag.ThongBao = strValidationError.ToString();
                                ViewBag.success = "0";

                            }
                            else
                            {
                                //Remove row duplicate SO_DK :

                                //Insert using SQLbulkCopy :
                                string tablename = "KT_DANGKIEM_TEMP_" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                                string SqlBulkConnectionString = System.Configuration.ConfigurationManager.AppSettings["SqlBulkCopyConnectionString"].ToString();
                                int _BatchSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BatchSize"].ToString());
                                //Begin 
                                using (db.Database.Connection)
                                {
                                    try
                                    {
                                        //Create temp table:
                                        var name = new SqlParameter("@tablename", tablename);
                                        db.Database.ExecuteSqlCommand("exec CreateTableKT_DANGKIEM_TEMP @tablename", name);
                                        db.SaveChanges();
                                        using (var txt = new TransactionScope())
                                        {
                                            try
                                            {
                                                using (var bcp = new SqlBulkCopy(SqlBulkConnectionString))
                                                {
                                                    bcp.BatchSize = _BatchSize;
                                                    bcp.DestinationTableName = tablename;
                                                    for (int _cIndex = 0; _cIndex < _KT_DANGKIEMmapPropertyNameToText.Count; _cIndex++)
                                                    {
                                                        bcp.ColumnMappings.Add(_KT_DANGKIEMmapPropertyNameToText.Values.ToList()[_cIndex], _KT_DANGKIEMmapPropertyNameToText.Values.ToList()[_cIndex]);
                                                    }
                                                    bcp.WriteToServer(dtData);
                                                }
                                                txt.Complete();
                                            }
                                            catch (Exception ex)
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Có lỗi xảy ra trong quá trình SqlBulkCopy! <br/>" + ex.ToString() + "</span> <br/>");
                                                txt.Dispose();
                                            }
                                            finally
                                            {
                                                txt.Dispose();
                                            }
                                        }
                                        //Merge data from Temp table KT_GIAYPHEP_TEMP to Table KT_GIAYPHEP
                                        //Merge complete then Drop Temp table 
                                        var name2 = new SqlParameter("@tablename", tablename);
                                        db.Database.ExecuteSqlCommand("exec MergeIntoDataKT_DANGKIEM @tablename", name2);
                                        db.SaveChanges();
                                        if (db.Database.Connection.State != ConnectionState.Closed)
                                            db.Database.Connection.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        strValidationError.Append("<span style=\"color:red\">Có lỗi xảy ra trong quá trình mở Connection! <br/>" + ex.ToString() + "</span> <br/>");
                                        if (db.Database.Connection.State != ConnectionState.Closed)
                                            db.Database.Connection.Close();
                                    }
                                    finally
                                    {
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
            DataTable dtNew = new DataTable("KT_DANGKIEM");
            //Tạo column với column name từ row thứ 2 của dt cũ:
            DataRow rHeader = dt.Rows[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dtNew.Columns.Add(rHeader[i].ToString(), dt.Columns[i].DataType);
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

        public FileResult KT_DANGKIEMDownLoadFile()
        {
            String FullPathFileName = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Import_DangKiem.xlsx");
            //string fileName = "FDB_Template_Import_DKTT.xls";
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
            return FDB.Common.Helpers.DownLoadFile(FullPathFileName, "FDB_Template_Import_DangKiem" + extension, contentType);
        }

        private Dictionary<string, string> _KT_DANGKIEMmapPropertyNameToText = new Dictionary<string, string>()
        {

                {"Hình thức kiểm tra", "DLOAI_KIEM_TRA_KTID"},
                {"Tỉnh/TP", "MA_TINHTP"},
                {"Số sổ đăng kiểm", "SO_SO_DANG_KIEM"},
                {"Ngày cấp sổ đăng kiểm", "NGAY_CAP_SO_DANG_KIEM"},
                {"Cơ quan đăng kiểm", "CO_QUAN_DANG_KIEM"},
                {"Đăng kiểm viên", "DANG_KIEM_VIEN"},
                {"Số biên bản KTKT", "SO_BB_KIEM_TRA_KT"},
                {"Ngày biên bản KTKT", "NGAY_KIEM_TRA"},
                {"Số chứng nhận ATKT tàu cá", "SO_AN_CHI_ATKT"},
                {"Ngày chứng nhận (ATKT)", "NGAY_CAP_ATKT_TCA"},
                {"Ngày hết hạn (ATKT) ", "NGAY_HH_ATKT_TCA"},
                {"Số đăng ký", "SO_DK"},
                {"Nơi đăng ký", "NOI_DANG_KY"},
                {"Năm đóng","KT_NAM_DONG"},
                {"Chủ phương tiện", "CHU_PHUONG_TIEN"},
                {"Lmax (m)", "KT_CHIEU_DAI"},
                {"Bmax (m)", "KT_CHIEU_RONG"},
                {"D (m)", "KT_CHIEU_CAO"},
                {"d (m)", "KT_MON_NUOC"},
                {"Mạn khô f (m)", "KT_MAN_KHO"},
                {"Ltk (m)", "KT_CHIEU_DAI_TK"},
                {"Tốc độ tàu (hl/h)", "KT_TOC_DO_TAU"},
                {"Số lượng máy tàu", "KT_SO_MAY_TAU"},
                {"Vật liệu vỏ", "DVAT_LIEU_VOID"},
                {"Tổng công suất (CV)", "KT_TONG_CONG_SUAT"},
                {"Nhóm tàu", "DNHOM_TAUID"},
                {"Ký hiệu máy", "M1_KY_HIEU_MAY"},
                {"Số máy", "M1_SO_MAY"},

                {"Nơi sản xuất", "M1_NOI_SX"},
                {"Năm chế tạo", "M1_NAM_CHE_TAO"},
                {"Hãng máy", "M1_HANG_MAY"},
                {"Vòng quay định mức(V/p)", "M1_VONG_QUAY"},
                {"Công suất", "M1_CONG_SUAT"},

                {"Ký hiệu máy 2", "M2_KY_HIEU_MAY"},
                {"Số máy 2", "M2_SO_MAY"},
                {"Nơi sản xuất 2", "M2_NOI_SX"},
                {"Năm chế tạo 2", "M2_NAM_CHE_TAO"},
                {"Hãng máy 2", "M2_HANG_MAY"},
                {"Vòng quay định mức(V/p) 2", "M2_VONG_QUAY"},
                {"Công suất 2", "M2_CONG_SUAT"},
                {"Kết luận chung", "DTINH_TRANG_DANG_KIEMID"},
                {"Cấp tàu", "DCAP_TAU_DANG_KIEMID"},
                {"Ngày hết hạn đăng kiểm", "HAN_DANG_KIEM"},
                {"Ghi chú", "GHI_CHU"}

                
        };

        public string ValidateDataTable(DataTable dt)
        {
            string sWarining = "";
            foreach (string s in this._KT_DANGKIEMmapPropertyNameToText.Keys)
            {
                if (dt.Columns.IndexOf(s) < 0)
                    sWarining += String.Format("<span stype=\"color:red\">Thiếu cột {0}</span> <br/>", s);
            }
            return sWarining;
        }

        //Lists dictionary dùng để mapping Text to Value các danh mục:
        private Dictionary<string, string> _dictTThanhPho = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictVungHoatDong = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMNghe = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMVatLieuVo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMNhomTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMKetLuanChung = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMCapTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictDMHinhThucKiemTra = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private void InitDictDanhMuc()
        {
            var dbQuanTri = new ApplicationDbContext();
            string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;

            var objTThanhPho = db.DTINHTP.First(o => o.MA_TINHTP == Ma_TinhTP);
            _dictTThanhPho.Add(objTThanhPho.TEN_TINHTP, objTThanhPho.MA_TINHTP);

            //DM nhóm nghề:
            var _objDMNghe = db.DM_NHOMNGHE.ToList();
            _objDMNghe.ForEach(o => _dictDMNghe.Add(o.TenNhomNghe, o.DM_NhomNgheID));

            //DM loại kiểm tra:
            var _objLoaiKiemTra = db.DLOAI_KIEM_TRA_KT.ToList();
            _objLoaiKiemTra.ForEach(o => _dictDMHinhThucKiemTra.Add(o.Name, o.ID));

            //DM vùng hoạt động:
            var objVungHoatDong = db.DVUNG_TUYEN.ToList();
            objVungHoatDong.ForEach(o => _dictVungHoatDong.Add(o.Name, o.ID));

            //DM vật liệu vỏ:
            var objVatLieuVo = db.DVAT_LIEU_VO.ToList();
            objVatLieuVo.ForEach(o => _dictDMVatLieuVo.Add(o.Name, o.ID));

            //DM nhóm tàu:
            var objNhomTau = db.DNHOM_TAU.ToList();
            objNhomTau.ForEach(o => _dictDMNhomTau.Add(o.Name, o.ID));

            //DM  Kết luận chung:
            var _objDMKetLuanChung = db.DTINH_TRANG_DANG_KIEM.ToList();
            _objDMKetLuanChung.ForEach(o => _dictDMKetLuanChung.Add(o.Name, o.ID));

            //DM cấp tàu
            var _objDMCapTau = db.DCAP_TAU_DANG_KIEM.ToList();
            _objDMCapTau.ForEach(o => _dictDMCapTau.Add(o.Name, o.ID));

        }

        #endregion

        #region"Working Files"
        private static String GENERATED_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_DangKiem.xlsx");
        #endregion



        #region "Export excel"



        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchKT_DANGKIEM t, List<String> lstMA_TTP)
        {
            //get nhóm nghề
            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(ref ma_TinhTP);


            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMA_TTP == null)
                lstMA_TTP = new List<String>();
            var dangkiem = db.KT_DANGKIEM.Where(o => (string.IsNullOrEmpty(t.SO_SO_DANG_KIEM) || o.SO_SO_DANG_KIEM.ToLower().Contains(t.SO_SO_DANG_KIEM.ToLower()))
                                             && (t.DLOAI_KIEM_TRA_KTID == null || o.DLOAI_KIEM_TRA_KTID == t.DLOAI_KIEM_TRA_KTID)
                                             && (t.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == t.DTINH_TRANG_DANG_KIEMID)
                                             && (t.DCAP_TAU_DANG_KIEMID == null || o.DCAP_TAU_DANG_KIEMID == t.DCAP_TAU_DANG_KIEMID)
                                             && (t.NGAY_HH_DK_TU == null || o.HAN_DANG_KIEM >= t.NGAY_HH_DK_TU)
                                             && (t.NGAY_HH_DK_DEN == null || o.HAN_DANG_KIEM <= t.NGAY_HH_DK_DEN)
                                             && (string.IsNullOrEmpty(t.SO_DK) || o.SO_DK.ToLower().Contains(t.SO_DK.ToLower()))
                                             && (string.IsNullOrEmpty(t.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(t.CHU_PHUONG_TIEN.ToLower()))
                                             && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                             && (t.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == t.DCONG_DUNG_TAUID)
                                         );



            var models = dangkiem.ToList();


            //lay nhom nghe 
            //var dictNhomNghes = db.DM_NHOMNGHE.ToDictionary(o => o.DM_NhomNgheID);

            //lay nhom tau
            var dictNhomTaus = db.DNHOM_TAU.ToDictionary(o => o.ID);



            for (int loop = 0; loop < models.Count(); loop++)
            {


                lstobj.Add(new List<object> { 
                                                models[loop].DTINHTP == null || models[loop].DTINHTP.TEN_TINHTP==null?" ": models[loop].DTINHTP.TEN_TINHTP.ToString(),
                                                models[loop].SO_SO_DANG_KIEM == null ?" ": models[loop].SO_SO_DANG_KIEM.ToString(),
                                                models[loop].NGAY_CAP_SO_DANG_KIEM == null ?" ": models[loop].NGAY_CAP_SO_DANG_KIEM.Value.ToShortDateString(),
                                                models[loop].DLOAI_KIEM_TRA_KT==null || models[loop].DLOAI_KIEM_TRA_KT.Name==null ?" ": models[loop].DLOAI_KIEM_TRA_KT.Name.ToString(),
                                                models[loop].CO_QUAN_DANG_KIEM==null?" ": models[loop].CO_QUAN_DANG_KIEM.ToString(),
                                                models[loop].DANG_KIEM_VIEN==null?" ": models[loop].DANG_KIEM_VIEN.ToString(),
                                                models[loop].SO_BB_KIEM_TRA_KT==null?" ": models[loop].SO_BB_KIEM_TRA_KT.ToString(),
                                                models[loop].NGAY_KIEM_TRA==null?" ": models[loop].NGAY_KIEM_TRA.Value.ToShortDateString(),
                                                models[loop].SO_AN_CHI_ATKT==null?" ": models[loop].SO_AN_CHI_ATKT.ToString(),
                                                models[loop].NGAY_CAP_ATKT_TCA==null?" ": models[loop].NGAY_CAP_ATKT_TCA.Value.ToShortDateString(),
                                                models[loop].NGAY_HH_ATKT_TCA==null?" ": models[loop].NGAY_HH_ATKT_TCA.Value.ToShortDateString(),
                                                 models[loop].SO_DK == null ?" ": models[loop].SO_DK.ToString(),
                                                 models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                                  models[loop].KT_NAM_DONG==null?" ": models[loop].KT_NAM_DONG.ToString(),
                                                  models[loop].NOI_DANG_KY==null?" ": models[loop].NOI_DANG_KY.ToString(),
                                                models[loop].DCONG_DUNG_TAU == null ||   models[loop].DCONG_DUNG_TAU.Name==null?" ": models[loop].DCONG_DUNG_TAU.Name.ToString(),
                                                   models[loop].KT_CHIEU_DAI==null?" ": models[loop].KT_CHIEU_DAI.ToString(),
                                                  models[loop].KT_CHIEU_RONG==null?" ": models[loop].KT_CHIEU_RONG.ToString(),
                                                  models[loop].KT_CHIEU_CAO==null?" ": models[loop].KT_CHIEU_CAO.ToString(),
                                                   models[loop].KT_MON_NUOC==null?" ": models[loop].KT_MON_NUOC.ToString(),
                                                  models[loop].KT_MAN_KHO==null?" ": models[loop].KT_MAN_KHO.ToString(),
                                                models[loop].KT_CHIEU_DAI_TK==null?" ": models[loop].KT_CHIEU_DAI_TK.ToString(),
                                                 models[loop].KT_TOC_DO_TAU==null?" ": models[loop].KT_TOC_DO_TAU.ToString(),
                                                   models[loop].DVAT_LIEU_VO == null || models[loop].DVAT_LIEU_VO.Name==null?" ": models[loop].DVAT_LIEU_VO.Name.ToString(),
                                                    models[loop].KT_SO_MAY_TAU==null?" ": models[loop].KT_SO_MAY_TAU.ToString(),
                                                     models[loop].KT_TONG_CONG_SUAT==null?" ": models[loop].KT_TONG_CONG_SUAT.ToString(),
                                                     models[loop].DNHOM_TAUID==null?" ": dictNhomTaus[models[loop].DNHOM_TAUID.Value].Name,
                                                     //may 1
                                                  models[loop].M1_KY_HIEU_MAY==null?" ": models[loop].M1_KY_HIEU_MAY.ToString(),
                                                  models[loop].M1_SO_MAY==null?" ": models[loop].M1_SO_MAY.ToString(),
                                                  models[loop].M1_NOI_SX==null?" ": models[loop].M1_NOI_SX.ToString(),
                                                  models[loop].M1_NAM_CHE_TAO==null?" ": models[loop].M1_NAM_CHE_TAO.ToString(),
                                                  models[loop].M1_HANG_MAY==null?" ": models[loop].M1_HANG_MAY.ToString(),
                                                  models[loop].M1_VONG_QUAY==null?" ": models[loop].M1_VONG_QUAY.ToString(),
                                                  models[loop].M1_CONG_SUAT==null?" ": models[loop].M1_CONG_SUAT.ToString(),
                                                  //may 2
                                                  models[loop].M2_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M2_SO_MAY==null?" ": models[loop].M2_SO_MAY.ToString(),
                                                  models[loop].M2_NOI_SX==null?" ": models[loop].M2_NOI_SX.ToString(),
                                                  models[loop].M2_NAM_CHE_TAO==null?" ": models[loop].M2_NAM_CHE_TAO.ToString(),
                                                  models[loop].M2_HANG_MAY==null?" ": models[loop].M2_HANG_MAY.ToString(),
                                                  models[loop].M2_VONG_QUAY==null?" ": models[loop].M2_VONG_QUAY.ToString(),
                                                  models[loop].M2_CONG_SUAT==null?" ": models[loop].M2_CONG_SUAT.ToString(),
                                                  //may 3
                                                  models[loop].M3_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M3_SO_MAY==null?" ": models[loop].M3_SO_MAY.ToString(),
                                                  models[loop].M3_NOI_SX==null?" ": models[loop].M3_NOI_SX.ToString(),
                                                  models[loop].M3_NAM_CHE_TAO==null?" ": models[loop].M3_NAM_CHE_TAO.ToString(),
                                                  models[loop].M3_HANG_MAY==null?" ": models[loop].M3_HANG_MAY.ToString(),
                                                  models[loop].M3_VONG_QUAY==null?" ": models[loop].M3_VONG_QUAY.ToString(),
                                                  models[loop].M3_CONG_SUAT==null?" ": models[loop].M3_CONG_SUAT.ToString(),
                                                  
                                                  models[loop].DTINH_TRANG_DANG_KIEM == null || models[loop].DTINH_TRANG_DANG_KIEM.Name==null ?" ": models[loop].DTINH_TRANG_DANG_KIEM.Name.ToString(),
                                                 models[loop].DCAP_TAU_DANG_KIEM == null || models[loop].DCAP_TAU_DANG_KIEM.Name==null?" ": models[loop].DCAP_TAU_DANG_KIEM.Name.ToString(),
                                                 models[loop].HAN_DANG_KIEM == null ?" ": models[loop].HAN_DANG_KIEM.Value.ToShortDateString(),
                                                 models[loop].GHI_CHU == null ?" ": models[loop].GHI_CHU.ToString(),

                                                    }
                                                          );

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }



        public ActionResult ExportExcel(ViewModelSearchKT_DANGKIEM SearchModel)
        {

            try
            {

                using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_EXPORT_NAME, GENERATED_FILE_EXPORT_NAME))
                {
                    helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet1";

                    helper.CurrentPosition = new CellRef("A4");
                    helper.InsertRange("header_1");

                    helper.CurrentPosition = new CellRef("A5");
                    helper.InsertRange("header_2");

                    //helper.CurrentPosition = new CellRef("A6");
                    //helper.InsertRange("header_3");

                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "c_1", "c_2", "c_3", "c_4", "c_5", "c_6", "c_7", "c_8", "c_9", "c_10", "c_11", "c_12", "c_14", "c_15", "c_16", "c_17", "c_18", "c_19", "c_20", "c_21", "c_22", "c_23", "c_24", "c_25", "c_26", "c_27", "c_28", "c_29", "c_30", "c_31", "c_32", "c_33", "c_34", "c_35", "c_36", "c_37", "c_38", "c_39", "c_40", "c_41", "c_42", "c_43", "c_44", "c_45", "c_46", "c_47", "c_48", "c_49", "c_50", "c_51", "c_52", "c_53" });
                    //CellRangeTemplate row_quanhuyen = helper.CreateCellRangeTemplate("row_quanhuyen", new List<string> { "stt_num", "tenquanhuyen",  "c_56", "c_57", "c_58", "c_59", "c_60", "c_61", "c_62", "c_63", "c_64", "c_65", "c_66", "c_67",  "c_69", "c_70", "c_71", "c_72", "c_73", "c_74", "c_75", "c_76", "c_77", "c_78", "c_79", "c_80", "c_81", "c_82", "c_83", "c_84", "c_85", "c_86", "c_87", "c_88", "c_89", "c_90", "c_91", "c_92", "c_93", "c_94", "c_95", "c_96", "c_97", "c_98", "c_99", "c_100", "c_101", "c_102", "c_103", "c_104", "c_105", "c_106", "c_107", "c_108", "c_109", "c_110", "c_111" });
                    //CellRangeTemplate row_11 = helper.CreateCellRangeTemplate("row_11", new List<string> { "str", "c_110", "c_111", "c_112", "c_113", "c_114", "c_115", "c_116", "c_117", "c118", "c_119", "c_120", "c_121", "c_122", "c_123", "c_124", "c_125", "c_126", "c_127", "c_128", "c_129", "c_130", "c_131", "c_132", "c_133", "c_134", "c_135", "c_136", "c_137", "c_138", "c_139", "c_140", "c_141", "c_142", "c_143", "c_144", "c_145", "c_146", "c_147", "c_148", "c_149", "c_150", "c_151", "c_152", "c_153", "c_154", "c_155", "c_156", "c_157", "c_158", "c_159", "c_160", "c_161", "c_162", "c_163", "c_164", "c_165"});

                    int k = 6;
                    List<String> _lstMA_TTP = new List<string>();
                    IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(SearchModel, _lstMA_TTP);

                    helper.CurrentPosition = new CellRef("A" + (k).ToString());

                    helper.InsertRange(row_tinhthanhpho, _lstTinhTP);


                    helper.DeleteSheet("Sheet2");
                    helper.CurrentSheetName = "Sheet1";

                }


                ViewBag.MSG_EXPORT = "Xuất file Excel thành công!";

            }
            catch (Exception ex)
            {

                ViewBag.MSG_EXPORT = "Xuất file Excel không thành công!";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region "Partial View Danh sách đăng kiểm hết hạn

        public ActionResult ListGiaHan_Partial(ViewModelSearchKT_DANGKIEM dkiem)
        {
            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(ref ma_TinhTP);
            var ID_Max = db.KT_DANGKIEM.Where(o => (string.IsNullOrEmpty(dkiem.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(dkiem.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == dkiem.MA_TINHTP).GroupBy(x => x.SO_SO_DANG_KIEM)
               .Select(xs => xs.Max(x => x.ID));

            var Items_DKHH = db.KT_DANGKIEM.Where(o => ID_Max.Contains(o.ID));

            var DangKiemHetHan = Items_DKHH.Where(o => o.HAN_DANG_KIEM < DateTime.Today).OrderByDescending(m => m.HAN_DANG_KIEM);

            //var dangkiem = db.KT_DANGKIEM.Where(o => ( o.HAN_DANG_KIEM < DateTime.Today)
            //                                        && ((string.IsNullOrEmpty(dkiem.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(dkiem.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == dkiem.MA_TINHTP)
            //                                    ).OrderByDescending(m => m.HAN_DANG_KIEM);

            ViewBag.TotalRow = DangKiemHetHan.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = dkiem.Page ?? 1;

            dkiem.SearchResults = DangKiemHetHan.ToPagedList(pageNumber, pageSize);
            return PartialView("ListGiaHan_Partial", dkiem);

        }


        public ActionResult ListSapHetHan_Partial(ViewModelSearchKT_DANGKIEM dkiem)
        {
            string ma_TinhTP = string.Empty;

            initialCategorySearchAction( ref ma_TinhTP);

            var ID_Max = db.KT_DANGKIEM.Where(o => (string.IsNullOrEmpty(dkiem.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(dkiem.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == dkiem.MA_TINHTP).GroupBy(x => x.SO_SO_DANG_KIEM)
              .Select(xs => xs.Max(x => x.ID));

            var Items_DKHH = db.KT_DANGKIEM.Where(o => ID_Max.Contains(o.ID));

            var DangKiemHetHan = Items_DKHH.Where(o => ((DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM) >= 1 && (DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM)) <= 10))).OrderByDescending(m => m.HAN_DANG_KIEM);
            //var dangkiem = db.KT_DANGKIEM.Where(o => ((DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM) >= 1 && (DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM)) <= 10)
            //                                        && ((string.IsNullOrEmpty(dkiem.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(dkiem.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == dkiem.MA_TINHTP)
            //                                    )).OrderByDescending(m => m.HAN_DANG_KIEM);

            ViewBag.TotalRow = DangKiemHetHan.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = dkiem.Page ?? 1;

            dkiem.SearchResults = DangKiemHetHan.ToPagedList(pageNumber, pageSize);
            return PartialView("ListSapHetHan_Partial", dkiem);

        }

        #endregion
    }
}