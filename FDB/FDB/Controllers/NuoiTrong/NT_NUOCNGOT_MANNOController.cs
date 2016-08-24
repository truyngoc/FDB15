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


namespace FDB.Controllers.NuoiTrong
{
    public class NT_NUOCNGOT_MANNOController : FDBController
    {
        //private FDBContext _context = new FDBContext();


        ////
        //// GET: /Nuoi trong man ngot no/   
        //public ActionResult Index(ViewModelSearchNT_NUOCNGOT_MANNO Searchmodel)
        //{
        //    //Load danh mục:
        //    this.LoadDanhMuc(Searchmodel);

        //    var models = _context.NT_NUOCNGOT_MANNO.Where(o => (Searchmodel.TThanhPho == null || o.MA_TINHTP == Searchmodel.TThanhPho)
        //                                                && (Searchmodel.Thang == null || o.THANG == Searchmodel.Thang)
        //                                                && (Searchmodel.Nam == null || o.NAM == Searchmodel.Nam)
        //                                                && (Searchmodel.DoiTuongNuoi == null || o.DSNT_NuocNgotManNoDetail.Any(d => d.ID_NUOITRONG_DOITUONG == Searchmodel.DoiTuongNuoi))
        //                                                && (Searchmodel.MoHinhNuoi == null || o.DSNT_NuocNgotManNoDetail.Any(d => d.ID_MOHINH_NUOI == Searchmodel.MoHinhNuoi))
        //                                    )
        //                                    ;

        //    // string sqlsanLuongs = ((ObjectQuery)sanLuongs).ToTraceString();
        //    ViewBag.TotalRow = models.Count().ToString();
        //    models = models.OrderByDescending(m => m.ID);
        //    //Phân trang ở đây:
        //    var pageIndex = Searchmodel.Page ?? 1;
        //    Searchmodel.SearchResults = models.ToPagedList(pageIndex, Constants.PageSize);

        //    return View(Searchmodel);
        //}

        //public ActionResult Search()
        //{
        //    return View();
        //}

        //public ActionResult Create()
        //{
        //    this.LoadDanhMuc();
        //    NT_NUOCNGOT_MANNO model = new NT_NUOCNGOT_MANNO();
        //    model.NGUOI_NHAP = User.Identity.Name;
        //    model.NGAY_NHAP = DateTime.Now;
        //    model.NAM = DateTime.Now.Year;
        //    //model.THANG = DateTime.Now.Month;

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Create(FormCollection _form, NT_NUOCNGOT_MANNO _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        //Save Header
        //        _obj.NGUOI_NHAP = User.Identity.Name;
        //        _obj.NGAY_NHAP = DateTime.Now;
        //        _context.NT_NUOCNGOT_MANNO.Add(_obj);
        //        _context.SaveChanges();

        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_NUOCNGOT_MANNO_DETAIL _objDetail = new Models.NT_NUOCNGOT_MANNO_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_NUOCNGOT_MANNO_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_NUOCNGOT_MANNO = Id;
        //                _context.NT_NUOCNGOT_MANNO_DETAIL.Add(_objDetail);
        //            }
        //            //Save data:
        //            _context.SaveChanges();
        //        }

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, "Nuôi trồng nước ngọt, mặn và lợ"));
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
        //    var model = _context.NT_NUOCNGOT_MANNO.First(o => o.ID == _ID);
        //    return View(model);
        //}

        //[HttpPost]
        ////async Task<ActionResult>
        //public ActionResult Edit(FormCollection _form, NT_NUOCNGOT_MANNO _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        var model = _context.NT_NUOCNGOT_MANNO.First(o => o.ID == _obj.ID);
        //        FDB.Common.Helpers.CopyObject<NT_NUOCNGOT_MANNO>(_obj, ref model);
        //        model.NGUOI_NHAP = User.Identity.Name;
        //        var dbEntityEntry = _context.Entry(model);

        //        _context.NT_NUOCNGOT_MANNO.Attach(model);
        //        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        //Xóa những detail cũ:
        //        _context.NT_NUOCNGOT_MANNO_DETAIL.Where(o => o.ID_NUOCNGOT_MANNO == _obj.ID).ToList().ForEach(o => _context.NT_NUOCNGOT_MANNO_DETAIL.Remove(o));

        //        //Thêm mới detail đã sửa
        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_NUOCNGOT_MANNO_DETAIL _objDetail = new Models.NT_NUOCNGOT_MANNO_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_NUOCNGOT_MANNO_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_NUOCNGOT_MANNO = Id;
        //                _context.NT_NUOCNGOT_MANNO_DETAIL.Add(_objDetail);
        //            }

        //        }
        //        //Save data:
        //        _context.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, "Nuôi trồng nước ngọt, mặn và lợ"));
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.LoadDanhMuc();
        //        _obj.DSNT_NuocNgotManNoDetail = new List<NT_NUOCNGOT_MANNO_DETAIL>();
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
        //    NT_NUOCNGOT_MANNO _obj = _context.NT_NUOCNGOT_MANNO.Find(id);
        //    _context.NT_NUOCNGOT_MANNO.Remove(_obj);
        //    _context.NT_NUOCNGOT_MANNO_DETAIL.Where(o => o.ID_NUOCNGOT_MANNO == _obj.ID).ToList().ForEach(o => _context.NT_NUOCNGOT_MANNO_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    _context.SaveChanges();
        //    this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, "Nuôi trồng nước ngọt, mặn và lợ"));
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
        //    if(String.IsNullOrEmpty(ma_NhomDoiTuong))
        //        return Content(String.Join("",new Object[]{"",""}));

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
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSAN_LUONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSAN_LUONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_TU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_TU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_DEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_DEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_GAP_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_GAP_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_NUOI_CDK_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_NUOI_CDK_" + lstInt[i].ToString() + "\"/></td>";

        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_MOHINH_NUOI_" + lstInt[i].ToString() + "\">";
        //            CategoryCommon.DM_MOHINH_NUOIs.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_MOHINH_NUOI_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN + "</option>";
        //            }
        //               );

        //            strHTML += "</select>";
        //            strHTML += "</td>";

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

        //public void LoadDanhMuc(ViewModelSearchNT_NUOCNGOT_MANNO Searchmodel)
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
        //    // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
        //    ViewBag.DMNhomDoiTuongNuoi = new SelectList(_context.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM", Searchmodel.NhomDoiTuongNuoi);
        //    ViewBag.DMDoiTuongNuoi = new SelectList(_context.DM_DOITUONG_NUOI, "ID", "TEN_DOI_TUONG", Searchmodel.DoiTuongNuoi);
        //    ViewBag.DMMoHinhNuoi = new SelectList(CategoryCommon.DM_MOHINH_NUOIs, "ID", "TEN", Searchmodel.MoHinhNuoi);
        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        //}

        //public void LoadDanhMuc()
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
        //    ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");

        //    //Load loại mặt nước:
        //    string sLoaiMatNuocs = "";
        //    _context.DM_LOAI_MATNUOC_NGOTMANLO.ToList().ForEach(d =>
        //    {
        //        sLoaiMatNuocs += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_LOAI + "</option>";
        //    });

        //    ViewBag.DMLoaiMatNuoc = sLoaiMatNuocs;

        //    //Load nhóm đối tượng:
        //    string sNhomDoiTuongNuois = "<option value></option>";
        //    _context.DM_NHOMDOITUONG_NUOI.Where(o=>o.ID_LOAI_MATNUOC_NUOI==1).ToList().ForEach(d =>
        //    {
        //        sNhomDoiTuongNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_NHOM + "</option>";
        //    });

        //    ViewBag.DMNhomDoiTuongNuoi = sNhomDoiTuongNuois;

        //    //Load đối tượng:
        //    string sDoiTuongNuois = "<option value></option>";
        //    _context.DM_DOITUONG_NUOI.ToList().ForEach(d =>
        //    {
        //        sDoiTuongNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
        //    });

        //    ViewBag.DMDoiTuongNuoi = sDoiTuongNuois;

        //    //Load Mô hình nuôi:
        //    string sMoHinhNuois = "";
        //    CategoryCommon.DM_MOHINH_NUOIs.ForEach(d =>
        //    {
        //        sMoHinhNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN + "</option>";
        //    });
        //    ViewBag.DMMoHinhNuoi = sMoHinhNuois;



        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        //}

        //#endregion

    }
}