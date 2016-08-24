using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;

namespace FDB.Controllers
{
    public class SanLuongController : Controller
    {
        //
        // GET: /SanLuong/
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
	}
}