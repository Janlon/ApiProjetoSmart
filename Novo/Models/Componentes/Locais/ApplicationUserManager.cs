using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System.Data.Entity;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            ApplicationUserManager applicationUserManager1 = new ApplicationUserManager((IUserStore<ApplicationUser>)new UserStore<ApplicationUser>((DbContext)OwinContextExtensions.Get<ApplicationDbContext>(context)));
            ApplicationUserManager applicationUserManager2 = applicationUserManager1;
            UserValidator<ApplicationUser> userValidator1 = new UserValidator<ApplicationUser>((UserManager<ApplicationUser, string>)applicationUserManager1);
            userValidator1.AllowOnlyAlphanumericUserNames = false;
            userValidator1.RequireUniqueEmail = true;
            UserValidator<ApplicationUser> userValidator2 = userValidator1;
            applicationUserManager2.UserValidator = (IIdentityValidator<ApplicationUser>)userValidator2;
            applicationUserManager1.PasswordValidator = (IIdentityValidator<string>)new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            IDataProtectionProvider protectionProvider = options.DataProtectionProvider;
            if (protectionProvider != null)
                applicationUserManager1.UserTokenProvider = (IUserTokenProvider<ApplicationUser, string>)new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ASP.NET Identity"));
            return applicationUserManager1;
        }
    }
}
