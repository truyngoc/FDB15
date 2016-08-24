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
using System.Data.SqlClient;
namespace FDB.Controllers.KhaiThac
{
    public class KT_CANGCAController : FDBController
    {
        //
        // GET: /KT_CANGCA
          
        private FDBContext db = new FDBContext();

        public void BindComboDM_CANGCA()
        {
            ApplicationUser curUser = getCurrentUser();


            ViewBag.DM_CANGCAs = new SelectList(db.DM_CANGCA.Where(s => s.Status == false && s.MA_TINHTP == curUser.MA_TINHTP ), "ID", "TEN_CANG");
        }

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }
        public ActionResult Index(ViewModelSearchKT_CANGCA SearchModel)
        {
            BindComboDM_CANGCA();


           
            var KT_CANGCAs = db.KT_CANGCA.Where(o => (SearchModel.DM_CANGCAID == null || o.DM_CANGCAID == SearchModel.DM_CANGCAID)
                                                               && ((SearchModel.TU_NGAY == null || o.NGAY_GHINHAN >= SearchModel.TU_NGAY)
                                                               && (SearchModel.DEN_NGAY == null || o.NGAY_GHINHAN <= SearchModel.DEN_NGAY))
                                                   //  ).Select(x => new { x.ID, x.DM_CANGCA, x.TONG_TRONGLUONG_HANG_QUACANG, x.SANLUONG_THUYSAN, x.SO_LUOTTAU_CAPCANG, x.NGAY_GHINHAN }).OrderByDescending(x => x.ID);
                                                     ).Select(x => new { x.ID, x.DM_CANGCA, x.SANLUONG_CA, x.SANLUONG_MUC, x.SANLUONG_TOM,x.SANLUONG_KHAC,x.HANG_NUOCDA,x.HANG_NUOCNGOT,x.HANG_XANGDAU,x.HANG_KHAC, x.NGAY_GHINHAN }).OrderByDescending(x => x.ID);
                                       
                                                   

            List<KT_CANGCA> DSKT_CANGCA = new List<KT_CANGCA>();
            foreach (var kt_cangca in KT_CANGCAs)
            {
                DSKT_CANGCA.Add(new KT_CANGCA
                {
                    ID = kt_cangca.ID,
                    DM_CANGCA = kt_cangca.DM_CANGCA,
                    SANLUONG_CA = kt_cangca.SANLUONG_CA,
                    SANLUONG_TOM = kt_cangca.SANLUONG_TOM,
                    SANLUONG_MUC = kt_cangca.SANLUONG_MUC,
                    SANLUONG_KHAC = kt_cangca.SANLUONG_KHAC,
                    HANG_NUOCDA = kt_cangca.HANG_NUOCDA,
                    HANG_NUOCNGOT = kt_cangca.HANG_NUOCNGOT,
                    HANG_XANGDAU = kt_cangca.HANG_XANGDAU,
                    HANG_KHAC = kt_cangca.HANG_KHAC,
                    NGAY_GHINHAN = kt_cangca.NGAY_GHINHAN
                    
                });
            }
            ViewBag.TotalRow = DSKT_CANGCA.Count().ToString();
            //Phân trang ở đây:

            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSKT_CANGCA.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);
          
        }


        public ActionResult Search(ViewModelSearchKT_CANGCA_THONGKE SearchModel)
        {
            BindComboDM_CANGCA();
            string strTitle = "";


                var result = db.Database.SqlQuery<ViewModelSearchKT_CANGCA_THONGKE>("exec KT_CANGCA_HANGHOAQUACANG @cangca_id, @fromdate, @todate "
                    , new SqlParameter("@cangca_id", SearchModel.DM_CANGCAID == null ? (object)DBNull.Value : SearchModel.DM_CANGCAID)
                    , new SqlParameter("@fromdate", SearchModel.TU_NGAY == null ? (object)DBNull.Value : SearchModel.TU_NGAY.Value)
                    , new SqlParameter("@todate", SearchModel.DEN_NGAY == null ? (object)DBNull.Value : SearchModel.DEN_NGAY.Value)
                    ).FirstOrDefault();

                if (SearchModel.DM_CANGCAID != null)
                { 
                strTitle = db.DM_CANGCA.Where(a => a.ID == SearchModel.DM_CANGCAID).FirstOrDefault().TEN_CANG;
                ViewBag.strTitle = strTitle;
                }
            return View(result);

        }



        public ActionResult Create()
        {
            //gan danh muc
            BindComboDM_CANGCA();
            KT_CANGCA model = new KT_CANGCA();
            model.NGUOI_CAPNHAP = User.Identity.Name;
            model.NGAY_GHINHAN = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( KT_CANGCA kt_cangca)
        {
            if (ModelState.IsValid)
            {
                db.KT_CANGCA.Add(kt_cangca);
                db.SaveChanges();

                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));

                return RedirectToAction("Index");
            }

            BindComboDM_CANGCA();
            return View(kt_cangca);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_CANGCA kt_cangca = db.KT_CANGCA.Find(id);
            if (kt_cangca == null)
            {
                return HttpNotFound();
            }
            BindComboDM_CANGCA();
            return View(kt_cangca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( KT_CANGCA kt_cangca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kt_cangca).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {
                //
                BindComboDM_CANGCA();

            }
            return View(kt_cangca);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_CANGCA kt_cangca = db.KT_CANGCA.Find(id);
            db.KT_CANGCA.Remove(kt_cangca);
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
	}
}