using System;
using Microsoft.EntityFrameworkCore;
using Meridian.PlatformManagement.Core.Models;

namespace Meridian.PlatformManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Platform> Platform { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Platform>().HasKey(e => e.PlatformId);

        }
    }
}