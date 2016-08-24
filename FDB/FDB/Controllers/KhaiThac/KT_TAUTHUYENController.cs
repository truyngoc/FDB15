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

using Microsoft.AspNet.Identity;


namespace FDB.Controllers
{
    public class KT_TAUTHUYENController : FDBController
    {
        FDBContext db = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }

        #region "them moi"
        public ActionResult Create()
        {
            initialCategoryCreateAction();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_TAUTHUYEN t)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(t.SO_SO_DANG_KIEM))
                {
                    t.IsDangKiem = true;
                }
                else
                {
                    t.IsDangKiem = false;
                }


                string msgErrs = string.Empty;
                if (t.DCONG_DUNG_TAUID == 1 && t.NGHE_CHINH_ID == null) // khai thác hải sản
                {
                    msgErrs += "Bạn phải nhập nghề chính cho tàu khai thác thủy sản";
                }


                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.KT_TAUTHUYEN.Add(t);
                    db.SaveChanges();

                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Tàu thuyền"));

                    return RedirectToAction("Index");
                }
                else { Inline_Danger(msgErrs, true); }
            }
            initialCategoryCreateAction();

            return View(t);
        }

        #endregion

        #region "sua"
        public ActionResult Edit(string id)
        {
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

            initialCategoryEditAction(t);

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_TAUTHUYEN t)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(t.SO_SO_DANG_KIEM))
                {
                    t.IsDangKiem = true;
                }
                else
                {
                    t.IsDangKiem = false;
                }


                string msgErrs = string.Empty;
                if (t.DCONG_DUNG_TAUID == 1 && t.NGHE_CHINH_ID == null) // khai thác hải sản
                {
                    msgErrs += "Bạn phải nhập nghề chính cho tàu khai thác thủy sản";
                }


                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Tàu Thuyền"));


                    return RedirectToAction("Index");
                }
                else { Inline_Danger(msgErrs, true); }

            }
            initialCategoryEditAction(t);



            return View(t);
        }
        #endregion

        #region "tim kiem"
        //public ActionResult Index(int? page)
        //{
        //    initialCategorySearchAction();

        //    var model = from m in db.KT_TAUTHUYEN
        //                select m;

        //    // phai Order thi moi paging dc
        //    model = model.OrderBy(m => m.NGAY_DK);

        //    int pageSize = 50;
        //    int pageNumber = page ?? 1; 

        //    return View(model.ToPagedList(pageNumber,pageSize));
        //}

        public ActionResult Index(ViewModelSearchKT_TAUTHUYEN t)
        {
            if (t.SearchButton == "Xuất Excel")
            {
                ExportExcel(t);
                //Dowload file:
                return File(GENERATED_FILE_EXPORT_NAME, "application/vnd.ms-excel", "KT_TAUTHUYENExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
            }
            else
            {
                string ma_TinhTP = string.Empty;

                initialCategorySearchAction(t, ref ma_TinhTP);

                var tau = db.KT_TAUTHUYEN.Where(o => (string.IsNullOrEmpty(t.SO_DK) || o.SO_DK.ToLower().Contains(t.SO_DK.ToLower()))
                                                    && (string.IsNullOrEmpty(t.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(t.CHU_PHUONG_TIEN.ToLower()))
                    //&& (string.IsNullOrEmpty(t.MA_TINHTP) || o.MA_TINHTP == t.MA_TINHTP)
                    //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                    && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                    && (string.IsNullOrEmpty(t.MA_QUANHUYEN) || o.MA_QUANHUYEN == t.MA_QUANHUYEN)
                                                    && (string.IsNullOrEmpty(t.MA_PHUONGXA) || o.MA_PHUONGXA == t.MA_PHUONGXA)
                                                    && (t.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == t.DCONG_DUNG_TAUID)
                                                    && (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                    && (t.DTINH_TRANG_TAUID == null || o.DTINH_TRANG_TAUID == t.DTINH_TRANG_TAUID)
                                                    && (t.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == t.DTINH_TRANG_DANG_KIEMID)
                                                     && (t.DNHOM_NGHECHINHID == null || o.NGHE_CHINH_ID == t.DNHOM_NGHECHINHID)
                                                     && (t.DNHOM_NGHEPHUID == null || o.NGHE_PHU_ID == t.DNHOM_NGHEPHUID)
                                                    && (t.KT_TONG_CONG_SUAT_TU == null || o.KT_TONG_CONG_SUAT >= t.KT_TONG_CONG_SUAT_TU)
                                                    && (t.KT_TONG_CONG_SUAT_DEN == null || o.KT_TONG_CONG_SUAT <= t.KT_TONG_CONG_SUAT_DEN)
                                                    && (t.NGAY_DK_TU == null || o.NGAY_DK >= t.NGAY_DK_TU)
                                                    && (t.NGAY_DK_DEN == null || o.NGAY_DK <= t.NGAY_DK_DEN)
                                                ).OrderByDescending(m => new { m.NGAY_DK, m.SO_DK });

                ViewBag.TotalRow = tau.Count();
                ViewBag.TotalPower = tau.Sum(m => m.KT_TONG_CONG_SUAT) ?? 0;


                int pageSize = FDB.Common.Constants.PageSize;
                int pageNumber = t.Page ?? 1;

                t.SearchResults = tau.ToPagedList(pageNumber, pageSize);

                return View(t);
            }
        }
        #endregion

        #region "xoa"
        //public JsonResult Delete(string id)
        //{
        //    try
        //    {
        //        KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(id);

        //        if (t.KT_GIAYPHEPs.Count > 0)
        //        {
        //            db.KT_GIAYPHEP.Where(p => p.SO_DK == id).ToList().ForEach(o => db.KT_GIAYPHEP.Remove(o));

        //            db.SaveChanges();
        //        }


        //        db.KT_TAUTHUYEN.Remove(t);
        //        db.SaveChanges();

        //        return Json(t, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }                
        //}

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
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(id);

            // xoa giay phep
            foreach (var gp in t.KT_GIAYPHEPs)
            {
                db.KT_GIAYPHEP_NK.Where(m => m.KT_GIAYPHEPID == gp.KT_GIAYPHEPID).ToList().ForEach(o => db.KT_GIAYPHEP_NK.Remove(o));

            }

            db.KT_GIAYPHEP.Where(p => p.SO_DK == id).ToList().ForEach(o => db.KT_GIAYPHEP.Remove(o));

            // xoa tau thuyen
            db.KT_TAUTHUYEN.Remove(t);

            await db.SaveChangesAsync();

            return Json(new { success = true });
        }

        #endregion

        #region "Tau chua cap phep"
        public ActionResult TauChuaCapPhep(ViewModelSearchKT_TAUTHUYEN t)
        {
            if (t.SearchButton == "Xuất Excel")
            {
                ExportExcel_TauChuaCapPhep(t);
                //Dowload file:
                return File(GENERATED_FILE_EXPORT_NAME, "application/vnd.ms-excel", "Tau_Chua_Cap_PhepExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
            }
            else
            {
                string ma_TinhTP = string.Empty;

                initialCategorySearchAction(t, ref ma_TinhTP);

                var tau = db.KT_TAUTHUYEN.Where(o => o.IsCapPhep == null
                                                    && (string.IsNullOrEmpty(t.SO_DK) || o.SO_DK.ToLower().Contains(t.SO_DK.ToLower()))
                                                    && (string.IsNullOrEmpty(t.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(t.CHU_PHUONG_TIEN.ToLower()))
                    //&& (string.IsNullOrEmpty(t.MA_TINHTP) || o.MA_TINHTP == t.MA_TINHTP)
                    // && ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                    && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                    && (string.IsNullOrEmpty(t.MA_QUANHUYEN) || o.MA_QUANHUYEN == t.MA_QUANHUYEN)
                                                    && (string.IsNullOrEmpty(t.MA_PHUONGXA) || o.MA_PHUONGXA == t.MA_PHUONGXA)
                                                    && (t.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == t.DCONG_DUNG_TAUID)
                                                    && (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                    && (t.DTINH_TRANG_TAUID == null || o.DTINH_TRANG_TAUID == t.DTINH_TRANG_TAUID)
                                                    && (t.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == t.DTINH_TRANG_DANG_KIEMID)
                                                    && (t.KT_TONG_CONG_SUAT_TU == null || o.KT_TONG_CONG_SUAT >= t.KT_TONG_CONG_SUAT_TU)
                                                    && (t.KT_TONG_CONG_SUAT_DEN == null || o.KT_TONG_CONG_SUAT <= t.KT_TONG_CONG_SUAT_DEN)
                                                    && (t.NGAY_DK_TU == null || o.NGAY_DK >= t.NGAY_DK_TU)
                                                    && (t.NGAY_DK_DEN == null || o.NGAY_DK <= t.NGAY_DK_DEN)
                                                ).OrderByDescending(m => new { m.NGAY_DK, m.SO_DK });

                ViewBag.TotalRow = tau.Count();
                int pageSize = FDB.Common.Constants.PageSize;
                int pageNumber = t.Page ?? 1;

                t.SearchResults = tau.ToPagedList(pageNumber, pageSize);

                return View(t);
            }
        }
        #endregion


        //public ActionResult Details(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    KT_TAUTHUYEN t = db.KT_TAUTHUYEN.Find(id);

        //    if (t == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return PartialView(t);
        //}

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KT_TAUTHUYEN t = await db.KT_TAUTHUYEN.FindAsync(id);

            if (t == null)
            {
                return HttpNotFound();
            }

            return PartialView("Details", t);
        }


        #region "load danh muc"
        public void initialCategoryCreateAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");
            ViewBag.TINH_TRANG_TAUs = new SelectList(db.DTINH_TRANG_TAU, "ID", "Name");
            ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");

            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;


            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");

            ViewBag.NGHE_KHAI_THACs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
        }

        public void initialCategoryEditAction(KT_TAUTHUYEN tau)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");
            ViewBag.TINH_TRANG_TAUs = new SelectList(db.DTINH_TRANG_TAU, "ID", "Name");
            ViewBag.LOAI_KIEM_TRA_KY_THUATs = new SelectList(db.DLOAI_KIEM_TRA_KT, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == tau.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == tau.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");

            ViewBag.NGHE_KHAI_THACs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
        }

        public void initialCategorySearchAction(ViewModelSearchKT_TAUTHUYEN tau, ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");
            ViewBag.TINH_TRANG_TAUs = new SelectList(db.DTINH_TRANG_TAU, "ID", "Name");
            ViewBag.TINH_TRANG_DANG_KIEMs = new SelectList(db.DTINH_TRANG_DANG_KIEM, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == tau.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == tau.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            ViewBag.CAP_TAU_DANG_KIEMs = new SelectList(db.DCAP_TAU_DANG_KIEM, "ID", "Name");

            ViewBag.NGHE_KHAI_THACs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");

            ma_Tinh = curUser.MA_TINHTP;
        }

        public void initialCategoryStatisticsAction(ViewModelSearchTK_TAUTHUYEN tau, ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            //ViewBag.CONG_DUNG_TAU_CAs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");
            ViewBag.VAT_LIEU_VOs = new SelectList(db.DVAT_LIEU_VO, "ID", "Name");
            ViewBag.TINH_TRANG_TAUs = new SelectList(db.DTINH_TRANG_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == tau.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == tau.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DCONG_DUNG_TAUs = new SelectList(db.DCONG_DUNG_TAU, "ID", "Name");

            ma_Tinh = curUser.MA_TINHTP;

            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }


        [HttpPost]
        public ActionResult getDistrict(string ma_TinhTP)
        {
            var districts = db.DQUANHUYEN.Where(d => d.MA_TINHTP == ma_TinhTP).Select(a => "<option value='" + a.MA_QUANHUYEN + "'>" + a.TEN_QUANHUYEN + "</option>");

            return Content(string.Join("", districts));
        }

        [HttpPost]
        public ActionResult getWard(string ma_QuanHuyen)
        {
            var districts = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == ma_QuanHuyen).Select(a => "<option value='" + a.MA_PHUONGXA + "'>" + a.TEN_PHUONGXA + "</option>");

            return Content(string.Join("", districts));
        }

        [HttpPost]
        public JsonResult check_SO_DK_Exist(string SO_DK, int isEdit)
        {
            if (isEdit == 0)
            {
                var tau = db.KT_TAUTHUYEN.FirstOrDefault(m => m.SO_DK == SO_DK);

                return Json(tau == null);
            }

            return Json(true);
        }
        #endregion

        #region"Import Excel"
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


                        if (string.IsNullOrEmpty(strValiteDataTable))
                        {
                            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._KT_TAUTHUYENmapPropertyNameToText);
                            string str2 = this.FDBValidateDataTableColumnEmpty(this._dictColumnValidate, dtData, 3);
                            if (!string.IsNullOrEmpty(str2))
                            {
                                strValidationError.Append(str2);
                                goto ExitForRow;
                            }


                            this.InitDictDanhMuc();

                            //DateTimeFormatInfo dateFormat = new CultureInfo("vi-VN", false).DateTimeFormat;
                            DateTimeFormatInfo dateFormat = new CultureInfo("en-AU", false).DateTimeFormat;

                            for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
                            {
                                DataRow r = dtData.Rows[rowIndex];
                                for (int i = 0; i < dtData.Columns.Count; i++)
                                {
                                    switch (dtData.Columns[i].ColumnName)
                                    {

                                        case "NGAY_DK":

                                            r[i] = Convert.ToDateTime(r[i].ToString());

                                            break;

                                        case "MA_TINHTP":

                                            if (this._dictTThanhPho.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictTThanhPho[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột tỉnh/TP tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }



                                            break;
                                        case "MA_QUANHUYEN":

                                            if (this._dictQHuyen.ContainsKey(r["MA_TINHTP"].ToString() + "#" + r[i].ToString()))
                                                r[i] = this._dictQHuyen[r["MA_TINHTP"].ToString() + "#" + r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột quận/huyện tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;
                                        case "MA_PHUONGXA":

                                            if (this._dictXaPhuong.ContainsKey(r["MA_QUANHUYEN"].ToString() + "#" + r[i].ToString()))
                                                r[i] = this._dictXaPhuong[r["MA_QUANHUYEN"].ToString() + "#" + r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột phường/xã tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;

                                        case "DCONG_DUNG_TAUID":

                                            if (this._dictCongDungTau.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictCongDungTau[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột công dụng tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;
                                        case "DNHOM_TAUID":

                                            if (this._dictNhomTau.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictNhomTau[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột nhóm tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;
                                        case "DTINH_TRANG_TAUID":

                                            if (this._dictTinhTrangTau.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictTinhTrangTau[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Tình trạng tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;

                                        case "DVAT_LIEU_VOID":

                                            if (this._dictVatLieuVo.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictVatLieuVo[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Vật liệu vỏ tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;

                                        //Validate Nghề chính:
                                        case "NGHE_CHINH_ID":

                                            if (this._dictNghe.ContainsKey(r[i].ToString()))
                                                r[i] = this._dictNghe[r[i].ToString()];
                                            else
                                            {
                                                strValidationError.Append("<span style=\"color:red\">Cột Nghề chính tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                goto ExitForRow;
                                            }


                                            break;

                                        //validate nghe phụ:
                                        case "NGHE_PHU_ID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictNghe.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictNghe[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Nghề phụ tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;

                                        //tình trạng đăng kiểm(kết quả kiểm tra)
                                        case "DTINH_TRANG_DANG_KIEMID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictKetQuaKiemTra.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictKetQuaKiemTra[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Kết quả kiểm tra tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        //Cấp tàu đăng ký:
                                        case "DCAP_TAU_DANG_KIEMID":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                if (this._dictCapTau.ContainsKey(r[i].ToString()))
                                                    r[i] = this._dictCapTau[r[i].ToString()];
                                                else
                                                {
                                                    strValidationError.Append("<span style=\"color:red\">Cột Cấp tàu tại dòng " + (rowIndex + 3).ToString() + " không đúng danh mục quy định!</span> <br/>");
                                                    goto ExitForRow;
                                                }
                                            }

                                            break;
                                        case "NGAY_CAP_SO_DANG_KIEM":
                                            if (!r[i].Equals(DBNull.Value))
                                            {
                                                r[i] = Convert.ToDateTime(r[i].ToString(), dateFormat);
                                            }

                                            break;
                                        case "HAN_DANG_KIEM":
                                            if (!r[i].Equals(DBNull.Value))
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
                                //Ghi dữ liệu vào bảng Temp
                                //Remove row duplicate SO_DK :
                                DataTable dtDataDistinct = FDB.Common.Helpers.RemoveDuplicateRows(dtData, "SO_DK");
                                //string str1 = "";
                                //foreach (DataRow r in dtDataDistinct.Rows)
                                //{
                                //    for (int i = 0; i < dtDataDistinct.Columns.Count; i++)
                                //    {
                                //        if (!r[i].Equals(DBNull.Value))
                                //            str1 += dtDataDistinct.Columns[i].ColumnName + ":" + r[i].ToString() + ";";
                                //    }

                                //}
                                //    ViewBag.ThongBao2 = str1;
                                //Insert using SQLbulkCopy :
                                string tablename = "KT_TAUTHUYEN_TEMP_" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                                string SqlBulkConnectionString = System.Configuration.ConfigurationManager.AppSettings["SqlBulkCopyConnectionString"].ToString();
                                int _BatchSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BatchSize"].ToString());
                                //Begin 
                                using (db.Database.Connection)
                                {
                                    try
                                    {
                                        //Create temp table:
                                        var name = new SqlParameter("@tablename", tablename);
                                        db.Database.ExecuteSqlCommand("exec CreateTableKT_TAUTHUYEN_TEMP @tablename", name);
                                        db.SaveChanges();
                                        using (var txt = new TransactionScope())
                                        {
                                            try
                                            {
                                                using (var bcp = new SqlBulkCopy(SqlBulkConnectionString))
                                                {
                                                    bcp.BatchSize = _BatchSize;
                                                    bcp.DestinationTableName = tablename;
                                                    bcp.WriteToServer(dtDataDistinct);
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

                                        //Đồng bộ từ temp sang thật:
                                        //Merge data from Temp table KT_TAUTHUYEN_TEMP to Table KT_TAUTHUYEN
                                        //Merge complete then Drop Temp table 
                                        var name2 = new SqlParameter("@tablename", tablename);
                                        db.Database.ExecuteSqlCommand("exec MergeIntoDataKT_TAUTHUYEN @tablename", name2);
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

        //Dictionary dùng để mapping tên cột file excel thành tên cột trong bảng KT_TAUTHUYEN
        private Dictionary<string, string> _KT_TAUTHUYENmapPropertyNameToText = new Dictionary<string, string>()
        {

                {"Số ĐK", "SO_DK"},
                {"Ngày ĐK", "NGAY_DK"},
                {"Nhóm tàu", "DNHOM_TAUID"},
                {"Công dụng tàu", "DCONG_DUNG_TAUID"},
                {"Tình trạng tàu", "DTINH_TRANG_TAUID"},
                {"Tên tàu", "TEN_TAU"},
                {"Nghề chính", "NGHE_CHINH_ID"},
                {"Nghề phụ", "NGHE_PHU_ID"},
                {"Số thuyền viên", "SO_THUYEN_VIEN"},
                {"Chủ phương tiện", "CHU_PHUONG_TIEN"},
                {"Số CMND", "SO_CMND"},
                {"Điện thoại", "DIEN_THOAI"},
                {"Tỉnh/TP", "MA_TINHTP"},
                {"Quận/huyện", "MA_QUANHUYEN"},
                {"Xã/phường", "MA_PHUONGXA"},
                {"Địa chỉ", "DIA_CHI"},
                {"Số sổ đăng kiểm", "SO_SO_DANG_KIEM"},
                {"Ngày cấp sổ", "NGAY_CAP_SO_DANG_KIEM"},
                {"Kết quả kiểm tra", "DTINH_TRANG_DANG_KIEMID"},
                {"Cấp tàu", "DCAP_TAU_DANG_KIEMID"},
                {"Cơ quan đăng kiểm", "CO_QUAN_DANG_KIEM"},
                {"Ngày hết hạn đăng kiểm", "HAN_DANG_KIEM"},
                {"Đăng kiểm viên", "DANG_KIEM_VIEN"},
                {"Số chứng nhận ATKT - Tàu cá", "SO_AN_CHI_ATKT"},
                {"Lmax (m)", "KT_CHIEU_DAI"},
                {"Bmax (m)", "KT_CHIEU_RONG"},
                {"D (m)", "KT_CHIEU_CAO"},
                {"d (m)", "KT_MON_NUOC"},
                {"Mạn khô f (m)", "KT_MAN_KHO"},
                {"Tấn đăng ký (RT)", "KT_TAN_DANG_KY"},
                {"Vật liệu vỏ", "DVAT_LIEU_VOID"},
                {"Tốc độ tàu (hl/h)", "KT_TOC_DO_TAU"},
                {"Cơ sở đóng/sửa tàu", "KT_NOI_DONG"},
                {"Năm đóng", "KT_NAM_DONG"},
                {"Số lượng máy tàu", "KT_SO_MAY_TAU"},
                {"Tổng công suất", "KT_TONG_CONG_SUAT"},
                {"Kí hiệu máy", "M1_KY_HIEU_MAY"},
                {"Số máy", "M1_SO_MAY"},
                {"Nơi SX", "M1_NOI_SX"},
                {"Năm chế tạo", "M1_NAM_CHE_TAO"},
                {"Hãng Máy", "M1_HANG_MAY"},
                {"Vòng quay (V/p)", "M1_VONG_QUAY"},
                {"Công suất", "M1_CONG_SUAT"}
                
              
        };

        //Dictionary Column can not Empty:
        private Dictionary<string, string> _dictColumnValidate = new Dictionary<string, string>()
        {
            {"SO_DK","Số ĐK"},
            {"NGAY_DK","Ngày ĐK"},
            {"CHU_PHUONG_TIEN","Chủ phương tiện"},
            {"MA_TINHTP","tỉnh/TP"},
            {"MA_QUANHUYEN","quận/huyện"},
            {"MA_PHUONGXA","Xã/Phường"},
            {"DIA_CHI","Địa chỉ"},
            {"DCONG_DUNG_TAUID","Công dụng tàu"},
            {"DNHOM_TAUID","Nhóm tàu"},
            {"DTINH_TRANG_TAUID","Tình trạng tàu"},
            {"KT_CHIEU_DAI","Chiều dài"},
            {"KT_CHIEU_RONG","Chiều rộng"},
            {"KT_CHIEU_CAO","Chiều cao"},
            {"KT_MAN_KHO","Mạn khô"},
            {"KT_MON_NUOC","d (m)"},
            {"DVAT_LIEU_VOID","Vật liệu vỏ"},
            {"NGHE_CHINH_ID","Nghề chính"},
            {"KT_TONG_CONG_SUAT","Tổng công suất"},
            {"KT_SO_MAY_TAU","Số lượng máy"},
            {"M1_CONG_SUAT","Công suất"},
           
        };

        //Hàm dùng để validate file excel import có đầy đủ số cột qui định không:
        public string ValidateDataTable(DataTable dt)
        {
            string sWarining = "";
            foreach (string s in this._KT_TAUTHUYENmapPropertyNameToText.Keys)
            {
                if (dt.Columns.IndexOf(s) < 0)
                    sWarining += String.Format("<span style=\"color: #b94a48;\">&nbsp;&nbsp +) Thiếu cột: {0}</span> <br/>", s);
            }
            return sWarining;
        }

        //validate datarow empty :
        public string FDBValidateDataTableColumnEmpty(Dictionary<string, string> _dictColumnValidate, DataTable dtData, int _rowIndex)
        {
            int _Error = 0;
            StringBuilder strValidationError = new StringBuilder(string.Empty);

            for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
            {
                DataRow r = dtData.Rows[rowIndex];
                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    string _columnName = dtData.Columns[i].ColumnName;
                    if (_dictColumnValidate.Keys.Contains(_columnName) && r[i].Equals(DBNull.Value))
                    {
                        strValidationError.Append("<span style=\"color:red\">Cột " + _dictColumnValidate[_columnName] + " tại dòng " + (rowIndex + _rowIndex).ToString() + " không được để trống!</span> <br/>");
                        _Error = 1;
                    }
                }
                if (_Error > 0)
                    break;
            }

            return strValidationError.ToString();
        }


        //Create new DataTable:hàm này dùng để tạo 1 DataTable mới, với columnname sẽ là row thứ 2 của DataTable truyền vào
        // Add giá trị từ row thứ 3 của datatable cũ  vào DataTable mới
        public DataTable CreateNewDataTableSecondRowIsColumnName(DataTable dt)
        {
            DataTable dtNew = new DataTable("KT_TAUTHUYEN");
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



        public FileResult KT_TAUTHUYENDownLoadFile()
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
            return FDB.Common.Helpers.DownLoadFile(FullPathFileName, "FDB_Template_Import_DKTT" + extension, contentType);
        }


        //Create excel file:
        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_DKTT_FileDownload.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_DKTT.xlsx");

        private void CreateExcelFile()
        {

            using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_NAME, GENERATED_FILE_NAME))
            {
                helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                helper.CurrentSheetName = "DanhMuc";

                helper.CurrentPosition = new CellRef("A2");
                helper.InsertRange("row_empty");

                CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("tinhthanhpho", new List<string> { "tinhthanhpho", "c1", "c2" });
                CellRangeTemplate row_quanhuyen = helper.CreateCellRangeTemplate("quanhuyen", new List<string> { "c3", "quanhuyen", "c4" });
                CellRangeTemplate row_phuongxa = helper.CreateCellRangeTemplate("row_phuongxa", new List<string> { "c5", "c6", "phuongxa" });

                int k = 3;
                List<String> _lstMA_TTP = new List<string>();
                var dbQuanTri = new ApplicationDbContext();

                //get mã tỉnh thành phố by user logon:
                string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
                IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(Ma_TinhTP, _lstMA_TTP);

                for (int i = 0; i < _lstTinhTP.Count(); i++)
                {

                    //insert Tinh thanh pho
                    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    helper.InsertRange(row_tinhthanhpho, _lstTinhTP.ToArray()[i]);
                    k = k + 1;
                    //insert Co so
                    List<string> _lstMaQuanHuyen = new List<string>();
                    IEnumerable<List<object>> _lstQuanHuyen = this.getQuanHuyenByTinhTP(_lstMA_TTP[i], _lstMaQuanHuyen);
                    for (int j = 0; j < _lstQuanHuyen.Count(); j++)
                    {
                        helper.CurrentPosition = new CellRef("A" + (k).ToString());
                        helper.InsertRange(row_quanhuyen, _lstQuanHuyen.ToArray()[j]);
                        k = k + 1;

                        //Insert co so detail
                        helper.CurrentPosition = new CellRef("A" + (k).ToString());
                        IEnumerable<List<object>> lstPhuongXa = this.getPhuongXaByQuanHuyen(_lstMaQuanHuyen[j]);
                        helper.InsertRange(row_phuongxa, lstPhuongXa);
                        k = k + lstPhuongXa.Count();
                    }
                    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    helper.InsertRange("row_empty");
                    k = k + 1;
                }

                //fill danh mục chuẩn:
                Dictionary<string, IEnumerable<List<object>>> dictDanhMuc = this.getAllDanhMucChuan();

                CellRangeTemplate row_nhomtau = helper.CreateCellRangeTemplate("nhomtau", new List<string> { "nhomtau1" });
                helper.CurrentPosition = new CellRef("E3");
                helper.InsertRange(row_nhomtau, dictDanhMuc["DNHOM_TAU"]);

                CellRangeTemplate row_congdungtau = helper.CreateCellRangeTemplate("congdungtau", new List<string> { "congdungtau1" });
                helper.CurrentPosition = new CellRef("F3");
                helper.InsertRange(row_congdungtau, dictDanhMuc["DCONG_DUNG_TAU"]);

                CellRangeTemplate row_tinhtrangtau = helper.CreateCellRangeTemplate("tinhtrangtau", new List<string> { "tinhtrangtau1" });
                helper.CurrentPosition = new CellRef("G3");
                helper.InsertRange(row_tinhtrangtau, dictDanhMuc["DTINH_TRANG_TAU"]);

                CellRangeTemplate row_nghe = helper.CreateCellRangeTemplate("nghe", new List<string> { "nghe1" });
                helper.CurrentPosition = new CellRef("H3");
                helper.InsertRange(row_nghe, dictDanhMuc["DM_NHOMNGHE"]);

                CellRangeTemplate row_ketquakiemtra = helper.CreateCellRangeTemplate("ketquakiemtra", new List<string> { "ketquakiemtra1" });
                helper.CurrentPosition = new CellRef("I3");
                helper.InsertRange(row_ketquakiemtra, dictDanhMuc["DTINH_TRANG_DANG_KIEM"]);

                CellRangeTemplate row_captau = helper.CreateCellRangeTemplate("captau", new List<string> { "captau1" });
                helper.CurrentPosition = new CellRef("J3");
                helper.InsertRange(row_captau, dictDanhMuc["DCAP_TAU_DANG_KIEM"]);

                CellRangeTemplate row_vatlieuvo = helper.CreateCellRangeTemplate("vatlieuvo", new List<string> { "vatlieuvo1" });
                helper.CurrentPosition = new CellRef("K3");
                helper.InsertRange(row_vatlieuvo, dictDanhMuc["DVAT_LIEU_VO"]);


                helper.DeleteSheet("Sheet2");
                helper.CurrentSheetName = "DK_TAU";

            }
        }


        //Lists dictionary dùng để mapping Text to Value các danh mục:
        private Dictionary<string, string> _dictTThanhPho = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _dictQHuyen = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _dictXaPhuong = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> _dictCongDungTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictNhomTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictTinhTrangTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictNghe = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictKetQuaKiemTra = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictCapTau = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, int> _dictVatLieuVo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ok
        private Dictionary<string, bool> _dictCapPhep = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            {"Y",true},
            {"N",false}
        };


        //Chưa tối ưu, sẽ tối ưu sau
        private void InitDictDanhMuc()
        {
            var dbQuanTri = new ApplicationDbContext();
            string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
            var objTThanhPho = db.DTINHTP.First(o => o.MA_TINHTP == Ma_TinhTP);
            _dictTThanhPho.Add(objTThanhPho.TEN_TINHTP, objTThanhPho.MA_TINHTP);
            var lstQHuyen = db.DQUANHUYEN.Where(o => o.MA_TINHTP == Ma_TinhTP).ToList();
            for (int i = 0; i < lstQHuyen.Count; i++)
            {
                //trùng key vì nhiều quận huyện trùng tên nhau==> cộng chuỗi = MA_TINHTP+#+TEN_QUANHUYEN
                _dictQHuyen.Add(objTThanhPho.MA_TINHTP + "#" + lstQHuyen[i].TEN_QUANHUYEN, lstQHuyen[i].MA_QUANHUYEN);
                string q = lstQHuyen[i].MA_QUANHUYEN.ToString();
                var lstPhuongXa = db.DPHUONGXA.Where(o => o.MA_QUANHUYEN == q).ToList();
                for (int j = 0; j < lstPhuongXa.ToList().Count; j++)
                    //trùng key vì nhiều phường xã trùng tên nhau==> cộng chuỗi = MA_QUANHUYEN+#+TEN_PHUONGXA
                    _dictXaPhuong.Add(lstQHuyen[i].MA_QUANHUYEN + "#" + lstPhuongXa[j].TEN_PHUONGXA, lstPhuongXa[j].MA_PHUONGXA);
            }

            //tạo Dictionary công dụng tàu
            var objCongDungTau = db.DCONG_DUNG_TAU.ToList();
            objCongDungTau.ForEach(o => _dictCongDungTau.Add(o.Name, o.ID));

            //tạo Dictionary nhóm tàu
            var objNhomTau = db.DNHOM_TAU.ToList();
            objNhomTau.ForEach(o => _dictNhomTau.Add(o.Name, o.ID));

            //tạo Dictionary tình trạng tàu
            var objTinhTrangTau = db.DTINH_TRANG_TAU.ToList();
            objTinhTrangTau.ForEach(o => _dictTinhTrangTau.Add(o.Name, o.ID));

            //tạo Dictionary vật liệu vỏ tàu
            var objVatLieuVo = db.DVAT_LIEU_VO.ToList();
            objVatLieuVo.ForEach(o => _dictVatLieuVo.Add(o.Name, o.ID));

            //tạo Dictionary nhóm nghề
            var objNghe = db.DM_NHOMNGHE.ToList();
            objNghe.ForEach(o => _dictNghe.Add(o.TenNhomNghe, o.DM_NhomNgheID));

            //tạo Dictionary kết quả kiểm tra <=> tình trạng đăng kiểm
            var objKetQuaKiemTra = db.DTINH_TRANG_DANG_KIEM.ToList();
            objKetQuaKiemTra.ForEach(o => _dictKetQuaKiemTra.Add(o.Name, o.ID));

            //tạo Dictionary cấp tàu đăng kiểm
            var objCapTau = db.DCAP_TAU_DANG_KIEM.ToList();
            objCapTau.ForEach(o => _dictCapTau.Add(o.Name, o.ID));

        }


        //get tinh thanh pho 
        private IEnumerable<List<object>> getTinhThanhPho(string MA_TTP, List<string> lstMaTTP)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMaTTP == null)
                lstMaTTP = new List<string>();

            var selects = db.DTINHTP.Where(o => o.MA_TINHTP == MA_TTP);
            var models = selects.ToList();
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { models[loop].TEN_TINHTP, " ", " " });
                lstMaTTP.Add(models[loop].MA_TINHTP);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get co so by tinh tp
        private IEnumerable<List<object>> getQuanHuyenByTinhTP(String MA_TTP, List<string> lstMaQuanHuyen)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMaQuanHuyen == null)
                lstMaQuanHuyen = new List<string>();

            var selects = db.DQUANHUYEN.Where(o => o.MA_TINHTP == MA_TTP);

            var models = selects.ToList();
            //  var models = _context.KT_DONGSUA_TAUTHUYEN;
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { " ", models[loop].TEN_QUANHUYEN, " " });
                lstMaQuanHuyen.Add(models[loop].MA_QUANHUYEN);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get Phuong Xa by quan huyen
        private IEnumerable<List<object>> getPhuongXaByQuanHuyen(string MA_QHUYEN)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();

            var selects = db.DPHUONGXA.Where(o => o.MA_QUANHUYEN == MA_QHUYEN);
            var models = selects.ToList();
            //  var models = _context.KT_DONGSUA_TAUTHUYEN;
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { " ", " ", models[loop].TEN_PHUONGXA });

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get all danh mục chuẩn:
        private Dictionary<string, IEnumerable<List<object>>> getAllDanhMucChuan()
        {
            //tạo Dictionary để lưu IEnumerable<List<object>> các danh mục, mỗi Key,value tương ứng với 1 bảng danh mục
            Dictionary<string, IEnumerable<List<object>>> dictItems = new Dictionary<string, IEnumerable<List<object>>>();

            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj;

            //get nhóm tàu
            var selects1 = db.DNHOM_TAU.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects1.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects1[loop].Name });

            }
            dictItems.Add("DNHOM_TAU", lstobj.AsEnumerable<List<object>>());

            //get công dụng tàu
            var selects2 = db.DCONG_DUNG_TAU.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects2.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects2[loop].Name });

            }
            dictItems.Add("DCONG_DUNG_TAU", lstobj.AsEnumerable<List<object>>());

            //get tình trạng tàu
            var selects3 = db.DTINH_TRANG_TAU.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects3.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects3[loop].Name });

            }
            dictItems.Add("DTINH_TRANG_TAU", lstobj.AsEnumerable<List<object>>());

            //get nhóm nghề
            var selects4 = db.DM_NHOMNGHE.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects4.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects4[loop].TenNhomNghe });

            }
            dictItems.Add("DM_NHOMNGHE", lstobj.AsEnumerable<List<object>>());

            //get kết quả kiểm tra
            var selects5 = db.DTINH_TRANG_DANG_KIEM.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects5.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects5[loop].Name });

            }
            dictItems.Add("DTINH_TRANG_DANG_KIEM", lstobj.AsEnumerable<List<object>>());

            //get cấp tầu
            var selects6 = db.DCAP_TAU_DANG_KIEM.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects6.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects6[loop].Name });

            }
            dictItems.Add("DCAP_TAU_DANG_KIEM", lstobj.AsEnumerable<List<object>>());

            //get vật liệu vỏ
            var selects7 = db.DVAT_LIEU_VO.ToList();
            lstobj = new List<List<object>>();
            for (int loop = 0; loop < selects7.Count(); loop++)
            {
                lstobj.Add(new List<object> { selects7[loop].Name });

            }
            dictItems.Add("DVAT_LIEU_VO", lstobj.AsEnumerable<List<object>>());


            return dictItems;
        }

        #endregion


        #region"Working Files"
        private static String GENERATED_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_EXPORT_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_DKTT.xlsx");
        #endregion



        #region "Export excel"



        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchKT_TAUTHUYEN t, List<String> lstMA_TTP)
        {
            //get nhóm nghề


            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(t, ref ma_TinhTP);
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMA_TTP == null)
                lstMA_TTP = new List<String>();
            var selects = db.KT_TAUTHUYEN.Where(o => (string.IsNullOrEmpty(t.SO_DK) || o.SO_DK.ToLower().Contains(t.SO_DK.ToLower()))
                                                && (string.IsNullOrEmpty(t.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(t.CHU_PHUONG_TIEN.ToLower()))
                //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && (string.IsNullOrEmpty(t.MA_QUANHUYEN) || o.MA_QUANHUYEN == t.MA_QUANHUYEN)
                                                && (string.IsNullOrEmpty(t.MA_PHUONGXA) || o.MA_PHUONGXA == t.MA_PHUONGXA)
                                                && (t.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == t.DCONG_DUNG_TAUID)
                                                && (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                && (t.DTINH_TRANG_TAUID == null || o.DTINH_TRANG_TAUID == t.DTINH_TRANG_TAUID)
                                                && (t.DNHOM_NGHECHINHID == null || o.NGHE_CHINH_ID == t.DNHOM_NGHECHINHID)
                                                && (t.DNHOM_NGHEPHUID == null || o.NGHE_PHU_ID == t.DNHOM_NGHEPHUID)
                                                && (t.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == t.DTINH_TRANG_DANG_KIEMID)
                                                && (t.KT_TONG_CONG_SUAT_TU == null || o.KT_TONG_CONG_SUAT >= t.KT_TONG_CONG_SUAT_TU)
                                                && (t.KT_TONG_CONG_SUAT_DEN == null || o.KT_TONG_CONG_SUAT <= t.KT_TONG_CONG_SUAT_DEN)
                                                && (t.NGAY_DK_TU == null || o.NGAY_DK >= t.NGAY_DK_TU)
                                                && (t.NGAY_DK_DEN == null || o.NGAY_DK <= t.NGAY_DK_DEN)
                                            );
            //.Select(x => new { MA_TTP = x.MA_TINHTP, TEN_TTP = x.DTINHTP.TEN_TINHTP }).Distinct();

            var models = selects.ToList();


            //lay nhom nghe 
            var dictNhomNghes = db.DM_NHOMNGHE.ToDictionary(o => o.DM_NhomNgheID);


            for (int loop = 0; loop < models.Count(); loop++)
            {


                lstobj.Add(new List<object> { (loop + 1).ToString(),
                                                models[loop].DTINHTP == null || models[loop].DTINHTP.TEN_TINHTP==null?" ": models[loop].DTINHTP.TEN_TINHTP.ToString(),
                                                models[loop].DQUANHUYEN == null ||models[loop].DQUANHUYEN.TEN_QUANHUYEN==null?" ": models[loop].DQUANHUYEN.TEN_QUANHUYEN.ToString(),
                                                models[loop].SO_DK==null?" ": models[loop].SO_DK.ToString(),
                                                models[loop].NGAY_DK==null?" ": models[loop].NGAY_DK.ToShortDateString(),
                                                models[loop].DNHOM_TAU == null ||  models[loop].DNHOM_TAU.Name==null?" ": models[loop].DNHOM_TAU.Name.ToString(),
                                                models[loop].DCONG_DUNG_TAU == null ||   models[loop].DCONG_DUNG_TAU.Name==null?" ": models[loop].DCONG_DUNG_TAU.Name.ToString(),
                                                models[loop].DTINH_TRANG_TAU == null ||  models[loop].DTINH_TRANG_TAU.Name==null?" ": models[loop].DTINH_TRANG_TAU.Name.ToString(),
                                                models[loop].TEN_TAU==null?" ": models[loop].TEN_TAU.ToString(),
                                                models[loop].NGHE_CHINH_ID==null?" ": dictNhomNghes[models[loop].NGHE_CHINH_ID.Value].TenNhomNghe,
                                                //models[loop].NGHE_CHINH_ID==null?" ": models[loop].NGHE_CHINH_ID.ToString(),
                                                //models[loop].NGHE_PHU_ID==null?" ": models[loop].NGHE_PHU_ID.ToString(),
                                                models[loop].NGHE_PHU_ID==null?" ": dictNhomNghes[models[loop].NGHE_PHU_ID.Value].TenNhomNghe,
                                                models[loop].SO_THUYEN_VIEN==null?" ": models[loop].SO_THUYEN_VIEN.ToString(),
                                                models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                                models[loop].SO_CMND==null?" ": models[loop].SO_CMND.ToString(),
                                                models[loop].DIEN_THOAI==null?" ": models[loop].DIEN_THOAI.ToString(),                                                
                                                 models[loop].DPHUONGXA == null ||models[loop].DPHUONGXA.TEN_PHUONGXA==null?" ": models[loop].DPHUONGXA.TEN_PHUONGXA.ToString(),
                                                 models[loop].DIA_CHI==null?" ": models[loop].DIA_CHI.ToString(),
                                                 models[loop].SO_SO_DANG_KIEM==null?" ": models[loop].SO_SO_DANG_KIEM.ToString(),
                                                 models[loop].NGAY_CAP_SO_DANG_KIEM==null?" ": models[loop].NGAY_CAP_SO_DANG_KIEM.ToString(),
                                                 models[loop].DTINH_TRANG_DANG_KIEM == null || models[loop].DTINH_TRANG_DANG_KIEM.Name==null ?" ": models[loop].DTINH_TRANG_DANG_KIEM.Name.ToString(),
                                                 models[loop].DCAP_TAU_DANG_KIEM == null || models[loop].DCAP_TAU_DANG_KIEM.Name==null?" ": models[loop].DCAP_TAU_DANG_KIEM.Name.ToString(),
                                                 models[loop].CO_QUAN_DANG_KIEM==null?" ": models[loop].CO_QUAN_DANG_KIEM.ToString(),
                                                 models[loop].HAN_DANG_KIEM==null?" ": models[loop].HAN_DANG_KIEM.ToString(),
                                                 models[loop].DANG_KIEM_VIEN==null?" ": models[loop].DANG_KIEM_VIEN.ToString(),
                                                 models[loop].SO_AN_CHI_ATKT==null?" ": models[loop].SO_AN_CHI_ATKT.ToString(),
                                                  models[loop].KT_CHIEU_DAI==null?" ": models[loop].KT_CHIEU_DAI.ToString(),
                                                  models[loop].KT_CHIEU_RONG==null?" ": models[loop].KT_CHIEU_RONG.ToString(),
                                                  models[loop].KT_CHIEU_CAO==null?" ": models[loop].KT_CHIEU_CAO.ToString(),
                                                  models[loop].KT_MON_NUOC==null?" ": models[loop].KT_MON_NUOC.ToString(),
                                                  models[loop].KT_MAN_KHO==null?" ": models[loop].KT_MAN_KHO.ToString(),
                                                  models[loop].KT_TAN_DANG_KY==null?" ": models[loop].KT_TAN_DANG_KY.ToString(),
                                                  models[loop].DVAT_LIEU_VO == null || models[loop].DVAT_LIEU_VO.Name==null?" ": models[loop].DVAT_LIEU_VO.Name.ToString(),
                                                  models[loop].KT_TOC_DO_TAU==null?" ": models[loop].KT_TOC_DO_TAU.ToString(),
                                                  models[loop].KT_NOI_DONG==null?" ": models[loop].KT_NOI_DONG.ToString(),
                                                  models[loop].KT_NAM_DONG==null?" ": models[loop].KT_NAM_DONG.ToString(),
                                                  models[loop].KT_SO_MAY_TAU==null?" ": models[loop].KT_SO_MAY_TAU.ToString(),
                                                  models[loop].KT_TONG_CONG_SUAT==null?" ": models[loop].KT_TONG_CONG_SUAT.ToString(),
                                                  models[loop].M1_KY_HIEU_MAY==null?" ": models[loop].M1_KY_HIEU_MAY.ToString(),
                                                  models[loop].M1_SO_MAY==null?" ": models[loop].M1_SO_MAY.ToString(),
                                                  models[loop].M1_NOI_SX==null?" ": models[loop].M1_NOI_SX.ToString(),
                                                  models[loop].M1_NAM_CHE_TAO==null?" ": models[loop].M1_NAM_CHE_TAO.ToString(),
                                                  models[loop].M1_HANG_MAY==null?" ": models[loop].M1_HANG_MAY.ToString(),
                                                  models[loop].M1_VONG_QUAY==null?" ": models[loop].M1_VONG_QUAY.ToString(),
                                                  models[loop].M1_CONG_SUAT==null?" ": models[loop].M1_CONG_SUAT.ToString(),
                                                  models[loop].M2_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M2_SO_MAY==null?" ": models[loop].M2_SO_MAY.ToString(),
                                                  models[loop].M2_NOI_SX==null?" ": models[loop].M2_NOI_SX.ToString(),
                                                  models[loop].M2_NAM_CHE_TAO==null?" ": models[loop].M2_NAM_CHE_TAO.ToString(),
                                                  models[loop].M2_HANG_MAY==null?" ": models[loop].M2_HANG_MAY.ToString(),
                                                  models[loop].M2_VONG_QUAY==null?" ": models[loop].M2_VONG_QUAY.ToString(),
                                                  models[loop].M2_CONG_SUAT==null?" ": models[loop].M2_CONG_SUAT.ToString(),
                                                  models[loop].M3_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M3_SO_MAY==null?" ": models[loop].M3_SO_MAY.ToString(),
                                                  models[loop].M3_NOI_SX==null?" ": models[loop].M3_NOI_SX.ToString(),
                                                  models[loop].M3_NAM_CHE_TAO==null?" ": models[loop].M3_NAM_CHE_TAO.ToString(),
                                                  models[loop].M3_HANG_MAY==null?" ": models[loop].M3_HANG_MAY.ToString(),
                                                  models[loop].M3_VONG_QUAY==null?" ": models[loop].M3_VONG_QUAY.ToString(),
                                                  models[loop].M3_CONG_SUAT==null?" ": models[loop].M3_CONG_SUAT.ToString(),}
                                                          );

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        private IEnumerable<List<object>> getTAUTHUYENByTinhTP(String MA_TTP, List<string> lstSoDK)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstSoDK == null)
                lstSoDK = new List<string>();

            var selects = db.KT_TAUTHUYEN.Where(o => o.MA_TINHTP == MA_TTP)
                        .Select(x => new { SO_DK = x.SO_DK, TEN_QUANHUYEN = x.DQUANHUYEN.TEN_QUANHUYEN }).Distinct();

            var models = selects.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { (loop + 1).ToString(),models[loop].TEN_QUANHUYEN,
                                                             " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "
                                                             , " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "
                                                             , " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "
                                                             , " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "
                                                            , " ", " ", " ", " ", " ", " "," " }
                                                          );
                lstSoDK.Add(models[loop].SO_DK);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get detail by co so
        private IEnumerable<List<object>> getDetailTauthuyen(string MaQuanHuyen, string sodk)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();


            var selects = db.KT_TAUTHUYEN.Where(o => o.SO_DK == sodk); //&& o.DQUANHUYEN.MA_QUANHUYEN == MaQuanHuyen);
            var models = selects.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { " "," ",
                                           models[loop].SO_DK==null?" ": models[loop].SO_DK.ToString(),
                                           models[loop].NGAY_DK==null?" ": models[loop].NGAY_DK.ToShortDateString(),
                                          models[loop].DNHOM_TAU == null ||  models[loop].DNHOM_TAU.Name==null?" ": models[loop].DNHOM_TAU.Name.ToString(),
                                          models[loop].DCONG_DUNG_TAU == null ||   models[loop].DCONG_DUNG_TAU.Name==null?" ": models[loop].DCONG_DUNG_TAU.Name.ToString(),
                                           models[loop].DTINH_TRANG_TAU == null ||  models[loop].DTINH_TRANG_TAU.Name==null?" ": models[loop].DTINH_TRANG_TAU.Name.ToString(),
                                             models[loop].TEN_TAU==null?" ": models[loop].TEN_TAU.ToString(),
                                             models[loop].NGHE_CHINH_ID==null?" ": models[loop].NGHE_CHINH_ID.ToString(),
                                              models[loop].NGHE_PHU_ID==null?" ": models[loop].NGHE_PHU_ID.ToString(),
                                              models[loop].SO_THUYEN_VIEN==null?" ": models[loop].SO_THUYEN_VIEN.ToString(),
                                              models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                               models[loop].SO_CMND==null?" ": models[loop].SO_CMND.ToString(),
                                               models[loop].DIEN_THOAI==null?" ": models[loop].DIEN_THOAI.ToString(),                                                
                                                 models[loop].DPHUONGXA == null ||models[loop].DPHUONGXA.TEN_PHUONGXA==null?" ": models[loop].DPHUONGXA.TEN_PHUONGXA.ToString(),
                                                 models[loop].DIA_CHI==null?" ": models[loop].DIA_CHI.ToString(),
                                                 models[loop].SO_SO_DANG_KIEM==null?" ": models[loop].SO_SO_DANG_KIEM.ToString(),
                                                 models[loop].NGAY_CAP_SO_DANG_KIEM==null?" ": models[loop].NGAY_CAP_SO_DANG_KIEM.ToString(),
                                                 models[loop].DTINH_TRANG_DANG_KIEM == null || models[loop].DTINH_TRANG_DANG_KIEM.Name==null ?" ": models[loop].DTINH_TRANG_DANG_KIEM.Name.ToString(),
                                                 models[loop].DCAP_TAU_DANG_KIEM == null || models[loop].DCAP_TAU_DANG_KIEM.Name==null?" ": models[loop].DCAP_TAU_DANG_KIEM.Name.ToString(),
                                                 models[loop].CO_QUAN_DANG_KIEM==null?" ": models[loop].CO_QUAN_DANG_KIEM.ToString(),
                                                 models[loop].HAN_DANG_KIEM==null?" ": models[loop].HAN_DANG_KIEM.ToString(),
                                                 models[loop].DANG_KIEM_VIEN==null?" ": models[loop].DANG_KIEM_VIEN.ToString(),
                                                 models[loop].SO_AN_CHI_ATKT==null?" ": models[loop].SO_AN_CHI_ATKT.ToString(),
                                                  models[loop].KT_CHIEU_DAI==null?" ": models[loop].KT_CHIEU_DAI.ToString(),
                                                  models[loop].KT_CHIEU_RONG==null?" ": models[loop].KT_CHIEU_RONG.ToString(),
                                                  models[loop].KT_CHIEU_CAO==null?" ": models[loop].KT_CHIEU_CAO.ToString(),
                                                  models[loop].KT_MON_NUOC==null?" ": models[loop].KT_MON_NUOC.ToString(),
                                                  models[loop].KT_MAN_KHO==null?" ": models[loop].KT_MAN_KHO.ToString(),
                                                  models[loop].KT_TAN_DANG_KY==null?" ": models[loop].KT_TAN_DANG_KY.ToString(),
                                                  models[loop].DVAT_LIEU_VO == null || models[loop].DVAT_LIEU_VO.Name==null?" ": models[loop].DVAT_LIEU_VO.Name.ToString(),
                                                  models[loop].KT_TOC_DO_TAU==null?" ": models[loop].KT_TOC_DO_TAU.ToString(),
                                                  models[loop].KT_NOI_DONG==null?" ": models[loop].KT_NOI_DONG.ToString(),
                                                  models[loop].KT_NAM_DONG==null?" ": models[loop].KT_NAM_DONG.ToString(),
                                                  models[loop].KT_SO_MAY_TAU==null?" ": models[loop].KT_SO_MAY_TAU.ToString(),
                                                  models[loop].KT_TONG_CONG_SUAT==null?" ": models[loop].KT_TONG_CONG_SUAT.ToString(),
                                                  models[loop].M1_KY_HIEU_MAY==null?" ": models[loop].M1_KY_HIEU_MAY.ToString(),
                                                  models[loop].M1_SO_MAY==null?" ": models[loop].M1_SO_MAY.ToString(),
                                                  models[loop].M1_NOI_SX==null?" ": models[loop].M1_NOI_SX.ToString(),
                                                  models[loop].M1_NAM_CHE_TAO==null?" ": models[loop].M1_NAM_CHE_TAO.ToString(),
                                                  models[loop].M1_HANG_MAY==null?" ": models[loop].M1_HANG_MAY.ToString(),
                                                  models[loop].M1_VONG_QUAY==null?" ": models[loop].M1_VONG_QUAY.ToString(),
                                                  models[loop].M1_CONG_SUAT==null?" ": models[loop].M1_CONG_SUAT.ToString(),
                                                  models[loop].M2_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M2_SO_MAY==null?" ": models[loop].M2_SO_MAY.ToString(),
                                                  models[loop].M2_NOI_SX==null?" ": models[loop].M2_NOI_SX.ToString(),
                                                  models[loop].M2_NAM_CHE_TAO==null?" ": models[loop].M2_NAM_CHE_TAO.ToString(),
                                                  models[loop].M2_HANG_MAY==null?" ": models[loop].M2_HANG_MAY.ToString(),
                                                  models[loop].M2_VONG_QUAY==null?" ": models[loop].M2_VONG_QUAY.ToString(),
                                                  models[loop].M2_CONG_SUAT==null?" ": models[loop].M2_CONG_SUAT.ToString(),
                                                  models[loop].M3_KY_HIEU_MAY==null?" ": models[loop].M2_KY_HIEU_MAY.ToString(),
                                                  models[loop].M3_SO_MAY==null?" ": models[loop].M3_SO_MAY.ToString(),
                                                  models[loop].M3_NOI_SX==null?" ": models[loop].M3_NOI_SX.ToString(),
                                                  models[loop].M3_NAM_CHE_TAO==null?" ": models[loop].M3_NAM_CHE_TAO.ToString(),
                                                  models[loop].M3_HANG_MAY==null?" ": models[loop].M3_HANG_MAY.ToString(),
                                                  models[loop].M3_VONG_QUAY==null?" ": models[loop].M3_VONG_QUAY.ToString(),
                                                  models[loop].M3_CONG_SUAT==null?" ": models[loop].M3_CONG_SUAT.ToString(),

                                                            }
                                                           );


            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }


        private string NumberToRoman(int num)
        {
            string[] thou = { "", "M", "MM", "MMM" };
            string[] hun = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] ten = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            string roman = "";
            roman += thou[(int)(num / 1000) % 10];
            roman += hun[(int)(num / 100) % 10];
            roman += ten[(int)(num / 10) % 10];
            roman += ones[num % 10];

            return roman;
        }

        public ActionResult ExportExcel(ViewModelSearchKT_TAUTHUYEN SearchModel)
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

                    helper.CurrentPosition = new CellRef("A6");
                    helper.InsertRange("header_3");

                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "stt_lama", "c_1", "c_2", "c_3", "c_4", "c_5", "c_6", "c_7", "c_8", "c_9", "c_10", "c_11", "c_12", "c_14", "c_15", "c_16", "c_17", "c_18", "c_19", "c_20", "c_21", "c_22", "c_23", "c_24", "c_25", "c_26", "c_27", "c_28", "c_29", "c_30", "c_31", "c_32", "c_33", "c_34", "c_35", "c_36", "c_37", "c_38", "c_39", "c_40", "c_41", "c_42", "c_43", "c_44", "c_45", "c_46", "c_47", "c_48", "c_49", "c_50", "c_51", "c_52", "c_53", "c_54", "c_55", "c_56", "c_57", "c_58" });
                    //CellRangeTemplate row_quanhuyen = helper.CreateCellRangeTemplate("row_quanhuyen", new List<string> { "stt_num", "tenquanhuyen",  "c_56", "c_57", "c_58", "c_59", "c_60", "c_61", "c_62", "c_63", "c_64", "c_65", "c_66", "c_67",  "c_69", "c_70", "c_71", "c_72", "c_73", "c_74", "c_75", "c_76", "c_77", "c_78", "c_79", "c_80", "c_81", "c_82", "c_83", "c_84", "c_85", "c_86", "c_87", "c_88", "c_89", "c_90", "c_91", "c_92", "c_93", "c_94", "c_95", "c_96", "c_97", "c_98", "c_99", "c_100", "c_101", "c_102", "c_103", "c_104", "c_105", "c_106", "c_107", "c_108", "c_109", "c_110", "c_111" });
                    //CellRangeTemplate row_11 = helper.CreateCellRangeTemplate("row_11", new List<string> { "str", "c_110", "c_111", "c_112", "c_113", "c_114", "c_115", "c_116", "c_117", "c118", "c_119", "c_120", "c_121", "c_122", "c_123", "c_124", "c_125", "c_126", "c_127", "c_128", "c_129", "c_130", "c_131", "c_132", "c_133", "c_134", "c_135", "c_136", "c_137", "c_138", "c_139", "c_140", "c_141", "c_142", "c_143", "c_144", "c_145", "c_146", "c_147", "c_148", "c_149", "c_150", "c_151", "c_152", "c_153", "c_154", "c_155", "c_156", "c_157", "c_158", "c_159", "c_160", "c_161", "c_162", "c_163", "c_164", "c_165"});

                    int k = 7;
                    List<String> _lstMA_TTP = new List<string>();
                    IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(SearchModel, _lstMA_TTP);

                    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    //helper.InsertRange(row_tinhthanhpho, _lstTinhTP.ToArray()[i]);
                    helper.InsertRange(row_tinhthanhpho, _lstTinhTP);

                    //for (int i = 0; i < _lstTinhTP.Count(); i++)
                    //{

                    //    //insert Tinh thanh pho
                    //    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    //    //helper.InsertRange(row_tinhthanhpho, _lstTinhTP.ToArray()[i]);
                    //    helper.InsertRange(row_tinhthanhpho, _lstTinhTP);
                    //    k = k + 1;
                    //    //insert detail
                    //    List<string> _lstSODK = new List<string>();

                    //    IEnumerable<List<object>> _lstQuanHuyen = this.getTAUTHUYENByTinhTP(_lstMA_TTP[i], _lstSODK);

                    //    for (int j = 0; j < _lstQuanHuyen.Count(); j++)
                    //    {
                    //        helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    //        helper.InsertRange(row_quanhuyen, _lstQuanHuyen.ToArray()[j]);
                    //        k = k + 1;


                    //            //Insert co so detail
                    //            helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    //            IEnumerable<List<object>> lstDetail = this.getDetailTauthuyen(  "",_lstSODK[j]);
                    //            helper.InsertRange(row_11, lstDetail);
                    //            k = k + lstDetail.Count();


                    //    }

                    // k = k + 1;
                    //}
                    helper.DeleteSheet("Sheet3");
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


        #region"Working Files TAU CHUA CAP PHEP"
        private static String GENERATED_FILE_EXPORT_NAME_CHUACAPPHEP = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_EXPORT_NAME_CHUACAPPHEP = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_TauChuaCapPhep.xlsx");
        #endregion

        #region "Export excel Tau Chua Cap Phep"



        private IEnumerable<List<object>> getTinhThanhPho_TAUCHUACAPPHEP(ViewModelSearchKT_TAUTHUYEN t)
        {
            //get nhóm nghề


            string ma_TinhTP = string.Empty;

            initialCategorySearchAction(t, ref ma_TinhTP);
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();


            var selects = db.KT_TAUTHUYEN.Where(o => o.IsCapPhep == null
                                                && (string.IsNullOrEmpty(t.SO_DK) || o.SO_DK.ToLower().Contains(t.SO_DK.ToLower()))
                                                && (string.IsNullOrEmpty(t.CHU_PHUONG_TIEN) || o.CHU_PHUONG_TIEN.ToLower().Contains(t.CHU_PHUONG_TIEN.ToLower()))
                //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                 && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && (string.IsNullOrEmpty(t.MA_QUANHUYEN) || o.MA_QUANHUYEN == t.MA_QUANHUYEN)
                                                && (string.IsNullOrEmpty(t.MA_PHUONGXA) || o.MA_PHUONGXA == t.MA_PHUONGXA)
                                                && (t.DCONG_DUNG_TAUID == null || o.DCONG_DUNG_TAUID == t.DCONG_DUNG_TAUID)
                                                && (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                && (t.DTINH_TRANG_TAUID == null || o.DTINH_TRANG_TAUID == t.DTINH_TRANG_TAUID)
                                                && (t.DTINH_TRANG_DANG_KIEMID == null || o.DTINH_TRANG_DANG_KIEMID == t.DTINH_TRANG_DANG_KIEMID)
                                                && (t.KT_TONG_CONG_SUAT_TU == null || o.KT_TONG_CONG_SUAT >= t.KT_TONG_CONG_SUAT_TU)
                                                && (t.KT_TONG_CONG_SUAT_DEN == null || o.KT_TONG_CONG_SUAT <= t.KT_TONG_CONG_SUAT_DEN)
                                                && (t.NGAY_DK_TU == null || o.NGAY_DK >= t.NGAY_DK_TU)
                                                && (t.NGAY_DK_DEN == null || o.NGAY_DK <= t.NGAY_DK_DEN)
                                            );


            var models = selects.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {


                lstobj.Add(new List<object> {                                                 
                                               
                                                models[loop].SO_DK==null?" ": models[loop].SO_DK.ToString(),
                                                models[loop].NGAY_DK==null?" ": models[loop].NGAY_DK.ToShortDateString(),
                                                 models[loop].CHU_PHUONG_TIEN==null?" ": models[loop].CHU_PHUONG_TIEN.ToString(),
                                                 models[loop].DIA_CHI==null?" ": models[loop].DIA_CHI.ToString(),
                                                 models[loop].TEN_TAU==null?" ": models[loop].TEN_TAU.ToString(),
                                                models[loop].DNHOM_TAU == null ||  models[loop].DNHOM_TAU.Name==null?" ": models[loop].DNHOM_TAU.Name.ToString(),                                               
                                                models[loop].DTINH_TRANG_TAU == null ||  models[loop].DTINH_TRANG_TAU.Name==null?" ": models[loop].DTINH_TRANG_TAU.Name.ToString(),
                                                models[loop].DVAT_LIEU_VO == null || models[loop].DVAT_LIEU_VO.Name==null?" ": models[loop].DVAT_LIEU_VO.Name.ToString(),
                                                models[loop].DTINHTP == null || models[loop].DTINHTP.TEN_TINHTP==null?" ": models[loop].DTINHTP.TEN_TINHTP.ToString(),
                                            
                                               }
                                                          );

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }


        public ActionResult ExportExcel_TauChuaCapPhep(ViewModelSearchKT_TAUTHUYEN SearchModel)
        {

            try
            {

                using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_EXPORT_NAME_CHUACAPPHEP, GENERATED_FILE_EXPORT_NAME_CHUACAPPHEP))
                {
                    helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet1";

                    helper.CurrentPosition = new CellRef("A4");
                    helper.InsertRange("header_1");



                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "c_1", "c_2", "c_3", "c_4", "c_5", "c_6", "c_7", "c_8", "c_9" });


                    int k = 5;
                    List<String> _lstMA_TTP = new List<string>();
                    IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho_TAUCHUACAPPHEP(SearchModel);

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

        public ActionResult DangKy()
        {
            return View();
        }

        public ActionResult Search(ViewModelSearchTK_TAUTHUYEN t)
        {
            string ma_TinhTP = string.Empty;

            initialCategoryStatisticsAction(t, ref ma_TinhTP);



            // neu ko chon ma tinh tren dropdownlist thi se lay ma tinh cua user
            if (t.MA_TINHTP == null && !ma_TinhTP.StartsWith("Z")) t.MA_TINHTP = ma_TinhTP;

            ThongKe tk = new ThongKe(t);

            t.F = tk.F;
            t.Sum_F_By_Fishery = tk.Sum_F_by_Fishery;
            t.F_by_Status = tk.F_by_Status;
            t.Sum_F_by_Status = tk.Sum_F_by_Status;

            //t.F = Calc_F(t);
            //t.Sum_F_By_Fishery = Calc_Sum_F_by_Fishery(t);

            return View(t);
        }

        //private Dictionary<string, int> Calc_F(ViewModelSearchTK_TAUTHUYEN tk)
        //{
        //    Dictionary<string, int> _dictF = new Dictionary<string, int>();
        //    List<DM_NHOMNGHE> _lstDMNhomNghe;
        //    List<DNHOM_TAU> _lstDMNhomTau;
        //    List<KT_TAUTHUYEN> _lstTauThuyen;

        //    _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
        //    _lstDMNhomTau = db.DNHOM_TAU.ToList();
        //    _lstTauThuyen = db.KT_TAUTHUYEN.ToList();

        //    foreach (var nghe in _lstDMNhomNghe)
        //    {
        //        foreach (var nhomtau in _lstDMNhomTau)
        //        {
        //            var iF = 0;
        //            iF = _lstTauThuyen.Where(t => (t.NGHE_CHINH_ID == nghe.DM_NhomNgheID || t.NGHE_PHU_ID == nghe.DM_NhomNgheID)
        //                                            && t.DNHOM_TAUID == nhomtau.ID
        //                                            && t.DCONG_DUNG_TAUID == 1  // chi thong ke tau khai thac thuy san
        //                                            && (tk.MA_TINHTP == null || t.MA_TINHTP == tk.MA_TINHTP)
        //                                            && (tk.MA_QUANHUYEN == null || t.MA_QUANHUYEN == tk.MA_QUANHUYEN)
        //                                            && (tk.MA_PHUONGXA == null || t.MA_PHUONGXA == tk.MA_PHUONGXA)
        //                                            && (tk.DVAT_LIEU_VOID == null || t.DVAT_LIEU_VOID == tk.DVAT_LIEU_VOID)
        //                                            ).Count();

        //            _dictF.Add(tk.MA_TINHTP + "#" + nhomtau.ID + "#" + nghe.DM_NhomNgheID, iF);
        //        }
        //    }

        //    return _dictF;
        //}

        //// tinh tong tau thuyen theo nghe
        //private Dictionary<string, int> Calc_Sum_F_by_Fishery(ViewModelSearchTK_TAUTHUYEN tk)
        //{
        //    List<DM_NHOMNGHE> _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
        //    List<DNHOM_TAU> _lstDMNhomTau = db.DNHOM_TAU.ToList();

        //    Dictionary<string, int> _dictSumF = new Dictionary<string, int>();

        //    Dictionary<string, int> _dictF1 = Calc_F(tk);




        //    if (_dictF1 != null && _dictF1.Count > 0)
        //    {
        //        foreach (var nghe in _lstDMNhomNghe)
        //        {
        //            var iSumF = 0;
        //            foreach (var nhomtau in _lstDMNhomTau)
        //            {
        //                iSumF += _dictF1[tk.MA_TINHTP + "#" + nhomtau.ID + "#" + nghe.DM_NhomNgheID];
        //            }

        //            _dictSumF.Add(tk.MA_TINHTP + "#" + nghe.DM_NhomNgheID, iSumF);
        //        }
        //    }

        //    return _dictSumF;
        //}
    }


    // customs class for staticstic fishery
    class ThongKe
    {
        List<DM_NHOMNGHE> _lstDMNhomNghe;
        List<DNHOM_TAU> _lstDMNhomTau;
        List<DTINH_TRANG_TAU> _lstTinhTrangTau;
        List<KT_TAUTHUYEN> _lstTauThuyen;
        FDBContext db = new FDBContext();

        ViewModelSearchTK_TAUTHUYEN t;

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

        public Dictionary<string, int> F_by_Status
        {
            get
            {
                return Calc_F_by_Status(t);
            }
        }

        public Dictionary<string, int> Sum_F_by_Status
        {
            get
            {
                return Calc_Sum_F_by_Status(t);
            }
        }

        public ThongKe(ViewModelSearchTK_TAUTHUYEN t)
        {
            this.t = t;

            _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
            _lstDMNhomTau = db.DNHOM_TAU.ToList();
            _lstTauThuyen = db.KT_TAUTHUYEN.ToList();
            _lstTinhTrangTau = db.DTINH_TRANG_TAU.ToList();
        }

        // thong ke tau thuyen theo nghe
        private Dictionary<string, int> Calc_F(ViewModelSearchTK_TAUTHUYEN tk)
        {
            Dictionary<string, int> _dictF = new Dictionary<string, int>();

            foreach (var nghe in _lstDMNhomNghe)
            {
                foreach (var nhomtau in _lstDMNhomTau)
                {
                    var iF = 0;
                    iF = _lstTauThuyen.Where(t => ((t.NGHE_CHINH_ID == nghe.DM_NhomNgheID || t.NGHE_PHU_ID == nghe.DM_NhomNgheID) && ((t.DTINH_TRANG_TAUID ==1) || t.DTINH_TRANG_TAUID ==2  ))
                                                    && t.DNHOM_TAUID == nhomtau.ID
                                                    && t.DCONG_DUNG_TAUID == 1  // chi thong ke tau khai thac thuy san
                                                    && (tk.MA_TINHTP == null || t.MA_TINHTP == tk.MA_TINHTP)
                                                    && (tk.MA_QUANHUYEN == null || t.MA_QUANHUYEN == tk.MA_QUANHUYEN)
                                                    && (tk.MA_PHUONGXA == null || t.MA_PHUONGXA == tk.MA_PHUONGXA)
                                                    && (tk.DVAT_LIEU_VOID == null || t.DVAT_LIEU_VOID == tk.DVAT_LIEU_VOID)
                                                    && (tk.DTINH_TRANG_TAUID == null || t.DTINH_TRANG_TAUID == tk.DTINH_TRANG_TAUID )
                                                    //&& ((tk.DTINH_TRANG_TAUID == null || t.DTINH_TRANG_TAUID == 1) || (tk.DTINH_TRANG_TAUID == null || t.DTINH_TRANG_TAUID == 2))                                                    
                                                    && (tk.NGAY_DK_TU == null || t.NGAY_DK >= tk.NGAY_DK_TU)
                                                    && (tk.NGAY_DK_DEN == null || t.NGAY_DK <= tk.NGAY_DK_DEN)
                                                    ).Count();

                    _dictF.Add(tk.MA_TINHTP + "#" + nhomtau.ID + "#" + nghe.DM_NhomNgheID, iF);
                }
            }

            return _dictF;
        }


        private Dictionary<string, int> Calc_Sum_F_by_Fishery(ViewModelSearchTK_TAUTHUYEN tk)
        {
            Dictionary<string, int> _dictSumF = new Dictionary<string, int>();

            if (this.F != null && this.F.Count > 0)
            {
                foreach (var nghe in _lstDMNhomNghe)
                {
                    var iSumF = 0;
                    foreach (var nhomtau in _lstDMNhomTau)
                    {
                        iSumF += this.F[tk.MA_TINHTP + "#" + nhomtau.ID + "#" + nghe.DM_NhomNgheID];
                    }

                    _dictSumF.Add(tk.MA_TINHTP + "#" + nghe.DM_NhomNgheID, iSumF);
                }
            }

            return _dictSumF;
        }


        // thong ke tau thuyen theo trang thai
        private Dictionary<string, int> Calc_F_by_Status(ViewModelSearchTK_TAUTHUYEN tk)
        {
            Dictionary<string, int> _dictF = new Dictionary<string, int>();

           
            var _lstTinhTrangTau_Filter = db.DTINH_TRANG_TAU.Take(2).ToList();
   

            foreach (var s in _lstTinhTrangTau)
            {
                foreach (var nhomtau in _lstDMNhomTau)
                {
                    var iF = 0;
                    iF = _lstTauThuyen.Where(t => (t.DTINH_TRANG_TAUID == s.ID)
                       
                                                    && t.DNHOM_TAUID == nhomtau.ID
                        //&& t.DCONG_DUNG_TAUID == 1  // chi thong ke tau khai thac thuy san
                                                    && (tk.MA_TINHTP == null || t.MA_TINHTP == tk.MA_TINHTP)
                                                    && (tk.MA_QUANHUYEN == null || t.MA_QUANHUYEN == tk.MA_QUANHUYEN)
                                                    && (tk.MA_PHUONGXA == null || t.MA_PHUONGXA == tk.MA_PHUONGXA)
                                                    && (tk.DVAT_LIEU_VOID == null || t.DVAT_LIEU_VOID == tk.DVAT_LIEU_VOID)
                                                    && (tk.DCONG_DUNG_TAUID == null || t.DCONG_DUNG_TAUID == tk.DCONG_DUNG_TAUID)
                                                    && (tk.DTINH_TRANG_TAUID == null || t.DTINH_TRANG_TAUID == tk.DTINH_TRANG_TAUID)                                                                                                        
                                                    && (tk.NGAY_DK_TU == null || t.NGAY_DK >= tk.NGAY_DK_TU)
                                                    && (tk.NGAY_DK_DEN == null || t.NGAY_DK <= tk.NGAY_DK_DEN)
                                                    ).Count();

                    _dictF.Add(tk.MA_TINHTP + "#" + nhomtau.ID + "#" + s.ID, iF);
                }
            }

            return _dictF;
        }

        private Dictionary<string, int> Calc_Sum_F_by_Status(ViewModelSearchTK_TAUTHUYEN tk)
        {
            Dictionary<string, int> _dictSumF = new Dictionary<string, int>();
            
            if (this.F_by_Status != null && this.F_by_Status.Count > 0)
            {
                foreach (var s in _lstTinhTrangTau)
                {
                    var iSumF = 0;
                    foreach (var nhomtau in _lstDMNhomTau)
                    {
                        iSumF += this.F_by_Status[tk.MA_TINHTP + "#" + nhomtau.ID + "#" + s.ID];
                    }

                    _dictSumF.Add(tk.MA_TINHTP + "#" + s.ID, iSumF);
                }
            }

            return _dictSumF;
        }
    }

}