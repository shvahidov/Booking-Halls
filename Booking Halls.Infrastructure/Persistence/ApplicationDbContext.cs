using Booking_Halls.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи Booking с User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User) // Один пользователь может забронировать несколько залов
                .WithMany(u => u.Bookings) // Один пользователь может иметь много бронирований
                .HasForeignKey(b => b.UserId) // Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); // При удалении пользователя - удалять все его бронирования
        }
    }
}
