using System;
using System.Collections.Generic;
using System.Text;
using HaragApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HaragApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationDbUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasKey(c => new { c.AdID, c.UserId});
            base.OnModelCreating(modelBuilder);
       
        }
        public virtual DbSet<AdImage> AdImages { get; set; }
        public virtual DbSet<Advertisment> Advertisments { get; set; }
        public virtual DbSet<AnimalCategory> AnimalCategories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
       // public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
    }
}
