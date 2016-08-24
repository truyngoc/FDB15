using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Common;
using FDB.DataAccessLayer;
using FDB.Models;
using PagedList;

namespace FDB.Controllers.KhaiThac
{
    public class KT_TONGQUAN_NGHECAController : FDBController
    {
        //
        // GET: /KT_TONGQUAN_NGHECA/
        public ActionResult ListTongQuanNgheCa()
        {
            return View();
        }

        public ActionResult GiayXacNhan()
        {
            return View();
        }

	}
}