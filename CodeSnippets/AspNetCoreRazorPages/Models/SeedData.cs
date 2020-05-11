using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AspNetCoreRazorPages.Data;

namespace AspNetCoreRazorPages.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AspNetCoreRazorPagesContext(
                serviceProvider.GetRequiredService<DbContextOptions<AspNetCoreRazorPagesContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = new DateTime(1989, 2, 12),
                        //DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = new DateTime(1984, 3, 13),
                        //DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = new DateTime(1986, 2, 23),
                        //DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = new DateTime(1959, 4, 15),
                        //DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M,
                        Rating = "NA"
                    },

                    new Movie
                    {
                        Title = "The Terminator",
                        ReleaseDate = new DateTime(1984, 10, 4),
                        //DateTime.Parse("1959-4-15"),
                        Genre = "Science fiction",
                        Price = 8.99M,
                        Rating = "R"
                    }


                ); ;
                context.SaveChanges();
            }
        }
    }
}
