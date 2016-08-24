using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Common;
using FDB.Models;
using FDB.DataAccessLayer;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;

namespace FDB.Controllers
{
    public class BC_TCTSController : Controller
    {
        //
        // GET: /BC_TCTS/
        public ActionResult KT_1_KTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/KTTS_1.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult KT_1a_KTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/KTTS_1a.pdf"));

                return File(stream, "application/pdf");

            }

            return View();

        }

        public ActionResult KT_2a_KTTS_N(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/KTTS_2a.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult KT_2b_KTTS_N(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/KTTS_2b.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult KT_3_KTTS_N(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/KTTS_3.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult NT_1_NTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/NTTS_1.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult NT_1a_NTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/NTTS_1a.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult NT_2a_NTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/NTTS_2a.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult NT_2b_NTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/NTTS_2b.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }

        public ActionResult NT_3_NTTS(BC_TCTS bc)
        {


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                System.IO.Stream stream = System.IO.File.OpenRead(Server.MapPath("~/Views/Reports/BC_TCTS/NTTS_3.pdf"));

                return File(stream, "application/pdf");

            }

            return View();
        }
	}
}