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
    public class BC_KHAITHACController : Controller
    {
        //
        // GET: /REPORTS/

        public ActionResult KT_BC_MAU1(KT_BC_MAU1 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU1.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU1.rpt"));

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

        public ActionResult KT_BC_MAU2(KT_BC_MAU2 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU2.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU2.rpt"));

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

        public ActionResult KT_BC_MAU3(KT_BC_MAU3 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU3.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU3.rpt"));

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

        public ActionResult KT_BC_MAU4(KT_BC_MAU4 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU4.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU4.rpt"));

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

        public ActionResult KT_BC_MAU5(KT_BC_MAU5 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU5.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU5.rpt"));

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

        public ActionResult KT_BC_MAU6(KT_BC_MAU6 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU6.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}


            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU6.rpt"));

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

        public ActionResult KT_BC_MAU7(KT_BC_MAU7 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU7.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU7.rpt"));

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

        public ActionResult KT_BC_MAU8(KT_BC_MAU8 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU8.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU8.rpt"));

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

        public ActionResult KT_BC_MAU9(KT_BC_MAU9 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU9.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU9.rpt"));

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

        public ActionResult KT_BC_MAU10(KT_BC_MAU10 bc)
        {
            // 1. Popup aspnet webform
            //if (bc.SearchButton == "Generate report")
            //{
            //    // Setting session for generating report
            //    this.HttpContext.Session["ReportName"] = "KT_BC_MAU10.rpt";
            //    //this.HttpContext.Session["rptSource"] = GetStudents();

            //    // Redirecting generic report viewer page from action
            //    return Redirect("~/Reports/KHAITHAC/Reports.aspx");
            //}

            // 2. Generate PDF view
            if (bc.SearchButton == "Generate report")
            {
                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(Server.MapPath("~/Reports/KhaiThac/KT_BC_MAU10.rpt"));

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