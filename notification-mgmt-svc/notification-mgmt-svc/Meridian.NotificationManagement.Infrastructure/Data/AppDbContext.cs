using System;
using Microsoft.EntityFrameworkCore;
using Meridian.NotificationManagement.Core.Models;

namespace Meridian.NotificationManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Template> EmailTemplate { get; set; }

        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Template>().HasKey(x => x.TemplateId);
        }


    }
}