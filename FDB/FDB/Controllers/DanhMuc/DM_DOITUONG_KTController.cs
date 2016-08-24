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
using System.Data.Entity;

using Microsoft.AspNet.Identity;
namespace FDB.Controllers
{
    public class DM_DOITUONG_KTController : FDBController
    {
        //private FDBContext db = new FDBContext();


        //public ActionResult Index(ViewModelSearchDM_DOITUONG_KT SearchModel)
        //{
        //    initialCategorySearchAction(SearchModel);
        //    var DM_DOITUONG_KTs = db.DM_DOITUONG_KT.Where(o => (SearchModel.TEN_DOI_TUONG == null || o.TEN_DOI_TUONG.ToUpper().Contains(SearchModel.TEN_DOI_TUONG.ToUpper()))
        //                                              //&& (SearchModel.DM_NHOMDOITUONG_KTID == null || o.DM_NHOMDOITUONG_KTID == SearchModel.DM_NHOMDOITUONG_KTID)
        //                                  ).OrderBy(x => x.ID);

        //    ViewBag.TotalRow = DM_DOITUONG_KTs.Count();

        //    int pageSize = FDB.Common.Constants.PageSize;
        //    int pageNumber = SearchModel.Page ?? 1;

        //    SearchModel.SearchResults = DM_DOITUONG_KTs.ToPagedList(pageNumber, pageSize);

        //    return View(SearchModel);
        //}


        //public void initialCategoryCreateAction()
        //{
            
            
        //    //ViewBag.DMLoaiKhaiThac = new SelectList(db.DM_LOAI_MATNUOC_KT, "ID", "TEN_LOAI");

        //    ViewBag.NHOMDOITUONGs = new SelectList(db.DM_NHOMDOITUONG_KT, "ID", "TEN_NHOM");
        //}

        //public void initialCategoryEditAction(DM_DOITUONG_KT searchModel)
        //{

        //   // ViewBag.DMLoaiKhaiThac = new SelectList(db.DM_LOAI_MATNUOC_KT, "ID", "TEN_LOAI");
        //   ////them chi tieu ID cua loai khai thac thi se load duoc nhom doi tuong theo loai khai thac
        //   // var nhomdoituongKT = db.DM_NHOMDOITUONG_KT.Where(d => d.ID_LOAI_MATNUOC_KT == searchModel.LOAI_KHAI_THAC);
        //   // ViewBag.NHOMDOITUONGs = new SelectList(nhomdoituongKT, "ID", "TEN_NHOM", searchModel.DM_NHOMDOITUONG_KTID);
        //}
        //public void initialCategorySearchAction(ViewModelSearchDM_DOITUONG_KT searchModel)
        //{
        //    //ViewBag.DMLoaiKhaiThac = new SelectList(db.DM_LOAI_MATNUOC_KT, "ID", "TEN_LOAI", searchModel.LOAI_KHAI_THAC);

        //    //var nhomdoituongKT = db.DM_NHOMDOITUONG_KT.Where(d => d.ID_LOAI_MATNUOC_KT == searchModel.LOAI_KHAI_THAC);
        //    //ViewBag.NHOMDOITUONGs = new SelectList(nhomdoituongKT, "ID", "TEN_NHOM", searchModel.DM_NHOMDOITUONG_KTID);

        //}
        //private void BindComboDM()
        //{
          
        //    ViewBag.NHOMDOITUONGs = new SelectList(db.DM_NHOMDOITUONG_KT, "ID", "TEN_NHOM");
            
        //}

        //public ActionResult Create()
        //{
        //    initialCategoryCreateAction();
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(DM_DOITUONG_KT dm_doituong)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var checkExist = db.DM_DOITUONG_KT.Where(b => b.TEN_DOI_TUONG.ToUpper().Contains(dm_doituong.TEN_DOI_TUONG.ToUpper().Trim())
        //                                  //&& b.DM_NHOMDOITUONG_KTID == dm_doituong.DM_NHOMDOITUONG_KTID
        //                                  //&& b.LOAI_KHAI_THAC == dm_doituong.LOAI_KHAI_THAC
                                          
        //                              );

        //        string msgErrs = string.Empty;
        //        if (checkExist.Count() > 0)
        //        {
        //            DM_DOITUONG_KT obj = checkExist.FirstOrDefault();
        //            //var nhom = db.DM_NHOMDOITUONG_KT.Where(b => b.ID == dm_doituong.DM_NHOMDOITUONG_KTID).Select(b => b.TEN_NHOM);
        //            //var loai_khai_thac = db.DM_LOAI_MATNUOC_KT.Where(b => b.ID == dm_doituong.LOAI_KHAI_THAC).Select(b => b.TEN_LOAI);
        //            //msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}] thuộc nhóm đối tượng [{1}] và loại [{2}]", dm_doituong.TEN_DOI_TUONG.ToString(),obj.DM_NHOMDOITUONG_KT.TEN_NHOM.ToString(), obj.DM_LOAI_MATNUOC_KT.TEN_LOAI.ToString()) + "<br />";
        //            msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}]", dm_doituong.TEN_DOI_TUONG.ToString()) + "<br />";
                    
        //        }

        //        if (string.IsNullOrEmpty(msgErrs))
        //        {
        //            db.DM_DOITUONG_KT.Add(dm_doituong);
        //            db.SaveChanges();

        //            this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, string.Empty));

        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            Inline_Danger(msgErrs, true);
        //        }
              
        //    }

        //    initialCategoryCreateAction();
        //    return View(dm_doituong);
        //}


        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DM_DOITUONG_KT dm_doituong = db.DM_DOITUONG_KT.Find(id);
        //    if (dm_doituong == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    initialCategoryEditAction(dm_doituong);
        //    return View(dm_doituong);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(DM_DOITUONG_KT dm_doituong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(dm_doituong).State = EntityState.Modified;
        //        db.SaveChanges();

        //        this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    initialCategoryEditAction(dm_doituong);
        //    return View(dm_doituong);
        //}


        //public ActionResult Delete(int? id)
        //{
        //    try
        //    {
        //        DM_DOITUONG_KT dm_doituong = db.DM_DOITUONG_KT.Find(id);
        //        db.DM_DOITUONG_KT.Remove(dm_doituong);
        //        db.SaveChanges();

        //        this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, string.Empty));
        //    }
        //    catch
        //    {
        //        Inline_Danger("Đối tượng này đã được sử dụng tại chức năng khác nên không thể xóa");
        //    }
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //[HttpPost]
        //public ActionResult getNhomDoiTuongByMatNuoc(string ma_MatNuoc)
        //{
        //    if (String.IsNullOrEmpty(ma_MatNuoc))
        //        return Content(String.Join("", new Object[] { "", "" }));
        //    int IDLoaiMatNuoc = int.Parse(ma_MatNuoc);
        //    var Nghes = db.DM_NHOMDOITUONG_KT.Where(d => d.ID_LOAI_MATNUOC_KT == IDLoaiMatNuoc).Select(a => "<option value='" + a.ID + "'>" + a.TEN_NHOM + "</option>");

        //    return Content(string.Join("", Nghes));
        //}


        //[HttpPost]
        //public ActionResult getDoiTuongByNhom(string ma_NhomDoiTuong)
        //{
        //    if (String.IsNullOrEmpty(ma_NhomDoiTuong))
        //        return Content(String.Join("", new Object[] { "", "" }));

        //    int IDNhomDoiTuong = int.Parse(ma_NhomDoiTuong);
        //    var Nghes = db.DM_DOITUONG_KT.Where(d => d.DM_NHOMDOITUONG_KTID == IDNhomDoiTuong).Select(a => "<option value='" + a.ID + "'>" + a.TEN_DOI_TUONG + "</option>");

        //    return Content(string.Join("", Nghes));
        //}

	}
}