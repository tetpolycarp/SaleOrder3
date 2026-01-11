using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Mitchell.ScmConsoles
{
    public partial class Startup {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and also store information about a user logging in with a third party login provider.
            // This is required if your application allows users to login
            /*Gary note: since we create this application using "Individual Authentication"
           // now we need to switch and use window authentication which ultilize Active Directory
           //so need to comment the redirect to Login page
           app.UseCookieAuthentication(new CookieAuthenticationOptions
           {
               AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
               LoginPath = new PathString("/Account/Login")
           });
           app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
           */

           // Uncomment the following lines to enable logging in with third party login providers
           //app.UseMicrosoftAccountAuthentication(
           //    clientId: "",
           //    clientSecret: "");

           //app.UseTwitterAuthentication(
           //   consumerKey: "",
           //   consumerSecret: "");

           //app.UseFacebookAuthentication(
           //   appId: "",
           //   appSecret: "");

           //app.UseGoogleAuthentication();
       }
   }
}
