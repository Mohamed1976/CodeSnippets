// *******************************
// START HERE IF YOU WANT A MORE CHALLENGING STARTING POINT FOR THIS ASSIGNMENT
// *******************************
//
// Module 4 Assignment Instructions.
//
// The idea of this assignment is to take an existing array of names
// and then output either Hello 'Name' or Good Bye 'Name' to the console.
// The program should say "Hello" to any name except names that start with a "J"
// or "j", otherwise, the program should say "Good Bye". So, the final output
// on the console should look like this:
/*
Hello Yaakov
Good Bye John
Good Bye Jen
Good Bye Jason
Hello Paul
Hello Frank
Hello Larry
Hello Paula
Hello Laura
Good Bye Jim

WARNING!!! WARNING!!!
The code does NOT currently work! It is YOUR job to make it work
as described in the requirements and the steps in order to complete this
assignment.
WARNING!!! WARNING!!!

*/

// STEP 1:
// Wrap the entire contents of script.js inside of an IIFE
// See Lecture 52, part 2
// (Note, Step 2 will be done in the SpeakHello.js file.)

var names = ["Yaakov", "John", "Jen", "Jason", "Paul", "Frank", "Larry", "Paula", "Laura", "Jim"];

// STEP 10:
// Loop over the names array and say either 'Hello' or "Good Bye"
// using the 'speak' method or either helloSpeaker's or byeSpeaker's
// 'speak' method.
// See Lecture 50, part 1
/* fill in parts of the 'for' loop to loop over names array */
var message = null;
for (var i =0; i < names.length; i++) {

    // STEP 11:
    // Retrieve the first letter of the current name in the loop.
    // Use the string object's 'charAt' function. Since we are looking for
    // names that start with either upper case or lower case 'J'/'j', call
    // string object's 'toLowerCase' method on the result so we can compare
    // to lower case character 'j' afterwards.
    // Look up these methods on Mozilla Developer Network web site if needed.
    var firstLetter = names[i].toLowerCase().charAt(0);

    // STEP 12:
    // Compare the 'firstLetter' retrieved in STEP 11 to lower case
    // 'j'. If the same, call byeSpeaker's 'speak' method with the current name
    // in the loop. Otherwise, call helloSpeaker's 'speak' method with the current
    // name in the loop.
    //console.log(names[i] + ", " + firstLetter); // + ", j===" + firstLetter.toLowerCase() === 'j');
    //console.log(names[i] + ", " + firstLetter);    
    if(firstLetter === "j") {
        message = byeSpeaker.speak(names[i]);
    }
    else {
        message = helloSpeaker.speak(names[i]); 
    }
    //console.log(message);
    //if (/* fill in condition here */) {
    // byeSpeaker.xxxx
    //} else {
    // helloSpeaker.xxxx
    //}
}

//You have a few choices when it comes to using JavaScript to access the DOM. You can
//access DOM elements through a global object provided by the browser, called document, or
//through the elements themselves after you obtain a reference to one.

//getElementById Gets an individual element on the page by its unique id attribute value
//getElementsByClassName Gets all the elements that have the specified CSS class applied to them
//getElementsByTagName Gets all the elements of the page that have the specified tag name or element name
//querySelector Gets the first child element found that matches the provided CSS selector criteria
//querySelectorAll Gets all the child elements that match the provided CSS selector criteria
window.onload = function () {
    var list = document.getElementById("persons");
    //To find all the <ul> elements on a page, you can use this syntax:
    var pElements = document.querySelectorAll("ul");

    //pElements[0];
    /*Flexible querySelector and querySelectorAll methods.
    The querySelector and querySelectorAll methods allow you to achieve most of what you’ve
    already done with the other methods. Both methods take a parameter in the form of a CSS
    selector. The querySelector method returns the first element it finds that matches the selector
    criteria passed to it, whereas the querySelectorAll method returns all elements that match
    the selector criteria passed in. The elements are still returned in the form of a NodeList object.
    Both methods exist not only on the document itself, but also on each element. Therefore,
    when you have a reference to an element, you can use these methods to search its children
    without having to traverse the entire document. */

    var message = null;
    
    console.log(pElements[0]);
    //Add names in array as li to ul     
    for (var i =0; i < names.length; i++) {
        var firstLetter = names[i].toLowerCase().charAt(0);
        if(firstLetter === "j") {
            message = byeSpeaker.speak(names[i]);
        }
        else {
            message = helloSpeaker.speak(names[i]); 
        }
        
        var listItem = document.createElement("li");
        listItem.innerText = message;
        //Note we can use getElementById or querySelectorAll to retrieve reference to <ul>
        //list.appendChild(listItem);
        pElements[0].appendChild(listItem);
    }
} /* window.onload = function () */