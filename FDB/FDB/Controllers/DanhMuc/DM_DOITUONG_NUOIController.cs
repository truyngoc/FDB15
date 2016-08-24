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

namespace FDB.Controllers.DanhMuc
{
    public class DM_DOITUONG_NUOIController : FDBController
    {
        //private FDBContext db = new FDBContext();


        //public ActionResult Index(string txtTenNhom, string txtTenDoiTuong)
        //{
        //    var dm_doituong = db.DM_DOITUONG_NUOI.Include(d => d.DM_NHOMDOITUONG_NUOI).Where(d => ((txtTenNhom == null || txtTenNhom == "") || d.DM_NHOMDOITUONG_NUOI.TEN_NHOM.ToUpper().Contains(txtTenNhom.ToUpper()))
        //                                                                                    && ((txtTenDoiTuong == null || txtTenDoiTuong == "") || d.TEN_DOI_TUONG.ToUpper().Contains(txtTenDoiTuong.ToUpper()))

        //        );

        //    ViewBag.TotalRow = dm_doituong.Count();

        //    return View(dm_doituong.ToList());
        //}
         

       
        //public ActionResult Create()
        //{
        //    ViewBag.DM_NHOMDOITUONG_NUOIs = new SelectList(db.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM");
        //    return View();
        //}

        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(DM_DOITUONG_NUOI dm_doituong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DM_DOITUONG_NUOI.Add(dm_doituong);
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DM_NHOMDOITUONG_NUOIs = new SelectList(db.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM", dm_doituong.DM_NHOMDOITUONG_NUOIID);
        //    return View(dm_doituong);
        //}

        
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DM_DOITUONG_NUOI dm_doituong = db.DM_DOITUONG_NUOI.Find(id);
        //    if (dm_doituong == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DM_NHOMDOITUONG_NUOIs = new SelectList(db.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM", dm_doituong.DM_NHOMDOITUONG_NUOIID);
        //    return View(dm_doituong);
        //}

        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(DM_DOITUONG_NUOI dm_doituong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(dm_doituong).State = EntityState.Modified;
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DM_NHOMDOITUONG_NUOIs = new SelectList(db.DM_NHOMDOITUONG_NUOI, "ID", "TEN_NHOM", dm_doituong.DM_NHOMDOITUONG_NUOIID);
        //    return View(dm_doituong);
        //}

        
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DM_DOITUONG_NUOI dm_doituong = db.DM_DOITUONG_NUOI.Find(id);            
        //    db.DM_DOITUONG_NUOI.Remove(dm_doituong);
        //    db.SaveChanges();

        //    this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, string.Empty));

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
	}
}