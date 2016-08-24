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
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FDB.Reports.KhaiThac;
using System.Reflection;
using FDB.Reports.KhaiThac.KT_KHUNEODAU;
using Microsoft.AspNet.Identity;

namespace FDB.Controllers.KhaiThac
{
    public class KT_KHUNEODAUController : FDBController //Controller
    {
        //
        // GET: /KhuNeoDau/
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
            ViewBag.LOAIKHUNEODAUs = new SelectList(db.DPHAN_LOAI_KHUNEODAU, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        public void initialCategoryEditAction(KT_KHUNEODAU searchModel)
        {
            ViewBag.LOAIKHUNEODAUs = new SelectList(db.DPHAN_LOAI_KHUNEODAU, "ID", "Name");
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
        public void initialCategorySearchAction(ViewModelSearchKT_KHUNEODAU searchModel, ref string ma_Tinh)
        {
            ViewBag.LOAIKHUNEODAUs = new SelectList(db.DPHAN_LOAI_KHUNEODAU, "ID", "Name");
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
        
        public ActionResult Index(ViewModelSearchKT_KHUNEODAU SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);
         

            var KT_KHUNEODAUs = db.KT_KHUNEODAU.Where(o => (SearchModel.TEN_KHUNEODAU == null || o.TEN_KHUNEODAU.ToUpper().Contains(SearchModel.TEN_KHUNEODAU.ToUpper()))
                                                        && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                          && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DPHAN_LOAI_KHUNEODAUID == null || o.DPHAN_LOAI_KHUNEODAUID == SearchModel.DPHAN_LOAI_KHUNEODAUID)

                                         ).Select(x => new { x.ID, x.TEN_KHUNEODAU, x.DIA_CHI, x.SOLUONGTAU_NEODAU, x.CONGSUATTAU_NEODAU, x.DPHAN_LOAI_KHUNEODAU, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA, x.VIDO, x.KINHDO }).OrderByDescending(x => x.ID)
                                         ;

            List<KT_KHUNEODAU> DSKT_KHUNEODAU = new List<KT_KHUNEODAU>();

            
            foreach (var kt_khuneodau in KT_KHUNEODAUs)
            {
                DSKT_KHUNEODAU.Add(new KT_KHUNEODAU
                {
                    ID = kt_khuneodau.ID,
                    TEN_KHUNEODAU = kt_khuneodau.TEN_KHUNEODAU,
                    DIA_CHI = kt_khuneodau.DIA_CHI,
                    SOLUONGTAU_NEODAU = kt_khuneodau.SOLUONGTAU_NEODAU,
                    CONGSUATTAU_NEODAU = kt_khuneodau.CONGSUATTAU_NEODAU,
                    DTINHTP = kt_khuneodau.DTINHTP,
                    DQUANHUYEN = kt_khuneodau.DQUANHUYEN,
                    DPHUONGXA = kt_khuneodau.DPHUONGXA,
                    DPHAN_LOAI_KHUNEODAU = kt_khuneodau.DPHAN_LOAI_KHUNEODAU,
                    VIDO = kt_khuneodau.VIDO,
                    KINHDO = kt_khuneodau.KINHDO
                });
            }

            ViewBag.TotalRow = DSKT_KHUNEODAU.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSKT_KHUNEODAU.ToPagedList(pageIndex,FDB.Common. Constants.PageSize);

            return View(SearchModel);
        }



        public ActionResult Search(ViewModelSearchKT_KHUNEODAU SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);


            var KT_KHUNEODAUs = db.KT_KHUNEODAU.Where(o => (SearchModel.TEN_KHUNEODAU == null || o.TEN_KHUNEODAU.ToUpper().Contains(SearchModel.TEN_KHUNEODAU.ToUpper()))
                                                        && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                         && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DPHAN_LOAI_KHUNEODAUID == null || o.DPHAN_LOAI_KHUNEODAUID == SearchModel.DPHAN_LOAI_KHUNEODAUID)

                                         ).Select(x => new { x.ID, x.TEN_KHUNEODAU, x.DIA_CHI, x.SOLUONGTAU_NEODAU, x.CONGSUATTAU_NEODAU, x.DPHAN_LOAI_KHUNEODAU, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA,x.VIDO,x.KINHDO }).OrderByDescending(x => x.ID)
                                         ;

            List<KT_KHUNEODAU> DSKT_KHUNEODAU = new List<KT_KHUNEODAU>();


            foreach (var kt_khuneodau in KT_KHUNEODAUs)
            {
                DSKT_KHUNEODAU.Add(new KT_KHUNEODAU
                {
                    ID = kt_khuneodau.ID,
                    TEN_KHUNEODAU = kt_khuneodau.TEN_KHUNEODAU,
                    DIA_CHI = kt_khuneodau.DIA_CHI,
                    SOLUONGTAU_NEODAU = kt_khuneodau.SOLUONGTAU_NEODAU,
                    CONGSUATTAU_NEODAU = kt_khuneodau.CONGSUATTAU_NEODAU,
                    DTINHTP = kt_khuneodau.DTINHTP,
                    DQUANHUYEN = kt_khuneodau.DQUANHUYEN,
                    DPHUONGXA = kt_khuneodau.DPHUONGXA,
                    DPHAN_LOAI_KHUNEODAU = kt_khuneodau.DPHAN_LOAI_KHUNEODAU,
                    VIDO = kt_khuneodau.VIDO,
                    KINHDO = kt_khuneodau.KINHDO

                });
            }

            ViewBag.TotalRow = DSKT_KHUNEODAU.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSKT_KHUNEODAU.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(SearchModel);
        }


        public ActionResult Create()
        {
            initialCategoryCreateAction();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_KHUNEODAU kt_khuneodau)
        {
            if (ModelState.IsValid)
            {
                db.KT_KHUNEODAU.Add(kt_khuneodau);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            initialCategoryCreateAction();
            return View(kt_khuneodau);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_KHUNEODAU kt_khuneodau = db.KT_KHUNEODAU.Find(id);
            if (kt_khuneodau == null)
            {
                return HttpNotFound();
            }
            initialCategoryEditAction(kt_khuneodau);
            return View(kt_khuneodau);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_KHUNEODAU kt_khuneodau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kt_khuneodau).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {

                initialCategoryEditAction(kt_khuneodau);

            }
            return View(kt_khuneodau);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_KHUNEODAU kt_khuneodau = db.KT_KHUNEODAU.Find(id);
            db.KT_KHUNEODAU.Remove(kt_khuneodau);
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

        public ActionResult GetReport(ViewModelSearchKT_KHUNEODAU SearchModel)
        {
            if (SearchModel.SearchButton == "Generate report")
            {

                var kt_khuneodaus = db.KT_KHUNEODAU.Where(o => (SearchModel.MA_TINHTP == null || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                               && (SearchModel.TEN_KHUNEODAU == null || o.TEN_KHUNEODAU.ToUpper().Contains(SearchModel.TEN_KHUNEODAU.ToUpper()))
                                                                 && (SearchModel.DPHAN_LOAI_KHUNEODAUID == null || o.DPHAN_LOAI_KHUNEODAUID == SearchModel.DPHAN_LOAI_KHUNEODAUID)
                                                                && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))

                                             ).Select(x => new { x.ID, x.MA_TINHTP, x.TEN_KHUNEODAU, x.DPHAN_LOAI_KHUNEODAUID, x.DIA_CHI, x.VIDO,x.KINHDO, x.DOSAU_DAUTAU, x.VITRI_VAOLUONG, x.HUONG_LUONG, x.CHIEUDAI_LUONG, x.SO_DIEN_THOAI, x.TAN_SO, x.DTINHTP, x.DPHAN_LOAI_KHUNEODAU }).ToList().OrderByDescending(x => x.ID)
                                             ;

                List<KT_KHUNEODAU> DSKT_KHUNEODAU = new List<KT_KHUNEODAU>();
                //duyet tung phan tu cua model da search duoc de dua ve kieu list 
                foreach (var kt_khuneodau in kt_khuneodaus)
                {
                    DSKT_KHUNEODAU.Add(new KT_KHUNEODAU
                    {
                        ID = kt_khuneodau.ID,
                        MA_TINHTP = kt_khuneodau.MA_TINHTP,
                        TEN_KHUNEODAU = kt_khuneodau.TEN_KHUNEODAU,
                        DIA_CHI = kt_khuneodau.DIA_CHI,
                        DPHAN_LOAI_KHUNEODAUID = kt_khuneodau.DPHAN_LOAI_KHUNEODAUID,
                        VIDO = kt_khuneodau.VIDO,
                        KINHDO = kt_khuneodau.KINHDO,
                        DOSAU_DAUTAU = kt_khuneodau.DOSAU_DAUTAU,
                        VITRI_VAOLUONG = kt_khuneodau.VITRI_VAOLUONG,
                        HUONG_LUONG = kt_khuneodau.HUONG_LUONG,
                        CHIEUDAI_LUONG = kt_khuneodau.CHIEUDAI_LUONG,
                        SO_DIEN_THOAI = kt_khuneodau.SO_DIEN_THOAI,
                        TAN_SO = kt_khuneodau.TAN_SO,
                        DTINHTP = kt_khuneodau.DTINHTP,
                        DPHAN_LOAI_KHUNEODAU = kt_khuneodau.DPHAN_LOAI_KHUNEODAU

                    });
                }

                //khoi tao dataset va dua tung record vao dataset
                _DS_KT_KHUNEODAU dt = new _DS_KT_KHUNEODAU();
                foreach (KT_KHUNEODAU kt in DSKT_KHUNEODAU)
                {
                    dt.DS_KT_KHUNEODAU.AddDS_KT_KHUNEODAURow(
                        kt.DTINHTP.TEN_TINHTP,
                        kt.TEN_KHUNEODAU,
                        kt.DPHAN_LOAI_KHUNEODAU.Name,
                        kt.DIA_CHI,
                        kt.VIDO,
                        kt.DOSAU_DAUTAU,
                        kt.VITRI_VAOLUONG,
                        kt.HUONG_LUONG,
                        kt.CHIEUDAI_LUONG,
                        kt.SO_DIEN_THOAI,
                        kt.TAN_SO

                     );

                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Server.MapPath("~/Reports/KhaiThac/KT_KHUNEODAU/RPT_KT_KHUNEODAU.rpt"));
                //rd.Load(Path.Combine(Server.MapPath("~/Reports/KhaiThac/KT_KHUNEODAU/RPT_KT_KHUNEODAU.rpt")));

                rd.SetDataSource(dt);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                System.IO.Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //Stream stream = rd.ExportToStream
                //    (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");


            }
            return View();
        }

        public ActionResult Report(ViewModelSearchKT_KHUNEODAU searchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(searchModel, ref ma_TinhTP);

            return View();
        }
	}
}