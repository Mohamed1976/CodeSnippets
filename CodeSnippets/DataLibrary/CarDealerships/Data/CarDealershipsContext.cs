using System;
using DataLibrary.CarDealerships.Models;
using DataLibrary.CarDealerships.Models.Base;
using DataLibrary.CarDealerships.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

//https://github.com/skimedic/presentations/tree/master/DOTNETCORE/Channel9_EFCoreShows/EntityFrameworkCoreExamples/Migrations
namespace DataLibrary.CarDealerships.Data
{
    public sealed class CarDealershipsContext : DbContext
    {
        public CarDealershipsContext(DbContextOptions<CarDealershipsContext> options) : base(options)
        {
        }

        public DbSet<CreditRisk> CreditRisks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomerOrderViewModel> CustomerOrderViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerOrderViewModel>(entity =>
            {
                entity.HasNoKey().ToView("CustomerOrderView", "dbo");
            });

            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.CreditRisks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CreditRisks_Customers");
                //entity.OwnsOne(o => o.PersonalInformation,
                //    pd =>
                //    {
                //        pd.Property(p => p.FirstName).HasColumnName(nameof(Person.FirstName));
                //        pd.Property(p => p.LastName).HasColumnName(nameof(Person.LastName));
                //    });

            });
            
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.OwnsOne(o => o.PersonalInformation,
                    pd =>
                    {
                        pd.Property(p => p.FirstName).HasColumnName(nameof(Person.FirstName));
                        pd.Property(p => p.LastName).HasColumnName(nameof(Person.LastName));
                    });

            });

            modelBuilder.Entity<Make>(entity =>
            {
                entity.HasMany(e => e.Cars)
                    .WithOne(c => c.MakeNavigation)
                    .HasForeignKey(k => k.MakeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Make_Inventory");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(d => d.CarNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Inventory");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");
                entity.HasIndex(cr => new { cr.CustomerId, cr.CarId }).IsUnique(true);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
