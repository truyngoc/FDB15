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

namespace FDB.Controllers
{
    public class KT_NHATKYKHAITHACController : FDBController
    {
        //
        // GET: /KT_NHATKYKHAITHAC/
        public ActionResult Create()
        {
            return View();
        }

	}
}