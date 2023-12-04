using CarWashApps.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWashApps.Models
{
    public class AppCtx : IdentityDbContext<User>
    {
        public AppCtx(DbContextOptions<AppCtx> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ListService> ListServices { get; set; }
        public DbSet<CostService> CostServices { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
