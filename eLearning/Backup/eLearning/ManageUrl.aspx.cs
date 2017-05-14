using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using System.Text.RegularExpressions;

namespace eLearning
{
    public partial class ManageUrl : System.Web.UI.Page
    {
        DataManager dmObj = new DataManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                FillFieldGrid();
                lblErrorMsg.Text = "";
            //}
           
        }

        public void FillFieldGrid()
        {
            DataTable dt = new DataTable();
            dt = dmObj.SelectFields();
            gvFields.DataSource = dt;
            gvFields.DataBind();
            
            //gvFields.HeaderRow.Cells[0].Visible = false;
            //gvFields.HeaderRow.Cells[1].Text = "Field Name";
            //for (int i = 0; i < gvFields.Rows.Count; i++)
            //{
            //    gvFields.Rows[i].Cells[0].Visible = false;
            //}

        }

        protected void gvFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fieldId = 0;
            try
            {

                //fieldId = Int32.Parse(gvFields.SelectedRow.Cells[0].Text);
                Label lblId = (Label)gvFields.SelectedRow.FindControl("lblID");
                fieldId = Int32.Parse(lblId.Text);
                FillUrlGrid(fieldId);
                pnlAddUrl.Visible = false;
                btnAddUrl.Visible = true;
            }
            catch (Exception ex)
            { }
        }

        protected void gvFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count >= 1)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'");
                    e.Row.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(gvFields, "Select$" + e.Row.RowIndex.ToString()));
                }
        }

        public void FillUrlGrid(int fieldId)
        {
            DataTable dtUrl = new DataTable();
            if (fieldId > 0)
            {
                dtUrl = dmObj.SelectUrlListByField(fieldId);
                //dtUrl.Columns.Add("Delete");

                if (dtUrl != null && dtUrl.Rows.Count > 0)
                {
                    gvUrls.DataSource = dtUrl;
                    gvUrls.DataBind();
                    //gvUrls.HeaderRow.Cells[0].Visible = false;
                    //gvUrls.HeaderRow.Cells[1].Visible = false;
                    //gvUrls.HeaderRow.Cells[2].Text = "Authentic Urls";
                    //for (int i = 0; i < gvUrls.Rows.Count; i++)
                    //{
                    //    gvUrls.Rows[i].Cells[0].Visible = false;
                    //    gvUrls.Rows[i].Cells[1].Visible = false;
                    //}
                }
                else
                {
                    gvUrls.DataSource = dtUrl;
                    gvUrls.DataBind();
                }
            }
            else
            {
                gvUrls.DataSource = dtUrl;
                gvUrls.DataBind();
            }

        }

        protected void gvUrls_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btnDel = sender as Button;
            GridViewRow row = (GridViewRow)btnDel.NamingContainer;
            Label lblId = (Label)row.FindControl("lblID");
            Label lblFieldId = (Label)row.FindControl("lblFieldID");
            int fieldId = Int32.Parse(lblFieldId.Text);
            int urlId = Int32.Parse(lblId.Text);
            if (urlId > 0)
            {
                string result = dmObj.DeleteUrl(urlId);
                if (result.Contains("Success"))
                {
                    FillUrlGrid(fieldId);
                }
                lblErrorMsg.Text = result;
            }
        }

        protected void btnSaveUrl_Click(object sender, EventArgs e)
        {
            string result = "";
            int fieldId = 0;
            try
            {
                string regEx = @"^(ht|f|sf)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
                string myString = txtUrl.Text;
                if (Regex.IsMatch(myString, regEx))
                {
                    Label lblId = (Label)gvFields.SelectedRow.FindControl("lblID");
                    fieldId = Int32.Parse(lblId.Text);
                    if (fieldId > 0)
                    {
                        if (txtUrl.Text != "")
                            result = dmObj.InsertUrl(fieldId, txtUrl.Text);
                        else
                            result = "Fail: Cannot insert blank";
                        FillUrlGrid(fieldId);
                        txtUrl.Text = "";
                    }
                    else
                        result = "Fail: No field is selected";
                }
                else
                {
                    result = "Fali: Invalid Url";
                }
                lblErrorMsg.Text = result;

                //Uri myUri;
                //if (Uri.IsWellFormedUriString(myString, UriKind.RelativeOrAbsolute))
                //{ 
                //}
                //else 
                //{
                //}
                
            }
            catch (Exception ex)
            { }
        }

        protected void btnAddUrl_Click(object sender, EventArgs e)
        {
            pnlAddUrl.Visible = true;
        }
    }
}