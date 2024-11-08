using Microsoft.EntityFrameworkCore;

namespace EventHandlerJoshProper.Entities
{
    public class EventManagerDbContext : DbContext
    {
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options) { }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().HasData(
                new Guest() {
                    GuestId = 1,
                    Name = "Janice",
                    Email = "Janice@gmail.com",
                    Status = Guest.EnrollmentConfirmationStatus.EnrollmentConfirmed,
                    EventId = 1
                },
                new Guest(){
                    GuestId = 2,
                    Name = "Steve",
                    Email = "Steve@gmail.com",
                    EventId = 1
                }
                );
            modelBuilder.Entity<Event>().HasData(
                new Event() { 
                    EventId = 1,
                    Name = "Birthday Party",
                    Host = "John Kelly",
                    Date = new DateTime(2024, 10, 31),
                    RoomNum = "2C21"
                });
        }
    }
}
