using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;

namespace FDB.Reports.KhaiThac
{
    public partial class Reports : System.Web.UI.Page
    {
        private ReportDocument crReportDocument;

        protected void Page_Init(object sender, EventArgs e)
        {
            CrystalReportViewer1.Attributes.Add("onclick", "window.setTimeout(function() { _spFormOnSubmitCalled = false; }, 10);");
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            ShowReport();
        }

        public void ShowReport()
        {
            try
            {
                //CrystalReportViewer1.HasSearchButton = false;
                CrystalReportViewer1.HasCrystalLogo = false;
                // turn off tree group
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                bool isValid = true;
                // Setting ReportName
                string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                if (string.IsNullOrEmpty(strReportName))
                {
                    strReportName = "KT_BC_MAU1.rpt";
                }
                
                // Setting Report Data Source
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

                if (string.IsNullOrEmpty(strReportName)) // Checking is Report name provided or not
                {
                    isValid = false;
                }
                if (isValid) // If Report Name provided then do other operation
                {
                    //ReportDocument rd = new ReportDocument();
                    crReportDocument = new ReportDocument();
                    string strRptPath = Server.MapPath("~/") + "Reports/KhaiThac//" + strReportName;

                    //Loading Report
                    crReportDocument.Load(strRptPath);

                    // Setting report data source
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        crReportDocument.SetDataSource(rptSource);

                    CrystalReportViewer1.ReportSource = crReportDocument;
                    Session["ReportName"] = "";
                    Session["rptSource"] = "";
                }
                else
                {
                    Response.Write("Nothing found/ No report found");
                }
            }
            catch (Exception ex) { Response.Write(ex.ToString()); }
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (this.crReportDocument != null)
            {
                crReportDocument.Close();
                crReportDocument.Dispose();
                GC.Collect();
            }
        }
    }
}