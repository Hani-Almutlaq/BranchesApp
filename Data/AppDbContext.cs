using BranchesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BranchesApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Day> Days { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Branch primary key configuration
            modelBuilder.Entity<Branch>().HasKey(b => b.BranchId);
            // Shift primary key configuration
            modelBuilder.Entity<Shift>().HasKey(s => s.ShiftId);
            // Day primary key configuration
            modelBuilder.Entity<Day>().HasKey(d => d.DayId);

            // Define relation (Shift (M→1) Branch) and foreign key
            modelBuilder.Entity<Shift>().HasOne(s => s.Branch).WithMany().HasForeignKey(s => s.BranchId);
            // Define relation (Shift (M→1) Day) and foreign key
            modelBuilder.Entity<Shift>().HasOne(s => s.Day).WithMany().HasForeignKey(s => s.DayId);

            // Adding week days
            modelBuilder.Entity<Day>().HasData(
                new Day { DayId = 1, DayName = "Sunday" },
                new Day { DayId = 2, DayName = "Monday" },
                new Day { DayId = 3, DayName = "Tuesday" },
                new Day { DayId = 4, DayName = "Wednesday" },
                new Day { DayId = 5, DayName = "Thursday" },
                new Day { DayId = 6, DayName = "Friday" },
                new Day { DayId = 7, DayName = "Saturday" }
            );
        }
    }
}
