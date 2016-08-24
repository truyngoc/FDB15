using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;
using PagedList;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

using Aron.Sinoai.OfficeHelper;
using System.Data;
using Excel;
using System.Text;
using System.IO;
//using System.Data;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNet.Identity;
using FDB.Helpers;

namespace FDB.Controllers.KhaiThac
{
    public class KT_DONGSUA_TAUTHUYENController : FDBController
    {
        private FDBContext _context = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }


        //
        // GET: /DongSua TauThuyen/   
        public ActionResult Index(ViewModelSearchKT_DONGSUA_TAUTHUYEN Searchmodel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                    .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
            ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.DMPhuongXa = new SelectList(_context.DPHUONGXA, "MA_PHUONGXA", "TEN_PHUONGXA");
            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            if (Searchmodel.SearchButton == "Xuất Excel")
            {
                CreateExcelFile(Searchmodel);
                //Dowload file:
                return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "exportKT_DONGSUA_TAUTHUYEN" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
            }
            else
            {
                //Chưa làm Search không dấu, sẽ tìm hiểu và sửa sau:
                var models = _context.KT_DONGSUA_TAUTHUYEN.Where(o => (Searchmodel.TenCoSo == null || o.TEN_COSO.ToUpper().Contains(Searchmodel.TenCoSo.ToUpper()))
                                                            && (Searchmodel.TenChuCoSo == null || o.TEN_CHU_COSO.ToUpper().Contains(Searchmodel.TenChuCoSo.ToUpper()))
                                                            && (Searchmodel.DiaChi == null || o.DIA_CHI.ToUpper().Contains(Searchmodel.DiaChi.ToUpper()))
                                                             && ((string.IsNullOrEmpty(Searchmodel.TThanhPho) && string.IsNullOrEmpty(curUser.MA_TINHTP)) || (string.IsNullOrEmpty(Searchmodel.TThanhPho) && o.MA_TINHTP == curUser.MA_TINHTP) || o.MA_TINHTP == Searchmodel.TThanhPho)
                                                )
                                                ;

                // string sqlsanLuongs = ((ObjectQuery)sanLuongs).ToTraceString();
                ViewBag.TotalRow = models.Count().ToString();
                models = models.OrderBy(m => m.ID);
                //Phân trang ở đây:
                var pageIndex = Searchmodel.Page ?? 1;
                Searchmodel.SearchResults = models.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

                return View(Searchmodel);
            }

        }

        public ActionResult Search(ViewModelSearchKT_DONGSUA_TAUTHUYEN Searchmodel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP;//.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
            //            .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.DMPhuongXa = new SelectList(_context.DPHUONGXA, "MA_PHUONGXA", "TEN_PHUONGXA");

            if (Searchmodel.SearchButton == "Xuất Excel")
            {
                CreateExcelFile(Searchmodel);
                //Dowload file:
                return File(GENERATED_FILE_NAME, "application/vnd.ms-excel", "exportKT_DONGSUA_TAUTHUYEN" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
            }
            else
            {
                //Chưa làm Search không dấu, sẽ tìm hiểu và sửa sau:
                var models = _context.KT_DONGSUA_TAUTHUYEN.Where(o => (Searchmodel.TenCoSo == null || o.TEN_COSO.ToUpper().Contains(Searchmodel.TenCoSo.ToUpper()))
                                                            && (Searchmodel.TenChuCoSo == null || o.TEN_CHU_COSO.ToUpper().Contains(Searchmodel.TenChuCoSo.ToUpper()))
                                                            && (Searchmodel.DiaChi == null || o.DIA_CHI.ToUpper().Contains(Searchmodel.DiaChi.ToUpper()))
                                                            && (string.IsNullOrEmpty(Searchmodel.TThanhPho) || o.MA_TINHTP == Searchmodel.TThanhPho)
                                                )
                                                ;

                // string sqlsanLuongs = ((ObjectQuery)sanLuongs).ToTraceString();
                ViewBag.TotalRow = models.Count().ToString();
                models = models.OrderBy(m => m.ID);
                //Phân trang ở đây:
                var pageIndex = Searchmodel.Page ?? 1;
                Searchmodel.SearchResults = models.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

                return View(Searchmodel);
            }
        }

        public ActionResult Create()
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                                        .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
            var quan = _context.DQUANHUYEN.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP);
            //   quan.Insert(0, new DQUANHUYEN());
            ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");
            ViewBag.DMPhuongXa = new SelectList(_context.DPHUONGXA, "MA_PHUONGXA", "TEN_PHUONGXA");

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection _form, KT_DONGSUA_TAUTHUYEN _obj)
        {
            List<string> lstKeytxtNAM = _form.AllKeys.ToList().Where(s => s.StartsWith("txtNAM_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeytxtNAM.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            //var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key };
            //var a = ModelState.IsValidField("TRONG_TAI_TOIDA_COTHE");
            //FDB.Common.Helpers.GetValueForm<KT_DONGSUA_TAUTHUYEN>(_form, ref _obj, "TRONG_TAI_TOIDA_COTHE");

            if (ModelState.IsValid)//|| (!ModelState.IsValid && errors.ToList().Count==1 && a == false))
            {
                _obj.NGUOI_NHAP = User.Identity.Name;
                _obj.NGAY_NHAP = DateTime.Now;
                //Save Header
                _context.KT_DONGSUA_TAUTHUYEN.Add(_obj);
                _context.SaveChanges();

                int Id = _obj.ID;

                if (lstKeytxtNAM != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        KT_DONGSUA_TAUTHUYEN_DETAIL _objDetail = new Models.KT_DONGSUA_TAUTHUYEN_DETAIL();
                        FDB.Common.Helpers.GetValueForm<KT_DONGSUA_TAUTHUYEN_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_DONGSUA_TAUTHUYEN = Id;
                        _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Add(_objDetail);
                    }
                    //Save detail:
                    _context.SaveChanges();
                }
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Đóng sửa tàu thuyền"));
                return RedirectToAction("Index");
            }
            else
            {
                ApplicationUser curUser = this.getCurrentUser();
                var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                                            .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
                ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");
                var quan = _context.DQUANHUYEN.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP);
                //   quan.Insert(0, new DQUANHUYEN());
                ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");
                ViewBag.DMPhuongXa = new SelectList(_context.DPHUONGXA, "MA_PHUONGXA", "TEN_PHUONGXA");

                ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

                TempData["_SUCCESS"] = "";
                //build html :
                int maxID = 0;
                String strHTML = this.GenderHTML(lstInt, _form, ref maxID);
                ViewBag.AddHTML = strHTML;
                ViewBag.sMaxID = maxID + 1;
                return View(_obj);
            }
        }

        [EncryptedActionParameter]
        public ActionResult Edit(String questionId = "0")
        {
            int _ID = int.Parse(questionId);
            var model = _context.KT_DONGSUA_TAUTHUYEN.Find(_ID);
            if (model == null)
            {
                return HttpNotFound();
            }
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                                .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");


            var quan = _context.DQUANHUYEN.Where(u => u.MA_TINHTP == model.MA_TINHTP);
            //   quan.Insert(0, new DQUANHUYEN());
            ViewBag.DMQhuyen = new SelectList(quan, "MA_QUANHUYEN", "TEN_QUANHUYEN");


            var phuongxa = _context.DPHUONGXA.Where(u => model.MA_QUANHUYEN == null || u.MA_QUANHUYEN == model.MA_QUANHUYEN);
            ViewBag.DMPhuongXa = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");


            return View(model);
        }

        [HttpPost]
        //async Task<ActionResult>
        public ActionResult Edit(FormCollection _form, KT_DONGSUA_TAUTHUYEN _obj)
        {
            List<string> lstKeytxtNAM = _form.AllKeys.ToList().Where(s => s.StartsWith("txtNAM_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeytxtNAM.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            //var errors = from modelstate in ModelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select new { Title = modelstate.Key };
            //var a = ModelState.IsValidField("TRONG_TAI_TOIDA_COTHE");
            //FDB.Common.Helpers.GetValueForm<KT_DONGSUA_TAUTHUYEN>(_form, ref _obj, "TRONG_TAI_TOIDA_COTHE");

            if (ModelState.IsValid )//|| (!ModelState.IsValid && errors.ToList().Count==1 && a == false))
            {
                var model = _context.KT_DONGSUA_TAUTHUYEN.First(o => o.ID == _obj.ID);
                FDB.Common.Helpers.CopyObject<KT_DONGSUA_TAUTHUYEN>(_obj, ref model);
                model.NGUOI_NHAP = User.Identity.Name;
                model.NGAY_NHAP = DateTime.Now;
                var dbEntityEntry = _context.Entry(model);

                _context.KT_DONGSUA_TAUTHUYEN.Attach(model);
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //Xóa những detail cũ:
                _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Where(o => o.ID_DONGSUA_TAUTHUYEN == _obj.ID).ToList().ForEach(o => _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Remove(o));

                //Thêm mới detail đã sửa
                int Id = _obj.ID;

                if (lstKeytxtNAM != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        KT_DONGSUA_TAUTHUYEN_DETAIL _objDetail = new Models.KT_DONGSUA_TAUTHUYEN_DETAIL();
                        FDB.Common.Helpers.GetValueForm<KT_DONGSUA_TAUTHUYEN_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_DONGSUA_TAUTHUYEN = Id;
                        _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Add(_objDetail);
                    }

                }
                //Save data:
                _context.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Đóng sửa tàu thuyền"));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["_SUCCESS"] = "";
                ApplicationUser curUser = this.getCurrentUser();
                var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                                    .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
                ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");


                var quan = _context.DQUANHUYEN.Where(u => u.MA_TINHTP == _obj.MA_TINHTP);
                //   quan.Insert(0, new DQUANHUYEN());
                ViewBag.DMQhuyen = new SelectList(quan, "MA_QUANHUYEN", "TEN_QUANHUYEN");


                var phuongxa = _context.DPHUONGXA.Where(u => _obj.MA_QUANHUYEN == null || u.MA_QUANHUYEN == _obj.MA_QUANHUYEN);
                ViewBag.DMPhuongXa = new SelectList(phuongxa, "MA_PHUONGXA", "TEN_PHUONGXA");

                _obj.DSDongSuaTauThuyenDetail = new List<KT_DONGSUA_TAUTHUYEN_DETAIL>();
                //build html :
                int maxID = 0;
                String strHTML = this.GenderHTML(lstInt, _form, ref maxID);
                ViewBag.sEditHTML = strHTML;
                ViewBag.MaxID = maxID + 1;
                return View(_obj);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            //Xóa header
            KT_DONGSUA_TAUTHUYEN _obj = _context.KT_DONGSUA_TAUTHUYEN.Find(id);
            _context.KT_DONGSUA_TAUTHUYEN.Remove(_obj);
            _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Where(o => o.ID_DONGSUA_TAUTHUYEN == _obj.ID).ToList().ForEach(o => _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Remove(o));
            //Update thay đổi vào DB
            _context.SaveChanges();
            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Đóng sửa tàu thuyền"));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult getDistrict(string ma_TinhTP)
        {
            var districts = _context.DQUANHUYEN.Where(d => d.MA_TINHTP == ma_TinhTP).Select(a => "<option value='" + a.MA_QUANHUYEN + "'>" + a.TEN_QUANHUYEN + "</option>");

            return Content(string.Join("", districts));
        }

        [HttpPost]
        public ActionResult getWard(string ma_QuanHuyen)
        {
            var districts = _context.DPHUONGXA.Where(d => d.MA_QUANHUYEN == ma_QuanHuyen).Select(a => "<option value='" + a.MA_PHUONGXA + "'>" + a.TEN_PHUONGXA + "</option>");

            return Content(string.Join("", districts));
        }

        #region "Function"
        //Nếu dùng Jquery Validate thì hàm này có thể bỏ
        private String GenderHTML(List<int> lstInt, FormCollection _form, ref int MaxID)
        {
            String strHTML = "";
            int _maxID = 0;
            if (lstInt != null && lstInt.Count > 0)
            {

                for (int i = 0; i < lstInt.Count; i++)
                {

                    strHTML += "<tr>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtDONGMOI_VOGO_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDONGMOI_VOGO_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtDONGMOI_VOTHEP_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDONGMOI_VOTHEP_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtDONGMOI_VOCOMPOSITE_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDONGMOI_VOCOMPOSITE_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"text\" value=\"" + _form["txtTONG_TAITRONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtTONG_TAITRONG_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtSUA_CHUA_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSUA_CHUA_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtGIAI_BAN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtGIAI_BAN_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"0\" value=\"" + _form["txtBAN_TINHKHAC_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtBAN_TINHKHAC_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><input type=\"number\" min=\"1900\" value=\"" + _form["txtNAM_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNAM_" + lstInt[i].ToString() + "\"/></td>";
                    strHTML += "<td><label class=\"BtnPlus\" style=\"cursor:pointer\"><img src=\"/fonts/button-add-icon.png\" title=\"Thêm mới\" /></label>";
                    strHTML += "<label class=\"BtnMinus\" id=\"lblDetail_\"" + (i + 1).ToString() + " style=\"cursor:pointer\"><img src=\"/fonts/DELETE.GIF\" title=\"Xóa\" /></label></td>";
                    strHTML += "</tr>";

                    if (lstInt[i] > _maxID)
                        _maxID = lstInt[i];
                }

            }
            MaxID = _maxID;
            return strHTML;
        }

        #endregion

        #region"Working Files"
        private static String GENERATED_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_generated_1.xlsx");
        private static String TEMPLATE_FILE_NAME = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_CoSoDongSuaTauThuyen_Template.xlsx");

        //get tinh thanh pho 
        private IEnumerable<List<object>> getTinhThanhPho(ViewModelSearchKT_DONGSUA_TAUTHUYEN Searchmodel, List<String> lstMA_TTP)
        {
            ApplicationUser curUser = this.getCurrentUser();

            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstMA_TTP == null)
                lstMA_TTP = new List<String>();

            var selects = _context.KT_DONGSUA_TAUTHUYEN.Where(o => (Searchmodel.TenCoSo == null || o.TEN_COSO.ToUpper().Contains(Searchmodel.TenCoSo.ToUpper()))
                                                      && (Searchmodel.TenChuCoSo == null || o.TEN_CHU_COSO.ToUpper().Contains(Searchmodel.TenChuCoSo.ToUpper()))
                                                      && (Searchmodel.DiaChi == null || o.DIA_CHI.ToUpper().Contains(Searchmodel.DiaChi.ToUpper()))
                                                       && (string.IsNullOrEmpty(Searchmodel.TThanhPho) || o.MA_TINHTP == Searchmodel.TThanhPho)
                                            ).Select(x => new { MA_TTP = x.MA_TINHTP, TEN_TTP = x.DTinhTP.TEN_TINHTP }).Distinct();

            var models = selects.ToList();
            //  var models = _context.KT_DONGSUA_TAUTHUYEN;
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { this.NumberToRoman(loop + 1),models[loop].TEN_TTP
                                                            , " ", " ", " ", " ", " ", " " }
                                                          );
                lstMA_TTP.Add(models[loop].MA_TTP);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }
        //get co so by tinh tp
        private IEnumerable<List<object>> getCoSoByTinhTP(String MA_TTP, List<int> lstID)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();
            if (lstID == null)
                lstID = new List<int>();

            var selects = _context.KT_DONGSUA_TAUTHUYEN.Where(o => o.MA_TINHTP == MA_TTP).Select(x => new { ID = x.ID, TEN_COSO = x.TEN_COSO });

            var models = selects.ToList();
            //  var models = _context.KT_DONGSUA_TAUTHUYEN;
            for (int loop = 0; loop < models.Count(); loop++)
            {
                lstobj.Add(new List<object> { (loop + 1).ToString(),models[loop].TEN_COSO
                                                            , " ", " ", " ", " ", " ", " " }
                                                          );
                lstID.Add(models[loop].ID);
            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }
        //get detail by co so
        private IEnumerable<List<object>> getDetailCoSo(int DONGSUA_TAUTHUYEN)
        {
            IEnumerable<List<object>> items = Enumerable.Empty<List<object>>();
            List<List<object>> lstobj = new List<List<object>>();

            var selects = _context.KT_DONGSUA_TAUTHUYEN_DETAIL.Where(o => o.ID_DONGSUA_TAUTHUYEN == DONGSUA_TAUTHUYEN);

            var lstDetail = selects.ToList();
            //  var models = _context.KT_DONGSUA_TAUTHUYEN;
            for (int loop = 0; loop < lstDetail.Count(); loop++)
            {
                lstobj.Add(new List<object> {" "," ", lstDetail[loop].DONGMOI_VOGO==null?" ":lstDetail[loop].DONGMOI_VOGO.ToString()
                                              , lstDetail[loop].DONGMOI_VOTHEP==null?" ": lstDetail[loop].DONGMOI_VOTHEP.ToString()
                                              , lstDetail[loop].DONGMOI_VOCOMPOSITE==null?" ":lstDetail[loop].DONGMOI_VOCOMPOSITE.ToString()
                                              , lstDetail[loop].TONG_TAITRONG==null?" ":lstDetail[loop].TONG_TAITRONG.ToString()
                                              , lstDetail[loop].SUA_CHUA==null?" ":lstDetail[loop].SUA_CHUA.ToString()
                                              , lstDetail[loop].NAM==null?" ":lstDetail[loop].NAM.ToString()
                                            });

            }
            items = lstobj.AsEnumerable<List<object>>();
            return items;
        }

        private void CreateExcelFile(ViewModelSearchKT_DONGSUA_TAUTHUYEN Searchmodel)
        {

            using (ExcelHelper helper = new ExcelHelper(TEMPLATE_FILE_NAME, GENERATED_FILE_NAME))
            {
                helper.Direction = ExcelHelper.DirectionType.TOP_TO_DOWN;
                helper.CurrentSheetName = "Sheet1";

                helper.CurrentPosition = new CellRef("A5");
                helper.InsertRange("header_1");

                helper.CurrentPosition = new CellRef("A6");
                helper.InsertRange("header_2");

                CellRangeTemplate row_tinhthanhpho = helper.CreateCellRangeTemplate("row_tinhthanhpho", new List<string> { "stt_lama", "tinhthanhpho", "c1", "c2", "c3", "c4", "c5", "c6" });
                CellRangeTemplate row_coso = helper.CreateCellRangeTemplate("row_coso", new List<string> { "stt_num", "tencoso", "c7", "c8", "c9", "c10", "c11", "c12" });
                CellRangeTemplate row_11 = helper.CreateCellRangeTemplate("row_11", new List<string> { "str", "c_11", "c_12", "c_13", "c_14", "c_15", "c_16", "c_17" });

                int k = 7;
                List<String> _lstMA_TTP = new List<string>();

                IEnumerable<List<object>> _lstTinhTP = this.getTinhThanhPho(Searchmodel, _lstMA_TTP);

                for (int i = 0; i < _lstTinhTP.Count(); i++)
                {

                    //insert Tinh thanh pho
                    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    helper.InsertRange(row_tinhthanhpho, _lstTinhTP.ToArray()[i]);
                    k = k + 1;
                    //insert Co so
                    List<int> _lstCoSoID = new List<int>();
                    IEnumerable<List<object>> _lstCoSo = this.getCoSoByTinhTP(_lstMA_TTP[i], _lstCoSoID);
                    for (int j = 0; j < _lstCoSo.Count(); j++)
                    {
                        helper.CurrentPosition = new CellRef("A" + (k).ToString());
                        helper.InsertRange(row_coso, _lstCoSo.ToArray()[j]);
                        k = k + 1;

                        //Insert co so detail
                        helper.CurrentPosition = new CellRef("A" + (k).ToString());
                        IEnumerable<List<object>> lstDetail = this.getDetailCoSo(_lstCoSoID[j]);
                        helper.InsertRange(row_11, lstDetail);
                        k = k + lstDetail.Count();
                    }
                    helper.CurrentPosition = new CellRef("A" + (k).ToString());
                    helper.InsertRange("row_empty");
                    k = k + 1;
                }
                helper.DeleteSheet("Sheet3");
                helper.CurrentSheetName = "Sheet1";

            }
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
        #endregion

        #region"Import Excel"
        public ActionResult ImportExcel()
        {
            //initialCategoryCreateAction();
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase fileUpload)
        {
            //// StringBuilder strValidations = new StringBuilder(string.Empty);
            //StringBuilder strValidationError = new StringBuilder(string.Empty);
            //if (fileUpload != null && fileUpload.ContentLength > 0)
            //{
            //    string extension = Path.GetExtension(fileUpload.FileName);
            //    if (extension.StartsWith(".xls") || extension.StartsWith(".xlsx"))
            //    {
            //        string _filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/" + Path.GetFileNameWithoutExtension(fileUpload.FileName) + DateTime.Now.ToString("ddMMyyyyhhMMss") + extension);
            //        fileUpload.SaveAs(_filePath);
            //        DataTable dtData = FDB.Common.Helpers.ConvertExcelToDataTable(_filePath);
            //        if (dtData != null && dtData.Rows.Count > 0)
            //        {
            //            //FDB.Common.Helpers.RenameDataColumn(ref dtData, MapPropertyNameToText);
            //            FDB.Common.Helpers.RenameDataColumn(ref dtData, FDB.Common.Helpers.MapPropertyNameToText, this._mapPropertyNameToText);

            //            if (dtData.Columns.Contains("SO_DK"))
            //            {
            //                int k = 3;

            //                for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
            //                {
            //                    DataRow r = dtData.Rows[rowIndex];
            //                    for (int i = 0; i < dtData.Columns.Count; i++)
            //                    {
            //                        switch (dtData.Columns[i].ColumnName)
            //                        {
            //                            case "SO_DK":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Số ĐK tại dòng " + (rowIndex + 2).ToString() + " không được để trống! </span> <br/>");
            //                                }
            //                                else
            //                                {
            //                                    //check số ĐK tồn tại
            //                                    string _SO_DK = r[i].ToString();
            //                                    bool isExists = _context.KT_TAUTHUYEN.Any(o => o.SO_DK == _SO_DK);
            //                                    if (isExists)
            //                                        strValidationError.Append("<span style=\"color:red\">Cột Số ĐK =" + _SO_DK + " tại dòng " + (rowIndex + 2).ToString() + " Đã tồn tại! </span> <br/>");
            //                                }
            //                                break;
            //                            case "NGAY_DK":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Ngày ĐK tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "CHU_PHUONG_TIEN":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Chủ phương tiện tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "MA_TINHTP":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Mã tỉnh tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "MA_QUANHUYEN":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Mã huyện tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "MA_PHUONGXA":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Mã xã tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "DIA_CHI":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Địa chỉ tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "DCONG_DUNG_TAUID":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Công dụng tàu tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "DNHOM_TAUID":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Nhóm tàu tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "DTINH_TRANG_TAUID":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Tình trạng tàu tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_CHIEU_DAI":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Chiều dài tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_CHIEU_RONG":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Chiều rộng tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_CHIEU_CAO":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Chiều cao tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_MAN_KHO":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Mạn khô tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_MON_NUOC":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Mớn nước tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "KT_TAN_DANG_KY":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Tấn ĐK tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;
            //                            case "DVAT_LIEU_VOID":
            //                                if (r[i].Equals(DBNull.Value))
            //                                {
            //                                    strValidationError.Append("<span style=\"color:red\">Cột Vật liệu vỏ tại dòng " + (rowIndex + 2).ToString() + " không được để trống!</span> <br/>");
            //                                }
            //                                break;

            //                        }
            //                    }

            //                    //KT_TAUTHUYEN _kt = this.MapDataRowToObject<KT_TAUTHUYEN>(r);
            //                    //db.KT_TAUTHUYEN.Add(_kt);

            //                    k++;
            //                }

            //                if (strValidationError != null && strValidationError.Length > 0 && !strValidationError.Equals(string.Empty))
            //                {
            //                    ViewBag.ThongBao = strValidationError.ToString();
            //                    ViewBag.success = "0";

            //                }
            //                else
            //                {
            //                    string _count = (dtData.Rows.Count).ToString();
            //                    int i = 0;
            //                    foreach (DataRow r in dtData.Rows)
            //                    {
            //                        string _SO_DK = r["SO_DK"].ToString();
            //                        bool isExists = _context.KT_TAUTHUYEN.Any(o => o.SO_DK == _SO_DK);
            //                        if (!isExists)
            //                        {
            //                            KT_TAUTHUYEN _kt = FDB.Common.Helpers.MapDataRowToObject<KT_TAUTHUYEN>(r);
            //                            _context.KT_TAUTHUYEN.Add(_kt);
            //                            i++;
            //                        }
            //                    }
            //                    _context.SaveChanges();

            //                    strValidationError.Clear();
            //                    strValidationError.Append("<span style=\"color:blue\">Import file thành công " + i.ToString() + "/" + _count + " bản ghi!</span><br/>");
            //                    ViewBag.ThongBao = strValidationError.ToString();
            //                    ViewBag.success = "1";
            //                }
            //            }
            //            else
            //            {

            //                strValidationError.Clear();
            //                strValidationError.Append("<span style=\"color:red\">Mẫu file không đúng, vui lòng tải file mẫu excel!</span><br/>");
            //                ViewBag.ThongBao = strValidationError.ToString();

            //            }
            //        }
            //        else
            //        {
            //            strValidationError.Clear();
            //            strValidationError.Append("<span style=\"color:red\">File không có dữ liệu!</span><br/>");
            //            ViewBag.ThongBao = strValidationError.ToString();

            //        }
            //        if (System.IO.File.Exists(_filePath))
            //        {
            //            System.IO.File.Delete(_filePath);
            //        }
            //    }
            //    else
            //    {

            //    }

            //}
            //else
            //{

            //}

            return View();
        }

        public FileResult DownLoadFile()
        {
            String FullPathFileName = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Templates/FDB_Template_Import_DKTT.xlsx");
            string fileName = "FDB_Template_Import_DKTT.xls";
            string contentType = "application/vnd.ms-excel";
            string extension = new FileInfo(FullPathFileName).Extension;
            switch (extension)
            {
                case ".xls":
                    fileName = "FDB_Template_Import_DKTT.xls";
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    fileName = "FDB_Template_Import_DKTT.xlsx";
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }
            return File(FullPathFileName, contentType, fileName);
        }

        private Dictionary<string, string> _mapPropertyNameToText = new Dictionary<string, string>()
        {
                {"Số ĐK", "SO_DK"},
                {"Ngày ĐK", "NGAY_DK"},
                {"Số sổ ĐKTCQG", "SO_DKTCQG"},
                {"Ngày vào sổ", "NGAY_DKTCQG"},
                {"Chủ phương tiện", "CHU_PHUONG_TIEN"},
                {"Số CMND", "SO_CMND"},
                {"Điện thoại", "DIEN_THOAI"},
                {"Mã tỉnh", "MA_TINHTP"},
                {"Mã huyện", "MA_QUANHUYEN"},
                {"Mã xã", "MA_PHUONGXA"},
                {"Địa chỉ", "DIA_CHI"},
                {"Tên tàu", "TEN_TAU"},
                {"Hô hiệu", "HO_HIEU"},
                {"Công dụng tàu", "DCONG_DUNG_TAUID"},
                {"Nhóm tàu", "DNHOM_TAUID"},
                {"Số DK cũ", "SO_DK_CU"},
                {"Vùng hoạt động", "VUNG_HOAT_DONG"},
                {"Số thuyền viên", "SO_THUYEN_VIEN"},
                {"Tình trạng tàu", "DTINH_TRANG_TAUID"},
                {"Nơi kiểm tra", "NOI_KIEM_TRA"},
                {"Ngày kiểm tra", "NGAY_KIEM_TRA"},
                {"Loại kiểm tra", "DLOAI_KIEM_TRA_KTID"},
                {"Số biên bản KT", "SO_BB_KIEM_TRA_KT"},
                {"Lần đăng kiểm", "DTTDANG_KIEMID"},
                {"Tình trạng đăng kiểm", "DTINH_TRANG_DANG_KIEMID"},
                {"Cơ quan đăng kiểm", "CO_QUAN_DANG_KIEM"},
                {"Số sổ đăng kiểm", "SO_SO_DANG_KIEM"},
                {"Ngày cấp sổ đăng kiểm", "NGAY_CAP_SO_DANG_KIEM"},
                {"Hạn đăng kiểm", "HAN_DANG_KIEM"},
                {"Đăng kiểm viên", "DANG_KIEM_VIEN"},
                {"Số giấy VSTP", "SO_GIAY_VSTP"},
                {"Ngày cấp VSTP", "NGAY_CAP_GIAY_VSTP"},
                {"Số ấn chỉ ATKT", "SO_AN_CHI_ATKT"},
                {"Chiều dài", "KT_CHIEU_DAI"},
                {"Chiều rộng", "KT_CHIEU_RONG"},
                {"Chiều cao", "KT_CHIEU_CAO"},
                {"Mạn khô", "KT_MAN_KHO"},
                {"Mớn nước", "KT_MON_NUOC"},
                {"Tấn ĐK", "KT_TAN_DANG_KY"},
                {"Vật liệu vỏ", "DVAT_LIEU_VOID"},
                {"Tổng công suất", "KT_TONG_CONG_SUAT"},
                {"Tốc độ", "KT_TOC_DO_TAU"},
                {"Nơi đóng", "KT_NOI_DONG"},
                {"Năm đóng", "KT_NAM_DONG"},
                {"Tên người đóng", "KT_TEN_NGUOI_DONG"},
                {"Mẫu thiết kế", "KT_MAU_THIET_KE"},
                {"Cơ quan TK", "KT_CQ_THIET_KE"},
                {"Phiếu duyệt TK", "KT_SO_PD_TK"},
                {"Ngày duyệt TK", "KT_NGAY_PD_TK"},
                {"Tên TK", "KT_TEN_THIET_KE"},
                {"Loại TK", "KT_LOAI_THIET_KE"},
                {"Năm TK", "KT_NAM_THIET_KE"},
                {"Kí hiệu máy", "M1_KY_HIEU_MAY"},
                {"Số máy", "M1_SO_MAY"},
                {"Nơi sX", "M1_NOI_SX"},
                {"Năm chế tạo", "M1_NAM_CHE_TAO"},
                {"Hãng Máy", "M1_HANG_MAY"},
                {"Vòng quay", "M1_VONG_QUAY"},
                {"Công suất", "M1_CONG_SUAT"},
                {"Giấy phép", "Name"}
        };
        public string ValidateDataTable(DataTable dt)
        {
            // InitMapPropertyNameToText();
            string sWarining = "";
            //For Each s As String In _mapPropertyNameToText.Keys
            //    If dt.Columns.IndexOf(s) < 0 Then sWarining += String.Format("Thiếu cột {0} <br/>", s)
            //Next
            return sWarining;
        }


        #endregion

    }
}