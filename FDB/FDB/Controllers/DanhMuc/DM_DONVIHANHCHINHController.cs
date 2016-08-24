using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using PagedList;

namespace FDB.Controllers.DanhMuc
{
    public class DM_DONVIHANHCHINHController : Controller
    {
        //private FDBContext db = new FDBContext();
        //public ActionResult Index(int? page, string txtMaDonVi, string txtTenDonVi)
        //{
        //    var model =db.DM_DONVIHANHCHINH.Where(d=>((txtMaDonVi==null||txtMaDonVi=="")||d.MA_DV.ToUpper().Contains(txtMaDonVi))
        //                                            && ((txtTenDonVi == null || txtTenDonVi == "") || d.TEN_DV.ToUpper().Contains(txtTenDonVi))
        //        );
        //    ViewBag.txtMaDonVi = txtMaDonVi;
        //    ViewBag.txtTenDonVi = txtTenDonVi;
        //    // phai Order thi moi paging dc
        //    model = model.OrderBy(m => m.Cap);

        //    int pageSize = 50;
        //    int pageNumber = page ?? 1;     // null-coalescing operator: return the value of page if it has a value, or return 1 if page is null.            

        //    return View(model.ToPagedList(pageNumber, pageSize));
        //}
	}
}