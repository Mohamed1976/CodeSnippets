using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRazorPages.Data;
using AspNetCoreRazorPages.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreRazorPages.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly AspNetCoreRazorPages.Data.AspNetCoreRazorPagesContext _context;

        public IndexModel(AspNetCoreRazorPages.Data.AspNetCoreRazorPagesContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        //SearchString: contains the text users enter in the search text box.SearchString has 
        //the [BindProperty] attribute. [BindProperty] binds form values and query strings with 
        //the same name as the property. (SupportsGet = true) is required for binding on GET requests.
        //Genres: contains the list of genres.Genres allows the user to select a genre from the list.
        //SelectList requires using Microsoft.AspNetCore.Mvc.Rendering;
        //MovieGenre: contains the specific genre the user selects(for example, "Western").
        //Genres and MovieGenre are used later in this tutorial.

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        //Note MovieGenre is not decorated with [BindProperty(SupportsGet = true)]  
        //https://localhost:5001/Movies?MovieGenre=&SearchString=
        public async Task OnGetAsync()
        {
            // Use LINQ to get list of genres.
            //The following code is a LINQ query that retrieves all the genres from the database.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            //The query is only defined at this point, it has not been run against the database.
            //Deffered execution
            var movies = from m in _context.Movie
                         select m;

            //If the SearchString property is not null or empty, the movies query is 
            //modified to filter on the search string:
            //The s => s.Title.Contains() code is a Lambda Expression.Lambdas are used in 
            //method - based LINQ queries as arguments to standard query operator methods 
            //such as the Where method or Contains(used in the preceding code). 
            //LINQ queries are not executed when they're defined or when they're modified 
            //by calling a method(such as Where, Contains or OrderBy). Rather, 
            //query execution is deferred.That means the evaluation of an expression 
            //is delayed until its realized value is iterated over or the ToListAsync method is called.
            if (!string.IsNullOrEmpty(SearchString))
            {
                //The Contains method is run on the database, not in the C# code. 
                //The case sensitivity on the query depends on the database and the collation. 
                //On SQL Server, Contains maps to SQL LIKE, which is case insensitive. 
                //In SQLite, with the default collation, it's case sensitive.
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }

            //The SelectList of genres is created by projecting the distinct genres.
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
            //Movie = await _context.Movie.ToListAsync();
        }
    }
}
