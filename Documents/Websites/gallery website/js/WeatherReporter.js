const api = {
    key: "175bce777dda85d8fbff08fad9286267",
    baseurl: "https://api.openweathermap.org/data/2.5/"
}   

// This sets the options parameter while geolocating client
const geo_options = {
    enableHighAccuracy: true,
    maximumAge: Infinity,
    timeout: 27000
  };

const WeatherReporterLatitudeKey = "WeatherReporterLatitude";
const WeatherReporterLongitudeKey = "WeatherReporterLongitude";
const CelsiusKey = "Celsius";

// Global variable for user temperature preference
var latitude = localStorage.getItem(WeatherReporterLatitudeKey);
var longitude = localStorage.getItem(WeatherReporterLongitudeKey);
var tempInCelsius = 0;

// All event Listeners
document.getElementById("locate").addEventListener("click", getLocation);
document.getElementById("search").addEventListener("click", searchWeatherByCity);

//  OpenweatherMap API call by city
function searchWeatherByCity() {

    const query = document.getElementById("searchParam").value;

    if(query === null || query === "") {
        console.log("Search parameter is empty.");
        return false;    
    }

    console.log(`${api.baseurl}weather?q=${query}&units=metric&appid=${api.key}`);
    fetch(`${api.baseurl}weather?q=${query}&units=metric&appid=${api.key}`)
        .then((response) => {
            console.log(response);
            return response.json();
        })
        .then((json) => {
            console.log(json);
            console.log(JSON.stringify(json));
            mapResponseToUI(json); 
        })
        .catch(error => {
            console.log(error);
            $("#errorModal").modal("show")
        }); 
}

function getLocation() {
    navigator.geolocation.getCurrentPosition(geo_success, geo_error, geo_options);
}

// This is executed after succesfully locating clients location
// Also sets the coordinates in local storage to prevent overhead time in future
function geo_success(position) {
    latitude = position.coords.latitude;
    longitude = position.coords.longitude;
    localStorage.setItem(WeatherReporterLongitudeKey, longitude);
    localStorage.setItem(WeatherReporterLatitudeKey, latitude);
    searchWeatherByCoordinates(latitude, longitude);
}
  
// Error while geolocating, alert user to choose other option
function geo_error(e) {
    searchWeatherByCoordinates(latitude, longitude);
}
  
//  OpenweatherMap API call by coordinates
function searchWeatherByCoordinates(latitude, longitude) {
    const query = "lat=" + latitude + "&lon=" + longitude;
    //console.log(`${api.baseurl}weather?${query}&units=metric&appid=${api.key}`);
    fetch(`${api.baseurl}weather?${query}&units=metric&appid=${api.key}`)
        .then((response) => {
            console.log(response);
            return response.json();
        })
        .then((json) => {
            console.log(json);
            console.log(JSON.stringify(json));
            mapResponseToUI(json); 
        })
        .catch(error => {
            console.log(error);
            $("#errorModal").modal("show")
        });    
}

// map json response to UI
function mapResponseToUI(json) {
    //`${api.baseurl}weather?${query}&units=metric&appid=${api.key}`
    // let temp = Math.round(json.main.temp - 273.15);
    // let tempinF = Math.round(json.main.temp * 1.8 - 459.67);
    tempInCelsius = json.main.temp;
    const temp = Math.round(tempInCelsius); 
    const windspeed = Math.round(json.wind.speed);
    const direction = getWindIcon(json.wind.deg);
    var sunrise = new Date(json.sys.sunrise * 1000);
    var sunset = new Date(json.sys.sunset * 1000);
  
    document.getElementById("cityDetails").innerText = json.name;
  
    document.getElementById("card1").innerHTML = `
      <ul>
      <li>Latitude: ${json.coord.lat}</li>
      <li>Longitude: ${json.coord.lon}</li>
      <li>
      <img src="./resources/images/windmill.png" alt="wind" style="margin-left:-2rem">
      <i class="wi wi-wind ${direction}"></i>${3.6 * windspeed} kmph
      </li>
      </ul>
        `;
  
    document.getElementById("card2").innerHTML = `
        <ul>
        <li>${json.weather[0].main}</li>
        <li style="text-transform: capitalize">${json.weather[0].description}</li>
        <li style="font-size: 5rem" id="temp"><i class="wi wi-thermometer"></i>${temp}<i class="wi wi-celsius"></i></li>
        <li>
        <button id="changeTemp" class="btn btn-outline-secondary" style="margin-left: 0.6rem" onclick="changeTemp(this)">Don't understand Celsius!</button>
        </li>
        </ul>
        `;
  
    document.getElementById("card3").innerHTML = ` 
        <ul>
        <li> <i class="wi wi-humidity"></i> ${json.main.humidity} %</li>
        <li> <i class="wi wi-barometer"></i> ${json.main.pressure} millibars </li>
        <li> <i class="fas fa-eye"></i>  ${json.visibility} m</li>
        <li> <i class="wi wi-sunrise"></i>  ${sunrise.toLocaleTimeString()}</li>
        <li> <i class="wi wi-sunset"></i>  ${sunset.toLocaleTimeString()}</li>
        </ul>
        `;
  
    document.getElementById("cardbox").style.visibility = "visible";
  }

  // set Wind Direction Icons
function getWindIcon(degree) {
    if (degree > 337.5) return "wi-towards-n";
    if (degree > 292.5) return "wi-towards-nw";
    if (degree > 247.5) return "wi-towards-w";
    if (degree > 202.5) return "wi-towards-sw";
    if (degree > 157.5) return "wi-towards-s";
    if (degree > 122.5) return "wi-towards-se";
    if (degree > 67.5) return "wi-towards-e";
    if (degree > 22.5) return "wi-towards-ne";
    return "wi-towards-n";
} 

//https://careerkarma.com/blog/javascript-string-contains/
// convert Celsius to Fahrenheit & vice-versa
function changeTemp(sender) {

    let tempIcon = "wi-celsius";
    let metric = CelsiusKey;
    let value = Math.round(tempInCelsius); 

    if (sender.innerHTML.includes(CelsiusKey)) {
        tempIcon = "wi-fahrenheit";
        metric = "Fahrenheit";
        value = Math.round(1.8 * tempInCelsius + 32); 
    } 
    console.log(`${tempIcon}, ${metric}, ${value}`);

    sender.innerHTML = `Don't understand ${metric}!`;
    document.getElementById("temp")
        .innerHTML = `<i class="wi wi-thermometer"></i>${value}<i class="wi ${tempIcon}"></i>`;
}

// JQuery Initializers
$(function() {
    console.log("JQuery Initializers called.");
    $('[data-toggle="tooltip"]').tooltip(); //Makes the Tooltip visible.
    $("#errroModal").modal({ show: false });
});