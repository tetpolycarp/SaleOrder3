using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Telerik.Web.UI;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

namespace SaintPolycarp.BanhChung
{
    public partial class DefaultMobile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
           
                if (User.Identity.IsAuthenticated)
                {
                    LoginForm.Visible = false;
                }
                else
                {
                    LoginForm.Visible = true;
                }
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
                
                 this.Response.Redirect("~/DefaultMobile.aspx");

                //write to log
                SharedMethod.WriteLog(user.UserName + "signed in", "Mobile Login Form", SharedConstants.ACTIVITY_LOGS_FILE);

            }
            else
            {
                StatusText.Text = "Invalid username or password.";
                LoginStatus.Visible = true;
                //write to log
                SharedMethod.WriteLog(UserName.Text + " attempt to sign-in but with wrong password.", "Mobile Login Form", SharedConstants.ACTIVITY_LOGS_FILE);
            }
        }

        protected void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/DefaultMobile.aspx");
        }
    }
}