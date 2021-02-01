const api = {
    key: "e1a8c46",
    baseurl: "https://www.omdbapi.com/"
} 
//DOM
const movieHeader = document.getElementById("movieHeader");

/*
https://www.omdbapi.com/?apikey=e1a8c46&i=tt0103064
{"Title":"Terminator 2: Judgment Day","Year":"1991","Rated":"R","Released":"03 Jul 1991","Runtime":"137 min","Genre":"Action, Sci-Fi","Director":"James Cameron","Writer":"James Cameron, William Wisher","Actors":"Arnold Schwarzenegger, Linda Hamilton, Edward Furlong, Robert Patrick","Plot":"A cyborg, identical to the one who failed to kill Sarah Connor, must now protect her teenage son, John Connor, from a more advanced and powerful cyborg.","Language":"English, Spanish","Country":"USA, France","Awards":"Won 4 Oscars. Another 32 wins & 33 nominations.","Poster":"https://m.media-amazon.com/images/M/MV5BMGU2NzRmZjUtOGUxYS00ZjdjLWEwZWItY2NlM2JhNjkxNTFmXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg","Ratings":[{"Source":"Internet Movie Database","Value":"8.5/10"},{"Source":"Rotten Tomatoes","Value":"93%"},{"Source":"Metacritic","Value":"75/100"}],"Metascore":"75","imdbRating":"8.5","imdbVotes":"991,958","imdbID":"tt0103064","Type":"movie","DVD":"N/A","BoxOffice":"$205,881,154","Production":"Lightstorm Entertainment, Carolco Pictures Inc., Pacific Western","Website":"N/A","Response":"True"}
*/
function getMovieDetails(movieId) {
    const getRequest = `${api.baseurl}?apikey=${api.key}&i=${movieId}`;
    //console.log(getRequest);
    let movieDetails = null;
    
    $.ajax({
        url: getRequest,
        type: "GET",
        datatype: JSON,
        async: false,
        success: function(response) {
            movieDetails = response;
        },
        failure: function (response) {
            console.error(response);     
        },
        error: function (response) {
            console.error(response);
        }        
    });

    return movieDetails;
  }

  function formatMovieDetailsAsHTML(movie) {
    let poster = movie.Poster === "N/A" ? "images/noImg.png" : movie.Poster;  

    let out = 
        `<div class="row p-5">
            <div class="col-4 text-center d-flex align-self-center justify-content-center">
                <div class="row">
                    <img src="${poster}" width="350" height="500" class="p-5 img-responsive border" alt="${movie.Title} Poster">
                    <a href="MovieDatabase.html" class="p-3 btn btn-outline-success m-3" ><i class="fas fa-undo-alt"></i> Search Again</a>
                </div>
                <div class="col-4">
                </div>
            </div>

            <div class="col-8">
                <li class="list-group-item"><h1 class="display-4 m-1">${movie.Title}</h1></li>
                <li class="list-group-item"><h3>${movie.Year}</h3></li>
                <ul class="list-group">
                    <li class="list-group-item lead">Rated : ${movie.Rated}</li>
                    <li class="list-group-item lead">Runtime : ${movie.Runtime}</li>
                    <li class="list-group-item lead">Genre : ${movie.Genre}</li>
                    <li class="list-group-item lead">Director : ${movie.Director}</li>
                    <li class="list-group-item lead">Writer : ${movie.Writer}</li>
                    <li class="list-group-item lead">Actors : ${movie.Actors}</li>
                    <li class="list-group-item lead">Plot : ${movie.Plot}</li>
                    <li class="list-group-item lead">Language : ${movie.Language}</li>
                    <li class="list-group-item lead">Country : ${movie.Country}</li>
                    <li class="list-group-item lead">IMDB Ratings : ${movie.imdbRating}</li>
                    <li class="list-group-item lead">Type : ${movie.Type}</li>
                    <li class="list-group-item lead">Production : ${movie.Production}</li>
                </ul> 
            </div>
        </div>`;
        
        return out;
}

function Run() {
    const movieId = sessionStorage.getItem("movieId");
    const movieTitle =  sessionStorage.getItem("movieTitle");

    movieHeader.innerHTML = `${movieTitle}`;    
    const movieDetails = getMovieDetails(movieId);
    const htmlText = formatMovieDetailsAsHTML(movieDetails);  
    $("#movieDetails").html(htmlText);
}

Run();








