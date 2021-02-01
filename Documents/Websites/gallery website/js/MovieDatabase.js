const api = {
    key: "e1a8c46",
    baseurl: "https://www.omdbapi.com/"
} 

// grab DOM
const searchInputElement = document.getElementById('searchMovie');
const searchBtnElement = document.getElementById('searchBtn');
const movieCountElement = document.getElementById('movieCount');

searchInputElement.addEventListener('keypress', (sender) => {
    if(sender.keyCode == 13) { /* Enter key */
        processRequest(); 
    }
});

searchBtnElement.addEventListener("click", () => {
    processRequest();
});

function processRequest() {
    const query = searchInputElement.value;
    
    if(query !== "") {
        const movies = GetMovies(query);
       
        movieCountElement.innerHTML = "";
        if(movies === null || movies === undefined) {
            movieCountElement.style.color = "red";
            movieCountElement.innerHTML = "(No movies found.)";
        }    
        else {
            movieCountElement.style.color = "black";
            movieCountElement.innerHTML = `(${movies.length} movies found.)`;
            const out = formatMoviesAsHTML(movies);
            $("#movies").html(out);                                  
        }
    } 
}

/*
{"Search":[{"Title":"Terminator 2: Judgment Day","Year":"1991","imdbID":"tt0103064","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BMGU2NzRmZjUtOGUxYS00ZjdjLWEwZWItY2NlM2JhNjkxNTFmXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg"},{"Title":"The Terminator","Year":"1984","imdbID":"tt0088247","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BYTViNzMxZjEtZGEwNy00MDNiLWIzNGQtZDY2MjQ1OWViZjFmXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SX300.jpg"},{"Title":"Terminator 3: Rise of the Machines","Year":"2003","imdbID":"tt0181852","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BMTk5NzM1ODgyN15BMl5BanBnXkFtZTcwMzA5MjAzMw@@._V1_SX300.jpg"},{"Title":"Terminator Salvation","Year":"2009","imdbID":"tt0438488","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BODBlOTJhZjItMGRmYS00YzM1LWFmZTktOTJmNDMyZTBjMjBkXkEyXkFqcGdeQXVyMjMwNDgzNjc@._V1_SX300.jpg"},{"Title":"Terminator Genisys","Year":"2015","imdbID":"tt1340138","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BMjM1NTc0NzE4OF5BMl5BanBnXkFtZTgwNDkyNjQ1NTE@._V1_SX300.jpg"},{"Title":"Terminator: Dark Fate","Year":"2019","imdbID":"tt6450804","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BOWExYzVlZDgtY2E1ZS00NTFjLWFmZWItZjI2NWY5ZWJiNTE4XkEyXkFqcGdeQXVyMTA3MTA4Mzgw._V1_SX300.jpg"},{"Title":"Terminator: The Sarah Connor Chronicles","Year":"2008–2009","imdbID":"tt0851851","Type":"series","Poster":"https://m.media-amazon.com/images/M/MV5BZGE2ZDgyOWUtNzdiNS00OTI3LTkwZGQtMTMwNzM4YWUxNGNhXkEyXkFqcGdeQXVyNjU2NjA5NjM@._V1_SX300.jpg"},{"Title":"Terminator 3: Rise of the Machines","Year":"2003","imdbID":"tt0364056","Type":"game","Poster":"https://m.media-amazon.com/images/M/MV5BMjA5OTk4MTQwNV5BMl5BanBnXkFtZTgwMzkxNTEwMTE@._V1_SX300.jpg"},{"Title":"Terminator 2: Judgment Day","Year":"1991","imdbID":"tt0244839","Type":"game","Poster":"https://m.media-amazon.com/images/M/MV5BN2FhOTQ2MmQtNTY0OC00NWYyLThjNjMtZmZiOTBmYTY4MmM2XkEyXkFqcGdeQXVyMzM4MjM0Nzg@._V1_SX300.jpg"},{"Title":"Lady Terminator","Year":"1989","imdbID":"tt0095483","Type":"movie","Poster":"https://m.media-amazon.com/images/M/MV5BMTg5NTA1NzEtNWNiNy00ZTc4LWJhZTgtYmJkODZhYWI3NmQ4XkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SX300.jpg"}],"totalResults":"97","Response":"True"}
*/
function GetMovies(query) {
    const getRequest = `${api.baseurl}?apikey=${api.key}&s=${query}`;
    let movies = null;

    $.ajax({
        url: getRequest,
        type: "GET",
        datatype: JSON,
        async: false,
        success: function(response) {
            movies = response.Search.filter((movie) => movie.Type === "movie"); 
        },
        failure: function (response) {
            console.error(response);     
        },
        error: function (response) {
            console.error(response);
        }        
    });        
        
    return movies; 
}

function formatMoviesAsHTML(movies) {
    let out = "";

    movies.forEach(function(movie) {
        let poster = movie.Poster === "N/A" ? "images/noImg.png" : movie.Poster;  

        console.log(`movie.Poster: ${poster}`);
        out += `<div class='col-md-3'>
        <div class="text-center">        
        <img src="${poster}" width="350" height="500px" class="p-5 img-responsive" alt="${movie.Title} poster">
        <h5 class="m-3">${movie.Title}</h5>
        <a href="https://www.imdb.com/title/${movie.imdbID}" target="_blank" class="btn btn-outline-primary">IMDB</a>
        <a href="#" id="movie-details" onclick="selectMovie('${movie.imdbID}', '${movie.Title}');" class="btn btn-outline-dark">Details</a>
        </div>
        </div>`;
    });

    return out;
}

function selectMovie(id, title) {
    console.log(`selectMovie(id, title): ${id},${title}`);
    sessionStorage.setItem("movieId", id);
    sessionStorage.setItem("movieTitle", title);
    window.location = "MovieDetails.html";
    return false;
  }

  