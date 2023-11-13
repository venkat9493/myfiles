using System;
using Microsoft.EntityFrameworkCore;


namespace Meridian.CatalogManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //public DbSet<Tenant> Tenant { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Tenant>().HasKey(e => e.TenantId);

        //}
    }
}