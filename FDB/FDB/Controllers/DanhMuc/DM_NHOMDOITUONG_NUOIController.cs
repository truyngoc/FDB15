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
    public class DM_NHOMDOITUONG_NUOIController : FDBController
    {
        //private FDBContext db = new FDBContext();

     
        //public ActionResult Index(string txtTenNhom)
        //{
        //    var result = db.DM_NHOMDOITUONG_NUOI.Where(d => (txtTenNhom == null || txtTenNhom == "") || d.TEN_NHOM.ToUpper().Contains(txtTenNhom.ToUpper()));

        //    ViewBag.TotalRow = result.Count();

        //    return View(result.ToList());
        //}


     
        //public ActionResult Create()
        //{
        //    return View();
        //}

    
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(DM_NHOMDOITUONG_NUOI dm_doituong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DM_NHOMDOITUONG_NUOI.Add(dm_doituong);
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }

        //    return View(dm_doituong);
        //}

        
        //public ActionResult Edit(int id)
        //{
        //    DM_NHOMDOITUONG_NUOI dm_doituong = db.DM_NHOMDOITUONG_NUOI.Find(id);
        //    if (dm_doituong == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dm_doituong);
        //}

       

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(DM_NHOMDOITUONG_NUOI dm_doituong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(dm_doituong).State = EntityState.Modified;
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    return View(dm_doituong);
        //}

        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        DM_NHOMDOITUONG_NUOI dm_doituong = db.DM_NHOMDOITUONG_NUOI.Find(id);
        //        db.DM_NHOMDOITUONG_NUOI.Remove(dm_doituong);
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        this.Error("Không thể xóa nhóm đối tượng nuôi khi tồn tại đối tượng nuôi thuộc nhóm, hoặc đối tượng nuôi thuộc nhóm đã được sử dụng");
        //        return RedirectToAction("Index");
        //    }
            
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