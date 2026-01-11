using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Text.RegularExpressions;
using SaintPolycarp.BanhChung.SharedMethods;

namespace SaintPolycarp.BanhChung
{
    public partial class Inventory : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isReadOnly = false;
                if (!string.IsNullOrEmpty(Request.QueryString["Mode"]))
                {
                    if (Request.QueryString["Mode"].ToUpper().Equals("READONLY"))
                    {
                        isReadOnly = true;
                        ButtonEdit.Visible = false;
                    }
                }

                //skip this permission if it's readonly
                if (!isReadOnly)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string LoginPageUrl = rootURL + "Account/Login.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                    }
                    else
                    {
                        //check again, if user login but not inventory admin, then reject as well.
                        if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "inventory admin"))
                        {
                            string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                            string OriginalUrl = HttpContext.Current.Request.RawUrl;
                            string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                            HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Client"]))
                {
                    //display the iframe Desktop mode
                    if (Request.QueryString["Client"].ToUpper().Equals("DESKTOP"))
                    {
                        DesktopView.Visible = true;
                    }
                    //if this is mobile phone, display the iframe readonly but also display the button to edit the googlesheet directly in google
                    else if (Request.QueryString["Client"].ToUpper().Equals("MOBILE"))
                    {
                        MobileView.Visible = true;
                        MobileButtonView.Visible = true;
                    }
                }
                
            }
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect(@"https://docs.google.com/spreadsheets/d/1i0WCKcebnyzmJn6C5hYO0n8ctIUsMypS3UoUCVw84vQ/edit?usp=sharing");

        }
    }
}