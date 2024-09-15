using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEFCore : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEFCore(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserJobInfo> UserJobInfo { get; set; }
        public DbSet<UserSalary> UserSalary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    _config.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure()
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<Users>().HasKey(c => c.UserId);
            modelBuilder.Entity<UserJobInfo>().HasKey(c => c.UserId);
            modelBuilder.Entity<UserSalary>().HasKey(c => c.UserId);
        }
    }
}
