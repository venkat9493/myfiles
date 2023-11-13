using System;
using Microsoft.EntityFrameworkCore;
using Meridian.UserManagement.Core.Models;

namespace Meridian.UserManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserAndRoleInfo> UserAndRoleInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(e => e.UserId);

        }
    }
}