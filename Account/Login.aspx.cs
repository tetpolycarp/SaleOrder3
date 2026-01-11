using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI.WebControls;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

namespace SaintPolycarp.BanhChung
{
   
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                if (User.Identity.IsAuthenticated)
                {
                    StatusText.Text = string.Format("Hello {0}!!", User.Identity.GetUserName());
                    LoginStatus.Visible = true;
                    LogoutButton.Visible = true;
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
                //if login successful, go the the previous page
                if (this.Request.QueryString["ReturnUrl"] != null)
                {
                    this.Response.Redirect(Request.QueryString["ReturnUrl"].ToString());
                }
                else
                {
                    this.Response.Redirect("~/Account/Login.aspx");
                }

                //write to log
                SharedMethod.WriteLog(user.UserName + "signed in", "Desktop Login Form", SharedConstants.ACTIVITY_LOGS_FILE);
            }
            else
            {
                StatusText.Text = "Invalid username or password.";
                LoginStatus.Visible = true;
                //write to log
                SharedMethod.WriteLog(UserName.Text + "attempt to sign-in but with wrong password.", "Desktop Login Form", SharedConstants.ACTIVITY_LOGS_FILE);
            }
        }

        protected void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}