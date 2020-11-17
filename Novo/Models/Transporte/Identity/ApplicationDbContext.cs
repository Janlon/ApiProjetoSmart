namespace WEBAPI_VOPAK.Models
{

    using Microsoft.AspNet.Identity.EntityFramework;


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", false) { }
        public static ApplicationDbContext Create() { return new ApplicationDbContext(); }

    }
}
