using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities.Context
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FridgeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeModelConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Fridge>? Fridge { get; set; }

        public DbSet<FridgeModel>? FridgeModels { get; set; }

        public DbSet<FridgeProduct>? FridgeProducts { get; set; }

        public DbSet<Product>? Products { get; set; }
    }
}
