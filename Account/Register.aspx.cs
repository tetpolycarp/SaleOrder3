using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using SaintPolycarp.BanhChung.SharedMethods;

//tutorial  https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/adding-aspnet-identity-to-an-empty-or-existing-web-forms-project

namespace SaintPolycarp.BanhChung
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if user hasn't login, then need to ask user to do so
                if (!User.Identity.IsAuthenticated)
                {
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    string OriginalUrl = HttpContext.Current.Request.RawUrl;
                    string LoginPageUrl = rootURL + "Account/Login.aspx";
                    HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                }
                else //check again, if user login but not financial admin, then reject as well.
                {
                    //if the current login user is not admin
                    if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "global admin"))
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                    }
                }

            }


        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            // Default UserStore constructor uses the default connection string named: DefaultConnection
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var user = new IdentityUser() { UserName = UserName.Text };

            IdentityResult result = manager.Create(user, Password.Text);

            if (result.Succeeded)
            {
                //add user into as regular role if it's selected
                if (CheckBoxUser.Checked)
                {
                    //if the admin role not there yet, then add it in
                    if (!SharedIdentityMethod.RoleExists("regular user"))
                    {
                        SharedIdentityMethod.AddRole("regular user");
                    }
                    SharedIdentityMethod.AddUserToRole(user.Id, "regular user");
                }

                //add user into inventory admin role if it's selected
                if (CheckBoxInventoryAdmin.Checked)
                {
                    //if the admin role not there yet, then add it in
                    if (!SharedIdentityMethod.RoleExists("inventory admin"))
                    {
                        SharedIdentityMethod.AddRole("inventory admin");
                    }
                    SharedIdentityMethod.AddUserToRole(user.Id, "inventory admin");
                }

                //add user into financial admin role if it's selected
                if (CheckBoxFinancialAdmin.Checked)
                {
                    //if the admin role not there yet, then add it in
                    if (!SharedIdentityMethod.RoleExists("financial admin"))
                    {
                        SharedIdentityMethod.AddRole("financial admin");
                    }
                    SharedIdentityMethod.AddUserToRole(user.Id, "financial admin");
                }

                //add user into global admin role if it's selected
                if (CheckBoxGlobalAdmin.Checked)
                {
                    //if the admin role not there yet, then add it in
                    if(!SharedIdentityMethod.RoleExists("gloal admin"))
                    {
                        SharedIdentityMethod.AddRole("global admin");
                    }
                    SharedIdentityMethod.AddUserToRole(user.Id, "global admin");
                }
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                //Gary's notes: send SMS text message to notify user
                string baseURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~");

                string message = string.Format("Gian Hàng Tết St. Polycarp - Xuân Bính Ngọ 2026. Your account has created as username \"{0}\" and password \"{1}\". Please try to log into [{2}]. Any issues, please contact Chau Nguyen. ", UserName.Text, Password.Text, baseURL);

                SharedMethod.SendNewSmsTextMessage(PhoneNumber.Text, message);

                //Gary's notes: don't need to redirect and sign in with the new created user
                //authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                Response.Redirect("~/Account/Register.aspx");
            }
            else
            {
                StatusMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }
    }
    public static class SharedIdentityMethod
    {
        public static void AddRole(string roleName)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(roleName));
        }
        public static void AddUserToRole(string userId, string roleName)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var idResult = userManager.AddToRole(userId, roleName);

        }
        public static bool UserBelongToRole(string userName, string roleName)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindByName(userName);
            if (user != null)
            {
                var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                foreach (IdentityUserRole role in user.Roles)
                {
                    var roleObj = rm.FindById(role.RoleId);
                    if (roleObj.Name.Equals(roleName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

      
        public static bool RoleExists(string roleName)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
           
            return rm.RoleExists(roleName);
        }
    }
    
}