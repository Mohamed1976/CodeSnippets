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

/* Entry point when page is loaded. */
window.addEventListener("load", function() {
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