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
using Microsoft.AspNet.Identity;
using FDB.Helpers;
using System.Data.SqlClient;


namespace FDB.Controllers.NuoiTrong
{
    public class NT_NUOILONGBEController : FDBController
    {
        private FDBContext _context = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }

        ////
        //// GET: /Nuoi trong long be/   
        //public ActionResult Index(ViewModelSearchNT_NUOILONGBE Searchmodel)
        //{
        //    //Load danh mục:
        //    this.LoadDanhMuc(Searchmodel);

        //    var models = _context.NT_NUOILONGBE.Where(o => (Searchmodel.TThanhPho == null || o.MA_TINHTP == Searchmodel.TThanhPho)
        //                                                && (Searchmodel.Thang == null || o.THANG == Searchmodel.Thang)
        //                                                && (Searchmodel.Nam == null || o.NAM == Searchmodel.Nam)
        //                                                && (Searchmodel.DoiTuongNuoi == null || o.DSNT_NuoiLongBeDetail.Any(d => d.ID_NUOITRONG_DOITUONG == Searchmodel.DoiTuongNuoi))

        //                                    )
        //                                    ;

        //    ViewBag.TotalRow = models.Count().ToString();
        //    models = models.OrderByDescending(m => m.ID);
        //    //Phân trang ở đây:
        //    var pageIndex = Searchmodel.Page ?? 1;
        //    Searchmodel.SearchResults = models.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

        //    return View(Searchmodel);
        //}

        public ActionResult Search(ViewModelBaoCaoNT_NUOILONGBE BaocaoModel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                            .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", BaocaoModel.TThanhPho);


            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;


            string _MA_TTP = BaocaoModel.TThanhPho;
            if (BaocaoModel.LoaiBaoCao != null)
            {


                var results = _context.Database.SqlQuery<ViewModelBaoCaoNT_NUOILONGBE_Detail>("exec NT_NUOILONGBE_SEARCH @LoaiBaoCao, @LoaiMatNuoc, @Thang, @Nam, @TuNgay, @DenNgay, @TThanhPho"
                     , new SqlParameter("@LoaiBaoCao", BaocaoModel.LoaiBaoCao == null ? (object)DBNull.Value : BaocaoModel.LoaiBaoCao)
                     , new SqlParameter("@LoaiMatNuoc", BaocaoModel.LoaiMatNuoc == null ? (object)DBNull.Value : BaocaoModel.LoaiMatNuoc)
                     , new SqlParameter("@Thang", BaocaoModel.Thang == null ? (object)DBNull.Value : BaocaoModel.Thang)
                     , new SqlParameter("@Nam", BaocaoModel.Nam == null ? (object)DBNull.Value : BaocaoModel.Nam)
                     , new SqlParameter("@TuNgay", BaocaoModel.TuNgay == null ? (object)DBNull.Value : BaocaoModel.TuNgay)
                     , new SqlParameter("@DenNgay", BaocaoModel.DenNgay == null ? (object)DBNull.Value : BaocaoModel.DenNgay)
                     , new SqlParameter("@TThanhPho", BaocaoModel.TThanhPho == null ? (object)DBNull.Value : BaocaoModel.TThanhPho)
                     ).ToList();


                BaocaoModel.ReportResults = results;
                string strTitle = @"Kết quả thống kê hình thức nuôi lồng bè " + (BaocaoModel.LoaiMatNuoc == null ? " " : BaocaoModel.LoaiMatNuoc == 1 ? " nước  ngọt" : " nước mặn lợ ")
                                + (BaocaoModel.LoaiBaoCao == 1 ? "(tuần từ ngày " + BaocaoModel.TuNgay.Value.ToString("dd/MM/yyyy") + " đến ngày " + BaocaoModel.DenNgay.Value.ToString("dd/MM/yyyy") + ")"
                                : BaocaoModel.Thang != null ? " tháng " + BaocaoModel.Thang.ToString() + " Năm " + BaocaoModel.Nam.ToString() : " Năm " + BaocaoModel.Nam.ToString())
                    ;

                strTitle = strTitle + (BaocaoModel.TThanhPho != null ? " của " + _context.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == _MA_TTP).TEN_TINHTP : " của toàn quốc ");

                ViewBag.strTitle = strTitle;
            }

            return View(BaocaoModel);
        }


        //public ActionResult Create()
        //{
        //    this.LoadDanhMuc();
        //    NT_NUOILONGBE model = new NT_NUOILONGBE();
        //    model.NGUOI_NHAP = User.Identity.Name;
        //    model.NGAY_NHAP = DateTime.Now;
        //    model.NAM = DateTime.Now.Year;
        //    // model.THANG = DateTime.Now.Month;

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Create(FormCollection _form, NT_NUOILONGBE _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        //Save Header
        //        _obj.NGUOI_NHAP = User.Identity.Name;
        //        _obj.NGAY_NHAP = DateTime.Now;
        //        _context.NT_NUOILONGBE.Add(_obj);
        //        _context.SaveChanges();

        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_NUOILONGBE_DETAIL _objDetail = new Models.NT_NUOILONGBE_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_NUOILONGBE_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_NUOILONGBE = Id;
        //                _context.NT_NUOILONGBE_DETAIL.Add(_objDetail);
        //            }
        //            //Save data:
        //            _context.SaveChanges();
        //        }

        //        this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "nuôi thủy sản lồng bè"));
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.LoadDanhMuc();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML(lstInt, _form, ref maxID);
        //        ViewBag.AddHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult Edit(String id)
        //{
        //    this.LoadDanhMuc();
        //    int _ID = int.Parse(id);
        //    var model = _context.NT_NUOILONGBE.First(o => o.ID == _ID);
        //    return View(model);
        //}

        //[HttpPost]
        ////async Task<ActionResult>
        //public ActionResult Edit(FormCollection _form, NT_NUOILONGBE _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        var model = _context.NT_NUOILONGBE.First(o => o.ID == _obj.ID);
        //        FDB.Common.Helpers.CopyObject<NT_NUOILONGBE>(_obj, ref model);
        //        model.NGUOI_NHAP = User.Identity.Name;
        //        var dbEntityEntry = _context.Entry(model);

        //        _context.NT_NUOILONGBE.Attach(model);
        //        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        //Xóa những detail cũ:
        //        //  _context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).ToList().ForEach(o => _context.NT_NUOILONGBE_DETAIL.Remove(o));
        //        _context.NT_NUOILONGBE_DETAIL.RemoveRange(_context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).AsEnumerable());
        //        //Thêm mới detail đã sửa
        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_NUOILONGBE_DETAIL _objDetail = new Models.NT_NUOILONGBE_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_NUOILONGBE_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_NUOILONGBE = Id;
        //                _context.NT_NUOILONGBE_DETAIL.Add(_objDetail);
        //            }

        //        }
        //        //Save data:
        //        _context.SaveChanges();

        //        this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "nuôi thủy sản lồng bè"));
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.LoadDanhMuc();
        //        _obj.DSNT_NuoiLongBeDetail = new List<NT_NUOILONGBE_DETAIL>();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML(lstInt, _form, ref maxID);
        //        ViewBag.sEditHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult Delete(int id)
        //{
        //    //Xóa header
        //    NT_NUOILONGBE _obj = _context.NT_NUOILONGBE.Find(id);
        //    _context.NT_NUOILONGBE.Remove(_obj);
        //    _context.NT_NUOILONGBE_DETAIL.RemoveRange(_context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).AsEnumerable());
        //    //  _context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).ToList().ForEach(o => _context.NT_NUOILONGBE_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    _context.SaveChanges();
        //    this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "nuôi thủy sản lồng bè"));
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public ActionResult getNhomDoiTuongByMatNuoc(string ma_MatNuoc)
        //{
        //    int IDLoaiMatNuoc = int.Parse(ma_MatNuoc);
        //    var Nghes = _context.DM_NHOMDOITUONG_NUOI.Where(d => d.ID_LOAI_MATNUOC_NUOI == IDLoaiMatNuoc).Select(a => "<option value='" + a.ID + "'>" + a.TEN_NHOM + "</option>");

        //    return Content(string.Join("", Nghes));
        //}


        //[HttpPost]
        //public ActionResult getDoiTuongByNhom(string ma_NhomDoiTuong)
        //{
        //    if (String.IsNullOrEmpty(ma_NhomDoiTuong))
        //        return Content(String.Join("", new Object[] { "", "" }));

        //    int IDNhomDoiTuong = int.Parse(ma_NhomDoiTuong);
        //    var Nghes = _context.DM_DOITUONG_NUOI.Where(d => d.DM_NHOMDOITUONG_NUOIID == IDNhomDoiTuong).Select(a => "<option value='" + a.ID + "'>" + a.TEN_DOI_TUONG + "</option>");

        //    return Content(string.Join("", Nghes));
        //}

        //[HttpPost]
        //public ActionResult getDistrict(string ma_TinhTP)
        //{
        //    var districts = _context.DQUANHUYEN.Where(d => d.MA_TINHTP == ma_TinhTP).Select(a => "<option value='" + a.MA_QUANHUYEN + "'>" + a.TEN_QUANHUYEN + "</option>");

        //    return Content(string.Join("", districts));
        //}

        //[HttpPost]
        //public ActionResult getWard(string ma_QuanHuyen)
        //{
        //    var districts = _context.DPHUONGXA.Where(d => d.MA_QUANHUYEN == ma_QuanHuyen).Select(a => "<option value='" + a.MA_PHUONGXA + "'>" + a.TEN_PHUONGXA + "</option>");

        //    return Content(string.Join("", districts));
        //}


        //#region "Function"
        ////Nếu dùng Jquery Validate thì hàm này có thể bỏ
        //private String GenderHTML(List<int> lstInt, FormCollection _form, ref int MaxID)
        //{
        //    String strHTML = "";
        //    int _maxID = 0;
        //    if (lstInt != null && lstInt.Count > 0)
        //    {
        //        List<DM_NHOMDOITUONG_NUOI> lstDMNhomDoiTuongNuoi = _context.DM_NHOMDOITUONG_NUOI.ToList();
        //        List<DM_DOITUONG_NUOI> lstDMDoiTuongNuoi = _context.DM_DOITUONG_NUOI.ToList();
        //        List<DM_DOITUONG_NUOI> lstDMDoiTuongNuoiSelected = new List<DM_DOITUONG_NUOI>();
        //        int ID_NhomDoiTuongSelected = 1;

        //        for (int i = 0; i < lstInt.Count; i++)
        //        {
        //            strHTML += "<tr><td><select class=\"selectNhomNghe\" name=\"ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString() + "\" id=\"ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString() + "\">";
        //            lstDMNhomDoiTuongNuoi.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString()])
        //                {
        //                    ID_NhomDoiTuongSelected = d.ID;
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN_NHOM + "</option>";
        //                }

        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN_NHOM + "</option>";
        //            }
        //            );
        //            strHTML += "</td>";
        //            lstDMDoiTuongNuoiSelected = lstDMDoiTuongNuoi.Where(o => o.DM_NHOMDOITUONG_NUOIID == ID_NhomDoiTuongSelected).ToList();
        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString() + "\" id=\"ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString() + "\"><option value></option>";
        //            lstDMDoiTuongNuoiSelected.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN_DOI_TUONG + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN_DOI_TUONG + "</option>";
        //            }
        //                );

        //            strHTML += "</select>";
        //            strHTML += "</td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtTHE_TICH_LONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtTHE_TICH_LONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSAN_LUONG_LONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSAN_LUONG_LONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_LONG_TU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_LONG_TU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_LONG_DEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_LONG_DEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtTHUC_AN_LONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtTHUC_AN_LONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_VEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_VEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSAN_LUONG_VEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSAN_LUONG_VEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_VEN_TU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_VEN_TU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_VEN_DEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_VEN_DEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNUOI_CDK_VEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNUOI_CDK_VEN_" + lstInt[i].ToString() + "\"/></td>";

        //            strHTML += "<td><label class=\"BtnPlus\" style=\"cursor:pointer\"><img src=\"/fonts/button-add-icon.png\" title=\"Thêm mới\" /></label>";
        //            strHTML += "<label class=\"BtnMinus\" id=\"lblDetail_\"" + (i + 1).ToString() + " style=\"cursor:pointer\"><img src=\"/fonts/DELETE.GIF\" title=\"Xóa\" /></label></td>";
        //            strHTML += "</tr>";

        //            if (lstInt[i] > _maxID)
        //                _maxID = lstInt[i];
        //        }

        //    }
        //    MaxID = _maxID;
        //    return strHTML;
        //}

        //public void LoadDanhMuc(ViewModelSearchNT_NUOILONGBE Searchmodel)
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
        //    // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
        //    ViewBag.DMNhomDoiTuongNuoi = new SelectList(_context.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM", Searchmodel.NhomDoiTuongNuoi);
        //    ViewBag.DMDoiTuongNuoi = new SelectList(_context.DM_DOITUONG_NUOI, "ID", "TEN_DOI_TUONG", Searchmodel.DoiTuongNuoi);

        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        //}

        //public void LoadDanhMuc()
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
        //    ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");

        //    //Load loại mặt nước:
        //    string sLoaiMatNuocs = "";
        //    _context.DM_LOAI_MATNUOC_NUOI_LONGBE.ToList().ForEach(d =>
        //    {
        //        sLoaiMatNuocs += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_LOAI + "</option>";
        //    });

        //    ViewBag.DMLoaiMatNuoc = sLoaiMatNuocs;

        //    //Load nhóm đối tượng:
        //    string sNhomDoiTuongNuois = "<option value></option>";
        //    _context.DM_NHOMDOITUONG_NUOI_LONGBE.Where(o => o.ID_LOAI_MATNUOC_NUOI_LONGBE == 1).ToList().ForEach(d =>
        //    {
        //        sNhomDoiTuongNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_NHOM + "</option>";
        //    });

        //    ViewBag.DMNhomDoiTuongNuoi = sNhomDoiTuongNuois;

        //    //Load đối tượng:
        //    string sDoiTuongNuois = "<option value></option>";
        //    _context.DM_DOITUONG_NUOI_LONGBE.ToList().ForEach(d =>
        //    {
        //        sDoiTuongNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
        //    });

        //    ViewBag.DMDoiTuongNuoi = sDoiTuongNuois;

        //    string sMoHinhNuois = "";
        //    CategoryCommon.DM_MOHINH_NUOIs.ForEach(d =>
        //    {
        //        sMoHinhNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN + "</option>";
        //    });
        //    ViewBag.DMMoHinhNuoi = sMoHinhNuois;
        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        //}

        //#endregion


        //public ActionResult CreateUnion()
        //{
        //    return View();
        //}
    }
}