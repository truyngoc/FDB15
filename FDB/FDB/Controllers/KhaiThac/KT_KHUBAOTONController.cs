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


namespace FDB.Controllers.KhaiThac
{
    public class KT_KHUBAOTONController : FDBController //Controller
    {
        //
        // GET: /KhuBaoTon/

        private FDBContext db = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }

        public void BindComboDM_KHUBAOTON()
        {

            ViewBag.DM_KHUBAOTONs = new SelectList(db.DKHU_BAO_TON, "ID", "Name");
        }


        public void initialCategoryCreateAction()
        {
            ViewBag.DM_KHUBAOTONs = new SelectList(db.DKHU_BAO_TON, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        public void initialCategoryEditAction(KT_KHUBAOTON searchModel)
        {
            ViewBag.DM_KHUBAOTONs = new SelectList(db.DKHU_BAO_TON, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            var quanhuyen = db.DQUANHUYEN.Where(d => d.MA_TINHTP == searchModel.MA_TINHTP);
            var phuongxa = db.DPHUONGXA.Where(d => d.MA_QUANHUYEN == searchModel.MA_QUANHUYEN);

            ViewBag.QUAN_HUYENs = new SelectList(quanhuyen, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.PHUONG_XAs = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }
        public void initialCategorySearchAction(ViewModelSearchKT_KHU_BAO_TON searchModel, ref string ma_Tinh)
        {
            ViewBag.DM_KHUBAOTONs = new SelectList(db.DKHU_BAO_TON, "ID", "Name");
           // ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

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
        public ActionResult Index(ViewModelSearchKT_KHU_BAO_TON SearchModel)
        {
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);


            var KT_KHUBAOTONs = db.KT_KHUBAOTON.Where(o => (SearchModel.TEN_KHU_BAO_TON == null || o.TEN_KHU_BAO_TON.ToUpper().Contains(SearchModel.TEN_KHU_BAO_TON.ToUpper()))
                                                      && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                        && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DKHU_BAO_TONID == null || o.DKHU_BAO_TONID == SearchModel.DKHU_BAO_TONID)
                                          ).Select(x => new { x.ID, x.TEN_KHU_BAO_TON, x.DIA_CHI, x.TONG_DIENTICH, x.PHAN_DATLIEN, x.PHAN_NUOC, x.DKHU_BAO_TON, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA }).OrderByDescending(x => x.ID)
                                          ;

            List<KT_KHUBAOTON> DSKT_KHUBAOTON = new List<KT_KHUBAOTON>();
            foreach (var kt_khubaoton in KT_KHUBAOTONs)
            {
                DSKT_KHUBAOTON.Add(new KT_KHUBAOTON
                {
                    ID = kt_khubaoton.ID,
                    TEN_KHU_BAO_TON = kt_khubaoton.TEN_KHU_BAO_TON,
                    DIA_CHI = kt_khubaoton.DIA_CHI,
                    TONG_DIENTICH = kt_khubaoton.TONG_DIENTICH,
                    PHAN_DATLIEN = kt_khubaoton.PHAN_DATLIEN,
                    PHAN_NUOC = kt_khubaoton.PHAN_NUOC,
                    DKHU_BAO_TON = kt_khubaoton.DKHU_BAO_TON,
                    DTINHTP = kt_khubaoton.DTINHTP,
                    DQUANHUYEN = kt_khubaoton.DQUANHUYEN,
                    DPHUONGXA = kt_khubaoton.DPHUONGXA
                });
            }
            ViewBag.TotalRow = DSKT_KHUBAOTON.Count().ToString();
           
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSKT_KHUBAOTON.ToPagedList(pageIndex,FDB.Common. Constants.PageSize);

            return View(SearchModel);
            
        }

        public ActionResult Search(ViewModelSearchKT_KHU_BAO_TON SearchModel)
        {
            if (SearchModel.SearchButton == "Xuất Excel")
            {
                ExportData(SearchModel);
                //Dowload file:

                return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "KT_KHUBAOTONExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");

            }
            else
            {
                string ma_TinhTP = string.Empty;
                initialCategorySearchAction(SearchModel, ref ma_TinhTP);


                var KT_KHUBAOTONs = db.KT_KHUBAOTON.Where(o => (SearchModel.TEN_KHU_BAO_TON == null || o.TEN_KHU_BAO_TON.ToUpper().Contains(SearchModel.TEN_KHU_BAO_TON.ToUpper()))
                                                          && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                            && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                            && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                           && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                          && (SearchModel.DKHU_BAO_TONID == null || o.DKHU_BAO_TONID == SearchModel.DKHU_BAO_TONID)
                                              ).Select(x => new { x.ID, x.TEN_KHU_BAO_TON, x.DIA_CHI, x.TONG_DIENTICH, x.PHAN_DATLIEN, x.PHAN_NUOC, x.DKHU_BAO_TON, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA }).OrderByDescending(x => x.ID)
                                              ;

                List<KT_KHUBAOTON> DSKT_KHUBAOTON = new List<KT_KHUBAOTON>();
                foreach (var kt_khubaoton in KT_KHUBAOTONs)
                {
                    DSKT_KHUBAOTON.Add(new KT_KHUBAOTON
                    {
                        ID = kt_khubaoton.ID,
                        TEN_KHU_BAO_TON = kt_khubaoton.TEN_KHU_BAO_TON,
                        DIA_CHI = kt_khubaoton.DIA_CHI,
                        TONG_DIENTICH = kt_khubaoton.TONG_DIENTICH,
                        PHAN_DATLIEN = kt_khubaoton.PHAN_DATLIEN,
                        PHAN_NUOC = kt_khubaoton.PHAN_NUOC,
                        DKHU_BAO_TON = kt_khubaoton.DKHU_BAO_TON,
                        DTINHTP = kt_khubaoton.DTINHTP,
                        DQUANHUYEN = kt_khubaoton.DQUANHUYEN,
                        DPHUONGXA = kt_khubaoton.DPHUONGXA
                    });
                }
                ViewBag.TotalRow = DSKT_KHUBAOTON.Count().ToString();

                //Phân trang ở đây:
                var pageIndex = SearchModel.Page ?? 1;
                SearchModel.SearchResults = DSKT_KHUBAOTON.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

                return View(SearchModel);
            }
        }



        public ActionResult Create()
        {
            initialCategoryCreateAction();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KT_KHUBAOTON kt_khubaoton)
        {
            if (ModelState.IsValid)
            {
                db.KT_KHUBAOTON.Add(kt_khubaoton);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }

            initialCategoryCreateAction();
            return View(kt_khubaoton);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_KHUBAOTON kt_khubaoton = db.KT_KHUBAOTON.Find(id);
            if (kt_khubaoton == null)
            {
                return HttpNotFound();
            }
            initialCategoryEditAction(kt_khubaoton);
            return View(kt_khubaoton);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KT_KHUBAOTON kt_khubaoton)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kt_khubaoton).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {

                initialCategoryEditAction(kt_khubaoton);

            }
            return View(kt_khubaoton);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KT_KHUBAOTON kt_khubaoton = db.KT_KHUBAOTON.Find(id);
            db.KT_KHUBAOTON.Remove(kt_khubaoton);
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

        public JsonResult getAllKT_KHUBAOTON()
        {
            List<KT_KHUBAOTON> allKHUBAOTON = new List<KT_KHUBAOTON>();


            // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

            db = new FDBContext();

            allKHUBAOTON = db.KT_KHUBAOTON.ToList();


            return new JsonResult { Data = allKHUBAOTON, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Gmap()
        {

            return View();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region"Working Files"
        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_KhuBaoTon.xlsx");
        #endregion



        #region "Export excel"

        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchKT_KHU_BAO_TON SearchModel, List<String> lstMA_TTP)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMA_TTP == null)
                lstMA_TTP = new List<String>();
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);
            var selects = db.KT_KHUBAOTON.Where(o => (SearchModel.TEN_KHU_BAO_TON == null || o.TEN_KHU_BAO_TON.ToUpper().Contains(SearchModel.TEN_KHU_BAO_TON.ToUpper()))
                                                      && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                      && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                       //&& (SearchModel.MA_TINHTP == null || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DKHU_BAO_TONID == null || o.DKHU_BAO_TONID == SearchModel.DKHU_BAO_TONID)
                                          //).Select(x => new { x.ID, x.TEN_KHU_BAO_TON, x.DIA_CHI, x.TONG_DIENTICH, x.PHAN_DATLIEN, x.PHAN_NUOC, x.DKHU_BAO_TON, x.DTINHTP, x.DQUANHUYEN, x.DPHUONGXA,x.DKHU_BAO_TON.Name,x.VIDO,x.KINHDO }).OrderByDescending(x => x.ID)
                                            ).Select(x => new { MA_TTP = x.MA_TINHTP, TEN_TTP = x.DTINHTP.TEN_TINHTP }).Distinct();

            var models = selects.ToList();
            
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { this.NumberToRoman(loop + 1),models[loop].TEN_TTP
                                                             , " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "," " }
                                                          );
                lstMA_TTP.Add(models[loop].MA_TTP);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        private IEnumerable<List<object>> getkhubaotonByTinhTP(String MA_TTP, List<int> lstID)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstID == null)
                lstID = new List<int>();

            var selects = db.KT_KHUBAOTON.Where(o => o.MA_TINHTP == MA_TTP).Select(x => new { ID = x.ID, TEN_KHU_BAO_TON = x.TEN_KHU_BAO_TON });

            var models = selects.ToList();
            
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { //(loop + 1).ToString(),models[loop].TEN_KHU_BAO_TON
                                                             " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "," " }
                                                          );
                lstID.Add(models[loop].ID);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get detail by co so
        private IEnumerable<List<object>> getDetailkhubaoton(ViewModelSearchKT_KHU_BAO_TON SearchModel,int IDkhubaoton)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            string ma_TinhTP = string.Empty;
            initialCategorySearchAction(SearchModel, ref ma_TinhTP);
            var selects = db.KT_KHUBAOTON.Where(o => o.ID == IDkhubaoton
                                                    && (SearchModel.TEN_KHU_BAO_TON == null || o.TEN_KHU_BAO_TON.ToUpper().Contains(SearchModel.TEN_KHU_BAO_TON.ToUpper()))
                                                      && (SearchModel.DIA_CHI == null || o.DIA_CHI.ToUpper().Contains(SearchModel.DIA_CHI.ToUpper()))
                                                      && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                       //&& (SearchModel.MA_TINHTP == null || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                        && (SearchModel.MA_QUANHUYEN == null || o.MA_QUANHUYEN == SearchModel.MA_QUANHUYEN)
                                                       && (SearchModel.MA_PHUONGXA == null || o.MA_PHUONGXA == SearchModel.MA_PHUONGXA)
                                                      && (SearchModel.DKHU_BAO_TONID == null || o.DKHU_BAO_TONID == SearchModel.DKHU_BAO_TONID)
                );
            var lstDetail = selects.ToList();
            
            for (int loop = 0; loop < lstDetail.Count(); loop++)
            {
                lstobj.Add(new List<object> {" ",
                                                lstDetail[loop].DKHU_BAO_TON == null ||    lstDetail[loop].DKHU_BAO_TON.Name==null?" ":lstDetail[loop].DKHU_BAO_TON.Name.ToString()
                                              , lstDetail[loop].TEN_KHU_BAO_TON==null?" ": lstDetail[loop].TEN_KHU_BAO_TON.ToString()
                                              , lstDetail[loop].VIDO==null?" ":lstDetail[loop].VIDO.ToString()
                                              , lstDetail[loop].KINHDO==null?" ":lstDetail[loop].KINHDO.ToString()                                              
                                              , lstDetail[loop].DQUANHUYEN == null ||lstDetail[loop].DQUANHUYEN.TEN_QUANHUYEN==null?" ":lstDetail[loop].DQUANHUYEN.TEN_QUANHUYEN.ToString()
                                              , lstDetail[loop].DPHUONGXA == null ||lstDetail[loop].DPHUONGXA.TEN_PHUONGXA==null?" ": lstDetail[loop].DPHUONGXA.TEN_PHUONGXA.ToString()
                                              , lstDetail[loop].DIA_CHI==null?" ":lstDetail[loop].DIA_CHI.ToString()
                                              , lstDetail[loop].TONG_DIENTICH==null?" ":lstDetail[loop].TONG_DIENTICH.ToString()
                                              , lstDetail[loop].PHAN_DATLIEN==null?" ":lstDetail[loop].PHAN_DATLIEN.ToString()
                                              , lstDetail[loop].PHAN_NUOC==null?" ":lstDetail[loop].PHAN_NUOC.ToString()
                                              , lstDetail[loop].PHAMVI_GIOIHAN==null?" ":lstDetail[loop].PHAMVI_GIOIHAN.ToString()
                                              , lstDetail[loop].NOI_SAU_NHAT==null?" ":lstDetail[loop].NOI_SAU_NHAT.ToString()
                                               , lstDetail[loop].HE_SINH_THAI==null?" ":lstDetail[loop].HE_SINH_THAI.ToString()
                                              , lstDetail[loop].CONGTAC_BAOTON==null?" ":lstDetail[loop].CONGTAC_BAOTON.ToString()
                                            });

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

          public ActionResult ExportData(ViewModelSearchKT_KHU_BAO_TON SearchModel)  
        {  
            
              try
              {

                  using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_NAME,GENERATED_FILE_NAME))
                  {
                      helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                      helper.CurrentSheetName = "Sheet1";

                      helper.CurrentPosition = new CellRef("A5");
                      helper.InsertRange("header_1");

                      helper.CurrentPosition = new CellRef("A6");
                      helper.InsertRange("header_2");

                      CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "stt_lama", "tinhthanhpho", "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "c10", "c11", "c12","c13" });
                     // CellRangeTemplate row_coso = helper.CreateCellRangeTemplate("row_coso", new List<string> { "stt_num", "tencoso", "c14", "c15", "c16", "c17", "c18", "c19", "c20", "c21", "c22", "c23", "c24", "c25", "c26" });
                      CellRangeTemplate row_11 = helper.CreateCellRangeTemplate("row_11", new List<string> { "str", "c_25", "c_26", "c_27", "c_28", "c_29", "c_30", "c_31", "c_32", "c_33", "c_34", "c_35", "c_36", "c_37", "c_38" });

                      int k = 7;
                      List<String> _lstMA_TTP = new List<string>();
                      IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(SearchModel, _lstMA_TTP);

                      for (int i = 0; i < _lstTinhTP.Count(); i++)
                      {

                          //insert Tinh thanh pho
                          helper.CurrentPosition = new CellRef("A" + (k).ToString());
                          helper.InsertRange(row_tinhthanhpho, _lstTinhTP.ToArray()[i]);
                          k = k + 1;
                          //insert Co so
                          List<int> _lstCoSoID = new List<int>();
                          IEnumerable<List<object>> _lstCoSo = this.getkhubaotonByTinhTP(_lstMA_TTP[i], _lstCoSoID);
                          for (int j = 0; j < _lstCoSo.Count(); j++)
                          {
                              //helper.CurrentPosition = new CellRef("A" + (k).ToString());
                              //helper.InsertRange(row_coso, _lstCoSo.ToArray()[j]);
                              //k = k + 1;

                              //Insert co so detail
                              helper.CurrentPosition = new CellRef("A" + (k).ToString());
                              IEnumerable<List<object>> lstDetail = this.getDetailkhubaoton(SearchModel,_lstCoSoID[j]);
                              helper.InsertRange(row_11, lstDetail);
                              k = k + lstDetail.Count();
                          }
                         
                      }
                      helper.DeleteSheet("Sheet3");
                      helper.CurrentSheetName = "Sheet1";
                      
                  }
                  
                  
                  TempData["Message"] = "Xuất fiel Excel thành công!";
                  ViewBag.MSG_EXPORT = TempData["Message"];
                 // return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "KT_KHUBAOTONExcel" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
              }
              catch(Exception ex)
              {

                  TempData["Message"] = "Xuất file Excel không thành công!";
                  ViewBag.MSG_EXPORT = TempData["Message"];
              }

              return RedirectToAction("Index");
        }  
 #endregion


    }
}