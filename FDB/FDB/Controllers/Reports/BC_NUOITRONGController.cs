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
    public class BC_NUOITRONGController : Controller
    {
        //
        // GET: /REPORTS/

        public ActionResult NT_BC_MAU1(NT_BC_MAU1 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "NT_BC_MAU1.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/NuoiTrong/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/NuoiTrong/NT_BC_MAU1.rpt"));

                    // rd.SetDataSource

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    System.IO.Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream, "application/pdf");
                }
            }
            return View();
        }

        public ActionResult NT_BC_MAU2(NT_BC_MAU2 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "NT_BC_MAU2.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/NuoiTrong/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/NuoiTrong/NT_BC_MAU2.rpt"));

                    // rd.SetDataSource

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    System.IO.Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream, "application/pdf");
                }
            }
            return View();
        }

        public ActionResult NT_BC_MAU3(NT_BC_MAU3 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "NT_BC_MAU3.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/NuoiTrong/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/NuoiTrong/NT_BC_MAU3.rpt"));

                    // rd.SetDataSource

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    System.IO.Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream, "application/pdf");
                }
            }
            return View();
        }

        public ActionResult NT_BC_MAU4(NT_BC_MAU4 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "NT_BC_MAU4.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/NuoiTrong/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/NuoiTrong/NT_BC_MAU4.rpt"));

                    // rd.SetDataSource

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    System.IO.Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream, "application/pdf");
                }
            }
            return View();
        }

        public ActionResult NT_BC_MAU5(NT_BC_MAU5 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "NT_BC_MAU5.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/NuoiTrong/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/NuoiTrong/NT_BC_MAU5.rpt"));

                    // rd.SetDataSource

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    System.IO.Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream, "application/pdf");
                }
            }
            return View();
        }
	}
}