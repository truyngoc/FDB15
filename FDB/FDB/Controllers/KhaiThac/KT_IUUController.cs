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

namespace FDB.Controllers.KhaiThac
{
    public class KT_IUUController : FDBController
    {
        //FDBContext db = new FDBContext();
        //// GET: /KT_IUU/

        //public void BindComboDM()
        //{

        //    ViewBag.DTINHTPs = new SelectList(db.DTINHTP, "MA_TINHTP", "TEN_TINHTP");

        //    //Load đối tượng IUU:
        //    string sDoiTuong_KT_IUUs ="";
        //    db.DM_DOITUONG_KT_IUU.ToList().ForEach(d =>
        //    {
        //        sDoiTuong_KT_IUUs += "<option value=\"" + d.ID.ToString() + "\">" + d.Name + "</option>";
        //    });

        //    ViewBag.DM_DOITUONG_KT_IUU = sDoiTuong_KT_IUUs;
        //}
        //public ActionResult ListXN(ViewModelSearchKT_IUU_XN SearchModel)
        //{
        //    BindComboDM();



        //    var KT_IUU_XNs = db.KT_IUU_GIAYXN_NGUYENLIEU.Where(o => (SearchModel.SO_XN == null || o.SO_XN == SearchModel.SO_XN)
        //                                                       && (SearchModel.NGAY_XN == null || o.NGAY_XN == SearchModel.NGAY_XN)
        //                                                       && (SearchModel.SO_DK_TAU == null || o.SO_DK_TAU == SearchModel.SO_DK_TAU)
        //                                                        && (SearchModel.MA_TINHTP == null || o.MA_TINHTP == SearchModel.MA_TINHTP)

        //                                             ).Select(x => new { x.SO_XN, x.NGAY_XN, x.SO_DK_TAU, x.TU_NGAY, x.DEN_NGAY, x.VUNG_KHAITHAC, x.CANG_DANGKY, x.DTINHTP }).OrderBy(m => m.NGAY_XN);



        //    List<KT_IUU_GIAYXN_NGUYENLIEU> DSKT_IUU_XN = new List<KT_IUU_GIAYXN_NGUYENLIEU>();
        //    foreach (var kt_iuu_xn in KT_IUU_XNs)
        //    {
        //        DSKT_IUU_XN.Add(new KT_IUU_GIAYXN_NGUYENLIEU
        //        {
        //            //ID = kt_iuu_xn.ID,
        //            SO_XN = kt_iuu_xn.SO_XN,
        //            NGAY_XN = kt_iuu_xn.NGAY_XN,
        //            SO_DK_TAU = kt_iuu_xn.SO_DK_TAU,
        //            TU_NGAY = kt_iuu_xn.TU_NGAY,
        //            DEN_NGAY = kt_iuu_xn.DEN_NGAY,
        //            VUNG_KHAITHAC = kt_iuu_xn.VUNG_KHAITHAC,
        //            CANG_DANGKY = kt_iuu_xn.CANG_DANGKY,
        //          DTINHTP = kt_iuu_xn.DTINHTP
                    
        //        });
        //    }
        //    ViewBag.TotalRow = DSKT_IUU_XN.Count().ToString();
        //    //Phân trang ở đây:

        //    var pageIndex = SearchModel.Page ?? 1;
        //    SearchModel.SearchResults = DSKT_IUU_XN.ToPagedList(pageIndex, Constants.PageSize);

        //    return View(SearchModel);

        //}

      

        //public ActionResult CreateXN()
        //{
        //    BindComboDM();
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateXN(FormCollection _form, KT_IUU_GIAYXN_NGUYENLIEU _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DM_DOITUONG_KT_IUU_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        //Save Header
               
        //        db.KT_IUU_GIAYXN_NGUYENLIEU.Add(_obj);
        //        db.SaveChanges();

        //        string SO_XN_Detail = _obj.SO_XN.Trim();

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                KT_IUU_GIAYXN_NGUYENLIEU_DETAIL _objDetail = new Models.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.SO_XN = SO_XN_Detail;
        //                db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.Add(_objDetail);
        //            }
        //            //Save data:
        //            db.SaveChanges();
        //        }

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, "giấy xác nhận nguyên liệu thủy sản khai thác"));
        //        return RedirectToAction("ListXN");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.BindComboDM();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML_XN(lstInt, _form, ref maxID);
        //        ViewBag.AddHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult EditXN(String id)
        //{
        //   // this.BindComboDM();
        //   // int _SO_XN = int.Parse(SO_XN);
        //   //// var model = db.KT_IUU_GIAYXN_NGUYENLIEU.First(o => o.SO_XN.ToUpper().Contains(SO_XN.ToUpper()));
        //   // var model = db.KT_IUU_GIAYXN_NGUYENLIEU.First(o => o.SO_XN == _SO_XN);
            
        //   // return View(model);

        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    KT_IUU_GIAYXN_NGUYENLIEU model = db.KT_IUU_GIAYXN_NGUYENLIEU.Find(id);

        //    if (model == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    BindComboDM();

        //    return View(model);
        
        
        //}

        //[HttpPost]
        ////async Task<ActionResult>
        //public ActionResult EditXN(FormCollection _form, KT_IUU_GIAYXN_NGUYENLIEU _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DM_DOITUONG_KT_IUU_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        var model = db.KT_IUU_GIAYXN_NGUYENLIEU.First(o => o.SO_XN == _obj.SO_XN);
        //        FDB.Common.Helpers.CopyObject<KT_IUU_GIAYXN_NGUYENLIEU>(_obj, ref model);
        //        //model.NGUOI_NHAP = User.Identity.Name;
        //        var dbEntityEntry = db.Entry(model);

        //        db.KT_IUU_GIAYXN_NGUYENLIEU.Attach(model);
        //        db.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        //Xóa những detail cũ:
        //        //  _context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).ToList().ForEach(o => _context.NT_NUOILONGBE_DETAIL.Remove(o));
        //        db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.RemoveRange(db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.Where(o => o.SO_XN == _obj.SO_XN).AsEnumerable());
        //        //Thêm mới detail đã sửa
        //        string SO_XN_ = _obj.SO_XN.Trim();

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                KT_IUU_GIAYXN_NGUYENLIEU_DETAIL _objDetail = new Models.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.SO_XN = SO_XN_;
        //                db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.Add(_objDetail);
        //            }

        //        }
        //        //Save data:
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, "cấp giấy xác nhận nguyên liệu thủy sản khai thác"));
        //        return RedirectToAction("ListXN");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.BindComboDM();
        //        _obj.DSKT_IUU_GIAYXN_NGUYENLIEU_DETAILs = new List<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL>();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML_XN(lstInt, _form, ref maxID);
        //        ViewBag.sEditHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult DeleteXN(string id)
        //{
        //    //Xóa header
        //    KT_IUU_GIAYXN_NGUYENLIEU _obj = db.KT_IUU_GIAYXN_NGUYENLIEU.Find(id);
        //    db.KT_IUU_GIAYXN_NGUYENLIEU.Remove(_obj);
        //    db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.RemoveRange(db.KT_IUU_GIAYXN_NGUYENLIEU_DETAIL.Where(o => o.SO_XN == _obj.SO_XN).AsEnumerable());
        //    //  _context.NT_NUOILONGBE_DETAIL.Where(o => o.ID_NUOILONGBE == _obj.ID).ToList().ForEach(o => _context.NT_NUOILONGBE_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    db.SaveChanges();
        //    this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, "giấy xác nhận nguyên liệu thủy sản khai thác"));
        //    return RedirectToAction("ListXN");
        //}
        //private String GenderHTML_XN(List<int> lstInt, FormCollection _form, ref int MaxID)
        //{
        //    String strHTML = "";
        //    int _maxID = 0;
        //    if (lstInt != null && lstInt.Count > 0)
        //    {

        //        List<DM_DOITUONG_KT_IUU> lstDM_DOITUONG_KT_IUU = db.DM_DOITUONG_KT_IUU.ToList();
        //        List<DM_DOITUONG_KT_IUU> lstDM_DOITUONG_KT_IUUSelected = new List<DM_DOITUONG_KT_IUU>();
        //        //int ID_NhomDoiTuongSelected = 1;

        //        for (int i = 0; i < lstInt.Count; i++)
        //        {

        //            //lstDM_DOITUONG_KT_IUUSelected = lstDM_DOITUONG_KT_IUU.Where(o => o.DM_NHOMDOITUONG_NUOIID == ID_NhomDoiTuongSelected).ToList();
        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString() + "\" id=\"ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString() + "\"><option value></option>";
        //            lstDM_DOITUONG_KT_IUUSelected.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.Name + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.Name + "</option>";
        //            }
        //                );

        //            strHTML += "</select>";
        //            strHTML += "</td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_KHAITHAC_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_KHAITHAC_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_NGUYENLIEU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_NGUYENLIEU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_DACHUNGNHAN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_DACHUNGNHAN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_TON_SAUCHUNGNHAN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_TON_SAUCHUNGNHAN_" + lstInt[i].ToString() + "\"/></td>";                 
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
        //public ActionResult ListCN(ViewModelSearchKT_IUU_CN SearchModel)
        //{
        //    BindComboDM();

        //    var KT_IUU_CNs = db.KT_IUU_GIAYCHUNGNHAN.Where(o => (SearchModel.SO_CN == null || o.SO_CN == SearchModel.SO_CN)
        //                                                       && (SearchModel.NGAY_CN == null || o.NGAY_CN == SearchModel.NGAY_CN)
        //                                                       && (SearchModel.TEN_DN == null || o.TEN_DN == SearchModel.TEN_DN)
        //                                                        && (SearchModel.MA_TINHTP == null || o.MA_TINHTP == SearchModel.MA_TINHTP)
        //                                                        && (SearchModel.MST == null || o.MST == SearchModel.MST)
        //                                                        && (SearchModel.DIACHI == null || o.DIACHI == SearchModel.DIACHI)

        //                                             ).Select(x => new { x.SO_CN, x.NGAY_CN, x.TEN_DN, x.MST, x.DIACHI, x.NUOC_DEN, x.DTINHTP }).OrderBy(m => m.NGAY_CN);



        //    List<KT_IUU_GIAYCHUNGNHAN> DSKT_IUU_CN = new List<KT_IUU_GIAYCHUNGNHAN>();
        //    foreach (var kt_iuu_cn in KT_IUU_CNs)
        //    {
        //        DSKT_IUU_CN.Add(new KT_IUU_GIAYCHUNGNHAN
        //        {
        //            //ID = kt_iuu_xn.ID,
        //            SO_CN = kt_iuu_cn.SO_CN,
        //            NGAY_CN = kt_iuu_cn.NGAY_CN,
        //            TEN_DN = kt_iuu_cn.TEN_DN,
        //            MST = kt_iuu_cn.MST,
        //            DIACHI = kt_iuu_cn.DIACHI,
        //            NUOC_DEN = kt_iuu_cn.NUOC_DEN,                   
        //            DTINHTP = kt_iuu_cn.DTINHTP

        //        });
        //    }
        //    ViewBag.TotalRow = DSKT_IUU_CN.Count().ToString();
        //    //Phân trang ở đây:

        //    var pageIndex = SearchModel.Page ?? 1;
        //    SearchModel.SearchResults = DSKT_IUU_CN.ToPagedList(pageIndex, Constants.PageSize);

        //    return View(SearchModel);

        //}

        //public ActionResult CreateCN()
        //{
        //    BindComboDM();
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateCN(FormCollection _form, KT_IUU_GIAYCHUNGNHAN _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DM_DOITUONG_KT_IUU_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        //Save Header

        //        db.KT_IUU_GIAYCHUNGNHAN.Add(_obj);
        //        db.SaveChanges();

        //        string SO_CN_Detail = _obj.SO_CN.Trim();

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                KT_IUU_GIAYCHUNGNHAN_DETAIL _objDetail = new Models.KT_IUU_GIAYCHUNGNHAN_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<KT_IUU_GIAYCHUNGNHAN_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.SO_CN = SO_CN_Detail;
        //                db.KT_IUU_GIAYCHUNGNHAN_DETAIL.Add(_objDetail);
        //            }
        //            //Save data:
        //            db.SaveChanges();
        //        }

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, "giấy chứng nhận thủy sản khai thác"));
        //        return RedirectToAction("ListCN");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.BindComboDM();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML_CN(lstInt, _form, ref maxID);
        //        ViewBag.AddHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult EditCN(String id)
        //{
          

        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    KT_IUU_GIAYCHUNGNHAN model = db.KT_IUU_GIAYCHUNGNHAN.Find(id);

        //    if (model == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    BindComboDM();

        //    return View(model);


        //}

        //[HttpPost]
        ////async Task<ActionResult>
        //public ActionResult EditCN(FormCollection _form, KT_IUU_GIAYCHUNGNHAN _obj)
        //{
        //    List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DM_DOITUONG_KT_IUU_")).ToList<String>();
        //    List<int> lstInt = new List<int>();
        //    lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

        //    if (ModelState.IsValid)
        //    {
        //        var model = db.KT_IUU_GIAYCHUNGNHAN.First(o => o.SO_CN == _obj.SO_CN);
        //        FDB.Common.Helpers.CopyObject<KT_IUU_GIAYCHUNGNHAN>(_obj, ref model);
        //        //model.NGUOI_NHAP = User.Identity.Name;
        //        var dbEntityEntry = db.Entry(model);

        //        db.KT_IUU_GIAYCHUNGNHAN.Attach(model);
        //        db.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        //Xóa những detail cũ:
                
        //        db.KT_IUU_GIAYCHUNGNHAN_DETAIL.RemoveRange(db.KT_IUU_GIAYCHUNGNHAN_DETAIL.Where(o => o.SO_CN == _obj.SO_CN).AsEnumerable());
        //        //Thêm mới detail đã sửa
        //        string SO_CN_ = _obj.SO_CN.Trim();

        //        if (lstKeyName != null)
        //        {

        //            for (int i = 0; i < lstInt.Count; i++)
        //            {
        //                KT_IUU_GIAYCHUNGNHAN_DETAIL _objDetail = new Models.KT_IUU_GIAYCHUNGNHAN_DETAIL();
        //                FDB.Common.Helpers.GetValueForm<KT_IUU_GIAYCHUNGNHAN_DETAIL>(_form, lstInt[i], ref _objDetail);
        //                _objDetail.SO_CN = SO_CN_;
        //                db.KT_IUU_GIAYCHUNGNHAN_DETAIL.Add(_objDetail);
        //            }

        //        }
        //        //Save data:
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, "giấy chứng nhận thủy sản khai thác"));
        //        return RedirectToAction("ListCN");
        //    }
        //    else
        //    {
        //        TempData["_SUCCESS"] = "";
        //        this.BindComboDM();
        //        _obj.DSKT_IUU_GIAYCHUNGNHAN_DETAILs = new List<KT_IUU_GIAYCHUNGNHAN_DETAIL>();
        //        //build html :
        //        int maxID = 0;
        //        String strHTML = this.GenderHTML_XN(lstInt, _form, ref maxID);
        //        ViewBag.sEditHTML = strHTML;
        //        ViewBag.sMaxID = maxID + 1;
        //        return View(_obj);
        //    }
        //}

        //public ActionResult DeleteCN(string id)
        //{
        //    //Xóa header
        //    KT_IUU_GIAYCHUNGNHAN _obj = db.KT_IUU_GIAYCHUNGNHAN.Find(id);
        //    db.KT_IUU_GIAYCHUNGNHAN.Remove(_obj);
        //    db.KT_IUU_GIAYCHUNGNHAN_DETAIL.RemoveRange(db.KT_IUU_GIAYCHUNGNHAN_DETAIL.Where(o => o.SO_CN == _obj.SO_CN).AsEnumerable());
           
        //    //Update thay đổi vào DB
        //    db.SaveChanges();
        //    this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, "giấy chứng nhận thủy sản khai thác"));
        //    return RedirectToAction("ListCN");
        //}
        //private String GenderHTML_CN(List<int> lstInt, FormCollection _form, ref int MaxID)
        //{
        //    String strHTML = "";
        //    int _maxID = 0;
        //    if (lstInt != null && lstInt.Count > 0)
        //    {

        //        List<DM_DOITUONG_KT_IUU> lstDM_DOITUONG_KT_IUU = db.DM_DOITUONG_KT_IUU.ToList();
        //        List<DM_DOITUONG_KT_IUU> lstDM_DOITUONG_KT_IUUSelected = new List<DM_DOITUONG_KT_IUU>();
        //        //int ID_NhomDoiTuongSelected = 1;

        //        for (int i = 0; i < lstInt.Count; i++)
        //        {

        //            //lstDM_DOITUONG_KT_IUUSelected = lstDM_DOITUONG_KT_IUU.Where(o => o.DM_NHOMDOITUONG_NUOIID == ID_NhomDoiTuongSelected).ToList();
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSO_XN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSO_XN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"date\" value=\"" + _form["txtNGAY_XN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNGAY_XN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSO_DK_TAU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSO_DK_TAU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString() + "\" id=\"ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString() + "\"><option value></option>";
        //            lstDM_DOITUONG_KT_IUUSelected.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_DM_DOITUONG_KT_IUU_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.Name + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.Name + "</option>";
        //            }
        //                );

        //            strHTML += "</select>";
        //            strHTML += "</td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtMA_SP_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtMA_SP_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_DUOC_CHUNGNHAN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_DUOC_CHUNGNHAN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_KHAITHAC_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_KHAITHAC_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtKL_NL_DA_XACNHAN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtKL_NL_DA_XACNHAN_" + lstInt[i].ToString() + "\"/></td>";
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
        //[HttpPost]
        //public JsonResult check_SO_XN_Exist(string SO_XN, int isEdit)
        //{
        //    if (isEdit == 0)
        //    {
        //        var so_xn = db.KT_IUU_GIAYXN_NGUYENLIEU.FirstOrDefault(m => m.SO_XN == SO_XN);

        //        return Json(so_xn == null);
        //    }

        //    return Json(true);
        //}


        //[HttpPost]
        //public JsonResult check_SO_CN_Exist(string SO_CN, int isEdit)
        //{
        //    if (isEdit == 0)
        //    {
        //        var so_cn = db.KT_IUU_GIAYCHUNGNHAN.FirstOrDefault(m => m.SO_CN == SO_CN);

        //        return Json(so_cn == null);
        //    }

        //    return Json(true);
        //}
	}
}