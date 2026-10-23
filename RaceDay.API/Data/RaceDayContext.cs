
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(
            DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventEnrollment> EventEnrollments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<EventRoute> EventRoutes { get; set; }
        public DbSet<EventWeather> EventWeather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventEnrollment>()
                .HasKey(e => e.EnrollmentID);

            modelBuilder.Entity<EventRoute>()
                .HasKey(r => r.RouteID);

            modelBuilder.Entity<EventWeather>()
                .HasKey(w => w.WeatherID);
        }
    }
}


