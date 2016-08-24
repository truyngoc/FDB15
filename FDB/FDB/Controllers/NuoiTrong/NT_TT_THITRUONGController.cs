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
using System.Data.Entity;


namespace FDB.Controllers.NuoiTrong
{
    public class NT_TT_THITRUONGController : FDBController
    {
        private FDBContext db = new FDBContext();

        // GET: /NT_TT_THITRUONG/
        public void initialSearchCategory(ViewModelSearchNT_TT_THITRUONG searchModel, ref string ma_Tinh)
        {
            ViewBag.DM_DOITUONG_NUOIs = new SelectList(db.DM_DOITUONG_GIA_THITRUONG, "ID", "Name");
            //ViewBag.DPHANLOAI_GIAs = new SelectList(db.DPHANLOAI_GIA, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");


            ApplicationUser curUser = getCurrentUser();


            var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP.StartsWith("Z") || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ma_Tinh = curUser.MA_TINHTP;
        }

        public void initialCategory()
        {
            ViewBag.DM_DOITUONG_NUOIs = new SelectList(db.DM_DOITUONG_GIA_THITRUONG, "ID", "Name");
            //ViewBag.DPHANLOAI_GIAs = new SelectList(db.DPHANLOAI_GIA, "ID", "Name");
            //ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");


            //ApplicationUser curUser = getCurrentUser();

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            //ViewBag.DTINHTPs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");


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
        public ActionResult Index(ViewModelSearchNT_TT_THITRUONG SearchModel)
        {
            
            string ma_TinhTP = string.Empty;
            initialSearchCategory(SearchModel, ref ma_TinhTP);


            var NT_TT_THITRUONGs = db.NT_TT_THITRUONG.Where(o =>
                                                      (SearchModel.DM_DOITUONG_GIA_THITRUONGID == null || o.DM_DOITUONG_GIA_THITRUONGID == SearchModel.DM_DOITUONG_GIA_THITRUONGID)
                                                      && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)

                                          ).Select(x => new { x.ID, x.DM_DOITUONG_GIA_THITRUONG,x.GIA_THANH_TU,x.GIA_THANH_DEN,x.GIA_GIONG_TU,x.GIA_GIONG_DEN,x.GIA_THUCAN_TU,x.GIA_THUCAN_DEN,x.GIA_BAN_TU,x.GIA_BAN_DEN,x.KICHCO,x.NGAY_THUTHAP,x.NOITHUTHAP,x.DTINHTP}
                                          ).OrderByDescending(x => x.ID)
                                          ;

            List<NT_TT_THITRUONG> DSNT_TT_THITRUONG = new List<NT_TT_THITRUONG>();
            foreach (var nt_tt_thitruong in NT_TT_THITRUONGs)
            {
                DSNT_TT_THITRUONG.Add(new NT_TT_THITRUONG
                {
                    ID = nt_tt_thitruong.ID,
                    DM_DOITUONG_GIA_THITRUONG = nt_tt_thitruong.DM_DOITUONG_GIA_THITRUONG,
                    GIA_THANH_TU = nt_tt_thitruong.GIA_THANH_TU,
                    GIA_THANH_DEN = nt_tt_thitruong.GIA_THANH_DEN,
                    GIA_GIONG_TU = nt_tt_thitruong.GIA_GIONG_TU,
                    GIA_GIONG_DEN = nt_tt_thitruong.GIA_GIONG_DEN,
                    GIA_BAN_TU = nt_tt_thitruong.GIA_BAN_TU,
                    GIA_BAN_DEN = nt_tt_thitruong.GIA_BAN_DEN,
                    GIA_THUCAN_TU = nt_tt_thitruong.GIA_THUCAN_TU,
                    GIA_THUCAN_DEN = nt_tt_thitruong.GIA_THUCAN_DEN,
                    KICHCO = nt_tt_thitruong.KICHCO,
                    NOITHUTHAP = nt_tt_thitruong.NOITHUTHAP,
                    NGAY_THUTHAP = nt_tt_thitruong.NGAY_THUTHAP,
                    DTINHTP = nt_tt_thitruong.DTINHTP                  
                });
            }

            ViewBag.TotalRow = DSNT_TT_THITRUONG.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = SearchModel.Page ?? 1;
            SearchModel.SearchResults = DSNT_TT_THITRUONG.ToPagedList(pageIndex,FDB.Common. Constants.PageSize);

            return View(SearchModel);
        }

        public ActionResult Search(ViewModelSearchNT_TT_THITRUONG SearchModel)
        {
            if (SearchModel.SearchButton == "Xuất Excel")
            {
                ExportExcel(SearchModel);
                //Dowload file:

                return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "NT_GIA_THITRUONGExport" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");

            }
            else
            {
                string ma_TinhTP = string.Empty;
                initialSearchCategory(SearchModel, ref ma_TinhTP);


                var NT_TT_THITRUONGs = db.NT_TT_THITRUONG.Where(o =>
                                                          (SearchModel.DM_DOITUONG_GIA_THITRUONGID == null || o.DM_DOITUONG_GIA_THITRUONGID == SearchModel.DM_DOITUONG_GIA_THITRUONGID)
                                                           && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                             && ((SearchModel.TU_NGAY == null || o.NGAY_THUTHAP >= SearchModel.TU_NGAY)
                                                                   && (SearchModel.DEN_NGAY == null || o.NGAY_THUTHAP <= SearchModel.DEN_NGAY))

                                              ).Select(x => new { x.ID, x.DM_DOITUONG_GIA_THITRUONG, x.GIA_THANH_TU, x.GIA_THANH_DEN, x.GIA_GIONG_TU, x.GIA_GIONG_DEN, x.GIA_THUCAN_TU, x.GIA_THUCAN_DEN, x.GIA_BAN_TU, x.GIA_BAN_DEN, x.KICHCO, x.NGAY_THUTHAP, x.NOITHUTHAP, x.DTINHTP }
                                              ).OrderByDescending(x => x.ID)
                                              ;

                List<NT_TT_THITRUONG> DSNT_TT_THITRUONG = new List<NT_TT_THITRUONG>();
                foreach (var nt_tt_thitruong in NT_TT_THITRUONGs)
                {
                    DSNT_TT_THITRUONG.Add(new NT_TT_THITRUONG
                    {
                        ID = nt_tt_thitruong.ID,
                        DM_DOITUONG_GIA_THITRUONG = nt_tt_thitruong.DM_DOITUONG_GIA_THITRUONG,
                        GIA_THANH_TU = nt_tt_thitruong.GIA_THANH_TU,
                        GIA_THANH_DEN = nt_tt_thitruong.GIA_THANH_DEN,
                        GIA_GIONG_TU = nt_tt_thitruong.GIA_GIONG_TU,
                        GIA_GIONG_DEN = nt_tt_thitruong.GIA_GIONG_DEN,
                        GIA_BAN_TU = nt_tt_thitruong.GIA_BAN_TU,
                        GIA_BAN_DEN = nt_tt_thitruong.GIA_BAN_DEN,
                        GIA_THUCAN_TU = nt_tt_thitruong.GIA_THUCAN_TU,
                        GIA_THUCAN_DEN = nt_tt_thitruong.GIA_THUCAN_DEN,
                        KICHCO = nt_tt_thitruong.KICHCO,
                        NOITHUTHAP = nt_tt_thitruong.NOITHUTHAP,
                        NGAY_THUTHAP = nt_tt_thitruong.NGAY_THUTHAP,
                        DTINHTP = nt_tt_thitruong.DTINHTP
                    });
                }

                ViewBag.TotalRow = DSNT_TT_THITRUONG.Count().ToString();
                //Phân trang ở đây:
                var pageIndex = SearchModel.Page ?? 1;
                SearchModel.SearchResults = DSNT_TT_THITRUONG.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

                return View(SearchModel);
            }
        }

        // GET: /NT_TT_THITRUONG/Create
        public ActionResult Create()
        
        {
            initialCategory();
            //NT_TT_THITRUONG model = new NT_TT_THITRUONG();
            //model.NGAY_NM = DateTime.Now;

            return View();
        }

        // POST: /NT_TT_THITRUONG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NT_TT_THITRUONG nt_tt_thitruong)
        {
            if (ModelState.IsValid)
            {
              
                
                nt_tt_thitruong.NGAY_NM = DateTime.Now;
                //nt_tt_thitruong.GIA_THANH_TU = Convert.ToDecimal(_form["GIA_THANH_TU"]);
                //nt_tt_thitruong.GIA_THANH_DEN = Convert.ToDecimal(_form["GIA_THANH_DEN"]);
               
               
                db.NT_TT_THITRUONG.Add(nt_tt_thitruong);
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }

            initialCategory();



            return View(nt_tt_thitruong);
        }

        // GET: /NT_TT_THITRUONG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NT_TT_THITRUONG nt_tt_thitruong = db.NT_TT_THITRUONG.Find(id);
            if (nt_tt_thitruong == null)
            {
                return HttpNotFound();
            }
            initialCategory();

            return View(nt_tt_thitruong);
        }

        // POST: /NT_TT_THITRUONG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NT_TT_THITRUONG nt_tt_thitruong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nt_tt_thitruong).State = EntityState.Modified;
                nt_tt_thitruong.NGAY_NM = DateTime.Now;
                db.SaveChanges();
                this.Information(String.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "bản ghi"));
                return RedirectToAction("Index");
            }
            else
            {

                initialCategory();

            }

            return View(nt_tt_thitruong);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NT_TT_THITRUONG nt_tt_thitruong = db.NT_TT_THITRUONG.Find(id);
            
            db.NT_TT_THITRUONG.Remove(nt_tt_thitruong);
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


        #region"Working Files"
        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Export_TT_THITRUONG.xlsx");
        #endregion



        #region "Export excel"

        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchNT_TT_THITRUONG SearchModel, List<String> lstMA_TTP)
        {
            string ma_TinhTP = string.Empty;
            initialSearchCategory(SearchModel, ref ma_TinhTP);
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMA_TTP == null)
                lstMA_TTP = new List<String>();
            var selects = db.NT_TT_THITRUONG.Where(o =>
                                                      (SearchModel.DM_DOITUONG_GIA_THITRUONGID == null || o.DM_DOITUONG_GIA_THITRUONGID == SearchModel.DM_DOITUONG_GIA_THITRUONGID)
                                                       && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                         && ((SearchModel.TU_NGAY == null || o.NGAY_THUTHAP >= SearchModel.TU_NGAY)
                                                               && (SearchModel.DEN_NGAY == null || o.NGAY_THUTHAP <= SearchModel.DEN_NGAY))
                                            ).Select(x => new { MA_TTP = x.MA_TINHTP, TEN_TTP = x.DTINHTP.TEN_TINHTP }).Distinct();

            var models = selects.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { this.NumberToRoman(loop + 1),models[loop].TEN_TTP
                                                             , " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " }
                                                          );
                lstMA_TTP.Add(models[loop].MA_TTP);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        private IEnumerable<List<object>> gettt_thitruongByTinhTP(String MA_TTP, List<int> lstID)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstID == null)
                lstID = new List<int>();

            var selects = db.NT_TT_THITRUONG.Where(o => o.MA_TINHTP == MA_TTP).Select(x => new { ID = x.ID, Name = x.DM_DOITUONG_GIA_THITRUONG.Name });

            var models = selects.ToList();

            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { //(loop + 1).ToString(),models[loop].TEN_KHUBAOVENGUONLOI
                                                           " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " }
                                                          );
                lstID.Add(models[loop].ID);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        //get detail by co so
        private IEnumerable<List<object>> getDetailtt_thitruong(ViewModelSearchNT_TT_THITRUONG SearchModel,int id)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            string ma_TinhTP = string.Empty;
            initialSearchCategory(SearchModel, ref ma_TinhTP);
            var selects = db.NT_TT_THITRUONG.Where(o => o.ID == id
                                                     && (SearchModel.DM_DOITUONG_GIA_THITRUONGID == null || o.DM_DOITUONG_GIA_THITRUONGID == SearchModel.DM_DOITUONG_GIA_THITRUONGID)
                                                       && ((string.IsNullOrEmpty(SearchModel.MA_TINHTP) && ma_TinhTP.StartsWith("Z")) || (string.IsNullOrEmpty(SearchModel.MA_TINHTP) && o.MA_TINHTP == ma_TinhTP) || o.MA_TINHTP == SearchModel.MA_TINHTP)
                                                         && ((SearchModel.TU_NGAY == null || o.NGAY_THUTHAP >= SearchModel.TU_NGAY)
                                                               && (SearchModel.DEN_NGAY == null || o.NGAY_THUTHAP <= SearchModel.DEN_NGAY))
                                    );
            var lstDetail = selects.ToList();

            for (int loop = 0; loop < lstDetail.Count(); loop++)
            {
                lstobj.Add(new List<object> {" "," ",
                                                lstDetail[loop].DM_DOITUONG_GIA_THITRUONG== null || lstDetail[loop].DM_DOITUONG_GIA_THITRUONG.Name==null?" ": lstDetail[loop].DM_DOITUONG_GIA_THITRUONG.Name.ToString()
                                                , lstDetail[loop].GIA_THANH_TU==null?" ":lstDetail[loop].GIA_THANH_TU.ToString()
                                                , lstDetail[loop].GIA_THANH_DEN==null?" ":lstDetail[loop].GIA_THANH_DEN.ToString()
                                              , lstDetail[loop].GIA_GIONG_TU==null?" ": lstDetail[loop].GIA_GIONG_TU.ToString()
                                              , lstDetail[loop].GIA_GIONG_DEN==null?" ":lstDetail[loop].GIA_GIONG_DEN.ToString()
                                               , lstDetail[loop].GIA_BAN_TU==null?" ":lstDetail[loop].GIA_BAN_TU.ToString()
                                                , lstDetail[loop].GIA_GIONG_DEN==null?" ":lstDetail[loop].GIA_GIONG_DEN.ToString()
                                              , lstDetail[loop].GIA_THUCAN_TU==null?" ":lstDetail[loop].GIA_THUCAN_TU.ToString()   
                                              , lstDetail[loop].GIA_THUCAN_DEN==null?" ":lstDetail[loop].GIA_THUCAN_DEN.ToString()                                         
                                              , lstDetail[loop].KICHCO==null?" ":lstDetail[loop].KICHCO.ToString()
                                              , lstDetail[loop].NOITHUTHAP==null?" ":lstDetail[loop].NOITHUTHAP.ToString()                                             
                                              , lstDetail[loop].NGAY_THUTHAP==null?" ":lstDetail[loop].NGAY_THUTHAP.Value.ToShortDateString()                                               
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

        public ActionResult ExportExcel(ViewModelSearchNT_TT_THITRUONG SearchModel)
        {

            try
            {

                using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_NAME, GENERATED_FILE_NAME))
                {
                    helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet1";

                    helper.CurrentPosition = new CellRef("A5");
                    helper.InsertRange("header_1");

                    helper.CurrentPosition = new CellRef("A6");
                    helper.InsertRange("header_2");

                    CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "stt_lama", "tinhthanhpho", "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "c10", "c11", "c12" });
                   // CellRangeTemplate row_coso = helper.CreateCellRangeTemplate("row_coso", new List<string> { "stt_num", "tencoso", "c13", "c14", "c15", "c16", "c17", "c18", "c19", "c20", "c21", "c22", "c23", "c24" });
                    CellRangeTemplate row_11 = helper.CreateCellRangeTemplate("row_11", new List<string> { "str", "c_23", "c_24", "c_25", "c_26", "c_27", "c_28", "c_29", "c_30", "c_31", "c_32", "c_33", "c_34", "c_35" });

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
                        IEnumerable<List<object>> _lstCoSo = this.gettt_thitruongByTinhTP(_lstMA_TTP[i], _lstCoSoID);
                        for (int j = 0; j < _lstCoSo.Count(); j++)
                        {
                           // helper.CurrentPosition = new CellRef("A" + (k).ToString());
                            //helper.InsertRange(row_coso, _lstCoSo.ToArray()[j]);
                            //k = k + 1;

                            //Insert co so detail
                            helper.CurrentPosition = new CellRef("A" + (k).ToString());
                            IEnumerable<List<object>> lstDetail = this.getDetailtt_thitruong(SearchModel,_lstCoSoID[j]);
                            helper.InsertRange(row_11, lstDetail);
                            k = k + lstDetail.Count();
                        }
                       
                    }
                    helper.DeleteSheet("Sheet3");
                    helper.CurrentSheetName = "Sheet1";

                }


                ViewBag.MSG_EXPORT = "Xuất file Excel thành công!";
               // return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "NT_TT_THITRUONGExcel" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
            }
            catch (Exception ex)
            {

                ViewBag.MSG_EXPORT = "Xuất file Excel không thành công!";
            }

            return RedirectToAction("Index");
        }
        #endregion

    }
}
