using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    LINQClassDataContext lnq = new LINQClassDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindPanding();
            bindCompleted();
        }
    }
    public void bindPanding()
    {
        // LINQ query
        var x = from y in lnq.Patient_General_Informations orderby y.PatientId_PK descending
                where y.Status == "Pending" &&
                    y.FacilityId_FK == Convert.ToInt32(Session["FacilityId"].ToString())
                select y;
        gvDetail.DataSource = x;
        gvDetail.DataBind();
        gvDetail.UseAccessibleHeader = true;
        if (gvDetail.Rows.Count > 0)
            gvDetail.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    public void bindCompleted()
    {
        //LINQ Query
        var x = from y in lnq.Patient_General_Informations
                orderby y.PatientId_PK descending
                where y.Status == "Final" &&
                    y.FacilityId_FK == Convert.ToInt32(Session["FacilityId"].ToString())
                select y;
        gvComplete.DataSource = x;
        gvComplete.DataBind();
        gvComplete.UseAccessibleHeader = true;
        if (gvComplete.Rows.Count > 0)
            gvComplete.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    
}