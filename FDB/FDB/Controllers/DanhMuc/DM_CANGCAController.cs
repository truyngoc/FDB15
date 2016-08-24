using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using System.IO;
using System.Web.UI;
using System.Reflection;
using Aron.Sinoai.OfficeHelper;
using Excel;

using System.Web.UI.WebControls;

using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

using System.Text;

using System.Globalization;

namespace FDB.Controllers
{
    public class DM_CANGCAController : FDBController //Controller
    {
        //
        // GET: /DanhMuc/

        private FDBContext db = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }
        public void initialCategoryCreateAction()
        {
            ViewBag.PHAN_LOAI_CANG_CAs = new SelectList(db.DPHAN_LOAI_CANG, "ID", "Name");
           // ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        public void initialCategoryEditAction(DM_CANGCA searchModel)
        {
            ViewBag.PHAN_LOAI_CANG_CAs = new SelectList(db.DPHAN_LOAI_CANG, "ID", "Name");
           // ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == searchModel.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == searchModel.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }
        public void initialCategorySearchAction(ViewModelSearchDM_CANGCA searchModel, ref string ma_Tinh)
        {
            ViewBag.PHAN_LOAI_CANG_CAs = new SelectList(db.DPHAN_LOAI_CANG, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == searchModel.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == searchModel.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
            ma_Tinh = curUser.MA_TINHTP;
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
        public ActionResult Index(ViewModelSearchDM_CANGCA SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);
      
            
            var DM_CANGCAs = db.DM_CANGCA.Where(o => (SearchModel.TEN_CANG == null || o.TEN_CANG.ToUpper().Contains(SearchModel.TEN_CANG.ToUpper()))
                                                      && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                      && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DPHAN_LOAI_CANGID == null || o.DPHAN_LOAI_CANGID == SearchModel.DPHAN_LOAI_CANGID)
                                          ).Select(x => new { x.ID, x.MA_TINHTP, x.TEN_CANG, x.DIA_CHI, x.DIEN_THOAI, x.DPHAN_LOAI_CANG, x.DTINHTP,x.DQUANHUYEN,x.DPHUONGXA,x.Status }).OrderByDescending(x => x.ID)
                                          ;

            List<DM_CANGCA> DSDM_CANGCA = new List<DM_CANGCA>();
            foreach (var dm_cangca in DM_CANGCAs)
            {
                DSDM_CANGCA.Add(new DM_CANGCA
                {
                    ID = dm_cangca.ID,
                    MA_TINHTP = dm_cangca.MA_TINHTP,
                    TEN_CANG = dm_cangca.TEN_CANG,
                    DIA_CHI = dm_cangca.DIA_CHI,
                    DPHAN_LOAI_CANG = dm_cangca.DPHAN_LOAI_CANG,
                    DIEN_THOAI = dm_cangca.DIEN_THOAI,
                    DTINHTP = dm_cangca.DTINHTP,
                    DQUANHUYEN = dm_cangca.DQUANHUYEN,
                    DPHUONGXA = dm_cangca.DPHUONGXA,
                    Status = dm_cangca.Status
                });
            }

            ViewBag.TotalRow = DSDM_CANGCA.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSDM_CANGCA.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);
            
                  
        }


        public ActionResult Search(ViewModelSearchDM_CANGCA SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);


            var DM_CANGCAs = db.DM_CANGCA.Where(o => (SearchModel.TEN_CANG == null || o.TEN_CANG.ToUpper().Contains(SearchModel.TEN_CANG.ToUpper()))
                                                      && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                       && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DPHAN_LOAI_CANGID == null || o.DPHAN_LOAI_CANGID == SearchModel.DPHAN_LOAI_CANGID)
                                          ).Select(x => new { x.ID, x.MA_TINHTP, x.TEN_CANG, x.DIA_CHI, x.DIEN_THOAI, x.DPHAN_LOAI_CANG, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA, x.Status }).OrderByDescending(x => x.ID)
                                          ;

            List<DM_CANGCA> DSDM_CANGCA = new List<DM_CANGCA>();
            foreach (var dm_cangca in DM_CANGCAs)
            {
                DSDM_CANGCA.Add(new DM_CANGCA
                {
                    ID = dm_cangca.ID,
                    MA_TINHTP = dm_cangca.MA_TINHTP,
                    TEN_CANG = dm_cangca.TEN_CANG,
                    DIA_CHI = dm_cangca.DIA_CHI,
                    DPHAN_LOAI_CANG = dm_cangca.DPHAN_LOAI_CANG,
                    DIEN_THOAI = dm_cangca.DIEN_THOAI,
                    DTINHTP = dm_cangca.DTINHTP,
                    DQUANHUYEN = dm_cangca.DQUANHUYEN,
                    DPHUONGXA = dm_cangca.DPHUONGXA,
                    Status = dm_cangca.Status
                });
            }

            ViewBag.TotalRow = DSDM_CANGCA.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSDM_CANGCA.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);


        }
     
        public ActionResult Create()
        {
            initialCategoryCreateAction();
      
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DM_CANGCA dm_cangca)
        {
            if (ModelState.IsValid)
            {
                //status cang ca khi them moi luon o trang thai mo (luon la false ,khong duoc check)
                dm_cangca.Status = false;
                db.DM_CANGCA.Add(dm_cangca);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));
                
                return RedirectToAction("Index");
            }

            initialCategoryCreateAction();

       

            return View(dm_cangca);
          
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_CANGCA dm_cangca = db.DM_CANGCA.Find(id);
            if (dm_cangca == null)
            {
                return HttpNotFound();
            }

            initialCategoryEditAction(dm_cangca);
          
            return View(dm_cangca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form, DM_CANGCA dm_cangca)
        {
            if (ModelState.IsValid)
            {
                //Convert.ToBoolean(collection["showAll"].Split(',')[0])
                dm_cangca.Status = Convert.ToBoolean(form["cbStatus"].Split(',')[0]);
                db.Entry(dm_cangca).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {
                initialCategoryEditAction(dm_cangca);

            }
            
            return View(dm_cangca);
        }

        public ActionResult Delete(int? id)
        {
            try 
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DM_CANGCA dm_cangca = db.DM_CANGCA.Find(id);
                db.DM_CANGCA.Remove(dm_cangca);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "bản ghi"));
                
            }
            catch
            {
                Inline_Danger("Cảng cá này đã được sử dụng tại chức năng khác nên không thể xóa", true);
            }

            return RedirectToAction("Index");    

           // Inline_Danger
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


	}
}