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
    public class NT_COSO_HATANGController : FDBController
    {
        //private FDBContext _context = new FDBContext();

        ////
        //// GET: /SanXuatGiong/

        //public ActionResult Index(ViewModelSearchNT_COSO_HATANG Searchmodel)
        //{
        //    //Load danh mục:
        //    this.LoadDanhMuc(Searchmodel);

        //    var models = _context.NT_COSO_HATANG.Where(o => (Searchmodel.TThanhPho == null || o.MA_TINHTP == Searchmodel.TThanhPho)
        //                                                && (Searchmodel.Qhuyen == null || o.MA_QUANHUYEN == Searchmodel.Qhuyen)
        //                                                && (Searchmodel.Thang == null || o.THANG == Searchmodel.Thang)
        //                                                && (Searchmodel.Nam == null || o.NAM == Searchmodel.Nam)
        //                                                && (Searchmodel.DoiTuongNuoi == null || o.DSNT_CoSoHaTangDetail.Any(d => d.ID_NUOITRONG_DOITUONG == Searchmodel.DoiTuongNuoi))
        //                                    )
        //                                    ;

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
        //    NT_COSO_HATANG model = new NT_COSO_HATANG();
        //    model.NGUOI_NHAP = User.Identity.Name;
        //    model.NGAY_NHAP = DateTime.Now;
        //    model.THANG = DateTime.Now.Month;
        //    model.NAM = DateTime.Now.Year;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(FormCollection _form, NT_COSO_HATANG _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        //Save Header
        //        _obj.NGUOI_NHAP = User.Identity.Name;
        //        _context.NT_COSO_HATANG.Add(_obj);
        //        _context.SaveChanges();

        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_COSO_HATANG_DETAIL _objDetail = new Models.NT_COSO_HATANG_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_COSO_HATANG_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_COSO_HATANG = Id;
        //                _context.NT_COSO_HATANG_DETAIL.Add(_objDetail);
        //            }
        //            //Save data:
        //            _context.SaveChanges();
        //        }

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, "Cơ sở hạ tầng"));
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
        //    var model = _context.NT_COSO_HATANG.First(o => o.ID == _ID);
        //    return View(model);
        //}

        //[HttpPost]
        ////async Task<ActionResult>
        //public ActionResult Edit(FormCollection _form, NT_COSO_HATANG _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOITRONG_DOITUONG_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        var model = _context.NT_COSO_HATANG.First(o => o.ID == _obj.ID);
        //        FDB.Common.Helpers.CopyObject<NT_COSO_HATANG>(_obj, ref model);
        //        model.NGUOI_NHAP = User.Identity.Name;
        //        var dbEntityEntry = _context.Entry(model);

        //        _context.NT_COSO_HATANG.Attach(model);
        //        _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        //Xóa những detail cũ:
        //        _context.NT_COSO_HATANG_DETAIL.Where(o => o.ID_COSO_HATANG == _obj.ID).ToList().ForEach(o => _context.NT_COSO_HATANG_DETAIL.Remove(o));

        //        //Thêm mới detail đã sửa
        //        int Id = _obj.ID;

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                NT_COSO_HATANG_DETAIL _objDetail = new Models.NT_COSO_HATANG_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<NT_COSO_HATANG_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.ID_COSO_HATANG = Id;
        //                _context.NT_COSO_HATANG_DETAIL.Add(_objDetail);
        //            }

        //        }
        //        //Save data:
        //        _context.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, "Cơ sở hạ tầng"));
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.LoadDanhMuc();
        //        _obj.DSNT_CoSoHaTangDetail = new List<NT_COSO_HATANG_DETAIL>();
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
        //    NT_COSO_HATANG _obj = _context.NT_COSO_HATANG.Find(id);
        //    _context.NT_COSO_HATANG.Remove(_obj);
        //    _context.NT_COSO_HATANG_DETAIL.Where(o => o.ID_COSO_HATANG == _obj.ID).ToList().ForEach(o => _context.NT_COSO_HATANG_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    _context.SaveChanges();
        //    this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, "Cơ sở hạ tầng"));
        //    return RedirectToAction("Index");
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
        //        List<DM_DOITUONG_NUOI> lstDMDoiTuongNuoi = _context.DM_DOITUONG_NUOI.ToList();

        //        for (int i = 0; i < lstInt.Count; i++)
        //        {
        //            strHTML += "<tr><td><select class=\"form-control\" name=\"ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString() + "\">";
        //            lstDMDoiTuongNuoi.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN_DOI_TUONG + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN_DOI_TUONG + "</option>";
        //            }
        //                );

        //            strHTML += "</select>";
        //            strHTML += "</td>";
        //            strHTML += "<td><input type=\"checkbox\""+ _form["chkAO_CHUA_" + lstInt[i].ToString()]=="on"?"checked":"" + " name=\"chkAO_CHUA_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkKENH_CAP_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkKENH_CAP_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkKENH_THOAT_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkKENH_THOAT_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkAO_XULY_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkAO_XULY_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkDAO_NUOC_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkDAO_NUOC_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkDIEN_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkDIEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"checkbox\"" + _form["chkTB_MOITRUONG_" + lstInt[i].ToString()] == "on" ? "checked" : "" + " name=\"chkTB_MOITRUONG_" + lstInt[i].ToString() + "\"/></td>";


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

        //public void LoadDanhMuc(ViewModelSearchNT_COSO_HATANG Searchmodel)
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
        //    ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
        //    ViewBag.DMDoiTuongNuoi = new SelectList(_context.DM_DOITUONG_NUOI, "ID", "TEN_DOI_TUONG", Searchmodel.DoiTuongNuoi);
        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        //}

        //public void LoadDanhMuc()
        //{
        //    //Load danh mục:
        //    ViewBag.DMTThanhPho = new SelectList(_context.DTINHTP, "MA_TINHTP", "TEN_TINHTP");
        //    ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");
        //    string sDoiTuongNuois = "";
        //    _context.DM_DOITUONG_NUOI.ToList().ForEach(d =>
        //    {
        //        sDoiTuongNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
        //    });

        //    ViewBag.DMDoiTuongNuoi = sDoiTuongNuois;
        //    ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        //}

        //#endregion

    }
}