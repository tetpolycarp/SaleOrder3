using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

using System.Web.UI.WebControls;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

public partial class Site_Mobile : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelBrowser.Text = "as " + SharedMethod.GetMobileDevice(Request, Response);
        //anh Chau doesn't want to display invoices to mobile version, so more curious people would see
        //SharedMethod.AddListOfInvoicesToMenu(RadMenu1, true);

        //write to log
        string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                SharedMethod.WriteLog("Anonymous loads this page: " + currentPage, "Mobile Master", SharedConstants.ACTIVITY_LOGS_FILE);
            }
            else
            {
                SharedMethod.WriteLog(HttpContext.Current.User.Identity.Name + " loads this page: " + currentPage, "Mobile Master", SharedConstants.ACTIVITY_LOGS_FILE);
            }
        }
        catch (Exception ex)
        {

        }


        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            RadMenu1.Visible = true;
        }
        else
        {
            RadMenu1.Visible = false;
        }



    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }

}
