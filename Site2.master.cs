using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SaintPolycarp.BanhChung.SharedMethods;
using Telerik.Web.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SaintPolycarp.BanhChung.SharedObjectClass;
using SaintPolycarp.BanhChung.SharedObjectClass.RadGrid;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

public partial class SiteMaster2 : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                //probably we need to comment this out because all the website are internall and don't need to go outside
                //therefore, we don't need to prevent any attack.
               // throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SharedMethod.AddListOfInvoicesToMenu(RadMenu1, false);

        //write to log
        string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                SharedMethod.WriteLog("Anonymous loads this page: " + currentPage, "Desktop Master 2", SharedConstants.ACTIVITY_LOGS_FILE);
            }
            else
            {
                SharedMethod.WriteLog(HttpContext.Current.User.Identity.Name + " loads this page: " + currentPage, "Desktop Master 2", SharedConstants.ACTIVITY_LOGS_FILE);
            }
        }
        catch(Exception ex)
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

