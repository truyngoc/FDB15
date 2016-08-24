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
    public class KT_NGAYHOATDONGController : FDBController
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
        // GET: /NGAYHOATDONG/
        public ActionResult Index(ViewModelSearchKT_NGAYHOATDONG t)
        {
            string ma_TinhTP = string.Empty;

            if (t.THANG == null) t.THANG = DateTime.Today.Month;
            if (t.NAM == null) t.NAM = DateTime.Today.Year;

            initialCategorySearchAction(ref ma_TinhTP);

            var ngayhd = db.KT_NGAYHOATDONG.Where(o => (t.DNHOM_TAUID == null || o.DNHOM_TAUID == t.DNHOM_TAUID)
                                                && (t.DM_NhomNgheID == null || o.DM_NhomNgheID == t.DM_NhomNgheID)
                                                && (t.NAM == null || o.NAM == t.NAM)
                                                && (t.THANG == null || o.THANG == t.THANG)
                                                //&& ((string.IsNullOrEmpty(t.MA_TINHTP) && string.IsNullOrEmpty(ma_TinhTP)) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                                && ((string.IsNullOrEmpty(t.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(t.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == t.MA_TINHTP)
                                            ).OrderByDescending(m => m.ID);

            ViewBag.TotalRow = ngayhd.Count();
            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = t.Page ?? 1;

            t.SearchResults = ngayhd.ToPagedList(pageNumber, pageSize);

            return View(t);
        }


        #endregion

        #region "them moi"
        public ActionResult Create()
        {
            KT_NGAYHOATDONG n = new KT_NGAYHOATDONG();

            n.NGAY_NM = DateTime.Today;
            n.NGUOI_NM = User.Identity.Name;
            n.THANG = DateTime.Today.Month;
            n.NAM = DateTime.Today.Year;

            initialCategoryCreateAction();

            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_NGAYHOATDONG t)
        {
            if (ModelState.IsValid)
            {
                var ngayhd = db.KT_NGAYHOATDONG.Where(n => n.MA_TINHTP == t.MA_TINHTP
                                                        && n.NAM == t.NAM
                                                        && n.THANG == t.THANG
                                                        && n.DNHOM_TAUID == t.DNHOM_TAUID
                                                        && n.DM_NhomNgheID == t.DM_NhomNgheID
                                                    );

                if (ngayhd.Count() > 0)
                {
                    KT_NGAYHOATDONG ngay = ngayhd.FirstOrDefault();

                    Inline_Danger(string.Format("Thông tin ngày hoạt động tiềm năng của nghề khai thác và nhóm công suất này đã tồn tại"), true);
                }
                else
                {
                    db.KT_NGAYHOATDONG.Add(t);
                    db.SaveChanges();

                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Ngày hoạt động tiềm năng"));

                    return RedirectToAction("Index");
                }
                
            }
            initialCategoryCreateAction();

            return View(t);
        }

        #endregion

        #region "Sua"
        public ActionResult Edit(int ID)
        {
            KT_NGAYHOATDONG t = db.KT_NGAYHOATDONG.Find(ID);
            if (t == null)
            {
                return HttpNotFound();
            }
            initialCategoryEditAction();

            return View(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_NGAYHOATDONG t)
        {
            if (ModelState.IsValid)
            {
                t.NGAY_NM = DateTime.Today;
                t.NGUOI_NM = User.Identity.Name;

                db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Ngày hoạt động tiềm năng"));


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
            KT_NGAYHOATDONG t = await db.KT_NGAYHOATDONG.FindAsync(id);


            db.KT_NGAYHOATDONG.Remove(t);

            await db.SaveChangesAsync();

            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Ngày hoạt động tiềm năng"));

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

    }
}