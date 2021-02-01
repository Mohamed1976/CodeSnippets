// DOM Elements
const time = document.getElementById('time'),
    greeting = document.getElementById('greeting'),
    nameElement = document.getElementById('name'),
    authorElement = document.getElementById('author');
    //focus = document.getElementById('focus');

const quoteElement = document.getElementById("quote");

nameElement.addEventListener('keypress', setName);
nameElement.addEventListener('blur', setName);

// Show Time
function showTime() {
    let today = new Date(),
        hour = today.getHours(),
        min = today.getMinutes(),
        sec = today.getSeconds();

    // Set AM or PM
    const amPM = hour >= 12 ? 'PM' : 'AM';

    // 12hr Format
    hour = hour % 12 || 12;

    // Output Time
    time.innerHTML = `${hour}<span>:</span>${addZero(min)}<span>:</span>${addZero(sec)} ${amPM}`;
}

// Add Zeros
function addZero(n) {
    return (parseInt(n, 10) < 10 ? '0' : '') + n;
}

var prevBackgroundImage; 
// var prevGreetingMsg;
// var prevColor;
// Set Background and Greeting
function setBGGreeting() {
    const today = new Date(),
        hour = today.getHours();

    let backgroundImage,
        greetingMsg,
        color;     

    if (hour < 12) {
        // Morning
        backgroundImage = "url('images/morning.jpg')";
        greetingMsg = 'Good Morning';
        color = 'black';
    } else if(hour < 18) {
        // Afternoon
        backgroundImage = "url('images/afternoon.jpg')";
        greetingMsg = 'Good Afternoon';
        color = 'black';
    } else {
        // Evening
        backgroundImage = "url('images/night.jpg')";
        greetingMsg = 'Good Evening';
        color = 'white';
    }

    if(prevBackgroundImage !== backgroundImage) {
        console.log("Updating Background Image.");
        document.body.style.backgroundImage = backgroundImage;
        greeting.textContent = greetingMsg;
        document.body.style.color = color; 

        prevBackgroundImage = backgroundImage;
    }
}

// Get Name
function getName() {
    if (localStorage.getItem('name') === null){
        nameElement.textContent = '[Enter Name]';
    } else  {
        nameElement.textContent = localStorage.getItem('name');
    }
}

// Set Name
function setName(e) {
    if(e.type === 'keypress'){
        if(e.keyCode == 13) { //Enter key
            localStorage.setItem('name', e.target.innerText);
            nameElement.blur(); 
        }
    } else {
        localStorage.setItem('name', e.target.innerText);
    }
}

var oneMinuteElapsed = 0;
//Main Run loop
function Run(bFirstTime = false) {
    if(bFirstTime) {
        getName();
        getQuote();
        setBGGreeting();
    }   
     
    showTime();
    
    if(oneMinuteElapsed++ >= 60) { /* Update background image every minute. */
        //console.log("Update background image.");
        setBGGreeting();
        oneMinuteElapsed = 0;
    }

    //Timer calls method every second. 
    setTimeout(Run, 1000);
}

function getQuote() {
    fetch("https://itsrockyy.herokuapp.com/api/quote-me")
        .then((response) => response.json())
        .then((json) => {
            quoteElement.innerHTML = `${json[0].quote}`;
            authorElement.innerHTML = `${json[0].author}`;
    })
    .catch((error) => console.log(error));   
}

Run(true);
