using DotNetNow.Domain;
using DotNetNow.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetNow.Persistence
{
    public class CoreDbContext : IdentityDbContext<AppUser>, IWebCoreAppDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
        }


        public DbSet<Category> Categories { set; get; }
        public DbSet<Product> Products { set; get; }
    }
}