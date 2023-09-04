using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineStore.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ContactNo { get; set; }
		public bool ConfirmedEmail { get; set; }
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
		public DbSet<RegisterViewModel> RegisterViewModels { get; set; }
		public System.Data.Entity.DbSet<OnlineStore.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Shipping> Shippings { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.OrderDetail> OrderDetails { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Users.Assistant> Assistants { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Users.Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<OnlineStore.Models.Users.Driver> Drivers { get; set; }
	}
}