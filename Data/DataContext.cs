using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.Data;
using System.Reflection.Emit;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> op) : base(op)
        {
            
        }
        
        public DbSet<User> User { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Reminder> Reminders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
          
            builder.Entity<User>()
                 .HasMany<Tasks>().WithOne(t => t.User).HasForeignKey(r=>r.UserId1).OnDelete(DeleteBehavior.Cascade);

           

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasNoKey();
             
            } );


        }

        //protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        //{
        //    builder.Properties<DateTime>().HaveConversion<TimeOnlyConverter>().HaveColumnType("time");
        //    base.ConfigureConventions(builder);
        //}

      
    }
}
