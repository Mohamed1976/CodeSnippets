const api = {
    key: "6f1aa5a06770929433faa4911e4334a1",
    baseurl: "https://api.openweathermap.org/data/2.5/"
}

const searchbox = document.querySelector(".search-box");
const searchbutton = document.querySelector(".search-btn");
const clear = document.querySelector(".re-set");

clear.addEventListener('click', event => {
    //Reload the page.    
    location.reload();
})

searchbutton.addEventListener('click', getWeather);

function getWeather() {
    getresults(searchbox.value);
}

searchbox.addEventListener('keypress', setQuery);

function setQuery(e) {
    if (e.keyCode == 13) { //Process enter key      
        getresults(searchbox.value);
    }
}

function getresults(query) {

    fetch(`${api.baseurl}weather?q=${query}&units=metric&appid=${api.key}`)
        .then((weather) => {
            console.log(weather);
            return weather.json();
        }).then(displayResults);
}

function displayResults(weather) {
    console.log(weather);
    const city = document.querySelector('.location .city');
    city.innerText = `${weather.name},${weather.sys.country}`;

    const now = new Date();
    const date = document.querySelector('.location .date');
    date.innerText = dateBuilder(now);

    const temp = document.querySelector('.current .temp');
    temp.innerHTML = `${Math.round(weather.main.temp)}<span>°C</span>`;

    const weather_name = document.querySelector('.current .weather');
    weather_name.innerText = `${weather.weather[0].main}`;

    const hi_low = document.querySelector('.current .hi-low');
    hi_low.innerText = `${weather.main.temp_min}°C/${weather.main.temp_max}°C`;
}

function dateBuilder(d) {
    const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thusday", "Friday", "Saturday"];
    const day = days[d.getDay()];
    const date = d.getDate();
    const month = months[d.getMonth()];
    const year = d.getFullYear();

    return `${day} ${date} ${month} ${year}`;
}
