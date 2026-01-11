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
using SaintPolycarp.BanhChung.SharedConstansts;
public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SharedMethod.RedirectIfOnMobileDevice(Request, Response);
        if (User.Identity.IsAuthenticated)
        {
            LoginForm.Visible = false;
        }
        else
        {
            LoginForm.Visible = true;
        }

    }
    protected void SignIn(object sender, EventArgs e)
    {

        var userStore = new UserStore<IdentityUser>();
        var userManager = new UserManager<IdentityUser>(userStore);
        IdentityUser user = userManager.Find(UserName.Text, Password.Text);

        if (user != null)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddMinutes(60) }, userIdentity);
            //if login successful, go to the current page

            this.Response.Redirect("~/Default.aspx");

            //write to log
            SharedMethod.WriteLog(user.UserName + "signed in", "Desktop Login Form", SharedConstants.ACTIVITY_LOGS_FILE);

        }
        else
        {
            StatusText.Text = "Invalid username or password.";
            LoginStatus.Visible = true;
            //write to log
            SharedMethod.WriteLog(UserName.Text + " attempt to sign-in but with wrong password.", "Desktop Login Form", SharedConstants.ACTIVITY_LOGS_FILE);
        }
    }

    protected void SignOut(object sender, EventArgs e)
    {
        var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        authenticationManager.SignOut();
        Response.Redirect("~/Default.aspx");
    }
}