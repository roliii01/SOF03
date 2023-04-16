using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SOF03.Models;
using System.Reflection.Emit;

namespace SOF03.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            builder.Entity<JobSiteUser>()
                .HasKey(jsu => new { jsu.JobUid, jsu.SiteUserId });

            builder.Entity<Job>()
                .Property(j => j.Uid)
                .HasMaxLength(100);

            builder.Entity<JobSiteUser>()
                .Property(jsu => jsu.JobUid)
                .HasMaxLength(100);

            builder.Entity<JobSiteUser>()
                .Property(jsu => jsu.SiteUserId)
                .HasMaxLength(450);

            builder.Entity<JobSiteUser>()
                .HasOne(jsu => jsu.Job)
                .WithMany(j => j.JobSiteUsers)
                .HasForeignKey(jsu => jsu.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobSiteUser>()
                .HasOne(jsu => jsu.SiteUser)
                .WithMany(su => su.Jobs)
                .HasForeignKey(jsu => jsu.SiteUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Job>()
                .HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}