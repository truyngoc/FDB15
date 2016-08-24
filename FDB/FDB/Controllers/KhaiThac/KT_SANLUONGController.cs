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
using System.Net;
using Microsoft.AspNet.Identity;
using FDB.Helpers;

namespace FDB.Controllers
{
    public class KT_SANLUONGController : FDBController
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
        // GET: /SanLuong/   
        public ActionResult Index(ViewModelSearchKT_SANLUONG Searchmodel)
        {
            // var qry = 

            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                        .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", Searchmodel.DonVi);

            //var nhomdoituongKT = FDB.Common.CategoryCommon.lstDMDoiTuongCPUE;
            var nhomdoituongKT = _context.DM_NHOMDOITUONG_KT;
           // ViewBag.DMNhomDoiTuongKT = new SelectList(nhomdoituongKT, "ID", "NAME", Searchmodel.NhomDoiTuongKhaiThac);
            ViewBag.DMNhomDoiTuongKT = new SelectList(nhomdoituongKT, "ID", "TEN_NHOM", Searchmodel.NhomDoiTuongKhaiThac);

            ViewBag.DMNgheKhaiThac = new SelectList(_context.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe", Searchmodel.NgheKhaiThac);
            ViewBag.DMLoaiCongSuat = new SelectList(_context.DNHOM_TAU, "ID", "NAME", Searchmodel.LoaiCongSuat);
            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            var sanLuongs = _context.KT_SANLUONG.GroupJoin(
                                      _context.KT_SANLUONG_DETAIL,
                                      H => H.ID,
                                      D => D.ID_KHAITHAC_SANLUONG,
                                      (x, y) => new { H = x, D = y })
                                .SelectMany(
                                      x => x.D.DefaultIfEmpty(),
                                      (x, y) => new { H = x.H, D = y }
                                ).Where(o => ((string.IsNullOrEmpty(Searchmodel.DonVi) && curUser.MA_TINHTP.StartsWith("Z")) || (string.IsNullOrEmpty(Searchmodel.DonVi) && o.H.MA_TINHTP == curUser.MA_TINHTP) || o.H.MA_TINHTP == Searchmodel.DonVi)
                                                        && (Searchmodel.Thang == null || o.H.THANG == Searchmodel.Thang)
                                                        && (Searchmodel.Nam == null || o.H.NAM == Searchmodel.Nam)
                                                        && (Searchmodel.NhomDoiTuongKhaiThac == null || o.D.ID_KHAITHAC_NHOM_DOITUONG == Searchmodel.NhomDoiTuongKhaiThac)
                                                        && (Searchmodel.NgheKhaiThac == null || o.D.ID_KHAITHAC_NHOM_NGHE == Searchmodel.NgheKhaiThac)
                                                        && (Searchmodel.LoaiCongSuat == null || o.D.ID_KHAITHAC_NHOM_CONGSUAT == Searchmodel.LoaiCongSuat)
                                            ).Select(x => new { x.H.ID, MA_DV = x.H.MA_TINHTP, x.H.NAM, x.H.THANG, x.H.NGAY_NHAP, x.H.NGUOI_NHAP, x.H.DTinhTP })
                                            .Take(200).OrderByDescending(o => o.ID).Distinct();

            ;
            // string sqlsanLuongs = ((ObjectQuery)sanLuongs).ToTraceString();

            List<KT_SANLUONG> DSKTSanLuong = new List<KT_SANLUONG>();
            foreach (var sanLuong in sanLuongs)
            {
                //if (!DSKTSanLuong.Any(o => o.ID == sanLuong.ID))
                //{
                DSKTSanLuong.Add(new KT_SANLUONG
                {
                    ID = sanLuong.ID,
                    MA_TINHTP = sanLuong.MA_DV,
                    NAM = sanLuong.NAM,
                    THANG = sanLuong.THANG,
                    NGUOI_NHAP = sanLuong.NGUOI_NHAP,
                    NGAY_NHAP = sanLuong.NGAY_NHAP,
                    DTinhTP = sanLuong.DTinhTP

                });
                // }

            }

            ViewBag.TotalRow = DSKTSanLuong.Count().ToString();
            //Phân trang ở đây:
            var pageIndex = Searchmodel.Page ?? 1;
            Searchmodel.SearchResults = DSKTSanLuong.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(Searchmodel);
        }

        public ActionResult Search(ViewModelSearchKT_SANLUONG_TRUCTIEP search)
        {
            string strTitle = "";
            ApplicationUser curUser = this.getCurrentUser();
            initialCategorySearchAction();

            if (search.Thang == null) search.Thang = DateTime.Today.Month;
            if (search.Nam == null) search.Nam = DateTime.Today.Year;
            search.TinhTP_User = curUser.MA_TINHTP;
            if (search.LoaiThongKe == 0) search.LoaiThongKe = 1;
            

            ThongKe_TrucTiep tk = new ThongKe_TrucTiep(search);

            search.San_Luong_Theo_Nghe = tk.SanLuong_TheoNghe;
            search.San_Luong_Theo_Nhom_Nghe = tk.SanLuong_TheoNhomNghe;
            search.San_Luong_Thang = tk.SanLuong_Thang;
            search.San_Luong_Theo_Loai = tk.SanLuong_TheoLoai;
            search.San_Luong_Theo_Nhom_Loai = tk.SanLuong_TheoNhomLoai;
            search.San_Luong_Loai_Thang = tk.SanLuong_Loai_Thang;


            strTitle = "Kết quả thống kê sản lượng thu thập trực tiếp Tháng " + (search.Thang < 10 ? "0" + search.Thang.ToString() : search.Thang.ToString()) + "/" + search.Nam.ToString() + " của " + (!string.IsNullOrEmpty(search.TinhTP) ? _context.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == search.TinhTP).TEN_TINHTP : "toàn quốc");
            ViewBag.strTitle = strTitle;


            return View(search);
        }

        // GET: /SanLuong/
        public ActionResult Create()
        {
            this.LoadDanhMuc();
            KT_SANLUONG model = new KT_SANLUONG();
            var dbQuanTri = new ApplicationDbContext();
            string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
            model.NGUOI_NHAP = User.Identity.Name;
            model.MA_TINHTP = Ma_TinhTP;
            model.NGAY_NHAP = DateTime.Now;
            model.THANG = DateTime.Now.Month;
            model.NAM = DateTime.Now.Year;
            //  ViewBag.AddHTML = this.InititalGetHTML();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection _form, KT_SANLUONG _obj)
        {

            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_KHAITHAC_NHOM_DOITUONG_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                //Save Header


                var dbQuanTri = new ApplicationDbContext();
                string Ma_TinhTP = dbQuanTri.Users.First(o => o.UserName == User.Identity.Name).MA_TINHTP;
                _obj.NGUOI_NHAP = User.Identity.Name;
                _obj.MA_TINHTP = Ma_TinhTP;
                _obj.NGAY_NHAP = DateTime.Now;

                _context.KT_SANLUONG.Add(_obj);
                _context.SaveChanges();

                int Id = _obj.ID;

                if (lstKeyName != null)
                {
                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        KT_SANLUONG_DETAIL _objDetail = new Models.KT_SANLUONG_DETAIL();

                        FDB.Common.Helpers.GetValueForm<KT_SANLUONG_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_KHAITHAC_SANLUONG = Id;

                        _context.KT_SANLUONG_DETAIL.Add(_objDetail);
                    }
                    //Save detail:
                    _context.SaveChanges();
                }
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Sản lượng"));
                return RedirectToAction("Index");
            }
            else
            {
                // TempData["_SUCCESS"] = "";
                this.LoadDanhMuc();
                //build html :
                int maxID = 0;
                //   String strHTML = this.GenderHTML(lstKeyddlDoiTuongNuoi, lstKeyddlNhomNgheKhaiThac, lstKeyddlLoaiKhaiThac, lstKeytxtSanLuong, _form, ref maxID);

                ViewBag.AddHTML = "";//strHTML;
                ViewBag.sMaxID = maxID + 1;
                return View(_obj);
            }
        }

        [EncryptedActionParameter]
        public ActionResult Edit(String questionId = "0")
        {
            if (string.IsNullOrEmpty(questionId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int _ID = int.Parse(questionId);
            var modelSanLuong = _context.KT_SANLUONG.Find(_ID);
            if (modelSanLuong == null)
                return HttpNotFound();
            this.LoadDanhMuc();

            return View(modelSanLuong);
        }

        [HttpPost]
        //async Task<ActionResult>
        public ActionResult Edit(FormCollection _form, KT_SANLUONG _obj)
        {


            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_KHAITHAC_NHOM_DOITUONG_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                var model = _context.KT_SANLUONG.First(o => o.ID == _obj.ID);
                FDB.Common.Helpers.CopyObject<KT_SANLUONG>(_obj, ref model);
                var dbEntityEntry = _context.Entry(model);

                _context.KT_SANLUONG.Attach(model);
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //Xóa những detail cũ:
                _context.KT_SANLUONG_DETAIL.Where(o => o.ID_KHAITHAC_SANLUONG == _obj.ID).ToList().ForEach(o => _context.KT_SANLUONG_DETAIL.Remove(o));

                //Thêm mới detail đã sửa
                int Id = _obj.ID;

                if (lstKeyName != null)
                {
                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        KT_SANLUONG_DETAIL _objDetail = new Models.KT_SANLUONG_DETAIL();

                        FDB.Common.Helpers.GetValueForm<KT_SANLUONG_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_KHAITHAC_SANLUONG = Id;

                        _context.KT_SANLUONG_DETAIL.Add(_objDetail);
                    }
                }
                //Save data:
                _context.SaveChanges();
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Sản lượng"));
                return RedirectToAction("Index");
            }
            else
            {
                //TempData["_SUCCESS"] = "";
                _obj.DSSanLuongDetail = new List<KT_SANLUONG_DETAIL>();
                this.LoadDanhMuc();
                //build html :
                int maxID = 0;
                String strHTML = "";// this.GenderHTML(lstKeyddlDoiTuongNuoi, lstKeyddlNhomNgheKhaiThac, lstKeyddlNgheKhaiThac, lstKeytxtSanLuong, _form, ref maxID);

                ViewBag.sEditHTML = strHTML;
                ViewBag.MaxID = maxID + 1;
                return View(_obj);

            }
        }

        //[HttpPost]
        //public ActionResult getNgheByNhomNhe(string ma_NhomNghe)
        //{
        //    if (String.IsNullOrEmpty(ma_NhomNghe))
        //        return Content(String.Join("", new Object[] { "", "" }));

        //    int IDNhomNghe = int.Parse(ma_NhomNghe);

        //    var Nghes = _context.DM_NGHE.Where(d => d.DM_NhomNgheID == IDNhomNghe).Select(a => "<option value='" + a.DM_NgheID + "'>" + a.TenNghe + "</option>");

        //    return Content(string.Join("", Nghes));
        //}

        #region "xoa"
        //public JsonResult Delete(string id)
        //{
        //    //Xóa header
        //    KT_SANLUONG _objSanLuong = _context.KT_SANLUONG.Find(int.Parse(id));
        //    _context.KT_SANLUONG.Remove(_objSanLuong);
        //    //var deltail = _context.KT_SANLUONG
        //    //            .Include()
        //    //            .ToList(); 
        //    _context.KT_SANLUONG_DETAIL.Where(o => o.ID_KHAITHAC_SANLUONG == _objSanLuong.ID).ToList().ForEach(o => _context.KT_SANLUONG_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    _context.SaveChanges();
        //    // TempData["_SUCCESS"] = "Xóa bản ghi thành công!";

        //    return Json(_objSanLuong, JsonRequestBehavior.AllowGet);
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            //Xóa header
            KT_SANLUONG _objSanLuong = _context.KT_SANLUONG.Find(id);
            _context.KT_SANLUONG.Remove(_objSanLuong);
            _context.KT_SANLUONG_DETAIL.Where(o => o.ID_KHAITHAC_SANLUONG == _objSanLuong.ID).ToList().ForEach(o => _context.KT_SANLUONG_DETAIL.Remove(o));
            //Update thay đổi vào DB
            _context.SaveChanges();
            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Sản lượng"));
            return RedirectToAction("Index");
        }

        #endregion

        [HttpPost]
        public ActionResult getNhomDoiTuongByMatNuoc(string ma_MatNuoc)
        {
            if (String.IsNullOrEmpty(ma_MatNuoc))
                return Content(String.Join("", new Object[] { "", "" }));
            int IDLoaiMatNuoc = int.Parse(ma_MatNuoc);
            var Nghes = _context.DM_NHOMDOITUONG_KT.Select(a => "<option value='" + a.ID + "'>" + a.TEN_NHOM + "</option>");

            return Content(string.Join("", Nghes));
        }

        //thanhnc5:comment ngay 13/06 vi khong dung nhom doi tuong khai thac nua
        //[HttpPost]
        //public ActionResult getDoiTuongByNhom(string ma_NhomDoiTuong)
        //{
        //    if (String.IsNullOrEmpty(ma_NhomDoiTuong))
        //        return Content(String.Join("", new Object[] { "", "" }));

        //    int IDNhomDoiTuong = int.Parse(ma_NhomDoiTuong);
        //    var Nghes = _context.DM_DOITUONG_KT.Where(d => d.DM_NHOMDOITUONG_KTID == IDNhomDoiTuong).Select(a => "<option value='" + a.ID + "'>" + a.TEN_DOI_TUONG + "</option>");

        //    return Content(string.Join("", Nghes));
        //}


        #region "Function"
        //Nếu dùng Jquery Validate thì hàm này có thể bỏ
        //private String GenderHTML(List<string> lstKeyddlDoiTuongNuoi, List<string> lstKeyddlNhomNgheKhaiThac, List<string> lstKeyddlNgheKhaiThac, List<string> lstKeytxtSanLuong, FormCollection _form, ref int MaxID)
        //{
        //    String strHTML = "";
        //    int _maxID = 0;
        //    if (lstKeyddlDoiTuongNuoi != null)
        //    {
        //        List<DM_NHOMNGHE> lstDMNhomNghe = _context.DM_NHOMNGHE.ToList();
        //        List<DM_NGHE> lstDMNghe = _context.DM_NGHE.ToList();
        //        List<DM_NHOMDOITUONG_KT> lstDMNhomDoiTuong = _context.DM_NHOMDOITUONG_KT.ToList();

        //        for (int i = 0; i < lstKeyddlDoiTuongNuoi.Count; i++)
        //        {
        //            if (i < 3)
        //            {
        //                strHTML += "<tr><td><select class=\"form-control\" name=\"" + lstKeyddlDoiTuongNuoi[i] + "\" readonly>";
        //                var strDDL = "";

        //                for (var j = 0; j < lstDMNhomDoiTuong.Count; j++)
        //                {
        //                    if (_form[lstKeyddlDoiTuongNuoi[i]] == lstDMNhomDoiTuong[j].ID.ToString())
        //                        strDDL += "<option value=\"" + lstDMNhomDoiTuong[j].ID.ToString() + "\" selected=\"selected\" >" + lstDMNhomDoiTuong[j].TEN_NHOM + "</option>";
        //                    else
        //                        strDDL += "<option value=\"" + lstDMNhomDoiTuong[j].ID.ToString() + "\">" + lstDMNhomDoiTuong[j].TEN_NHOM + "</option>";
        //                }
        //                strDDL += "</select>";
        //                strHTML += strDDL + "</td><td><select  name=\"" + lstKeyddlNhomNgheKhaiThac[i] + "\" id=\"" + lstKeyddlNhomNgheKhaiThac[i] + "\" class=\"selectNhomNghe\">";
        //                var strDDL2 = "";

        //                for (var k = 0; k < lstDMNhomNghe.Count; k++)
        //                {
        //                    if (_form[lstKeyddlNhomNgheKhaiThac[i]] == lstDMNhomNghe[k].DM_NhomNgheID.ToString())
        //                        strDDL2 += "<option value=\"" + lstDMNhomNghe[k].DM_NhomNgheID.ToString() + "\" selected=\"selected\"  >" + lstDMNhomNghe[k].TenNhomNghe + "</option>";
        //                    else
        //                        strDDL2 += "<option value=\"" + lstDMNhomNghe[k].DM_NhomNgheID.ToString() + "\">" + lstDMNhomNghe[k].TenNhomNghe + "</option>";
        //                }
        //                strDDL2 += "</select>";
        //                strHTML += strDDL2 + "</td><td><select class=\"form-control\" name=\"" + lstKeyddlNgheKhaiThac[i] + "\" id=\"" + lstKeyddlNgheKhaiThac[i] + "\"><option value>--chọn nghề--</option>";

        //                var strDDL3 = "";
        //                for (var k = 0; k < lstDMNghe.Count; k++)
        //                {
        //                    if (_form[lstKeyddlNgheKhaiThac[i]] == lstDMNghe[k].DM_NgheID.ToString())
        //                        strDDL3 += "<option value=\"" + lstDMNghe[k].DM_NgheID.ToString() + "\" selected=\"selected\" >" + lstDMNghe[k].TenNghe + "</option>";
        //                    else
        //                        strDDL3 += "<option value=\"" + lstDMNghe[k].DM_NgheID.ToString() + "\">" + lstDMNghe[k].TenNghe + "</option>";
        //                }
        //                strDDL3 += "</select>";

        //                strHTML += strDDL3 + "</td><td><input type=\"text\" value=\"" + _form[lstKeytxtSanLuong[i]] + "\" class=\"form-control\" name=\"" + lstKeytxtSanLuong[i] + "\"/></td>";
        //                strHTML += "</td></tr>";

        //            }
        //            else
        //            {
        //                strHTML += "<tr><td><select class=\"form-control\" name=\"" + lstKeyddlDoiTuongNuoi[i] + "\">";
        //                var strDDL = "";

        //                for (var j = 0; j < lstDMNhomDoiTuong.Count; j++)
        //                {
        //                    if (_form[lstKeyddlDoiTuongNuoi[i]] == lstDMNhomDoiTuong[j].ID.ToString())
        //                        strDDL += "<option value=\"" + lstDMNhomDoiTuong[j].ID.ToString() + "\" selected=\"selected\" >" + lstDMNhomDoiTuong[j].TEN_NHOM + "</option>";
        //                    else
        //                        strDDL += "<option value=\"" + lstDMNhomDoiTuong[j].ID.ToString() + "\">" + lstDMNhomDoiTuong[j].TEN_NHOM + "</option>";
        //                }
        //                strDDL += "</select>";
        //                strHTML += strDDL + "</td><td><select  name=\"" + lstKeyddlNhomNgheKhaiThac[i] + "\" id=\"" + lstKeyddlNhomNgheKhaiThac[i] + "\" class=\"selectNhomNghe\">";
        //                var strDDL2 = "";

        //                for (var k = 0; k < lstDMNhomNghe.Count; k++)
        //                {
        //                    if (_form[lstKeyddlNhomNgheKhaiThac[i]] == lstDMNhomNghe[k].DM_NhomNgheID.ToString())
        //                        strDDL2 += "<option value=\"" + lstDMNhomNghe[k].DM_NhomNgheID.ToString() + "\" selected=\"selected\" >" + lstDMNhomNghe[k].TenNhomNghe + "</option>";
        //                    else
        //                        strDDL2 += "<option value=\"" + lstDMNhomNghe[k].DM_NhomNgheID.ToString() + "\">" + lstDMNhomNghe[k].TenNhomNghe + "</option>";
        //                }
        //                strDDL2 += "</select>";
        //                strHTML += strDDL2 + "</td><td><select class=\"form-control\" name=\"" + lstKeyddlNgheKhaiThac[i] + "\" id=\"" + lstKeyddlNgheKhaiThac[i] + "\"><option value>--chọn nghề--</option>";

        //                var strDDL3 = "";
        //                for (var k = 0; k < lstDMNghe.Count; k++)
        //                {
        //                    if (_form[lstKeyddlNgheKhaiThac[i]] == lstDMNghe[k].DM_NgheID.ToString())
        //                        strDDL3 += "<option value=\"" + lstDMNghe[k].DM_NgheID.ToString() + "\" selected=\"selected\" >" + lstDMNghe[k].TenNghe + "</option>";
        //                    else
        //                        strDDL3 += "<option value=\"" + lstDMNghe[k].DM_NgheID.ToString() + "\">" + lstDMNghe[k].TenNghe + "</option>";
        //                }
        //                strDDL3 += "</select>";

        //                strHTML += strDDL3 + "</td><td><input type=\"text\" value=\"" + _form[lstKeytxtSanLuong[i]] + "\" class=\"form-control\" name=\"" + lstKeytxtSanLuong[i] + "\"/></td>";
        //                strHTML += "</td></tr>";

        //            }

        //            if (int.Parse(lstKeyddlDoiTuongNuoi[i].Split('_')[1]) > _maxID)
        //                _maxID = int.Parse(lstKeyddlDoiTuongNuoi[i].Split('_')[1]);
        //        }

        //    }
        //    MaxID = _maxID;
        //    return strHTML;
        //}

        //private String InititalGetHTML()
        //{
        //    String strHTML = "";
        //    List<DM_NHOMNGHE> lstDMNhomNghe = _context.DM_NHOMNGHE.ToList();
        //    var strDDL2 = "";

        //    for (var k = 0; k < lstDMNhomNghe.Count; k++)
        //    {
        //        strDDL2 += "<option value=\"" + lstDMNhomNghe[k].DM_NhomNgheID.ToString() + "\">" + lstDMNhomNghe[k].TenNhomNghe + "</option>";
        //    }
        //    strDDL2 += "</select>";

        //    List<DM_NGHE> lstDMNghe = _context.DM_NGHE.Where(o => o.DM_NhomNgheID == 1).ToList();
        //    var strDDL3 = "<option value>--chọn nghề--</option>";

        //    for (var k = 0; k < lstDMNghe.Count; k++)
        //    {
        //        strDDL3 += "<option value=\"" + lstDMNghe[k].DM_NgheID.ToString() + "\">" + lstDMNghe[k].TenNghe + "</option>";
        //    }
        //    strDDL3 += "</select>";

        //    List<DM_NHOMDOITUONG_KT> lstDMNhomDoiTuong = _context.DM_NHOMDOITUONG_KT.ToList();

        //    //row1:
        //    strHTML += "<tr>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlDoiTuongNuoi_1\" readonly >";
        //    strHTML += "<option value=\"1\">" + lstDMNhomDoiTuong[0].TEN_NHOM + "</option>";
        //    strHTML += "</select>";
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select c name=\"ddlNhomNgheKhaiThac_1\" id=\"ddlNhomNgheKhaiThac_1\" class=\"selectNhomNghe\">";
        //    strHTML += strDDL2;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlNgheKhaiThac_1\" id=\"ddlNgheKhaiThac_1\">";
        //    strHTML += strDDL3;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<input type=\"text\" value=\"0\" class=\"form-control\" name=\"txtSanLuong_1\"/>";
        //    strHTML += "</td>";
        //    strHTML += "<td></td>";
        //    strHTML += "</tr>";

        //    //row2:
        //    strHTML += "<tr>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlDoiTuongNuoi_2\" readonly>";
        //    strHTML += "<option value=\"2\">" + lstDMNhomDoiTuong[1].TEN_NHOM + "</option>";
        //    strHTML += "</select>";
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select name=\"ddlNhomNgheKhaiThac_2\" id=\"ddlNhomNgheKhaiThac_2\" class=\"selectNhomNghe\">";
        //    strHTML += strDDL2;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlNgheKhaiThac_2\" id=\"ddlNgheKhaiThac_2\">";
        //    strHTML += strDDL3;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<input type=\"text\" value=\"0\" class=\"form-control\" name=\"txtSanLuong_2\"/>";
        //    strHTML += "</td>";
        //    strHTML += "<td></td>";
        //    strHTML += "</tr>";

        //    //row3:
        //    strHTML += "<tr>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlDoiTuongNuoi_3\" readonly>";
        //    strHTML += "<option value=\"3\">" + lstDMNhomDoiTuong[2].TEN_NHOM + "</option>";
        //    strHTML += "</select>";
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select  name=\"ddlNhomNgheKhaiThac_3\" id=\"ddlNhomNgheKhaiThac_3\" class=\"selectNhomNghe\">";
        //    strHTML += strDDL2;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<select class=\"form-control\" name=\"ddlNgheKhaiThac_3\" id=\"ddlNgheKhaiThac_3\">";
        //    strHTML += strDDL3;
        //    strHTML += "</td>";
        //    strHTML += "<td>";
        //    strHTML += "<input type=\"text\" value=\"0\" class=\"form-control\" name=\"txtSanLuong_3\"/>";
        //    strHTML += "</td>";
        //    strHTML += "<td></td>";
        //    strHTML += "</tr>";


        //    return strHTML;
        //}



        private void LoadDanhMuc()
        {



            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

            //Load danh mục nhóm đối tượng 
            //String strNhomDTKT = "";
            //_context.DM_NHOMDOITUONG_KT.ToList().ForEach(d =>
            //{
            //    strNhomDTKT += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_NHOM + "</option>";
            //});

            //Load nhóm đối tượng CPUE:
            //string strNhomDTKT_CPUE = "";
            string strNhomDTKT = "";

           // FDB.Common.CategoryCommon.lstDMDoiTuongCPUE.ForEach(d =>
             _context.DM_NHOMDOITUONG_KT.ToList().ForEach(d =>
            {
                strNhomDTKT += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_NHOM + "</option>";
            });


            //Load dm nhóm nghề
            String strNhomNgheKT = "";
            _context.DM_NHOMNGHE.ToList().ForEach(d =>
            {
                strNhomNgheKT += "<option value=\"" + d.DM_NhomNgheID.ToString() + "\">" + d.TenNhomNghe + "</option>";
            });

            //Load nhóm công suất:
            string strNhomCSuat = "";
            _context.DNHOM_TAU.ToList().ForEach(d =>
            {
                strNhomCSuat += "<option value=\"" + d.ID.ToString() + "\">" + d.Name + "</option>";
            });

            ViewBag.DMNhomNgheKhaiThac = strNhomNgheKT;
            //ViewBag.DMNhomDoiTuongKT = strNhomDTKT_CPUE;
            ViewBag.DMNhomDoiTuongKT = strNhomDTKT;
            ViewBag.DMNhomCongSuat = strNhomCSuat;

            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                    .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DM_DonVis = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

        }

        #endregion



        public void initialCategorySearchAction()
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NHOMNGHEs = new SelectList(_context.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.NHOM_TAUs = new SelectList(_context.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = _context.DTINHTP.Where(u => (string.IsNullOrEmpty(curUser.MA_TINHTP) || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP).Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }

    }







    #region "thong ke san luong"
    class ThongKe_TrucTiep
    {
        List<DM_NHOMNGHE> _lstDMNhomNghe;
        List<DNHOM_TAU> _lstDMNhomTau;
        FDBContext db = new FDBContext();

        ViewModelSearchKT_SANLUONG_TRUCTIEP t;


        public Dictionary<string, float> SanLuong_TheoNghe
        {
            get
            {
                return ThongKe_SanLuong_TheoNghe(t);
            }
        }

        public Dictionary<string, float> SanLuong_TheoNhomNghe
        {
            get
            {
                return ThongKe_SanLuong_TheonhomNghe(t);
            }
        }

        public float SanLuong_Thang
        {
            get 
            {
                return ThongKe_SanLuong_Thang();
            }
        }

        public Dictionary<string, float> SanLuong_TheoLoai
        {
            get
            {
                return ThongKe_SanLuong_TheoLoai(t);
            }
        }

        public Dictionary<string, float> SanLuong_TheoNhomLoai
        {
            get
            {
                return ThongKe_SanLuong_TheonhomLoai(t);
            }
        }

        public float SanLuong_Loai_Thang
        {
            get
            {
                return ThongKe_SanLuong_LoaiThang();
            }
        }

        public ThongKe_TrucTiep(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            this.t = t;

            _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
            _lstDMNhomTau = db.DNHOM_TAU.ToList();
        }

        public List<KT_SANLUONG_TRUC_TIEP> Lay_Danh_Sach_San_Luong(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            // danh sach phang
            var tk = db.KT_SANLUONG.
                            Join(db.KT_SANLUONG_DETAIL, sl_ID => sl_ID.ID, detail_ID => detail_ID.ID_KHAITHAC_SANLUONG,
                                (sl_ID, detail_ID) => new
                                {
                                    THANG = sl_ID.THANG
                                  ,
                                    NAM = sl_ID.NAM
                                  ,
                                    MA_TINHTP = sl_ID.MA_TINHTP
                                  ,
                                    ID_KHAITHAC_NHOM_DOITUONG = detail_ID.ID_KHAITHAC_NHOM_DOITUONG
                                  ,
                                    ID_KHAITHAC_NHOM_NGHE = detail_ID.ID_KHAITHAC_NHOM_NGHE
                                  ,
                                    ID_KHAITHAC_NHOM_CONGSUAT = detail_ID.ID_KHAITHAC_NHOM_CONGSUAT
                                  ,
                                    SAN_LUONG = detail_ID.SAN_LUONG
                                }
                            ).
                            Where(o => (((string.IsNullOrEmpty(t.TinhTP) && t.TinhTP_User.StartsWith("Z")) || (string.IsNullOrEmpty(t.TinhTP) && o.MA_TINHTP == t.TinhTP_User) || (o.MA_TINHTP == t.TinhTP))
                                            && (t.Thang == null || o.THANG == t.Thang)
                                            && (t.Nam == null || o.NAM == t.Nam)
                                            ));

            // dua vao list
            List<KT_SANLUONG_TRUC_TIEP> lstSL = new List<KT_SANLUONG_TRUC_TIEP>();
            foreach (var sl in tk)
            {
                lstSL.Add(new KT_SANLUONG_TRUC_TIEP()
                {
                    THANG = sl.THANG,
                    NAM = sl.NAM,
                    MA_TINHTP = sl.MA_TINHTP,
                    ID_KHAITHAC_NHOM_DOITUONG = sl.ID_KHAITHAC_NHOM_DOITUONG,
                    ID_KHAITHAC_NHOM_NGHE = sl.ID_KHAITHAC_NHOM_NGHE,
                    ID_KHAITHAC_NHOM_CONGSUAT = sl.ID_KHAITHAC_NHOM_CONGSUAT,
                    SAN_LUONG = sl.SAN_LUONG
                });
            }

            return lstSL;
        }

        public Dictionary<string, float> ThongKe_SanLuong_TheoNghe(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            Dictionary<string, float> dictSL = new Dictionary<string, float>();

            
            // lay danh sach san luong
            List<KT_SANLUONG_TRUC_TIEP> lstSL = Lay_Danh_Sach_San_Luong(t);

            // tinh san luong
            foreach (var nghe in _lstDMNhomNghe)
            {
                foreach (var nhomtau in _lstDMNhomTau)
                {
                    var key = t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nhomtau.ID.ToString() + "#" + nghe.DM_NhomNgheID.ToString();

                    var sl = lstSL.Where(o => o.ID_KHAITHAC_NHOM_CONGSUAT == nhomtau.ID && o.ID_KHAITHAC_NHOM_NGHE == nghe.DM_NhomNgheID).Sum(s => s.SAN_LUONG);

                    dictSL.Add(key, sl ?? 0);

                }
            }

            return dictSL;
        }


        public Dictionary<string, float> ThongKe_SanLuong_TheonhomNghe(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            Dictionary<string, float> dictSumSL = new Dictionary<string, float>();

            if (this.SanLuong_TheoNghe != null && this.SanLuong_TheoNghe.Count > 0)
            {
                foreach (var nghe in _lstDMNhomNghe)
                {
                    float iSum = 0;
                    foreach (var nhomtau in _lstDMNhomTau)
                    {
                        iSum += SanLuong_TheoNghe[t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nhomtau.ID.ToString() + "#" + nghe.DM_NhomNgheID.ToString()];
                    }

                    dictSumSL.Add(t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nghe.DM_NhomNgheID.ToString(), iSum);
                }
            }

            return dictSumSL;
        }

        public float ThongKe_SanLuong_Thang()
        {
            float iSumThang = 0;

            foreach (var sl in SanLuong_TheoNhomNghe.Values)
            {
                iSumThang += sl;
            }

            return iSumThang;
        }

        public Dictionary<string, float> ThongKe_SanLuong_TheoLoai(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            Dictionary<string, float> dictSL = new Dictionary<string, float>();

            // lay danh sach san luong
            List<KT_SANLUONG_TRUC_TIEP> lstSL = Lay_Danh_Sach_San_Luong(t);

            // tinh san luong
            foreach (var nghe in _lstDMNhomNghe)
            {
               // foreach (var nhomloai in FDB.Common.CategoryCommon.lstDMDoiTuongCPUE)
                foreach (var nhomloai in db.DM_NHOMDOITUONG_KT.ToList())
                {
                    var key = t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nhomloai.ID.ToString() + "#" + nghe.DM_NhomNgheID.ToString();

                    var sl = lstSL.Where(o => o.ID_KHAITHAC_NHOM_DOITUONG == nhomloai.ID && o.ID_KHAITHAC_NHOM_NGHE == nghe.DM_NhomNgheID).Sum(s => s.SAN_LUONG);

                    dictSL.Add(key, sl ?? 0);

                }
            }

            return dictSL;
        }

        public Dictionary<string, float> ThongKe_SanLuong_TheonhomLoai(ViewModelSearchKT_SANLUONG_TRUCTIEP t)
        {
            Dictionary<string, float> dictSumSL = new Dictionary<string, float>();

             // tinh san luong
            foreach (var nghe in _lstDMNhomNghe)
            {
                float iSum = 0;
                //foreach (var nhomloai in FDB.Common.CategoryCommon.lstDMDoiTuongCPUE)
                foreach (var nhomloai in db.DM_NHOMDOITUONG_KT.ToList())
                {
                    iSum += SanLuong_TheoLoai[t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nhomloai.ID.ToString() + "#" + nghe.DM_NhomNgheID.ToString()];
                }

                dictSumSL.Add(t.TinhTP + "#" + t.Thang.ToString() + "#" + t.Nam.ToString() + "#" + nghe.DM_NhomNgheID.ToString(),iSum);
            }

            return dictSumSL;
        }

        public float ThongKe_SanLuong_LoaiThang()
        {
            float iSumThang = 0;

            foreach (var sl in SanLuong_TheoNhomLoai.Values)
            {
                iSumThang += sl;
            }

            return iSumThang;
        }

    }

    public class KT_SANLUONG_TRUC_TIEP
    {

        public int? THANG { get; set; }

        public int? NAM { get; set; }

        public string MA_TINHTP { get; set; }

        public int? ID_KHAITHAC_NHOM_DOITUONG { get; set; }

        public int? ID_KHAITHAC_NHOM_NGHE { get; set; }

        public int? ID_KHAITHAC_NHOM_CONGSUAT { get; set; }

        public float? SAN_LUONG { get; set; }
    }

    #endregion
}