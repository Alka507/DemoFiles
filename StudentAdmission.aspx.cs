using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using WebBusLogic;
using System.IO;
using System.Data.SqlClient;
using System.Collections;

public partial class Admission_StudentAdmission : System.Web.UI.Page
{
    StudentAdmission sa =new StudentAdmission();
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindClss();
            bindSess();

        }
        lblMsg.Text = "";
        
    }

    public void bindClss()
    {
        ddlClass.DataSource = sa.bindClass();
        ddlClass.DataTextField = "Class";
        ddlClass.DataValueField = "ID";
        ddlClass.DataBind();
    }

    public void bindSess()
    {
        ddlSeesion.DataSource = sa.bindSession();
        ddlSeesion.DataTextField = "SessionValue";
        ddlSeesion.DataValueField = "Yr_ID";
        ddlSeesion.DataBind();

    }
    ClsDataBase cls = new ClsDataBase();
   
    studentMasterClass sm = new studentMasterClass();
    public void AdmNo()
    {
      
        if (((DataTable)sm.getAdmNo()).Rows.Count > 0)
        {
            lblLastAdNo.Text += ((DataTable)sm.getAdmNo()).Rows[0].ItemArray[0].ToString();
        }
    }
    

    protected void btnShow_Click(object sender, EventArgs e)
    {
        
        gvSA.DataSource = sa.getCandidateForAdmission(Convert.ToInt16(ddlClass.SelectedValue),Convert.ToInt16(ddlSeesion.SelectedValue));
        gvSA.DataBind();
        if (gvSA.Rows.Count <= 0)
        {
            lblMsg.Text = "No Student Record Found Regarding This Class ";
        }
        else
        {
           
            //RangeValidator1.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            //RangeValidator1.MinimumValue = new DateTime(1600, 01, 01).ToString("dd/MM/yyyy");
            //RangeValidator1.ControlToValidate = "";
        }
       
    }

    protected void gvSA_RowDataBound(object sender, GridViewRowEventArgs e)
    { 
        DropDownList dl = (DropDownList)e.Row.FindControl("ddlSection");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            dl.DataSource = sa.bindSection(Convert.ToInt16(ddlClass.SelectedValue),Convert.ToInt32(ddlSeesion.Text));
            dl.DataTextField = "Section";
            dl.DataValueField = "ID";
            dl.DataBind();
            dl.Items.Insert(0, "--Select--");
            dl.Items[0].Value = "0";
            TextBox admDate = e.Row.FindControl("enrollDt") as TextBox;
            admDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            admDate.Attributes.Add("readonly", "readonly");
            DropDownList ddlGridClass = (DropDownList)e.Row.FindControl("ddlClass");
            ddlGridClass.DataSource = sa.bindClass();
            ddlGridClass.DataTextField = "Class";
            ddlGridClass.DataValueField = "id";
            ddlGridClass.DataBind();
            ddlGridClass.Items.FindByText(ddlClass.SelectedItem.Text).Selected = true;
            AjaxControlToolkit.CalendarExtender range = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender2");
            range.StartDate =Convert.ToDateTime(e.Row.Cells[5].Text);
            range.EndDate = DateTime.Now;
          


        }
    }
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList rd = (DropDownList)sender;
        GridViewRow r = (GridViewRow)rd.Parent.NamingContainer;
        DropDownList ddlGridClass = (DropDownList)gvSA.Rows[r.RowIndex].FindControl("ddlClass");
        DropDownList dl = (DropDownList)gvSA.Rows[r.RowIndex].FindControl("ddlSection");
        dl.DataSource = sa.bindSection(Convert.ToInt16(ddlGridClass.SelectedValue), Convert.ToInt32(ddlSeesion.Text));
        dl.DataTextField = "Section";
        dl.DataValueField = "ID";  
        dl.DataBind();
        dl.Items.Insert(0, "--Select--");
        dl.Items[0].Value = "0";
       
    }
    protected void lnk_click(object sender, EventArgs e)
    {
        LinkButton rd = (LinkButton)sender;
        GridViewRow r = (GridViewRow)rd.Parent.NamingContainer;
        LinkButton lnkenroll = (LinkButton)gvSA.Rows[r.RowIndex].FindControl("lnk");
        DataTable dt = sa.getstudentDetails(lnkenroll.Text.Trim());
        if (dt.Rows.Count > 0)
        {
            PopPanel.Show();

           

        }
    }



    public static string GeneratePWD()
    {

        int passwordLength = 5;

        int quantity = 1;

        ArrayList arrCharPool = new ArrayList();

        Random rndNum = new Random();

        arrCharPool.Clear();

        string password = "";

        //Lower Case

        for (int i = 97; i < 123; i++)
        {

            arrCharPool.Add(Convert.ToChar(i).ToString());

        }

        //Number

        for (int i = 48; i < 58; i++)
        {

            arrCharPool.Add(Convert.ToChar(i).ToString());

        }

        //Upper Case

        for (int i = 65; i < 91; i++)
        {

            arrCharPool.Add(Convert.ToChar(i).ToString());

        }

        for (int x = 0; x < quantity; x++)
        {

            //Iterate through the number of characters required in the password

            for (int i = 0; i < passwordLength; i++)
            {

                password += arrCharPool[rndNum.Next(arrCharPool.Count)].ToString();

            }

        }

        return password;

    }
    SMS sms = new SMS();
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        if (gvSA.Rows.Count > 0)
        {
            string status = "A";
            for (int j = 0; j < gvSA.Rows.Count; j++)
            {
                if (((CheckBox)(gvSA.Rows[j].FindControl("CheckBox1"))).Checked == true)
                {

                    AdmNo();
                    TextBox tx = (TextBox)gvSA.Rows[j].Cells[6].FindControl("enrollDt");
                    DropDownList ddlGridClass = (DropDownList)gvSA.Rows[j].Cells[7].FindControl("ddlClass");
                    DropDownList ddlsection = (DropDownList)gvSA.Rows[j].Cells[9].FindControl("ddlSection");
                    FileInfo fin = new FileInfo(Server.MapPath(gvSA.DataKeys[j].Values["ImageURL"].ToString()));
                    int flag = 0;
                    if (ddlsection.Items.Count > 1)
                    {
                        if (ddlsection.Text != "0")
                        {
                            flag = 1;
                        }
                        else
                        {
                            flag = 0;
                            status = "I";
                        }
                    }
                    else
                    {
                        flag = 1;
                    }
                    if (flag == 1)
                    {
                        DataTable dt = sa.getMaxRollNo(Convert.ToInt32(ddlClass.SelectedValue), Convert.ToInt32(((DropDownList)(gvSA.Rows[j].FindControl("ddlSection"))).SelectedValue));
                        lblMsg.Text = sa.CreateStudent(Convert.ToInt32(gvSA.DataKeys[j].Value.ToString()), Convert.ToInt32(ddlSeesion.SelectedValue), Convert.ToInt32(ddlGridClass.SelectedValue), dt.Rows[0].ItemArray[0].ToString(), ((DropDownList)(gvSA.Rows[j].FindControl("ddlSection"))).SelectedValue, gvSA.DataKeys[j].Values["Fthr_sms"].ToString(), Convert.ToInt32(ViewState["smsconfrm"]), tx.Text.Trim(), lblLastAdNo.Text.Trim());
                        lblpass.Text = GeneratePWD();
                        Membership.CreateUser(lblLastAdNo.Text.Trim(), lblpass.Text.Trim());
                        Roles.AddUserToRole(lblLastAdNo.Text.Trim(), "Student");
                        
                        if (fin.Exists)
                        {

                            File.Copy(Server.MapPath(gvSA.DataKeys[j].Values["ImageURL"].ToString()), Server.MapPath("~/Stud_Image/" + Convert.ToString(Convert.ToInt32(lblLastAdNo.Text)) + "." + SessVal.SessionID + Path.GetExtension(gvSA.DataKeys[j].Values["ImageURL"].ToString())), true);
                        }
                        lblLastAdNo.Text = "";
                    }
                }

            }

            gvSA.DataSource = sa.getCandidateForAdmission(Convert.ToInt16(ddlClass.SelectedValue), Convert.ToInt16(ddlSeesion.SelectedValue));
            gvSA.DataBind();
            if (gvSA.Rows.Count <= 0)
            {
                lblMsg.Text = "Admission has done successfully";
            }
            if (gvSA.Rows.Count > 0 && lblMsg.Text == "Admission has done successfully" && status == "I")
            {
                lblMsg.Text = "Admission has done successfully, And Select Section For Rest Admission Process";
            }
            else
            {
                lblMsg.Text = "Admission has done successfully";

            }
            

        }

        else
        {
            lblMsg.Text = "Record not found for Process!!!";
        }
        lblLastAdNo.Text = "";
    }



    protected void chkConfirm_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["smsconfrm"] = 1;
        
    }
    protected void gvSA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}