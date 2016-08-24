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
//dung expandoObject
using System.Data.Common;
using System.Dynamic;
using System.ComponentModel;



namespace FDB.Controllers.NuoiTrong
{
    public class NT_THIETHAIController : FDBController //Controller
    {
        private FDBContext db = new FDBContext();
     
        public void initialCategoryCreateAction()
        {
            ViewBag.DM_DOITUONG_NUOIs = new SelectList(db.DM_DOITUONG_NUOI_THIETHAI, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.DNGUYENNHAN_THIETHAIs = new SelectList(db.DNGUYENNHAN_THIETHAI, "ID", "Name");
            ViewBag.DTYLE_THIETHAIs = new SelectList(db.DTYLE_THIETHAI, "ID", "Name");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;


            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
        }
        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }
        public void initialCategoryEditAction(NT_THIETHAI searchModel)
        {
            ViewBag.DM_DOITUONG_NUOIs = new SelectList(db.DM_DOITUONG_NUOI_THIETHAI, "ID", "Name");
           // ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
            ApplicationUser curUser = getCurrentUser();


            


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DNGUYENNHAN_THIETHAIs = new SelectList(db.DNGUYENNHAN_THIETHAI, "ID", "Name");
            ViewBag.DTYLE_THIETHAIs = new SelectList(db.DTYLE_THIETHAI, "ID", "Name");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == searchModel.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == searchModel.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }
        public void initialCategorySearchAction(ViewModelSearchNT_THIETHAI searchModel, ref string ma_Tinh)
        {
            ViewBag.DM_DOITUONG_NUOIs = new SelectList(db.DM_DOITUONG_NUOI_THIETHAI, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.DNGUYENNHAN_THIETHAIs = new SelectList(db.DNGUYENNHAN_THIETHAI, "ID", "Name");

            ViewBag.DTYLE_THIETHAIs = new SelectList(db.DTYLE_THIETHAI, "ID", "Name");

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
        public ActionResult Index(ViewModelSearchNT_THIETHAI SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);


            var NT_THIETHAIs = db.NT_THIETHAI.Where(o => 
                                                      (SearchModel.DM_DOITUONG_NUOI_THIETHAIID == null || o.DM_DOITUONG_NUOI_THIETHAIID == SearchModel.DM_DOITUONG_NUOI_THIETHAIID)
                                                      && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                      && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                       && (SearchModel.DTYLE_THIETHAIID == null || o.DTYLE_THIETHAIID == SearchModel.DTYLE_THIETHAIID)
                                                       && (SearchModel.THANG == null || o.THANG == SearchModel.THANG)
                                                       && (SearchModel.NAM == null || o.NAM == SearchModel.NAM)
                                                        && (SearchModel.DO_MOITRUONG == null || o.DO_MOITRUONG == SearchModel.DO_MOITRUONG)
                                                        && (SearchModel.DO_DICHBENH == null || o.DO_DICHBENH == SearchModel.DO_DICHBENH)
                                                      // && (SearchModel.DNGUYENNHAN_THIETHAIID == null || o.DNGUYENNHAN_THIETHAIID == SearchModel.DNGUYENNHAN_THIETHAIID)
                                                       


                                          ).Select(x => new { x.ID, x.DM_DOITUONG_NUOI_THIETHAI, x.DIENTICH_THIETHAI, x.SANLUONG_THIETHAI, x.THIETHAI_UOCTINH, x.DTYLE_THIETHAI, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA,  x.THANG, x.NAM }).OrderByDescending(x => x.ID)
                                          ;

            List<NT_THIETHAI> DSNT_THIETHAI = new List<NT_THIETHAI>();
            foreach (var nt_thiethai in NT_THIETHAIs)
            {
                DSNT_THIETHAI.Add(new NT_THIETHAI
                {
                    ID = nt_thiethai.ID,
                    DM_DOITUONG_NUOI_THIETHAI = nt_thiethai.DM_DOITUONG_NUOI_THIETHAI,
                    DIENTICH_THIETHAI = nt_thiethai.DIENTICH_THIETHAI,
                    SANLUONG_THIETHAI = nt_thiethai.SANLUONG_THIETHAI,
                    THIETHAI_UOCTINH = nt_thiethai.THIETHAI_UOCTINH,
                    DTYLE_THIETHAI = nt_thiethai.DTYLE_THIETHAI,
                    //DNGUYENNHAN_THIETHAI = nt_thiethai.DNGUYENNHAN_THIETHAI,
                    
                    
                    DTINHTP = nt_thiethai.DTINHTP,
                    DQUANHUYEN = nt_thiethai.DQUANHUYEN,
                    DPHUONGXA = nt_thiethai.DPHUONGXA,
                    THANG = nt_thiethai.THANG,
                    NAM = nt_thiethai.NAM
                });
            }

            ViewBag.TotalRow = DSNT_THIETHAI.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSNT_THIETHAI.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);
        }

        public ActionResult Search(ViewModelSearchNT_THIETHAI SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);


            var NT_THIETHAIs = db.NT_THIETHAI.Where(o =>
                                                      (SearchModel.DM_DOITUONG_NUOI_THIETHAIID == null || o.DM_DOITUONG_NUOI_THIETHAIID == SearchModel.DM_DOITUONG_NUOI_THIETHAIID)
                                                       && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                      && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                       && (SearchModel.DTYLE_THIETHAIID == null || o.DTYLE_THIETHAIID == SearchModel.DTYLE_THIETHAIID)
                                                       && (SearchModel.THANG == null || o.THANG == SearchModel.THANG)
                                                       && (SearchModel.NAM == null || o.NAM == SearchModel.NAM)
                                                        && (SearchModel.DO_MOITRUONG == null || o.DO_MOITRUONG == SearchModel.DO_MOITRUONG)
                                                        && (SearchModel.DO_DICHBENH == null || o.DO_DICHBENH == SearchModel.DO_DICHBENH)
                // && (SearchModel.DNGUYENNHAN_THIETHAIID == null || o.DNGUYENNHAN_THIETHAIID == SearchModel.DNGUYENNHAN_THIETHAIID)



                                          ).Select(x => new { x.ID, x.DM_DOITUONG_NUOI_THIETHAI, x.DIENTICH_THIETHAI, x.SANLUONG_THIETHAI, x.THIETHAI_UOCTINH, x.DTYLE_THIETHAI, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA, x.THANG, x.NAM }).OrderByDescending(x => x.ID)
                                          ;

            List<NT_THIETHAI> DSNT_THIETHAI = new List<NT_THIETHAI>();
            foreach (var nt_thiethai in NT_THIETHAIs)
            {
                DSNT_THIETHAI.Add(new NT_THIETHAI
                {
                    ID = nt_thiethai.ID,
                    DM_DOITUONG_NUOI_THIETHAI = nt_thiethai.DM_DOITUONG_NUOI_THIETHAI,
                    DIENTICH_THIETHAI = nt_thiethai.DIENTICH_THIETHAI,
                    SANLUONG_THIETHAI = nt_thiethai.SANLUONG_THIETHAI,
                    THIETHAI_UOCTINH = nt_thiethai.THIETHAI_UOCTINH,
                    DTYLE_THIETHAI = nt_thiethai.DTYLE_THIETHAI,
                    //DNGUYENNHAN_THIETHAI = nt_thiethai.DNGUYENNHAN_THIETHAI,


                    DTINHTP = nt_thiethai.DTINHTP,
                    DQUANHUYEN = nt_thiethai.DQUANHUYEN,
                    DPHUONGXA = nt_thiethai.DPHUONGXA,
                    THANG = nt_thiethai.THANG,
                    NAM = nt_thiethai.NAM
                });
            }

            ViewBag.TotalRow = DSNT_THIETHAI.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSNT_THIETHAI.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);
        }
        public ActionResult Create()
        {
            initialCategoryCreateAction();
            //NT_THIETHAI model = new NT_THIETHAI();
            //model.NAM = DateTime.Now.Year;
            //model.THANG = DateTime.Now.Month;
            //var dbQuanTri = new ApplicationDbContext();
            //string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
            //model.MA_TINHTP = Ma_TinhTP;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NT_THIETHAI nt_thiethai)
        {
            if (ModelState.IsValid)
            {
                //var dbQuanTri = new ApplicationDbContext();
                //string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;                
                //nt_thiethai.MA_TINHTP = Ma_TinhTP;
                db.NT_THIETHAI.Add(nt_thiethai);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));

                return RedirectToAction("Index");
            }

            initialCategoryCreateAction();



            return View(nt_thiethai);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NT_THIETHAI nt_thiethai = db.NT_THIETHAI.Find(id);
            if (nt_thiethai == null)
            {
                return HttpNotFound();
            }

            initialCategoryEditAction(nt_thiethai);

            return View(nt_thiethai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( NT_THIETHAI nt_thiethai)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(nt_thiethai).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {

                initialCategoryEditAction(nt_thiethai);

            }

            return View(nt_thiethai);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NT_THIETHAI nt_thiethai = db.NT_THIETHAI.Find(id);
            db.NT_THIETHAI.Remove(nt_thiethai);
            db.SaveChanges();
            this.Information(String.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "bản ghi"));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult DynamicIndex()
        //{
        //    using (var ctx = new FDBContext())
        //    using (var cmd = ctx.Database.Connection.CreateCommand())
        //    {
        //        ctx.Database.Connection.Open();
        //        cmd.CommandText = "EXEC DynamicPivot";
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            var model = this._productsServices.Read(reader).ToList();
        //            return View(model);
        //        }
        //    }
        //}

      
	}
}