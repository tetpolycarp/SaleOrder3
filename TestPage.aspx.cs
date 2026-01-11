using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
namespace Mitchell.ScmConsoles
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            Response.Write(DateTime.Now.ToString() + getPostBackControlID());
            if (!Page.IsPostBack)
            {
                loadQuoteURLs();
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox1.Text = "selectedchange";
        }

        protected void RadioButtonList1_TextChanged(object sender, EventArgs e)
        {
            TextBox1.Text = "TextChange";
        }

        protected void loadQuoteURLs()
        {

            DropDownList1.Items.Clear();
            DropDownList1.Items.Add("Quote");

        }

        protected void radioOn_CheckedChanged(object sender, EventArgs e)
        {
            loadQuoteURLs();
        }

        protected void radioOff_CheckedChanged(object sender, EventArgs e)
        {
            loadApplyURLs();
        }

        protected void loadApplyURLs()
        {

            DropDownList1.Items.Clear();
            DropDownList1.Items.Add("Apply");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write("Button");
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add("Button");
            updatePanelToggle.Update();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void RBL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Text = "Change";
        }

        protected void RBL1_TextChanged(object sender, EventArgs e)
        {
            Label1.Text += "TextCHange";
        }

        private string getPostBackControlID()
        {
            Control control = null;
            //first we will check the "__EVENTTARGET" because if post back made by       the controls
            //which used "_doPostBack" function also available in Request.Form collection.
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = Page.FindControl(ctrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to
            // iterate over the form collection to find it
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in Page.Request.Form)
                {
                    //handle ImageButton they having an additional "quasi-property" in their Id which identifies
                    //mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = Page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = Page.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control.ID;
        }
    }
}