const api = {
    baseurl: "https://fcc-weather-api.glitch.me/api/current"
}   

//Global variable
var currentTempCelsius;

//https://learn.jquery.com/using-jquery-core/document-ready/
$( document ).ready(function() {
    $(".celsius").click(function(){
        //console.log(".celsius clicked");
        if (!$(this).hasClass("active")) {
            $(".fahrenheit").removeClass("active");
            $(this).addClass("active");
            //Cx = (Fx – 32) / 1,8
            $("#current-temp").html(Math.round(currentTempCelsius));
        }
    });

    $(".fahrenheit").click(function(){
        //console.log(".fahrenheit clicked");
        if (!$(this).hasClass("active")) {
            $(".celsius").removeClass("active");
            $(this).addClass("active");
            //Fx = (Cx * 1,8) + 32
            $("#current-temp").html(Math.floor(currentTempCelsius*1.8 + 32) + "&#176;");
        }
    });

    getLocation();
});

//Gets location using HTML 5 geolocation API
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position){
            const myLatitude = parseFloat(position.coords.latitude);
            const myLongitude = parseFloat(position.coords.longitude);
            console.log(`${myLatitude}, ${myLongitude}`);
            getWeather(myLatitude, myLongitude);
      }, function(error){
        alert("Error encountered while retrieving geolocation.")
      });
    }
    else {
        alert("Geolocation is not supported by this browser.")
    }
}
  
//Gets weather information using FreeCodeCamp's api
/*
https://fcc-weather-api.glitch.me/
https://fcc-weather-api.glitch.me/api/current?lat=51.880325299999996&lon=6.3850434

Response:
{"coord":{"lon":6.39,"lat":51.88},"weather":[{"id":804,"main":"Clouds","description":"overcast clouds","icon":"https://cdn.glitch.com/6e8889e5-7a72-48f0-a061-863548450de5%2F04d.png?1499366020964"}],"base":"stations","main":{"temp":3.69,"feels_like":0.3,"temp_min":3.33,"temp_max":3.89,"pressure":1016,"humidity":83},"visibility":10000,"wind":{"speed":2.24,"deg":47,"gust":5.81},"clouds":{"all":100},"dt":1609683779,"sys":{"type":3,"id":2003895,"country":"NL","sunrise":1609659692,"sunset":1609688163},"timezone":3600,"id":2755599,"name":"Gendringen","cod":200}
*/
function getWeather(latitude, longitude){
    const query = "lat=" + latitude + "&lon=" + longitude;
    const url = `${api.baseurl}?${query}`;
    console.log(url);
        
    $.ajax({
      method: "GET",
      url: url,
      cache: false,
      dataType: "json",
    })
    .done(function(data){
        console.log(data);
        console.log(JSON.stringify(data));

        var weatherDescription = (data.weather[0].description).split(' ');
        var weatherDescriptionFormatted = [];
        if (weatherDescription.length > 0) {
          weatherDescription.forEach(function(element){
            weatherDescriptionFormatted.push((element.charAt(0).toUpperCase() + element.substr(1, element.length)));
          });
        }
        else {
          weatherDescriptionFormatted.push((weatherDescription.charAt(0).toUpperCase() + weatherDescription.substr(1, weatherDescription.length)));
        }
        weatherDescriptionFormatted = weatherDescriptionFormatted.join(' ');
        $("#description").html(weatherDescriptionFormatted);
        $("#location").html("Location: " + data.name);
        $("#weather-pic").attr("src", data.weather[0].icon);
        currentTempCelsius = data.main.temp;
        $("#current-temp").html(Math.round(currentTempCelsius) + "&#176;");
    })
    .fail(function(response){
        console.log(response);
    });
  }