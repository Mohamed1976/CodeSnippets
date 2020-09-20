using DataLibrary.MusicStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.MusicStore.Data
{
    public class MusicContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        public MusicContext() : base() { }
        public MusicContext(DbContextOptions opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Album>().HasMany(a => a.Songs).WithOne(s => s.Album); // One-To-Many
            builder.Entity<Album>().HasData(
                new Album { Id = 1, Name = "The Court of the Crimson King" },
                new Album { Id = 2, Name = "Smile (Katy Perry song)" }
            );
            // https://wildermuth.com/2018/08/12/Seeding-Related-Entities-in-EF-Core-2-1-s-HasData()
            builder.Entity<Song>().HasData(
                new { Id = 1, AlbumId = 1, Name = "21st Century Schzoid Man", Duration = 110 },
                new { Id = 2, AlbumId = 1, Name = "I Talk to the Wind", Duration = 140 },
                new { Id = 3, AlbumId = 2, Name = "Happy days are coming to you", Duration = 89 },
                new { Id = 4, AlbumId = 2, Name = "Fly through the wind", Duration = 46 },
                new { Id = 5, AlbumId = 2, Name = "Love like its the last day", Duration = 71 }
            );
        }
    }
}
