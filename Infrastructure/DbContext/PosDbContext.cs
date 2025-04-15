using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ApplicationLayer.Interfaces.Infrastructure;

namespace Infrastructure.DbContext
{
    public class PosDbContext(DbContextOptions<PosDbContext> options) : IdentityDbContext<User>(options), IApplicationDbContext
    {
        private static readonly string DbPath = GetProjectRootDbPath();

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => { entity.ToTable("Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable("Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        private static string GetProjectRootDbPath()
        {
            var baseDir = AppContext.BaseDirectory;

            // Esto busca hacia arriba hasta encontrar el archivo .sln
            var dir = new DirectoryInfo(baseDir);
            while (dir != null && dir.GetFiles("*.sln").Length == 0)
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("No se encontró el archivo .sln al buscar hacia arriba.")!;

            return Path.Combine(dir.FullName, "pos.db");
        }

    }
}
