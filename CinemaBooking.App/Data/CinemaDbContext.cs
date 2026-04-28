using CinemaBooking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.App.Data;

public class CinemaDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<MovieSession> Sessions => Set<MovieSession>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=cinema.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Seat)
            .WithOne(x => x.Booking)
            .HasForeignKey<Booking>(x => x.SeatId);
    }
}
