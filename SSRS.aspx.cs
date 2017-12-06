using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using Microsoft.Reporting.WebForms;
using System.Collections;

public partial class SSRS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                IReportServerCredentials irsc = new CustomReportCredentials("Admin", "1234", "http://192.168.122.0/ReportServer");
                ReportViewer1.ServerReport.ReportServerCredentials = irsc;
                ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://192.168.122.0/ReportServer");
                ReportViewer1.ServerReport.ReportPath = "/Default/test";
                ReportViewer1.ShowParameterPrompts = true;
                ReportViewer1.ShowPrintButton = true;
                ReportViewer1.ServerReport.Refresh();

            }
            catch(Exception ex) {
                //ex.Message();
            }
        }
    }
}

    
