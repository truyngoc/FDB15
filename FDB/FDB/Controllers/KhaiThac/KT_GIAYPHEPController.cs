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
using System.Data.Entity;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;
using Aron.Sinoai.OfficeHelper;

using Microsoft.AspNet.Identity;
namespace FDB.Controllers.KhaiThac
{
    public class KT_GIAYPHEPController : FDBController
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
        public ActionResult Index(ViewModelSearchKT_GIAYPHEP gp)
        {

            if (gp.SearchButton == "Xuất Excel")
            {
                ExportExcel(gp);
                //Dowload file:

                return File(GENERATED_FILE_EXPORT_NAME, "application/vnd.ms-excel", "KT_GIAYPHEPExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");

            }
            else
            {
                string ma_TinhTP = string.Empty;

                initialCategorySearchAction(gp, ref ma_TinhTP);

                var giayphep = db.KT_GIAYPHEP.Where(o => (string.IsNullOrEmpty(gp.SO_GP) || o.SO_GP.ToLower().Contains(gp.SO_GP.ToLower()))
                                                        && (string.IsNullOrEmpty(gp.SO_DK) || o.SO_DK.ToLower().Contains(gp.SO_DK.ToLower()))
                    //&& (string.IsNullOrEmpty(gp.MA_TINHTP) || o.MA_TINHTP == gp.MA_TINHTP)
                    //&& ((string.IsNullOrEmpty(gp.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                        && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                        && (string.IsNullOrEmpty(gp.CANG_DANG_KY) || o.CANG_DANG_KY.ToLower().Contains(gp.CANG_DANG_KY.ToLower()))
                                                        && (gp.NGAY_GP_TU == null || o.NGAY_GP >= gp.NGAY_GP_TU)
                                                        && (gp.NGAY_GP_DEN == null || o.NGAY_GP <= gp.NGAY_GP_DEN)
                                                        && (gp.NGAY_HL_TU == null || o.NGAY_HL_GP >= gp.NGAY_HL_TU)
                                                        && (gp.NGAY_HL_DEN == null || o.NGAY_HL_GP <= gp.NGAY_HL_DEN)
                                                        && (gp.NGAY_HHL_TU == null || o.NGAY_HHL_GP >= gp.NGAY_HHL_TU)
                                                        && (gp.NGAY_HHL_DEN == null || o.NGAY_HHL_GP <= gp.NGAY_HHL_DEN)
                                                    ).OrderByDescending(m => m.NGAY_GP);

                ViewBag.TotalRow = giayphep.Count();
                int pageSize = FDB.Common.Constants.PageSize;
                int pageNumber = gp.Page ?? 1;

                gp.SearchResults = giayphep.ToPagedList(pageNumber, pageSize);

                return View(gp);
            }
        }

        #endregion
        public void BindComboDMSearch()
        {
            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");


        }
        #region "Search"
        #region "Tim kiem"

        public void initialCategoryStatisticsAction(ViewModelSearchKT_GIAYPHEP_THONGKE gp, ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();


            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");



            ma_Tinh = curUser.MA_TINHTP;

            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }
        public ActionResult Search(ViewModelSearchKT_GIAYPHEP_THONGKE SearchModel)
        {
            BindComboDMSearch();
            string strTitle = "";

            string _MA_TTP = SearchModel.MA_TINHTP;


            var result = db.Database.SqlQuery<ViewModelSearchKT_GIAYPHEP_THONGKE>("exec KT_GIAYPHEP_KHAITHACTHUYSAN_DYNAMIC @matinh_tp, @fromdate, @todate "
                 , new SqlParameter("@matinh_tp", SearchModel.MA_TINHTP == null ? (object)DBNull.Value : SearchModel.MA_TINHTP)
                 , new SqlParameter("@fromdate", SearchModel.TU_NGAY == null ? (object)DBNull.Value : SearchModel.TU_NGAY.Value)
                 , new SqlParameter("@todate", SearchModel.DEN_NGAY == null ? (object)DBNull.Value : SearchModel.DEN_NGAY.Value)
                 ).ToList();

            //DataTable dt = result.CopyToDataTable();

            SearchModel.StatisticsResults = result;
            if (SearchModel.MA_TINHTP != null)
            {
                strTitle = "Báo cáo tình hình cấp giấy phép khai thác thủy sản " + "của " + db.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == _MA_TTP).TEN_TINHTP;
                ViewBag.strTitle = strTitle;
            }
            else
            {
                strTitle = "Báo cáo tình hình cấp giấy phép khai thác thủy sản của toàn quốc ";
                ViewBag.strTitle = strTitle;
            }
            return View(SearchModel);



        }

        #region "ThongKe"
        class ThongKe
        {
            List<DM_NHOMNGHE> _lstDMNhomNghe;
            List<DNHOM_TAU> _lstDMNhomTau;
            List<DTINH_TRANG_TAU> _lstTinhTrangTau;
            List<KT_TAUTHUYEN> _lstTauThuyen;
            FDBContext db = new FDBContext();

            ViewModelSearchKT_GIAYPHEP_THONGKE t;

            public Dictionary<string, int> F
            {
                get
                {
                    return Calc_F(t);
                }
            }

            public Dictionary<string, int> Sum_F_by_Fishery
            {
                get
                {
                    return Calc_Sum_F_by_Fishery(t);
                }
            }



            public ThongKe(ViewModelSearchKT_GIAYPHEP_THONGKE t)
            {
                this.t = t;

                _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
                _lstDMNhomTau = db.DNHOM_TAU.ToList();
                _lstTauThuyen = db.KT_TAUTHUYEN.ToList();
                _lstTinhTrangTau = db.DTINH_TRANG_TAU.ToList();
            }

            // thong ke tau thuyen theo nghe
            private Dictionary<string, int> Calc_F(ViewModelSearchKT_GIAYPHEP_THONGKE tk)
            {
                Dictionary<string, int> _dictF = new Dictionary<string, int>();

                foreach (var nghe in _lstDMNhomNghe)
                {
                    foreach (var nhomtau in _lstDMNhomTau)
                    {
                        var iF = 0;
                        iF = _lstTauThuyen.Where(t => (t.NGHE_CHINH_ID == nghe.DM_NhomNgheID || t.NGHE_PHU_ID == nghe.DM_NhomNgheID)
                                                        && t.DNHOM_TAUID == nhomtau.ID
                                                        && t.DCONG_DUNG_TAUID == 1  // chi thong ke tau khai thac thuy san
                                                        && (tk.MA_TINHTP == null || t.MA_TINHTP == tk.MA_TINHTP)
                                                        ).Count();

                        _dictF.Add(tk.MA_TINHTP + "#" + nghe.DM_NhomNgheID + "#" + nhomtau.ID, iF);
                    }
                }

                return _dictF;
            }


            private Dictionary<string, int> Calc_Sum_F_by_Fishery(ViewModelSearchKT_GIAYPHEP_THONGKE tk)
            {
                Dictionary<string, int> _dictSumF = new Dictionary<string, int>();

                if (this.F != null && this.F.Count > 0)
                {
                    foreach (var nghe in _lstDMNhomNghe)
                    {
                        var iSumF = 0;
                        foreach (var nhomtau in _lstDMNhomTau)
                        {
                            iSumF += this.F[tk.MA_TINHTP + "#" + nghe.DM_NhomNgheID + "#" + nhomtau.ID];
                        }

                        _dictSumF.Add(tk.MA_TINHTP + "#" + nghe.DM_NhomNgheID, iSumF);
                    }
                }

                return _dictSumF;
            }




        }
        #endregion


        #endregion
        #endregion

        #region "Them moi"
        public ActionResult CapPhep()
        {
            // initial danh muc
            initialCategoryCreateAction();

            // luon thiet lap dong dau tien cua danh sach nghe la nghe chinh
            // -> ko phai kiem tra bat buoc phai chon 1 nghe chinh trong danh sach            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapPhep(KT_GIAYPHEP gp)
        {
            if (ModelState.IsValid)
            {
                // them moi giay phep
                db.KT_GIAYPHEP.Add(gp);
                db.SaveChanges();

                // cap nhat trang thai cua tau thuyen
                KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(gp.SO_DK);
                if (t != null)
                {
                    t.IsCapPhep = true;

                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }


                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Giấy phép số [" + gp.SO_GP + "] của tàu [" + gp.SO_DK + "]"));

                return RedirectToAction("Index");
            }
            initialCategoryCreateAction();

            return View(gp);
        }
        #endregion

        #region "them moi (tu man hinh danh sach tau chua cap phep) "


        public ActionResult Create(string id)
        {
            // get tauthuyen information
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string sodk = System.Uri.UnescapeDataString(id);

            KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(sodk);

            if (t == null)
            {
                return HttpNotFound();
            }

            // set info to giayphep
            KT_GIAYPHEP gp = new KT_GIAYPHEP();
            gp.SO_DK = t.SO_DK;
            gp.NGAY_DK = t.NGAY_DK;
            gp.TEN_TAU = t.TEN_TAU;
            gp.CHU_PHUONG_TIEN = t.CHU_PHUONG_TIEN;
            gp.DIA_CHI = t.DIA_CHI;
            gp.DIEN_THOAI = t.DIEN_THOAI;
            // luon thiet lap dong dau tien cua danh sach nghe la nghe chinh
            // -> ko phai kiem tra bat buoc phai chon 1 nghe chinh trong danh sach
            gp.IsNGHECHINH = true;

            gp.DM_NHOMNGHEID = t.NGHE_CHINH_ID;
            gp.DM_NHOMNGHE2ID = t.NGHE_PHU_ID;

            // initial danh muc
            initialCategoryCreateAction();

            return View(gp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_GIAYPHEP gp)
        {
            if (ModelState.IsValid)
            {
                // them moi giay phep
                db.KT_GIAYPHEP.Add(gp);
                db.SaveChanges();

                // cap nhat trang thai cua tau thuyen
                KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(gp.SO_DK);
                if (t != null)
                {
                    t.IsCapPhep = true;

                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }


                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Giấy phép số [" + gp.SO_GP + "] của tàu [" + gp.SO_DK + "]"));

                return RedirectToAction("Index");
            }
            initialCategoryCreateAction();

            return View(gp);
        }

        #endregion

        #region "Sua"
        public ActionResult Edit(int ID)
        {

            KT_GIAYPHEP t = db.KT_GIAYPHEP.Find(ID);

            if (t == null)
            {
                return HttpNotFound();
            }

            initialCategoryEditAction();

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_GIAYPHEP t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Giấy phép"));


                return RedirectToAction("Index");
            }
            initialCategoryEditAction();

            return View(t);
        }
        #endregion


        #region "Chi tiet"
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    KT_GIAYPHEP gp = db.KT_GIAYPHEP.Find(id);

        //    if (gp == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return PartialView(gp);
        //}

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KT_GIAYPHEP gp = await db.KT_GIAYPHEP.FindAsync(id);

            if (gp == null)
            {
                return HttpNotFound();
            }

            return PartialView("Details", gp);
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
            // xoa giay phep
            KT_GIAYPHEP gp = await db.KT_GIAYPHEP.FindAsync(id);

            if (gp.KT_GIAYPHEP_NKs.Count > 0)
            {
                db.KT_GIAYPHEP_NK.Where(m => m.KT_GIAYPHEPID == id).ToList().ForEach(o => db.KT_GIAYPHEP_NK.Remove(o));

                //db.SaveChanges();
            }

            // cap nhat IsCapPhep cua tau
            KT_TAUTHUYEN t = await db.KT_TAUTHUYEN.FindAsync(gp.SO_DK);
            t.IsCapPhep = null;

            db.KT_GIAYPHEP.Remove(gp);

            await db.SaveChangesAsync();

            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Giấy phép số [" + gp.SO_GP + "] của tàu [" + gp.SO_DK + "]"));

            return Json(new { success = true });
        }

        #endregion

        #region "Danh sach giay phep het han"
        public ActionResult ListGiaHan(ViewModelSearchKT_GIAYPHEP gp)
        {
            if (gp.SearchButton == "Xuất Excel")
            {
                ExportExcel_GIAYPHEPHETHAN(gp);
                //Dowload file:

                return File(GENERATED_FILE_EXPORT_NAME, "application/vnd.ms-excel", "GIAY_PHEP_HET_HANExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");

            }
            else
            {
                string ma_TinhTP = string.Empty;

                initialCategorySearchAction(gp, ref ma_TinhTP);

                var giayphep = db.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && o.NGAY_HHL_GP < DateTime.Today) || ((o.TRANG_THAI == 1) && o.NGAY_GIA_HAN_DEN < DateTime.Today))
                                                        && (string.IsNullOrEmpty(gp.SO_GP) || o.SO_GP.ToLower().Contains(gp.SO_GP.ToLower()))
                                                        && (string.IsNullOrEmpty(gp.SO_DK) || o.SO_DK.ToLower().Contains(gp.SO_DK.ToLower()))
                    //&& (string.IsNullOrEmpty(gp.MA_TINHTP) || o.MA_TINHTP == gp.MA_TINHTP)
                    //&& ((string.IsNullOrEmpty(gp.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                        && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                        && (string.IsNullOrEmpty(gp.CANG_DANG_KY) || o.CANG_DANG_KY == gp.CANG_DANG_KY)
                                                        && (gp.NGAY_GP_TU == null || o.NGAY_GP >= gp.NGAY_GP_TU)
                                                        && (gp.NGAY_GP_DEN == null || o.NGAY_GP <= gp.NGAY_GP_DEN)
                                                    ).OrderByDescending(m => m.NGAY_GP);

                ViewBag.TotalRow = giayphep.Count();
                int pageSize = FDB.Common.Constants.PageSize;
                int pageNumber = gp.Page ?? 1;

                gp.SearchResults = giayphep.ToPagedList(pageNumber, pageSize);

                return View(gp);
            }
        }

        #endregion

        #region "Gia han"

        public ActionResult GiaHan(int? id)
        {
            KT_GIAYPHEP_GIAHAN_ViewModel gh;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KT_GIAYPHEP gp = db.KT_GIAYPHEP.Find(id);

            if (gp == null)
            {
                return HttpNotFound();
            }
            else
            {
                gh = new KT_GIAYPHEP_GIAHAN_ViewModel();
                // manual copy object
                SetGiaHanGiayPhepViewModel(ref gh, gp);
            }

            return View(gh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GiaHan(KT_GIAYPHEP_GIAHAN_ViewModel h)
        {
            if (ModelState.IsValid)
            {
                KT_GIAYPHEP gp = db.KT_GIAYPHEP.Find(h.KT_GIAYPHEPID);

                if (gp != null)
                {
                    try
                    {
                        // cap nhat giay phep
                        SetGiaHanGiayPhep(h, ref gp);

                        db.Entry(gp).State = System.Data.Entity.EntityState.Modified;


                        // ghi nhat ky
                        db.KT_GIAYPHEP_NK.Add(GetNhatKyGiaHanGiayPhep(gp));


                        db.SaveChanges();

                        this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Giấy phép số [" + gp.SO_GP + "] của tàu [" + gp.SO_DK + "]"));

                        return RedirectToAction("ListGiaHan");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                        // Throw a new DbEntityValidationException with the improved exception message.
                        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    }
                }
            }

            return View(h);
        }

        public void SetGiaHanGiayPhep(KT_GIAYPHEP_GIAHAN_ViewModel h, ref KT_GIAYPHEP gp)
        {
            // cap nhat ngay gia han giay phep
            gp.NGAY_GIA_HAN = h.NGAY_GIA_HAN;
            gp.NGAY_GIA_HAN_TU = h.NGAY_GIA_HAN_TU;
            gp.NGAY_GIA_HAN_DEN = h.NGAY_GIA_HAN_DEN;
            gp.GHI_CHU = h.GHI_CHU;
            gp.TRANG_THAI = 1;  // da gia han
        }

        public void SetGiaHanGiayPhepViewModel(ref KT_GIAYPHEP_GIAHAN_ViewModel gh, KT_GIAYPHEP gp)
        {
            gh.KT_GIAYPHEPID = gp.KT_GIAYPHEPID;
            gh.SO_DK = gp.SO_DK;
            gh.NGAY_DK = gp.NGAY_DK;
            gh.SO_GP = gp.SO_GP;
            gh.NGAY_GP = gp.NGAY_GP;
            gh.NGAY_HL_GP = gp.NGAY_HL_GP;
            gh.NGAY_HHL_GP = gp.NGAY_HHL_GP;
            gh.NGAY_GIA_HAN_DEN_OLD = gp.NGAY_GIA_HAN_DEN ?? gp.NGAY_HHL_GP;
            gh.KT_GIAYPHEP_NKs = gp.KT_GIAYPHEP_NKs;
        }

        public KT_GIAYPHEP_NK GetNhatKyGiaHanGiayPhep(KT_GIAYPHEP gp)
        {
            KT_GIAYPHEP_NK nk = new KT_GIAYPHEP_NK();

            // da gia han
            nk.KT_GIAYPHEPID = gp.KT_GIAYPHEPID;
            nk.NGAY_GP = gp.NGAY_GIA_HAN;
            nk.NGAY_HL_GP = gp.NGAY_GIA_HAN_TU;
            nk.NGAY_HHL_GP = gp.NGAY_GIA_HAN_DEN;
            nk.GHI_CHU = gp.GHI_CHU;

            return nk;
        }

        #endregion


        #region "load danh muc"
        public void initialCategoryCreateAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.DVUNG_TUYENs = new SelectList(db.DVUNG_TUYEN, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));

            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }

        public void initialCategoryEditAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.DVUNG_TUYENs = new SelectList(db.DVUNG_TUYEN, "ID", "Name");
            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        }

        public void initialCategorySearchAction(ViewModelSearchKT_GIAYPHEP gp, ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            //ViewBag.TINH_THANHPHOs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;

            ma_Tinh = curUser.MA_TINHTP;
        }

        [HttpPost]
        public JsonResult check_SO_GP_Exist(string SO_GP, int isEdit, string MA_TINHTP)
        {
            if (isEdit == 0)
            {
                var gp = db.KT_GIAYPHEP.FirstOrDefault(m => m.SO_GP == SO_GP && m.MA_TINHTP == MA_TINHTP);

                return Json(gp == null);
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult check_SO_DK_Exist(string SO_DK, int isEdit, string MA_TINHTP)
        {
            if (isEdit == 0)
            {
                var tau = db.KT_TAUTHUYEN.FirstOrDefault(m => m.SO_DK == SO_DK && m.MA_TINHTP == MA_TINHTP);

                return Json(tau != null);
            }

            return Json(true);
        }

        public JsonResult Load_Tau_By_SoDK(string so_Dang_Ky)
        {
            ApplicationUser curUser = getCurrentUser();

            var dk = db.KT_TAUTHUYEN.Where(m => m.SO_DK.ToLower().Equals(so_Dang_Ky.ToLower()) && m.MA_TINHTP == curUser.MA_TINHTP).FirstOrDefault();

            if (dk != null)
            {
                // http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                // http://stackoverflow.com/questions/1153385/a-circular-reference-was-detected-while-serializing-an-object-of-type-subsonic
                return Json(
                    new
                    {
                        NGAY_DK = dk.NGAY_DK
                    ,
                        TEN_TAU = dk.TEN_TAU
                    ,
                        DIA_CHI = dk.DIA_CHI
                    ,
                        DIEN_THOAI = dk.DIEN_THOAI
                    ,
                        CHU_PHUONG_TIEN = dk.CHU_PHUONG_TIEN
                    ,
                        NGHE_CHINH_ID = dk.NGHE_CHINH_ID
                    ,
                        NGHE_PHU_ID = dk.NGHE_PHU_ID
                    }
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KT_TAUTHUYEN(), JsonRequestBehavior.AllowGet);
            }
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
                            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._KT_GIAYPHEPmapPropertyNameToText);

                            this.InitDictDanhMuc();
                            DateTimeFormatInfo dateFormat = new CultureInfo("en-AU", false).DateTimeFormat;

                            for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
                            {
                                DataRow r = dtData.Rows[rowIndex];
                                for (int i = 0; i < dtData.Columns.Count; i++)
                                {
                                    switch (dtData.Columns[i].ColumnName)
                                    {
                                        case "SO_DK":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số đăng ký tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }

                                            break;
                                        case "NGAY_DK":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày đăng ký tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "MA_TINHTP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tỉnh/thành phố tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictTThanhPho.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictTThanhPho[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Tỉnh/thành phố tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }


                                            }
                                            break;
                                        case "SO_GP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Số giấy phép tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            break;
                                        case "NGAY_GP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày cấp giấy phép tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "NGAY_HL_GP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày hiệu lực tại dòng " + (rowIndex + 3).ToString() + " không được để trống! </span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "NGAY_HHL_GP":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngày hết hạn tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "CANG_DANG_KY":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Cảng đăng ký tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }

                                            break;
                                        case "DM_NHOMNGHEID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Ngành nghề chính tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictDMNghe.ContainsKey(r[i].ToString()))
                                                {
                                                    r[i] = this._dictDMNghe[r[i].ToString()];
                                                    r["IsNGHECHINH"] = 1;
                                                }
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Ngành nghề chính tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;

                                        case "DVUNG_TUYENID":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Vùng tuyến tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                if (this._dictVungHoatDong.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictVungHoatDong[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Vùng tuyến tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }

                                            }
                                            break;
                                        case "CAP_PHEP_TU":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Thời gian cấp phép từ tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "CAP_PHEP_DEN":
                                            if (r[i].Equals(DBNull.Value))
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Thời gian cấp phép đến tại dòng " + (rowIndex + 3).ToString() + " không được để trống!</span> <br/>");
                                                goto ExitForRow;
                                            }
                                            else
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "DM_NHOMNGHE2ID":
                                            r["IsNGHECHINH_2"] = 0;
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictDMNghe.ContainsKey(r[i].ToString()))
                                                {
                                                    r[i] = this._dictDMNghe[r[i].ToString()];

                                                }

                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Ngành nghề phục 1 tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        case "DVUNG_TUYEN2ID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictVungHoatDong.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictVungHoatDong[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Vùng tuyến 1 tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        case "DM_NHOMNGHE3ID":
                                            r["IsNGHECHINH_3"] = 0;
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictDMNghe.ContainsKey(r[i].ToString()))
                                                {
                                                    r[i] = this._dictDMNghe[r[i].ToString()];
                                                    r["IsNGHECHINH_3"] = 0;
                                                }

                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Ngành nghề phục 2 tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        case "DVUNG_TUYEN3ID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictVungHoatDong.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictVungHoatDong[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Vùng tuyến 2 tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        case "CAP_PHEP_TU_2":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;

                                        case "CAP_PHEP_DEN_2":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;
                                        case "CAP_PHEP_TU_3":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString());
                                            }
                                            break;

                                        case "CAP_PHEP_DEN_3":
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
                                //DataTable dtDataDistinct = FDB.Common.Helpers.RemoveDuplicateRows(dtData, "SO_DK");
                                var _vNotExists = new List<ViewModelKT_GIAYPHEP_NOTEXIST>();
                                //Insert using SQLbulkCopy :
                                string tablename = "KT_GIAYPHEP_TEMP_" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                                string SqlBulkConnectionString = System.Configuration.ConfigurationManager.AppSettings["SqlBulkCopyConnectionString"].ToString();
                                int _BatchSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BatchSize"].ToString());
                                //Begin 
                                using (db.Database.Connection)
                                {
                                    try
                                    {
                                        //Create temp table:
                                        var name = new SqlParameter("@tablename", tablename);
                                        db.Database.ExecuteSqlCommand("exec CreateTableKT_GIAYPHEP_TEMP @tablename", name);
                                        db.SaveChanges();
                                        using (var txt = new TransactionScope())
                                        {
                                            try
                                            {
                                                using (var bcp = new SqlBulkCopy(SqlBulkConnectionString))
                                                {
                                                    bcp.BatchSize = _BatchSize;
                                                    bcp.DestinationTableName = tablename;
                                                    for (int _cIndex = 0; _cIndex < _KT_GIAYPHEPmapPropertyNameToText.Count; _cIndex++)
                                                    {
                                                        bcp.ColumnMappings.Add(_KT_GIAYPHEPmapPropertyNameToText.Values.ToList()[_cIndex], _KT_GIAYPHEPmapPropertyNameToText.Values.ToList()[_cIndex]);
                                                    }
                                                    bcp.ColumnMappings.Add("IsNGHECHINH", "IsNGHECHINH");
                                                    bcp.ColumnMappings.Add("IsNGHECHINH_2", "IsNGHECHINH_2");
                                                    bcp.ColumnMappings.Add("IsNGHECHINH_3", "IsNGHECHINH_3");


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
                                        _vNotExists = db.Database.SqlQuery<ViewModelKT_GIAYPHEP_NOTEXIST>("exec MergeIntoDataKT_GIAYPHEP @tablename", name2).ToList();
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

                                    strValidationError.Append("<span style=\"color:blue;font-size:14px;\">Import file thành công ! <b>" + (dtData.Rows.Count - _vNotExists.Count).ToString() + "/" + dtData.Rows.Count.ToString() + "</b> bản ghi  </span><br/><br/>");
                                    string strNotExist = _vNotExists.Count > 0 ? "<span style=\"color:blue\">Có <b>" + _vNotExists.Count.ToString() + "</b> số đăng ký không tồn tại:</span><br/><ul>" : "";
                                    _vNotExists.ForEach(o => strNotExist += "<li style=\"color:#CC3366\">" + o.SO_DK + "</li>");
                                    strValidationError.Append(!string.IsNullOrWhiteSpace(strNotExist) ? strNotExist + "</ul>" : "");
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
            DataTable dtNew = new DataTable("KT_GIAYPHEP");
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
            dtNew.Columns.Add("IsNGHECHINH", typeof(Object));
            dtNew.Columns.Add("IsNGHECHINH_2", typeof(Object));
            dtNew.Columns.Add("IsNGHECHINH_3", typeof(Object));

            return dtNew;
        }



        public FileResult KT_GIAYPHEPDownLoadFile()
        {
            String FullPathFileName = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Import_GiayPhepKT.xlsx");
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
            return FDB.Common.Helpers.DownLoadFile(FullPathFileName, "FDB_Template_Import_GiayPhep" + extension, contentType);
        }

        private Dictionary<string, string> _KT_GIAYPHEPmapPropertyNameToText = new Dictionary<string, string>()
        {

                {"Số đăng ký", "SO_DK"},
                {"Ngày đăng ký", "NGAY_DK"},
                {"Tỉnh/thành phố", "MA_TINHTP"},
                {"Tên tàu", "TEN_TAU"},
                {"Chủ tàu", "CHU_PHUONG_TIEN"},
                {"Địa chỉ", "DIA_CHI"},
                {"Điện thoại", "DIEN_THOAI"},
                {"Số giấy phép", "SO_GP"},
                {"Ngày cấp giấy phép", "NGAY_GP"},
                {"Ngày hiệu lực", "NGAY_HL_GP"},
                {"Ngày hết hạn", "NGAY_HHL_GP"},
                {"Cảng đăng ký", "CANG_DANG_KY"},
                {"Ngành nghề chính", "DM_NHOMNGHEID"},
                {"Vùng tuyến","DVUNG_TUYENID"},
                {"Thời gian cấp phép từ", "CAP_PHEP_TU"},
                {"Thời gian cấp phép đến", "CAP_PHEP_DEN"},
                {"Ngành nghề phụ 1", "DM_NHOMNGHE2ID"},
                {"Vùng tuyến 1", "DVUNG_TUYEN2ID"},
                {"Thời gian cấp phép từ 1", "CAP_PHEP_TU_2"},
                {"Thời gian cấp phép đến 1", "CAP_PHEP_DEN_2"},
                {"Ngành nghề phụ 2", "DM_NHOMNGHE3ID"},
                {"Vùng tuyến 2", "DVUNG_TUYEN3ID"},
                {"Thời gian cấp phép từ 2", "CAP_PHEP_TU_3"},
                {"Thời gian cấp phép đến 2", "CAP_PHEP_DEN_3"}

                // IsNGHECHINH=1,IsNGHECHINH_1=0, IsNGHECHINH_2=0

              
        };

        public string ValidateDataTable(DataTable dt)
        {
            string sWarining = "";
            foreach (string s in this._KT_GIAYPHEPmapPropertyNameToText.Keys)
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
        private Dictionary<string, bool> _dictNgheChinh = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            {"Y",true},
            {"N",false}
        };

        private void InitDictDanhMuc()
        {
            var dbQuanTri = new ApplicationDbContext();
            string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
            var objTThanhPho = db.DTINHTP.First(o => o.MA_TINHTP == Ma_TinhTP);
            _dictTThanhPho.Add(objTThanhPho.TEN_TINHTP, objTThanhPho.MA_TINHTP);

            //DM nghề:
            var _objDMNghe = db.DM_NHOMNGHE.ToList();
            _objDMNghe.ForEach(o => _dictDMNghe.Add(o.TenNhomNghe, o.DM_NhomNgheID));
            //DM vùng hoạt động:
            var objVungHoatDong = db.DVUNG_TUYEN.ToList();
            objVungHoatDong.ForEach(o => _dictVungHoatDong.Add(o.Name, o.ID));
        }

        #endregion

        #region"Working Files"
        private static String GENERATED_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_GiayPhep.xlsx");
        #endregion


        #region "Export excel"



        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchKT_GIAYPHEP gp)
        {
            //get nhóm nghề
            string ma_TinhTP = string.Empty;




            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();

            initialCategorySearchAction(gp, ref ma_TinhTP);

            var giayphep = db.KT_GIAYPHEP.Where(o => (string.IsNullOrEmpty(gp.SO_GP) || o.SO_GP.ToLower().Contains(gp.SO_GP.ToLower()))
                                                    && (string.IsNullOrEmpty(gp.SO_DK) || o.SO_DK.ToLower().Contains(gp.SO_DK.ToLower()))
                                                    && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                    && (string.IsNullOrEmpty(gp.CANG_DANG_KY) || o.CANG_DANG_KY.ToLower().Contains(gp.CANG_DANG_KY.ToLower()))
                                                    && (gp.NGAY_GP_TU == null || o.NGAY_GP >= gp.NGAY_GP_TU)
                                                    && (gp.NGAY_GP_DEN == null || o.NGAY_GP <= gp.NGAY_GP_DEN)
                                                    && (gp.NGAY_HL_TU == null || o.NGAY_HL_GP >= gp.NGAY_HL_TU)
                                                    && (gp.NGAY_HL_DEN == null || o.NGAY_HL_GP <= gp.NGAY_HL_DEN)
                                                    && (gp.NGAY_HHL_TU == null || o.NGAY_HHL_GP >= gp.NGAY_HHL_TU)
                                                    && (gp.NGAY_HHL_DEN == null || o.NGAY_HHL_GP <= gp.NGAY_HHL_DEN)
                                                );



            var models = giayphep.ToList();


            //lay nhom nghe 
            var dictNhomNghes = db.DM_NHOMNGHE.ToDictionary(o => o.DM_NhomNgheID);

            ////lay danh muc vung tuyen
            var dictVUNGTUYENs = db.DVUNG_TUYEN.ToDictionary(o => o.ID);



            for (int loop = 0; loop < models.Count(); loop++)
            {


                lstobj.Add(new List<object> { 
                                                    models[loop].SO_DK == null ?" ": models[loop].SO_DK.ToString(),
                                                    models[loop].NGAY_DK == null ?" ": models[loop].NGAY_DK.Value.ToShortDateString(),
                                                    models[loop].DTINHTP == null || models[loop].DTINHTP.TEN_TINHTP==null?" ": models[loop].DTINHTP.TEN_TINHTP.ToString(),
                                                    models[loop].TEN_TAU==null?" ": models[loop].TEN_TAU.ToString(),
                                                    models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                                     models[loop].DIA_CHI==null?" ": models[loop].DIA_CHI.ToString(),
                                                    models[loop].DIEN_THOAI==null?" ": models[loop].DIEN_THOAI.ToString(),       
                                                    models[loop].SO_GP == null ?" ": models[loop].SO_GP.ToString(),
                                                     models[loop].NGAY_GP == null ?" ": models[loop].NGAY_GP.Value.ToShortDateString(),
                                                     models[loop].NGAY_HL_GP == null ?" ": models[loop].NGAY_HL_GP.Value.ToShortDateString(),                                                  
                                                  models[loop].NGAY_HHL_GP == null ?" ": models[loop].NGAY_HHL_GP.Value.ToShortDateString(),
                                                  models[loop].CANG_DANG_KY == null ?" ": models[loop].CANG_DANG_KY.ToString(),
                                                   models[loop].DM_NHOMNGHEID==null?" ": dictNhomNghes[models[loop].DM_NHOMNGHEID.Value].TenNhomNghe,
                                                   models[loop].DVUNG_TUYENID==null?" ": dictVUNGTUYENs[models[loop].DVUNG_TUYENID].Name,
                                                 models[loop].CAP_PHEP_TU == null ?" ": models[loop].CAP_PHEP_TU.Value.ToShortDateString(),
                                                 models[loop].CAP_PHEP_DEN == null ?" ": models[loop].CAP_PHEP_DEN.Value.ToShortDateString(),
                                                 models[loop].DM_NHOMNGHE2ID==null?" ": dictNhomNghes[models[loop].DM_NHOMNGHE2ID.Value].TenNhomNghe,
                                                   models[loop].DVUNG_TUYEN2ID==null?" ": dictVUNGTUYENs[models[loop].DVUNG_TUYEN2ID.Value].Name,
                                                 models[loop].CAP_PHEP_TU_2 == null ?" ": models[loop].CAP_PHEP_TU_2.Value.ToShortDateString(),
                                                 models[loop].CAP_PHEP_DEN_2 == null ?" ": models[loop].CAP_PHEP_DEN_2.Value.ToShortDateString(),
                                                  models[loop].DM_NHOMNGHE3ID==null?" ": dictNhomNghes[models[loop].DM_NHOMNGHE3ID.Value].TenNhomNghe,
                                                   models[loop].DVUNG_TUYEN3ID==null?" ": dictVUNGTUYENs[models[loop].DVUNG_TUYEN3ID.Value].Name,
                                                 models[loop].CAP_PHEP_TU_3 == null ?" ": models[loop].CAP_PHEP_TU_3.Value.ToShortDateString(),
                                                 models[loop].CAP_PHEP_DEN_3 == null ?" ": models[loop].CAP_PHEP_DEN_3.Value.ToShortDateString(),
                                                models[loop].DM_NHOMNGHE4ID==null?" ": dictNhomNghes[models[loop].DM_NHOMNGHE4ID.Value].TenNhomNghe,
                                                   models[loop].DVUNG_TUYEN4ID==null?" ": dictVUNGTUYENs[models[loop].DVUNG_TUYEN4ID.Value].Name,
                                                 models[loop].CAP_PHEP_TU_4 == null ?" ": models[loop].CAP_PHEP_TU_4.Value.ToShortDateString(),
                                                 models[loop].CAP_PHEP_DEN_4 == null ?" ": models[loop].CAP_PHEP_DEN_4.Value.ToShortDateString(),
                                                   models[loop].DM_NHOMNGHE5ID==null?" ": dictNhomNghes[models[loop].DM_NHOMNGHE5ID.Value].TenNhomNghe,
                                                   models[loop].DVUNG_TUYEN5ID==null?" ": dictVUNGTUYENs[models[loop].DVUNG_TUYEN5ID.Value].Name,
                                                 models[loop].CAP_PHEP_TU_5 == null ?" ": models[loop].CAP_PHEP_TU_5.Value.ToShortDateString(),
                                                 models[loop].CAP_PHEP_DEN_5 == null ?" ": models[loop].CAP_PHEP_DEN_5.Value.ToShortDateString(),
                                                     

                                                    }
                                                          );

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }



        public ActionResult ExportExcel(ViewModelSearchKT_GIAYPHEP SearchModel)
        {

            try
            {

                using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_EXPORT_NAME, GENERATED_FILE_EXPORT_NAME))
                {
                    helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet1";

                    helper.CurrentPosition = new CellRef("A4");
                    helper.InsertRange("header_1");



                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "c_1", "c_2", "c_3", "c_4", "c_5", "c_6", "c_7", "c_8", "c_9", "c_10", "c_11", "c_12", "c_14", "c_15", "c_16", "c_17", "c_18", "c_19", "c_20", "c_21", "c_22", "c_23", "c_24", "c_25", "c_26", "c_27", "c_28", "c_29", "c_30", "c_31", "c_32", "c_33" });


                    int k = 5;

                    IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(SearchModel);

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

        #region"Working Files GIAY PHEP HET HAN"
        private static String GENERATED_FILE_EXPORT_NAME_GPHETHAN = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_EXPORT_NAME_GPHETHAN = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_GiayPhetHetHan.xlsx");
        #endregion


        #region "Export excel GIAY PHEP HET HAN"



        private IEnumerable<List<object>> getTinhThanhPho_GIAYPHEPHETHAN(ViewModelSearchKT_GIAYPHEP gp)
        {
            //get nhóm nghề
            string ma_TinhTP = string.Empty;




            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();

            initialCategorySearchAction(gp, ref ma_TinhTP);

            var giayphep = db.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && o.NGAY_HHL_GP < DateTime.Today) || ((o.TRANG_THAI == 1) && o.NGAY_GIA_HAN_DEN < DateTime.Today))
                                                   && (string.IsNullOrEmpty(gp.SO_GP) || o.SO_GP.ToLower().Contains(gp.SO_GP.ToLower()))
                                                   && (string.IsNullOrEmpty(gp.SO_DK) || o.SO_DK.ToLower().Contains(gp.SO_DK.ToLower()))
                                                    && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                   //&& ((string.IsNullOrEmpty(gp.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                   && (string.IsNullOrEmpty(gp.CANG_DANG_KY) || o.CANG_DANG_KY == gp.CANG_DANG_KY)
                                                   && (gp.NGAY_GP_TU == null || o.NGAY_GP >= gp.NGAY_GP_TU)
                                                   && (gp.NGAY_GP_DEN == null || o.NGAY_GP <= gp.NGAY_GP_DEN)
                                               );



            var models = giayphep.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {


                lstobj.Add(new List<object> { 
                                                    models[loop].SO_DK == null ?" ": models[loop].SO_DK.ToString(),
                                                    models[loop].NGAY_DK == null ?" ": models[loop].NGAY_DK.Value.ToShortDateString(),
                                                     models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                                      models[loop].DIA_CHI==null?" ": models[loop].DIA_CHI.ToString(),
                                                       models[loop].DIEN_THOAI==null?" ": models[loop].DIEN_THOAI.ToString(),    
                                                       models[loop].SO_GP == null ?" ": models[loop].SO_GP.ToString(),
                                                        models[loop].NGAY_GP == null ?" ": models[loop].NGAY_GP.Value.ToShortDateString(),
                                                         models[loop].NGAY_HL_GP == null ?" ": models[loop].NGAY_HL_GP.Value.ToShortDateString(),  
                                                           models[loop].NGAY_HHL_GP == null ?" ": models[loop].NGAY_HHL_GP.Value.ToShortDateString(),
                                                     models[loop].DTINHTP == null || models[loop].DTINHTP.TEN_TINHTP==null?" ": models[loop].DTINHTP.TEN_TINHTP.ToString(),

                                                    }
                                                          );

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }



        public ActionResult ExportExcel_GIAYPHEPHETHAN(ViewModelSearchKT_GIAYPHEP SearchModel)
        {

            try
            {

                using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_EXPORT_NAME_GPHETHAN, GENERATED_FILE_EXPORT_NAME_GPHETHAN))
                {
                    helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet1";

                    helper.CurrentPosition = new CellRef("A4");
                    helper.InsertRange("header_1");



                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "c_1", "c_2", "c_3", "c_4", "c_5", "c_6", "c_7", "c_8", "c_9", "c_10"});


                    int k = 5;

                    IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho_GIAYPHEPHETHAN(SearchModel);

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

        #region "Partial View Danh sách giấy phép hết han
      
        public ActionResult ListGiaHan_Partial(ViewModelSearchKT_GIAYPHEP gp)
        {
            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(gp, ref ma_TinhTP);

            var giayphep = db.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && o.NGAY_HHL_GP < DateTime.Today) || ((o.TRANG_THAI == 1) && o.NGAY_GIA_HAN_DEN < DateTime.Today))
                                                    && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                ).OrderByDescending(m => m.NGAY_GP);

            ViewBag.TotalRow = giayphep.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = gp.Page ?? 1;

            gp.SearchResults = giayphep.ToPagedList(pageNumber, pageSize);
                return PartialView("ListGiaHan_Partial",gp);
            
        }


        public ActionResult ListSapHetHan_Partial(ViewModelSearchKT_GIAYPHEP gp)
        {
            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(gp, ref ma_TinhTP);
            
            //var giayphep = db.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && o.NGAY_HHL_GP < DateTime.Today) || ((o.TRANG_THAI == 1) && o.NGAY_GIA_HAN_DEN < DateTime.Today))
            var giayphep = db.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && (DbFunctions.DiffDays(DateTime.Today,o.NGAY_HHL_GP ) >= 1 && (DbFunctions.DiffDays(DateTime.Today,o.NGAY_HHL_GP )) <= 10) || ((o.TRANG_THAI == 1) && (( DbFunctions.DiffDays(DateTime.Today,o.NGAY_GIA_HAN_DEN ) >= 1 && DbFunctions.DiffDays(DateTime.Today,o.NGAY_GIA_HAN_DEN ) <= 10)))
                                                    && ((string.IsNullOrEmpty(gp.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(gp.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == gp.MA_TINHTP)
                                                  
                                                ))).OrderByDescending(m => m.NGAY_GP);


            

            ViewBag.TotalRow = giayphep.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = gp.Page ?? 1;

            gp.SearchResults = giayphep.ToPagedList(pageNumber, pageSize);
            return PartialView("ListSapHetHan_Partial",gp);

        }
      
        #endregion
    }
}