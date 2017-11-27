using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CarFinder_API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
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

        public DbSet<Car> Cars { get; set; }

        public async Task<List<string>> GetYears()
        {
            return await Database.SqlQuery<string>("DistinctYears").ToListAsync();
        }

        public async Task<List<string>> GetMakes(string year)
        {
            return await Database.SqlQuery<string>("MakesByYear @Year",
                new SqlParameter("Year", year)).ToListAsync();
        }

        public async Task<List<string>> GetModels(string year, string make)
        {
            return await Database.SqlQuery<string>("AllModelForMakeYear @Year, @Make",
                new SqlParameter("Year", year),
                new SqlParameter("Make", make)).ToListAsync();
        }

        public async Task<List<string>> GetTrims(string year, string make, string model)
        {
            return await Database.SqlQuery<string>("AllTrimsByYearMakeModel @Year, @Make, @Model",
                new SqlParameter("Year", year),
                new SqlParameter("Make", make),
                new SqlParameter("Model", model)).ToListAsync();
        }

        public async Task<Car> GetCar(string year, string make, string model, string trim)
        {
            return await Database.SqlQuery<Car>("AllCarsForYearMakeModelTrim @Year, @Make, @Model, @Trim",
                new SqlParameter("Year", year),
                new SqlParameter("Make", make),
                new SqlParameter("Model", model),
                new SqlParameter("Trim", trim)).FirstOrDefaultAsync();
        }
    }
}