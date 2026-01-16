using Enoca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Carrier> Carriers => Set<Carrier>();
        public DbSet<CarrierConfiguration> CarrierConfigurations => Set<CarrierConfiguration>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

       
            modelBuilder.Entity<Carrier>(entity =>
            {
                entity.HasKey(x => x.CarrierId);

                entity.Property(x => x.CarrierName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(x => x.CarrierIsActive)
                      .IsRequired();

                entity.Property(x => x.CarrierPlusDesiCost)
                      .IsRequired();

                entity.HasMany(x => x.CarrierConfigurations)
                      .WithOne(x => x.Carrier)
                      .HasForeignKey(x => x.CarrierId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Orders)
                      .WithOne(x => x.Carrier)
                      .HasForeignKey(x => x.CarrierId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

    
            modelBuilder.Entity<CarrierConfiguration>(entity =>
            {
                entity.HasKey(x => x.CarrierConfigurationId);

                entity.Property(x => x.CarrierId)
                      .IsRequired();

                entity.Property(x => x.CarrierMinDesi)
                      .IsRequired();

                entity.Property(x => x.CarrierMaxDesi)
                      .IsRequired();

                entity.Property(x => x.CarrierCost)
                      .IsRequired()
                      .HasPrecision(18, 2);
            });

      
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.OrderId);

                entity.Property(x => x.CarrierId)
                      .IsRequired();

                entity.Property(x => x.OrderDesi)
                      .IsRequired();

                entity.Property(x => x.OrderDate)
                      .IsRequired();

                entity.Property(x => x.OrderCarrierCost)
                      .IsRequired()
                      .HasPrecision(18, 2);
            });
        }
    }
}
