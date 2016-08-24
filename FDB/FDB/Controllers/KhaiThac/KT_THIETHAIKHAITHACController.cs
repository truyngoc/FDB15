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
using System.Data.SqlClient;

namespace FDB.Controllers.KhaiThac
{
    public class KT_THIETHAIKHAITHACController : FDBController //Controller
    {
        //
        // GET: /ThietHaiKhaiThac/

        private FDBContext db = new FDBContext();

        public void BindCheckboxList()
        {

            ViewBag.SUCOVETAUs = new SelectList(CategoryCommon.SUCOVETAU, "ID", "TEN");

            ViewBag.SUCOVENGUOIs = new SelectList(CategoryCommon.SUCOVENGUOI, "ID", "TEN");

        }
        public ActionResult Index(ViewModelSearchKT_THIETHAIKHAITHAC SearchModel)
        {
            var KT_THIETHAIKHAITHACs = db.KT_THIETHAIKHAITHAC.Where
                //(o => (SearchModel.SODK_TAU == null || o.SO_DK_TAU == SearchModel.SODK_TAU)
                                                        (o => (SearchModel.SODK_TAU == null || o.SO_DK_TAU.ToUpper().Contains(SearchModel.SODK_TAU.ToUpper()))
                                                             && ((SearchModel.TU_NGAY == null || o.TG_GAPNAN >= SearchModel.TU_NGAY)
                                                             && (SearchModel.DEN_NGAY == null || o.TG_GAPNAN <= SearchModel.DEN_NGAY))
                                                   ).Select(x => new { x.ID, x.SO_DK_TAU, x.SO_THUYENVIEN, x.KHUVUC_GAPNAN, x.TG_GAPNAN, x.THIETHAI_UOCTINH }).OrderByDescending(x => x.ID);



            List<KT_THIETHAIKHAITHAC> DSKT_THIETHAI = new List<KT_THIETHAIKHAITHAC>();
            foreach (var kt_thiethai in KT_THIETHAIKHAITHACs)
            {
                DSKT_THIETHAI.Add(new KT_THIETHAIKHAITHAC
                {
                    ID = kt_thiethai.ID,
                    SO_DK_TAU = kt_thiethai.SO_DK_TAU,
                    SO_THUYENVIEN = kt_thiethai.SO_THUYENVIEN,
                    KHUVUC_GAPNAN = kt_thiethai.KHUVUC_GAPNAN,
                    TG_GAPNAN = kt_thiethai.TG_GAPNAN,
                    THIETHAI_UOCTINH = kt_thiethai.THIETHAI_UOCTINH

                });
            }
            ViewBag.TotalRow = DSKT_THIETHAI.Count().ToString();
            //Phân trang ở đây:

            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSKT_THIETHAI.ToPagedList(pageIndex, Constants.PageSize);

            return View(SearchModel);
        }

        public ActionResult Search(ViewModelSearchKT_THIETHAIKHAITHAC SearchModel)
        {
            // khoi tao default checkboxlist
            var allSUCOVETAU = db.DSUCOVETAU.ToList().OrderByDescending(x => x.ID); //returns List<SUCOVETAU>

            var allSUCOVENGUOI = db.DSUCOVENGUOI.ToList().OrderByDescending(x => x.ID); //returns List<SUCOVENGUOI>
            
            // -------------------------------------------------


            var KT_THIETHAIKHAITHACs = db.KT_THIETHAIKHAITHAC.Where(o => ((SearchModel.TU_NGAY == null || o.TG_GAPNAN >= SearchModel.TU_NGAY)
                                                             && (SearchModel.DEN_NGAY == null || o.TG_GAPNAN <= SearchModel.DEN_NGAY))

                                                   //).Select(x => new { x.ID, x.SO_DK_TAU, x.SO_THUYENVIEN, x.KHUVUC_GAPNAN, x.TG_GAPNAN, x.THIETHAI_UOCTINH,x.SUCOVETAU,x.SUCOVENGUOI })
                                                   ).OrderByDescending(x => x.ID);

            List<KT_THIETHAIKHAITHAC> DSKT_THIETHAI = KT_THIETHAIKHAITHACs.ToList();

            List<ViewModelAddKT_THIETHAIKHAITHAC> DSKT_THIETHAI_NEW = new List<ViewModelAddKT_THIETHAIKHAITHAC>();

            ViewModelAddKT_THIETHAIKHAITHAC fu;
            foreach (var kt_thiethai in DSKT_THIETHAI)
            {
                fu = new ViewModelAddKT_THIETHAIKHAITHAC();


                var checkBoxListItemSUCOVETAU = new List<CheckBoxListItem>();
                var checkBoxListItemSUCOVENGUOI = new List<CheckBoxListItem>();
                foreach (var sucovetau in allSUCOVETAU)
                {

                    checkBoxListItemSUCOVETAU.Add(new CheckBoxListItem()
                    {
                        ID = sucovetau.ID,
                        Display = sucovetau.Name,
                        IsChecked = kt_thiethai.SUCOVETAU.Where(x => x.ID == sucovetau.ID).Any()
                    });
                }


                foreach (var sucovenguoi in allSUCOVENGUOI)
                {

                    checkBoxListItemSUCOVENGUOI.Add(new CheckBoxListItem()
                    {
                        ID = sucovenguoi.ID,
                        Display = sucovenguoi.Name,
                        IsChecked = kt_thiethai.SUCOVENGUOI.Where(x => x.ID == sucovenguoi.ID).Any()
                    });
                }

                
                fu.SO_DK_TAU = kt_thiethai.SO_DK_TAU;
                fu.SO_THUYENVIEN = kt_thiethai.SO_THUYENVIEN;
                fu.KHUVUC_GAPNAN = kt_thiethai.KHUVUC_GAPNAN;
                fu.TG_GAPNAN = kt_thiethai.TG_GAPNAN;
                fu.THIETHAI_UOCTINH = kt_thiethai.THIETHAI_UOCTINH;                
                fu.COQUAN_XULY = kt_thiethai.COQUAN_XULY;
                fu.SUCOVENGUOI = checkBoxListItemSUCOVENGUOI;
                fu.SUCOVETAU = checkBoxListItemSUCOVETAU;
                fu.SO_NGUOI_CHET = kt_thiethai.SO_NGUOI_CHET;
                fu.SO_NGUOI_MAT_TICH = kt_thiethai.SO_NGUOI_MAT_TICH;

                DSKT_THIETHAI_NEW.Add(fu);
            }


            //tong so
            Sum(SearchModel);

            var tong_chet = KT_THIETHAIKHAITHACs.Sum(s => s.SO_NGUOI_CHET);
            var tong_mat_tich = KT_THIETHAIKHAITHACs.Sum(s => s.SO_NGUOI_MAT_TICH);

            ViewBag.tong_chet = tong_chet;
            ViewBag.tong_mat_tich = tong_mat_tich;
           

            //    cmd.CommandText = "SELECT COUNT(DSUCOVENGUOI_ID) AS [Ốm/tai nạn lao động] FROM KT_THIETHAIKHAITHACDSUCOVENGUOI where DSUCOVENGUOI_ID = 4 group by DSUCOVENGUOI_ID";
            //    var count_9 = cmd.ExecuteScalar();
            //    if (count_9 != null)
            //    {
            //        ViewBag.Count9 = count_9.ToString();
            //    }
            //}

            ViewBag.TotalRow = KT_THIETHAIKHAITHACs.Count();
            //Phân trang ở đây:

            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.StatisticsResults = DSKT_THIETHAI_NEW.ToPagedList(pageIndex, Constants.PageSize);

            return View(SearchModel);
        }

        private void Sum(ViewModelSearchKT_THIETHAIKHAITHAC SearchModel)
        {

            using (var ctx = new FDBContext())
            {
                //using (var cmd = ctx.Database.Connection.CreateCommand())
                //{
                //    ctx.Database.Connection.Open();

                var ListSuCoTAU = ctx.Database.SqlQuery<KT_SUCOVETAU>("exec KT_THIETHAIKHAITHAC_SUCOTAU @fromdate, @todate "
                    , new SqlParameter("@fromdate", SearchModel.TU_NGAY == null ? (object)DBNull.Value : SearchModel.TU_NGAY.Value)
                    , new SqlParameter("@todate", SearchModel.DEN_NGAY == null ? (object)DBNull.Value : SearchModel.DEN_NGAY.Value)
                    ).ToList();
                ViewBag.CountSuCoTAU = ListSuCoTAU;

                var ListSuCoNGUOI = ctx.Database.SqlQuery<KT_SUCOVENGUOI>("exec KT_THIETHAIKHAITHAC_SUCONGUOI @fromdate, @todate "
                 , new SqlParameter("@fromdate", SearchModel.TU_NGAY == null ? (object)DBNull.Value : SearchModel.TU_NGAY.Value)
                 , new SqlParameter("@todate", SearchModel.DEN_NGAY == null ? (object)DBNull.Value : SearchModel.DEN_NGAY.Value)
                 ).ToList();
                ViewBag.CountSuCoNGUOI = ListSuCoNGUOI;

                //const string selectCmd = @" select top 1 sum(THIETHAI_UOCTINH) as [tong_thiet_hai] from KT_THIETHAIKHAITHAC where (@fromdate is null or  TG_GAPNAN >= @fromdate) and (@todate is null or TG_GAPNAN <= @todate) ";

                var tong_thiethai = ctx.Database.SqlQuery<KT_SUCOVETAU>("exec KT_THIETHAIKHAITHAC_TONG @fromdate, @todate "
                  , new SqlParameter("@fromdate", SearchModel.TU_NGAY == null ? (object)DBNull.Value : SearchModel.TU_NGAY.Value)
                  , new SqlParameter("@todate", SearchModel.DEN_NGAY == null ? (object)DBNull.Value : SearchModel.DEN_NGAY.Value)
                  ).FirstOrDefault();
                ViewBag.tongthiethai = tong_thiethai.tong_thiet_hai==null?string.Empty :tong_thiethai.tong_thiet_hai.Value.ToString("#,###");


                //}
            }
          
        }

        public ActionResult Create()
        {
            ViewModelAddKT_THIETHAIKHAITHAC model = new ViewModelAddKT_THIETHAIKHAITHAC();

            var allSUCOVETAU = db.DSUCOVETAU.ToList().OrderByDescending(x => x.ID); //returns List<SUCOVETAU>

            var allSUCOVENGUOI = db.DSUCOVENGUOI.ToList().OrderByDescending(x => x.ID); //returns List<SUCOVENGUOI>
            var checkBoxListItemSUCOVETAU = new List<CheckBoxListItem>();
            var checkBoxListItemSUCOVENGUOI = new List<CheckBoxListItem>();
            foreach (var sucovetau in allSUCOVETAU)
            {

                checkBoxListItemSUCOVETAU.Add(new CheckBoxListItem()
                {
                    ID = sucovetau.ID,
                    Display = sucovetau.Name,
                    IsChecked = false
                });
                model.SUCOVETAU = checkBoxListItemSUCOVETAU;
            }


            foreach (var sucovenguoi in allSUCOVENGUOI)
            {

                checkBoxListItemSUCOVENGUOI.Add(new CheckBoxListItem()
                {
                    ID = sucovenguoi.ID,
                    Display = sucovenguoi.Name,
                    IsChecked = false
                });
                model.SUCOVENGUOI = checkBoxListItemSUCOVENGUOI;
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModelAddKT_THIETHAIKHAITHAC model)
        {

            if (ModelState.IsValid)
            {
                var selectedSucovetau = model.SUCOVETAU.Where(x => x.IsChecked).Select(x => x.ID).ToList();
                var selectedSucovenguoi = model.SUCOVENGUOI.Where(x => x.IsChecked).Select(x => x.ID).ToList();
                KT_THIETHAIKHAITHACManager.Add(model.SO_DK_TAU,
                    model.SO_THUYENVIEN,
                model.KHUVUC_GAPNAN,
                model.TG_GAPNAN,
                selectedSucovetau,
                selectedSucovenguoi,
                model.COQUAN_XULY,
                model.THIETHAI_UOCTINH,
                model.VIDO,
                model.KINHDO,
                model.TAU_KHAC,
                model.NGUOI_KHAC,
                model.SO_NGUOI_CHET,
                model.SO_NGUOI_MAT_TICH
                );

                this.Information(String.Format(Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kt_thiethai = KT_THIETHAIKHAITHACManager.GetByID(id);

            var model = new ViewModelEditKT_THIETHAIKHAITHAC()
            {
                ID = kt_thiethai.ID,
                SO_DK_TAU = kt_thiethai.SO_DK_TAU,
                SO_THUYENVIEN = kt_thiethai.SO_THUYENVIEN,
                KHUVUC_GAPNAN = kt_thiethai.KHUVUC_GAPNAN,
                TG_GAPNAN = kt_thiethai.TG_GAPNAN,
                COQUAN_XULY = kt_thiethai.COQUAN_XULY,
                THIETHAI_UOCTINH = kt_thiethai.THIETHAI_UOCTINH,
                NGUOI_KHAC = kt_thiethai.NGUOI_KHAC,
                TAU_KHAC = kt_thiethai.TAU_KHAC,
                VIDO = kt_thiethai.VIDO,
                KINHDO = kt_thiethai.KINHDO,
                   SO_NGUOI_CHET = kt_thiethai.SO_NGUOI_CHET,
                SO_NGUOI_MAT_TICH = kt_thiethai.SO_NGUOI_MAT_TICH
            };

            var SUCOVETAU = KT_THIETHAIKHAITHACManager.GetSUCOVETAUByID(id);
            var allSucovetau = KT_THIETHAIKHAITHACManager.GetAllSUCOVETAU();
            var checkBoxListItemsSUCOVETAU = new List<CheckBoxListItem>();
            foreach (var sucovetau in allSucovetau)
            {
                checkBoxListItemsSUCOVETAU.Add(new CheckBoxListItem()
                {
                    ID = sucovetau.ID,
                    Display = sucovetau.Name,
                    //We should have already-selected genres be checked
                    IsChecked = SUCOVETAU.Where(x => x.ID == sucovetau.ID).Any()
                });
            }
            model.SUCOVETAU = checkBoxListItemsSUCOVETAU;
            //su co ve nguoi
            var SUCOVENGUOI = KT_THIETHAIKHAITHACManager.GetSUCOVENGUOIByID(id);
            var allSucovenguoi = KT_THIETHAIKHAITHACManager.GetAllSUCOVENGUOI();
            var checkBoxListItemsSUCOVENGUOI = new List<CheckBoxListItem>();
            foreach (var sucovenguoi in allSucovenguoi)
            {
                checkBoxListItemsSUCOVENGUOI.Add(new CheckBoxListItem()
                {
                    ID = sucovenguoi.ID,
                    Display = sucovenguoi.Name,
                    //We should have already-selected genres be checked
                    IsChecked = SUCOVENGUOI.Where(x => x.ID == sucovenguoi.ID).Any()
                });
            }
            model.SUCOVENGUOI = checkBoxListItemsSUCOVENGUOI;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModelEditKT_THIETHAIKHAITHAC model)
        {
            if (ModelState.IsValid)
            {


                var selectedSucovetau = model.SUCOVETAU.Where(x => x.IsChecked).Select(x => x.ID).ToList();
                var selectedSucovenguoi = model.SUCOVENGUOI.Where(x => x.IsChecked).Select(x => x.ID).ToList();
                KT_THIETHAIKHAITHACManager.Edit(model.ID, model.SO_DK_TAU,
                   model.SO_THUYENVIEN,
               model.KHUVUC_GAPNAN,
               model.TG_GAPNAN,
               selectedSucovetau,
               selectedSucovenguoi,
               model.COQUAN_XULY,
               model.THIETHAI_UOCTINH,
               model.VIDO,
               model.KINHDO,
               model.TAU_KHAC,
               model.NGUOI_KHAC,
                model.SO_NGUOI_CHET,
               model.SO_NGUOI_MAT_TICH
               
               );

                this.Information(String.Format(Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }

            return View(model);

        }

        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //KT_THIETHAIKHAITHACManager.Delete(id);
            //this.Information(String.Format(Constants.NOTIFY_DELETE_SUCCESS, "bản ghi"));
            //return RedirectToAction("Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_THIETHAIKHAITHAC kt_thiethai = db.KT_THIETHAIKHAITHAC.Find(id);
            db.KT_THIETHAIKHAITHAC.Remove(kt_thiethai);
            db.SaveChanges();
            this.Information(String.Format(Constants.NOTIFY_DELETE_SUCCESS, "bản ghi"));
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