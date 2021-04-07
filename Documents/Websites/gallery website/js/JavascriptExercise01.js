//The id is case-sensitive. For example, the 'master' and 'Master' are totally different ids.
//Once you have selected an element, you can add styles to the element, manipulate its attributes, and traverse to parent and child elements.
const master = document.querySelector("#master");
console.log(master);

document.getElementById("btn").addEventListener('click', function (sender) {
//document.getElementById("btn").addEventListener('click', function master(sender) {
//document.getElementById("btn").addEventListener("click", (sender) => {
    const weather = ["Winter is coming.", "Storm is coming.", "Snow is coming."]; 
    console.log(sender);
    const master2 = document.getElementsByClassName("master2");
    // master2[0].innerHTML = "Winter is coming.";
    // master2[1].innerHTML = "Storm is coming.";
    // master2[2].innerHTML = "Snow is coming.";
    for(let i = 0; i < master2.length; i++) {
        master2[i].innerHTML = weather[i]; 
    }
});

document.querySelector("#btn2").addEventListener("click", () => {
    var msgs = []; 
    msgs.push("The eagle has landed.");
    msgs.push("Storm is coming.");
    msgs.push("Summer is coming.");

    const emElements = document.getElementsByTagName("em");
    for(let j = 0; j < emElements.length; j++) {
        emElements[j].innerHTML = msgs[j];      
    }
});

function onclickBtn4(sender) {
    console.log(sender);
    const pElement = document.querySelector("#master3Id");
    pElement.innerHTML = "Another one bites the dust.";
}

function onclickBtn5(sender) {
    console.log(sender);
    const pElements = document.querySelectorAll(".master3"); 
    for(let j = 0; j < pElements.length; j++) {
        pElements[j].innerHTML = `Another one bites the dust => ${j}`;
    }   
}

function onclickBt5() {
    const parent = document.getElementById('parent');
    console.log(parent.children.length);
    console.log(parent.lastElementChild) //<p>i am the last child</p>
    console.log(parent.firstElementChild) //<div id="firstchild">i am a first child</div>
    for(let i = 0; i < parent.children.length; i++) {
        console.log(parent.children[i]);
    }

    const secondchild = document.getElementById('secondchild')
    console.log(secondchild) //<p id="secondchild">i am the second child</p>
    console.log(secondchild.parentNode) //<div id="parent">...</div>
    console.log(secondchild.nextElementSibling); //<h4>i am alive</h4>
    console.log(secondchild.previousElementSibling); ////<div id="firstchild">i am a first child</div>
}

function onclickBtn6() {
    const divElement = document.createElement("div");
    divElement.innerHTML = 'i am a frontend developer';
    const parentEl = document.getElementById('parent2');
    parentEl.appendChild(divElement);
}

function onclickBtn7() {
    const parentEl = document.getElementById("parent3");
    const firstchildEl = document.getElementById("firstchild3");
    const createEl = document.createElement("p");
    createEl.innerHTML = "Moccasoft @ work.";
    parentEl.insertBefore(createEl, firstchildEl);
}

function onclickBtn8() {
    const parentEl = document.getElementById("parent4");
    const firstchildEl = document.getElementById("firstchild4");
    const createEl = document.createElement("p");
    createEl.innerHTML = "Item replaced.";
    parentEl.replaceChild(createEl, firstchildEl);
}

function onclickBtn9() {
    const parentEl = document.getElementById("parent5");
    const secondchildEl = document.getElementById("secondchild5");

    parentEl.removeChild(secondchildEl);
}

function onclickBtn10(sender) {
    console.log(sender);
    sender.classList.add('buttonStyle');
}

function onclickBtn11(sender) {
    console.log(sender);
    sender.classList.remove('buttonStyle');
}

function onclickBtn12(sender) {
    console.log(sender);
    sender.classList.toggle('buttonStyle');
}

function onclickBtn13() {
    const num = prompt("insert a number greater than 30 but less than 40")
    
    try { 
        if(isNaN(num)) 
            throw "Not a number (☉｡☉)!" 
        else if (num>40) 
            throw "Did you even read the instructions ಠ︵ಠ, less than 40"
        else if (num <= 30) 
            throw "Greater than 30 (ب_ب)" 
    }
    catch(e) {
        alert(e) 
    }
    finally {
        console.log("Finally executed.");
    }
}

function onclickBtn14() {

    try{
        getData(); // getData is not defined
    }catch(e){
        alert(e);
    }

}

function onclickBtn15() {
    try {
        throw new CustomError("Argument is out of range.");     
    } 
    catch(e) {
        console.log(`CustomError: ${e.value}, ${e.message}`);
    }
    finally {
        console.log("finally is executed.");
    }
}

function CustomError(message){
    this.value ="customError";
    this.message=message;
}

function onclickBtn16() {

    let x = parseInt(prompt("input a number less than 5"))
    
    try{
        y=x-10 
        if(y>=5) 
            throw new Error(" y is not less than 5") 
        else 
            alert(y) 
    }catch(e){ 
        if(e instanceof ReferenceError){ 
            throw e
        }
        else 
            alert(e) 
    }
}

/*
https://livecodestream.dev/post/date-manipulation-in-javascript-a-complete-guide/
You can simply create a date using new Date() . You can pass parameters to the Date constructor to create a date of your choice. The given parameter can take different forms.

*/
function onclickBtn17() {
    console.log("Processing Date object.");
    //In addition to the date we passed, the date object has more values, 
    //including a time and a timezone. Since we didn’t give a specific 
    //value for these parameters when creating the object, Javascript uses 
    //the local time and timezone of the code’s system.
    const date = new Date("2020-12-31");
    console.log(date);

    //If we want to pass the time or timezone with the parameter string, 
    //we can use a format like this. YYYY-MM-DDTHH:mm:ss.sssZ
    /*
        YYYY: year
        MM: month (1 to 12)
        DD: date (1 to 31)
        HH: hour in 24-hour format (0 to 23)
        mm: minutes (0 to 59)
        ss: seconds (00 to 59)
        sss: milliseconds (0 to 999)
        T is used to separate the date and time in the string
        If Z is present, the time is assumed to be in UTC. Otherwise, it assumes the local time.

        However, if T and Z are not present, the string’s created date may give different results 
        in different browsers. In that case, to always have the same timezone for the date, 
        add +HH:mm or -HH:mm to the end.

        */
       let newDate = new Date("2021-09-23T23:45Z");
       // Fri Sep 24 2021 01:45:00 GMT+0200 (Central European Summer Time)
       console.log("Fri Sep 24 2021 01:45:00 GMT+0200 (Central European Summer Time): " + newDate);

       newDate = new Date("2021-09-23T23:45");
       // Thu Sep 23 2021 23:45:00 GMT+0200 (Central European Summer Time)
       console.log("Thu Sep 23 2021 23:45:00 GMT+0200 (Central European Summer Time): " + newDate);
       
       newDate = new Date("2021-09-23T23:45+05:30");
       // Thu Sep 23 2021 20:15:00 GMT+0200 (Central European Summer Time)
       console.log("Thu Sep 23 2021 20:15:00 GMT+0200 (Central European Summer Time): " + newDate);
        /*
        You can get the same results using the Date.parse function instead of passing the date string to 
        the Date constructor. Date.parse is indirectly being called inside the constructor whenever you 
        pass a date string.

        The format used in these strings is the ISO 8601 calendar extended format. You can refer to its 
        details in the ECMAScript specification .

        Javascript uses a zero-based index to identify each month in a year. This means, for Javascript, 
        January is represented by 0 instead of 1. Similarly, October is represented by 9 instead of 10.
        */

       newDate = new Date(1998, 9, 30, 13, 40, 05);
       console.log("Fri Oct 30 1998 13:40:05 GMT+0100 (Central European Standard Time): " + newDate);

        /*
        In this method of creating a date, we can’t pass an argument to indicate its time zone. 
        So, it’s defaulted to the local time of the system. But we can use the Date.UTC function 
        to convert the date to UTC before passing it to the Date constructor
        */
        newDate = new Date(Date.UTC(1998, 09, 30, 13, 40, 05))
        // Fri Oct 30 1998 14:40:05 GMT+0100 (Central European Standard Time)
        console.log("Fri Oct 30 1998 14:40:05 GMT+0100 (Central European Standard Time): " + newDate);
        /*
        Pass a timestamp
        Remember that I mentioned Javascript stores the time elapsed since the epoch time in the Date object? 
        We can pass this elapsed time value, called a timestamp, to indicate the date we are creating.
        */
       newDate = new Date(1223727718982);
       // Sat Oct 11 2008 14:21:58 GMT+0200 (Central European Summer Time)
       console.log("newDate = new Date(1223727718982); => " + newDate);

       /*
        Create a Date object for the current date and time
        If you want to create a Date object for the current date and time of the system, 
        use the Date constructor without passing any argument.
       */

        let now = new Date();
        // Sat Jan 09 2021 22:06:33 GMT+0100 (Central European Standard Time)
        console.log("const now = new Date(); => " + now);
        //You can also use the Date.now() function for the same task.
        now = Date.now();
        console.log("const now = Date.now(); => " + now);

        /*
        Formatting dates
        Javascript provides several built-in functions to format a date. However, 
        these functions only convert the date to a format specific to each one.
        Let’s see how each formatting function works.
        */
        newDate = new Date("2021-01-09T14:56:23");
        console.log("newDate.toString(); => " + newDate.toString());
        // "Sat Jan 09 2021 14:56:23 GMT+0100 (Central European Standard Time)"
        console.log("newDate.toDateString(); => " + newDate.toDateString());
        // "Sat Jan 09 2021"

        console.log("newDate.toLocaleDateString(); => " + newDate.toLocaleDateString());
        // "1/9/2021"
        
        console.log("newDate.toLocaleTimeString(); => " + newDate.toLocaleTimeString());
        // "2:56:23 PM"
        
        console.log("newDate.toLocaleString(); => " + newDate.toLocaleString());
        // "1/9/2021, 2:56:23 PM"
        
        console.log("newDate.toGMTString(); => " + newDate.toGMTString());
        // "Sat, 09 Jan 2021 13:56:23 GMT"
        
        console.log("newDate.toUTCString(); => " + newDate.toUTCString());
        // "Sat, 09 Jan 2021 13:56:23 GMT"
        
        console.log("newDate.toISOString(); => " + newDate.toISOString());
        // "2021-01-09T13:56:23.000Z"
        
        console.log("newDate.toTimeString(); => " + newDate.toTimeString());
        // "14:56:23 GMT+0100 (Central European Standard Time)"
        
        console.log("newDate.getTime(); => " + newDate.getTime());
        // 1610200583000

        /*
        Internationalization API
        ECMAScript Internationalization API allows the formatting of a date into a specific 
        locale using the Intl object.
        */
        newDate = new Date("2021-01-09T14:56:23");
        //format according to the computer's default locale
        let dateInt = Intl.DateTimeFormat().format(newDate);
        // "1/9/2021"
        console.log("Intl.DateTimeFormat().format(newDate); => " + dateInt);

        //format according to a specific locale, e.g. de-DE (Germany)
        dateInt = Intl.DateTimeFormat("de-DE").format(newDate);
        // "9.1.2021"
        console.log("Intl.DateTimeFormat(de-DE).format(newDate); => " + dateInt);
        //format according to a specific locale, e.g. de-DE (Germany)
        dateInt = Intl.DateTimeFormat("en-US").format(newDate);
        // "9.1.2021"
        console.log("Intl.DateTimeFormat(en-US).format(newDate); => " + dateInt);
        //format according to a specific locale, e.g. de-DE (Germany)
        dateInt = Intl.DateTimeFormat("nl-NL").format(newDate);
        // "9.1.2021"
        console.log("Intl.DateTimeFormat(nl-NL).format(newDate); => " + dateInt);

        /*
        You can pass an options object to the DateTimeFormat function to display time values and 
        customize the output.
        */

        let options = {
            year: "numeric",
            month: "long",
            weekday: "long",
            hour: "numeric",
            minute: "numeric",
        }

        dateInt = Intl.DateTimeFormat("en-US", options).format(newDate)
        // "January 2021 Saturday, 2:56 PM"
        console.log("Intl.DateTimeFormat(en-US).format(newDate); => " + dateInt);

        dateInt = Intl.DateTimeFormat("nl-NL", options).format(newDate)
        // "January 2021 Saturday, 2:56 PM"
        console.log("Intl.DateTimeFormat(nl-NL).format(newDate); => " + dateInt);

        /*
        Custom date formats
        If you want to format the date to any other format beyond what these functions provide, 
        you’ll have to do so by accessing each part of the date separately and combining them.

        Javascript provides the following functions to retrieve the year, month, date, and day 
        from a Date object.

        newDate.getFullYear() // 2021
        newDate.getMonth()    // 0 (zero-based index)
        newDate.getDate()     // 9
        newDate.getDay()      // 6 (zero-based index starting from Sunday)
        newDate.getHours()    // 14
        newDate.getMinutes()  // 56
        newDate.getUTCHours() // 9
        newDate.getUTCDate()  // 9

        Now, you can convert the date to a custom format using retrieved parts.
        */

        const today = new Date();
        console.log(`${today.getFullYear()}, ${today.getMonth()}, ${today.getDate()}, ${today.getDay()}, ${today.getHours()}, ${today.getMinutes()}, ${today.getUTCHours()}, ${today.getUTCDate()}`);
        /*
        Updating dates
        Javascript provides several methods to edit an already created date.
        */
       newDate = new Date("2021-01-08T22:45:23")
       console.log(`newDate = new Date("2021-01-08T22:45:23"): ${newDate}`);
       newDate = newDate.setYear(1998);
       console.log(`newDate.setYear(1998): ${newDate}`);
       //Thu Jan 08 1998 22:45:23 GMT+0100 (Central European Standard Time)
       
        //newDate.setMonth(4)
        //Fri May 08 1998 22:45:23 GMT+0200 (Central European Summer Time)

        //newDate.setDate(12)
        //Tue May 12 1998 22:45:23 GMT+0200 (Central European Summer Time)

        //    newDate.setHours(12)
        //    newDate.setMinutes(21)
        //    newDate.setUTCDate(26)
        //    newDate.setUTCMinutes(56)

        /*
        Comparing dates
        If you want to know whether a specific date comes before another, 
        you can use greater than and less than operators directly for comparison.
        */
       let first = new Date(2010, 3, 19);
       let second = new Date(2010, 3, 24);
       console.log(`${first} > ${second} ? ${first > second}`); //false

       //However, if you want to check them for equality, neither == nor === operator works as intended.
       first = new Date(2009, 12, 23)
       second = new Date(2009, 12, 23)
       
       console.log(first == second)  // false
       console.log(first === second) // false

        /*
        Instead, you have to retrieve the timestamp of each date and compare them for equality.
        This is because Dates in JavaScript are objects, so each date has a different instance 
        of the class, and the == or === operator are comparing the memory address instead of the 
        actual values of the dates.
        */
        console.log(first.getTime() === second.getTime()); // true
        console.log(`${first.getTime()}, ${second.getTime()}`);//getTime in ms

        //https://momentjs.com/
        /*
        Javascript date manipulation libraries
        We can find several Javascript date and time manipulation libraries as open-source projects 
        or otherwise. Some of them, designed for all kinds of date-time manipulations, and some have 
        a specific set of use cases. In this section, I’ll only talk about popular multi-purpose libraries.

        Moment.js used to be the king of date manipulation libraries among Javascript developers. 
        However, its developers recently announced that it’s focusing on maintaining the current 
        codebase instead of adding new features. They recommend looking for an alternative solution 
        for those who are working on new projects.

        So, apart from Moment.js, what are the libraries we can use to make our life easier as developers?
        */
       let momentNow = moment().format('MMMM Do YYYY, h:mm:ss a'); // February 18th 2021, 10:45:11 pm
       console.log(momentNow);
       console.log(momentNow);
       momentNow = moment().format('dddd');                    // Thursday
       console.log(momentNow);
       momentNow = moment().format("MMM Do YY");               // Feb 18th 21
       console.log(momentNow);
       momentNow = moment().format('YYYY [escaped] YYYY');     // 2021 escaped 2021
       console.log(momentNow);
       momentNow = moment().format();                          // 2021-02-18T22:45:11+01:00
       console.log(momentNow);

       momentNow =moment().format('LLL');  // February 18, 2021 10:51 PM
       console.log(momentNow);
       momentNow =moment().format('lll');  // Feb 18, 2021 10:51 PM
       console.log(momentNow);
       momentNow =moment().format('LLLL'); // Thursday, February 18, 2021 10:51 PM
       console.log(momentNow);
       momentNow =moment().format('llll');  
       console.log(momentNow);

       /*
       https://cdnjs.com/libraries/lodash.js
       https://cdnjs.cloudflare.com/ajax/libs/lodash.js/4.17.20/lodash.min.js
        Date-fns
        Date-fns in an open-source library supporting date parsing and formatting, locales, 
        and date arithmetics like addition and subtraction. It’s dubbed as Lodash for dates 
        due to its versatility
        As you can see, you can easily convert a date into your preferred format by passing
        a simple formatting string.

        It also allows us to add and subtract dates easily.

       let _date = new Date(2019, 09, 22)

       let newDate1 = datefns.addDays(_date, 21)
       let newDate2 = datefns.addYears(_date, 2)
       let newDate3 = datefns.subMonths(_date, 3)
       
       console.log(datefns.format(newDate1, 'dd/MM/yyyy')) // 12/11/2019
       console.log(datefns.format(newDate2, 'dd/MM/yyyy')) // 22/10/2021
       console.log(datefns.format(newDate3, 'dd/MM/yyyy')) // 22/07/2019
       */

        /*
        Luxon
        Luxon is a date-time manipulation library created by one of the Moment.js developers 
        to suit modern application requirements. Similar to Date-fns, Luxon offers data formatting 
        and parsing functions. Also, it has native Intl support and is chainable.
        You can also measure the time interval between two dates.
        
       let _date = DateTime.local(2019, 08, 12)

       console.log(_date.toLocaleString(DateTime.DATETIME_FULL))
       console.log(_date.toLocaleString(DateTime.DATETIME_MED))
       */
}