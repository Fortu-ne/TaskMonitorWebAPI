using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Models;

namespace TaskMonitorWebAPI.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> op) : base(op)
        {
            
        }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}
