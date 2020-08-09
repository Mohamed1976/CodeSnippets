using System;
using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

/*
Install Nuget Pomelo.EntityFrameworkCore.MySql in library (DataLibrary)

Install Nuget Microsoft.EntityFrameworkCore.Design in client application (EFCoreDBDemo)
Install Nuget Microsoft.EntityFrameworkCore.Tools  in client application (EFCoreDBDemo)

Use the following command to create WorldContext and Models
C:\repos\CodeSnippets\CodeSnippets>dotnet ef dbcontext scaffold 
"server=localhost;database=world;user=UserName;pwd=MyPassword" "Pomelo.EntityFrameworkCore.MySql" 
-o .\Models -f --project DataLibrary --startup-project EFCoreDBDemo

Build started...
Build succeeded.

https://rajbos.github.io/blog/2020/04/23/EntityFramework-Core-NET-Standard-Migrations
*/
namespace DataLibrary.Data
{
    public partial class worldContext : DbContext
    {
        public worldContext()
        {
        }

        public worldContext(DbContextOptions<worldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Continent> Continent { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Region> Region { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.CountryCode)
                    .HasName("CountryCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnType("char(3)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnType("char(20)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("char(35)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("city_ibfk_1");
            });

            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("continent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("char(20)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.ToTable("country");

                entity.HasIndex(e => e.ContinentId)
                    .HasName("Country_Continent_idx");

                entity.HasIndex(e => e.RegionId)
                    .HasName("Country_Region_idx");

                entity.Property(e => e.Code)
                    .HasColumnType("char(3)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Code2)
                    .IsRequired()
                    .HasColumnType("char(2)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContinentId)
                    .HasColumnName("ContinentID")
                    .HasDefaultValueSql("'3'");

                entity.Property(e => e.Gnp)
                    .HasColumnName("GNP")
                    .HasColumnType("float(10,2)");

                entity.Property(e => e.Gnpold)
                    .HasColumnName("GNPOld")
                    .HasColumnType("float(10,2)");

                entity.Property(e => e.GovernmentForm)
                    .IsRequired()
                    .HasColumnType("char(45)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.HeadOfState)
                    .HasColumnType("char(60)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.LifeExpectancy).HasColumnType("float(3,1)");

                entity.Property(e => e.LocalName)
                    .IsRequired()
                    .HasColumnType("char(45)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("char(52)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SurfaceArea).HasColumnType("float(10,2)");

                entity.HasOne(d => d.Continent)
                    .WithMany(p => p.Country)
                    .HasForeignKey(d => d.ContinentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Country_Continent");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Country)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Country_Region");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => new { e.CountryCode, e.Name })
                    .HasName("PRIMARY");

                entity.ToTable("language");

                entity.HasIndex(e => e.CountryCode)
                    .HasName("CountryCode");

                entity.Property(e => e.CountryCode)
                    .HasColumnType("char(3)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("char(30)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.IsOfficial)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Percentage).HasColumnType("float(4,1)");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Language)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("countryLanguage_ibfk_1");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("region");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("char(40)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
