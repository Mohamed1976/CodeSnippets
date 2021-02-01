"use strict"

/* charset */
const LOWERCASE_CHAR_CODES = arrayFromLowToHigh(97,122);
const UPPERCASE_CHAR_CODES = arrayFromLowToHigh(65,90);
const NUMBER_CHAR_CODES = arrayFromLowToHigh(48,57);
const SYMBOL_CHAR_CODES=arrayFromLowToHigh(33,47)
    .concat(arrayFromLowToHigh(58,64))
    .concat(arrayFromLowToHigh(91,96))
    .concat(arrayFromLowToHigh(123,126));

const UPPERCASE_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
const LOWERCASE_ALPHABET = "abcdefghijklmnopqrstuvwxyz";
const DIGITS = "123456789";

const array1 = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'];
const array2 = ['a', 'b', 'c', 'd', 'e', 'f'];
const array3 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0];

// constructor function
function Person (person_name, person_age, person_gender) {

    // assigning  parameter values to the calling object
     this.name = person_name,
     this.age = person_age,
     this.gender = person_gender,
 
     this.greet = function () {
         return ('Hi' + ' ' + this.name);
     }
}

Person.prototype.hi = function () {
    console.log(`Hi! My name is ${this.name}.`);
};

function Charset() {

    this.UppercaseLetters = function() {
        return null; 
    }
}

// Flag Enumerations in JavaScript
// ===============================
// https://gist.github.com/corymartin/2393268
//
// * Cannot use 0 in a bitwise `&` operation to test for a flag b/c it will
// always result in zero. However, it can be used for logical comparisons.
//
// The perens are needed around the bitwise operation due to the
// greater operator precedence of `===`
//
// Flag enums
// ----------
// Values must increment by powers of 2

var SEASONS = {
    Spring : 1,
    Summer : 2,
    Fall   : 4,
    Winter : 8
  };
  
  var DAYS = {
    None      : 0x00,  // 0 *
    Sunday    : 0x01,  // 1
    Monday    : 0x02,  // 2
    Tuesday   : 0x04,  // 4
    Wednesday : 0x08,  // 8
    Thursday  : 0x10, // 16
    Friday    : 0x20, // 32
    Saturday  : 0x40  // 64
  };
  
 function processSeasons(seasons) {
    var selected = [];
    // The perens are needed around the bitwise operation due to the
    // greater operator precedence of `===`

    if ( (seasons & SEASONS.Spring) === SEASONS.Spring ) {
        console.log("SEASONS.Spring");      
    } 

    if ( (seasons & SEASONS.Summer) === SEASONS.Summer ) {
        console.log("SEASONS.Summer");              
    } 

    if ( (seasons & SEASONS.Fall) === SEASONS.Fall ) {
        console.log("SEASONS.Fall");              
    } 

    if ( (seasons & SEASONS.Winter) === SEASONS.Winter ) {
        console.log("SEASONS.Winter");              
    }
 } 

function processDays(days) {
    
    if (days === DAYS.None) {
        console.log("DAYS.None");
    }
    
    if ( (days & DAYS.Sunday) === DAYS.Sunday) {
        console.log("DAYS.Sunday");              
    } 

    if ( (days & DAYS.Monday) === DAYS.Monday) {
        console.log("DAYS.Monday");              
    } 
}

/* Entry point when page is loaded. */
window.addEventListener("load", function() {

    return;
    const timeout = 2000;

    console.log(`Start setTimeout, ${new Date()}`);            
    setTimeout(() => {
        console.log(`Timeout, ${new Date()}`);            
    }, timeout);

    const interval = 2000;
    let counter = 0;

    setInterval(() => {
        console.log(`Timer, ${counter++}, ${new Date()}`);    
    }, interval);

    //Function Objects and enum flags
    const personObj = new Person ("John", 34, "Male"); 
    console.log(personObj.greet());
    personObj.hi();
    
    processSeasons( SEASONS.Spring | SEASONS.Fall );
    processDays(DAYS.Sunday | DAYS.Monday);
 
    //Promises for long running operations
    //Async/await for managing promises, are built on promises 
    //Async/await is becoming the standard for asynchronous processsing 
    run();

    async function simulateLongOperation() {
        await promiseTimeout(4000);
        return 2030;     
    }    

    async function run() {
        console.log("Start run()");
        const result = await simulateLongOperation(); 
        console.log("Finished run(), result: " + result);
    }

    function promiseTimeout(ms) {
        return new Promise((resolve, reject) => {
            //setTimeout(reject, ms);
            setTimeout(resolve, ms);    
        });
    }

    //Call one service, wait for callback, call second service etc.
    promiseTimeout(4000)
        .then(() => {
            console.log("Done 1");
            return promiseTimeout(3000); 
        })
        .then(() => {
            console.log("Done 2");
            return promiseTimeout(1000);
        })
        .then(() => {
            console.log("Done 3");
            return Promise.resolve(1980);
        })
        .then((value) => {
            console.log("Done 4: " + value);
        })
        .catch(() => {
            console.log("Error");
        });
       
    const longRunningOperation = (func) => {
        func("Hi there");
    };

    longRunningOperation((msg) => {
        console.log("longRunningOperation is called: " + msg);
    });
    
    //Originally managed through callbacks
    function callback() {
        console.log("Callback");
    }

    setTimeout(callback,3000); //wait 3 seconds

    //in stand alone function this maps to the global object 
    //this in function represents the owner of the function
    //use globalThis to reference the global object consistently   
    console.log(globalThis); //refers to the window object
    const bookData = {name: "Excellence", birth: "Amsterdam" }
    const bookObjV3 = new Object(bookData);

    console.log(typeof bookObjV3);
    console.log(bookObjV3);
    console.log(bookObjV3.name  + ", " + bookObjV3.birth);
    
    const bookObjV2 = new Object();
    bookObjV2.title = "1984"; 
    bookObjV2.author = "George Orwell";
    bookObjV2.isAvailable  = true;
    bookObjV2.checkIn = function () {
        this.isAvailable = true;
    }

    bookObjV2.checkOut = function () {
        this.isAvailable = false;        
    }

    bookObjV2.print = function() {
        console.log(this.title + ", " + this.author + ", " + this.isAvailable);
    }

    bookObjV2.GetObject = function() {
        return this; //returns book objects
    }

    console.log(typeof bookObjV2);
    const _bookObjV2 = bookObjV2.GetObject();
    console.log("-----_bookObjV2.print()-----");
    bookObjV2.GetObject() === bookObjV2 ? console.log("Is same object") : console.log("NOT same object"); 
    _bookObjV2.print();
    console.log("-----_bookObjV2.print()-----");
    bookObjV2.print();
    bookObjV2.checkIn();
    bookObjV2.print();
    bookObjV2.checkOut();
    bookObjV2.print();    
    console.log(`${bookObjV2.title} - ${bookObjV2["title"]}`);
    //Access methods like properties
    console.log(`${bookObjV2.print} - ${bookObjV2["print"]}`);
    //Invoke method
    bookObjV2["print"]();
    bookObjV2.print();    

    console.log("\n\n\n");
    //Objects, song, book, have associated properties and methods  
    const bookObj = { //Is object literal
        /* Properties */
        title: "Razor-thin",
        author: "Erin Vegan",
        isAvailable: false,
        
        /* Methods */
        checkIn: function() {
            this.isAvailable = true;
        },

        checkOut: function() {
            this.isAvailable = false;
        },

        print: function() {
            console.log(this.title + ", " + this.author + ", " + this.isAvailable);
        }
    }

    console.log(typeof bookObj);
    bookObj.print();
    bookObj.checkIn();
    bookObj.print();
    bookObj.checkOut();
    bookObj.print();
    
    //Using JSON, JSON.stringify, JSON.parse 
    const book = {
        isbn: "1234589-21",
        author: "John Beers"
    }

    const myBooks = [
        {
            isbn: "1234589-21",
            author: "John Beers"
        },
        {
            isbn: "1234589-23",
            author: "Micheal Oconner"
        },
        {
            isbn: "1234589-27",
            author: "Erwin Don"
        }
    ]

    const myBooks2 = [
        new Object ({
            isbn: "1234589-21",
            author: "John Beers"
        }),
        new Object ({
            isbn: "1234589-23",
            author: "Micheal Oconner"
        }),
        new Object ({
            isbn: "1234589-27",
            author: "Erwin Don"
        })
    ]

    const json = JSON.stringify(book);
    console.log(json);

    const _json = JSON.stringify(myBooks);
    console.log(_json);

    //Convert json to to objects again      
    const _arr = JSON.parse(_json);
    console.log("Array.isArray(_arr): " + Array.isArray(_arr));
    for (const iterator of _arr) {
        console.log(iterator.isbn + ", " + iterator.author);
    }

    const json2 = JSON.stringify(myBooks2);
    console.log("typeof myBooks2: " + typeof myBooks2);
    console.log(json2);

    //Traditionally this is defined where the function is called, arrow functions inherit this from parent
    //Arrow and anonymous functions, catches its parent this context and makes it available in the function  
    //Arrow function does not have its own this context instead it inherits it from its (non arrow funtion) parent  
    //Use implicite return if only single line, you dont need to use return
    //You can assigne arrow function to variable or use it immediately   
    const addOperator = (x,y) => x + y; //implicite return 
    const subtractOperator = (x,y) => { //explicite return
        return x - y; 
    };     
    
    console.log("addOperator: " + addOperator(12,45));
    console.log("subtractOperator: " + subtractOperator(34,12));
    //We can also pass it to a function 
    console.log("addOperator (15): " + mathOperation(addOperator, 7, 8));
    console.log("subtractOperator(9): " + mathOperation(subtractOperator, 13, 4));
    console.log("4 * 5 = 20: " + mathOperation((z,n) => z * n, 4, 5));

    function mathOperation(operator, x, y) {
        return operator(x,y); 
    }

    //Functions
    //Note you can call a function with less arguments than specified in the function.
    //Note void functions return Undefined
    AddDigits(7, 9);
    AddDigits(12); //second argument will be Undefined

    //this value is determined by the parent scope 
    function AddDigits(x, y) {
        console.log("x: " + x + " y: " + y);
    }

    //While used to iterate unknown number of loops (for example, loop until helper function returns null)    
    //For known number of iterations 
    //For of, used in Array or collection iteration 
    const names = ["Justin", "Nora", "Chris"];

    for(let i = 0; i < names.length; i++) {
        console.log("for " + names[i]);
    }

    let whileIndex = 0; 
    while(whileIndex < names.length) {
        console.log("while " + names[whileIndex]);
        whileIndex++;
    }

    for (const name of names) { //Note uses of  instead of in (c#)
        console.log("for-of " + name);
    } 

    const arrayLength = 5;
    let myArray = new Array(arrayLength);
    let myArray2 = Array(arrayLength);
    let myArray3 = []; 
    let myArray4 = ["Alfa", "Beta", "Gamma", "Delta", 5, true];


    console.log(myArray.length);
    console.log(myArray2.length);
    console.log(myArray3.length);
    console.log(myArray4.length + ", " +  myArray4.join(", "));

    //Note & and | can be used not to shortcut 
    if(returnsTrue() | returnsTrue()) {
        console.log("Called twice..");
    }

    if(returnsTrue() || returnsTrue()) {
        console.log("Called once, using shortcut notation.");
    }
    
    function returnsTrue() {
        console.log("returnsTrue() is called.");
        return true;
    }

    //Example of switch 
    const returnVal = 400;

    switch(returnVal) {
        case 200:
            console.log("OK");
            break;
        case 300:
        case 400:
            console.log("Error");
            break;
        default:
            console.log("Unknown");
            break;
    }

    //Boolean Notes
    //Empty string is false
    //null or undefined test as false
    //number 0 test false 
    const condition = 0;
    if(!condition) {
        console.log("condition is not true.");
    }

    //String comparison is case sensitive 
    const nameJohn = "John";
    if(nameJohn !== "john") {
        console.log("Names are different");
    }

    if(nameJohn.toLowerCase() === "JOHN".toLowerCase()) {
        console.log("Names are Equal");
    } 

    /* Using localeCompare()
    JavaScript's String#localeCompare() method gives you more fine-grained control over string comparison. 
    For example, you can also compare two strings ignoring diacritics. Below is how you can do case-insensitive 
    string comparison using localeCompare(): */
    const str1 = 'Bill@microsoft.com';
    const str2 = 'bill@microsoft.com';
    // 0, means these two strings are equal according to `localeCompare()`
    if(str1.localeCompare(str2, undefined, { sensitivity: 'accent' }) === 0) {
        console.log("localeCompare, Names are Equal");
    } 

    const status = 200;

    if(status === 200) {
        console.log("OK");
    } 
    else if(status === 400) {
        console.log("ERROR");
    }
    else {
        console.log("Unknown status");
    }

    const message001 = status === 200 ? "_OK" : "_NOT OK"; 
    console.log(message001);

    //new Date(), milliseconds since 1-1-1970
    //Note Month counting starts with 0 
    const now = new Date();
    const earlyDate = new Date(2015,10,23,23,59,59,59);
    const earlyDate2 = new Date(2000,0,26);
    console.log(now);
    console.log(earlyDate);
    console.log(earlyDate2);
    
    const now2 = new Date(); 
    now2.setFullYear(2009);
    now2.setMonth(0);
    now2.setDate(19);
    now2.setHours(21);
    now2.setMinutes(29);
    now2.setSeconds(59);
    console.log(now2);

    const now3 = new Date(); 
    console.log(now3.getFullYear());
    console.log(now3.getMonth());
    console.log(now3.getDay() + ", "+ now3.getDate()); //now3.getDay(), sunday == 0 
    console.log(now3.getTime()); //milliseconds since 1-1-1970

    let num001 = "250";
    let num002 = "66.99";
    let num003 = new Number(1980);
    let num004 = new Number(20.21);

    console.log(`parseInt("${num001}") + 10}: ${parseInt(num001) + 10}`); 
    console.log(`parseInt("ABC"): ${parseInt("ABC")}, should be NaN`); 
    console.log(`(hex) parseInt("0x0F")}: ${parseInt("0x0F")}`); 
    console.log(`parseFloat("${num002}"): ${parseFloat(num002)}`);
    console.log(`parseFloat("ABC"): ${parseFloat("ABC")}, should be NaN`);
    console.log(parseInt(`${1 + 1}`))
    console.log("num003.toString(): " + num003.toString());
    console.log("num004.toString(): " + num004.toString());

    //use MATH object for calculations 
    console.log(`PI: ${Math.PI}`);
    console.log(`sqrt(100): ${Math.sqrt(new Number(100))}`);
    
    //Simple math
    let number01 = 100;   
    console.log(`${number01} + 25 = ${number01 + 25}`);
    console.log(`${number01} - 25 = ${number01 - 25}`);
    console.log(`${number01} * 100 = ${number01 * 100}`);
    console.log(`${number01} / 1500 = ${number01 / 1500}`);
    
    console.log(`${number01} % 30 (remainder) = ${number01 % 30}`);
    console.log(`++number01 = ${++number01}, should be 101`);
    console.log(`number01++ = ${number01++}, should be 101 because of previous increment`);
    console.log(`number01 = ${number01}, should be 102 because of previous increment`);

    //Number (float), String, Boolean, Date, Array, Object, Function
    //NaN, null, Undefined
    //Checking Type
    //typeof operator: Returning a string of the data type primitive 
    //instanceof operator: Returns true if a value matches the data type
    const people = ["John", "Nora", "Michael"];
    const digitOne = 1;
    const myMsg = "Hello world.";
    const b = true;
    //Object
    const person = {
        firstName: "John",
        lastName: "Beers"
    }

    function sayHello(personObj) {
        console.log(personObj.firstName +", "+ personObj.lastName);
    }

    console.log("------ typeof ------");
    console.log("people array: " + typeof(people));    
    console.log("Digit: " + typeof(digitOne));
    console.log("String: " + typeof(myMsg));
    console.log("Bool: " + typeof(b));
    console.log("Object: " + typeof person ); //You can also specify it without parentheses 
    console.log("Function: " + typeof sayHello );

    console.log("------ instanceof ------");
    console.log(`people instanceof Array: ${people instanceof Array}`); //true    
    console.log(`digitOne instanceof Number: ${digitOne instanceof Number}`);// false used literal value
    console.log(`(new Number(12)) instanceof Number: ${(new Number(12)) instanceof Number}`);//true is object and not literal

    console.log(`myMsg instanceof String: ${myMsg instanceof String}`); // false used literal value
    console.log(`(new String("A string value")) instanceof String: ${(new String("A string value")) instanceof String}`); //true is object and not literal

    console.log(`b instanceof Boolean: ${b instanceof Boolean}`); // false used literal value
    console.log(`(new Boolean(false)) instanceof Boolean: ${(new Boolean(false)) instanceof Boolean}`); //true is object and not literal
     
    console.log(`person instanceof Object: ${person instanceof Object}`); //true
    console.log(`sayHello instanceof Function: ${sayHello instanceof Function}`); //true    
    
    //Equals
    let isEqual = '' == 0;
    console.log(`let isEqual = '' == 0; : ${isEqual}, should be true`)
    isEqual = '' === 0;
    console.log(`isEqual = '' === 0; : ${isEqual}, should be false`);

    //Concatenation be carful with combining strings and numbers
    const num1 = 1;
    const num2 ="1";
    console.log(num1 + 1); //2 number addition
    console.log(num2 + 1); //11 string concat
    //The backtick char: `
    const bool1 = true;
    console.log(`7 + 8 = ${7 + 8}`);
    console.log(`The opposite of ${bool1} is ${!bool1}.`);
    
    const length = 20;    
    let charsetChosen = array1.concat(array2);    
    charsetChosen = charsetChosen.concat(array3);
    
    try {

        for(let k = 0; k < 100; k++) {
            let password = "";
            for(let j = 0; j < length; j++) {
                password += charsetChosen[getRandomInt(charsetChosen.length)]
            }
            console.log("%s", password);
        }        
    }
    catch(e) {
        console.log(e.message);
    }

    return;

    for(let j = 0; j < length; j++) {
        password += array5[getRandomInt(0, array5.length)];         
    }

    for(let j = 0; j < array5.length; j++) {
        console.log(array5[j]);
    }

    for(let j = 0; j < array1.length; j++) {
        console.log(array1[j]);
    }
        
    let charset = UPPERCASE_ALPHABET + LOWERCASE_ALPHABET + DIGITS;  
    charset = charset.replace("9", "");
    charset = charset.replace("3", "");
    
    for(let i = 0; i < charset.length; i++) {
        console.log(charset[i]);
    }
    console.log(charset);

    for(let i = 0; i < UPPERCASE_ALPHABET.length; i++) {
        console.log(UPPERCASE_ALPHABET[i]);     //let res = str.charAt(str.length-1); instead of index
    }

    let submitBtn = document.getElementById("submit");
    submitBtn.style = "background-color: blueviolet;";
    let msg = "This is a test message";
    let str = reverseString(msg, " ");
    console.log(`${msg}, ${str}`);

    let arr = arrayFromLowToHigh(5,10);
    arr.forEach(num => console.log(num));
    
    let chars = printCharset(LOWERCASE_CHAR_CODES);
    console.log(chars);

    chars = printCharset(UPPERCASE_CHAR_CODES);
    console.log(chars);

    chars = printCharset(NUMBER_CHAR_CODES);
    console.log(chars);

    chars = printCharset(SYMBOL_CHAR_CODES);
    console.log(chars);

    var one = 1; /* Flexible, Function scoped, can be changed in scope, available before declaration. */
    let two = 2; /* Block scoped {...}, can be changed in scope,  only available after declaration.*/
    const three = 3; /* Block scoped {...}, cannot be changed,  only available after declaration. */

    { /* Let can be redeclared once outside scope */
        let four = "four";
        console.log(four);
    }

    {
        let four = "#four";
        console.log(four);
    }

    let four = "##four";
    console.log(four);

    /* Let can be redeclared once outside scope */
    {
        const PI = 3.14;
        console.log(PI);
    }

    const PI = 7.14;
    console.log(PI);

})

/*
Getting a random integer between two values
This example returns a random integer between the specified values. 
The value is no lower than min (or the next integer greater than min 
if min isn't an integer), and is less than (but not equal to) max.

Can be improved using:

Assuming that window.crypto.getRandomValues is available 
var array = new Uint32Array(10);
window.crypto.getRandomValues(array);    

console.log("Your lucky numbers:");
for (var i = 0; i < array.length; i++) {
    console.log(array[i] % 20);
}

https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/MAX_SAFE_INTEGER
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Math/random
https://developer.mozilla.org/en-US/docs/Web/API/Crypto/getRandomValues
*/
function getRandomInt() { //(min, max) {

    let min = 0, max = 0; 

    if(arguments.length == 0) {
        min = 0;
        max = Number.MAX_SAFE_INTEGER; 
    }
    else if (arguments.length == 1) {
        min = 0;
        max = arguments[0]; 
    } 
    else if (arguments.length == 2) {
        min = arguments[0];
        max = arguments[1]; 
    } 
    else {
        throw new Error("Max two arguments can be passed, respectively max and min value.");
        //throw "Max two arguments can be passed resp. max and min value.";
    } 

    if (isNaN(min) || isNaN(max)) {
        throw new Error("Min and Max value must be a number.");
        //throw "Min and Max value must be a number.";
    }

    //console.log(`getRandomInt: min[${min}] max[${max}]`);   
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min) + min); //The maximum is exclusive and the minimum is inclusive      
}

function printCharset(charset) {
    let lowerCaseStr2 = "";  
    let lowerCaseArr = new Array(); 
    for(let i = 0; i < charset.length; i++) { 
        lowerCaseArr.push(String.fromCharCode(charset[i]));
        lowerCaseStr2 += String.fromCharCode(charset[i]);  
    }

    var lowerCaseStr = lowerCaseArr.join(", ");
    //console.log(lowerCaseStr);
    //console.log(lowerCaseStr2);
    return lowerCaseStr;
}

function arrayFromLowToHigh(e,t) {    
    const n=[];
    
    for(let c = e; c <= t; c++) {
        n.push(c);
    }        
        
    return n
}

/*-------------------------------
This code reverses a string.
-------------------------------*/
function reverseString(message, separator) {
    
    var strArray = message.split(separator);
    let reversedValue = "";

    for(var i = strArray.length - 1; i >= 0; i -- ) {
        reversedValue += " " + strArray[i];    
    }

    return reversedValue; 
}