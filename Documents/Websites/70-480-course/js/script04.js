"use strict";

/*
Structured error handling is provided by the JavaScript language in the form of the
try…catch…finally block.

■ The try…catch…finally block provides a way to try some logic, catch an error and
handle it appropriately, and finally do some clean up.

■ The finally block always runs whether or not an exception is thrown.

■ Checking for a null value before accessing any objects to ensure that they are initialized
is good practice.
*/
window.addEventListener("load", () => {
    example001();
    example002();
    example003();
    example004();
    example005();
});

/* Checking for null values
One way to prevent many errors is to check for null values before using something. A null
value in a JavaScript program is what a variable equals before it’s initialized. JavaScript knows
about the variable’s existence but doesn’t yet have a value.
A common place to ensure that variables have values is in functions that accept parameters.
Consider the following function: 
In this code, the developer forgot to initialize the variable c, resulting in a null value. In the
multiplyNumbers method, the parameters are evaluated for a null value and, if found, an error
is thrown. If this method didn’t check for null values and assumed that every developer calling
it would never make a mistake, the results would be unexpected to the consumer of the
method. In this case, the result would be NaN (not a number), a special JavaScript type. This is
because of the attempt to perform a mathematical operation against a null value. */
function example005() {
    try {
        var a, b, c;
        a = 5;
        b = 10;
        c = 12;
        var result = multiplyNumbers(a, b, c);
        console.log(result);
    } 
    catch (e) {
        console.log(e.message + ", " + e.number);
    }
    
    /* Function within function are allowed. */
    function multiplyNumbers(first, second, third) {
        if (first == null || second == null || third == null) {
            var error = new Error("Forgot to initialize a number.");
            error.number = 5;
            throw error;
        }
            
        return first * second * third;
    }
}


/* More commonly when working with custom libraries, you can create custom exceptions to
give users information specific to the situation that occurred: 
In this code, a custom object to represent a ball is created. It has a draw method that
expects a canvas context to draw itself on. However, if the coordinates for the ball aren’t
initialized, the ball object throws a custom error. The calling code has a try…catch block so
that it can handle any unexpected errors. In this example, the consumer of the ball object
would get a meaningful message that the x-coordinate needs to be set to something valid.
A new object, Error, is used here to create the exception. The Error object constructor takes
two parameters, in this order: the error number followed by an error description. This information
should be as specific as possible to provide as much detail as possible to the calling code. */
function example004() {
    var ball = {
        x: -1,
        y: -1,
        draw: function DrawBall(c) {
            if (this.x < 0)
                /* Create custom error object. */
                var error = new Error("Invalid X coordinate");  
                error.number = 25;
                error.name = "Draw error.";
                throw error;  
            }
    }

    try {
        ball.draw(null);
    } catch (e) {
        var msg = "message: " + e.message + ", number: " + e.number + ", name: " + e.name;
        console.log(msg);    
    } 
}

/*
Exceptions bubble up the call stack, a special stack in the processing environment that represents
the functions currently being processed in sequential order. Take the following code sample: 

Because the processData method has no exception handling, the exception bubbles
up to the calling method, the next method in the stack. This continues through the stack until
either an exception handler is met or the browser receives the exception and treats it as an
unhandled exception. Of course, in this case, the variables in the processData method
can’t be accessed, so if anything needed to be done in a finally block, the try…catch…finally
block should be either moved into the WorkWithCanvas method, or the WorkWithCanvas
method can handle the error and rethrow it for further processing.
The concept of raising an error is also known as throwing an exception. Custom objects and
libraries throw exceptions as needed to the consumers of the libraries. The objects or libraries
expect you to meet certain conditions and if those conditions aren’t met, they can throw
an exception for the consumer to deal with. To continue with the example, the exception is
handled in the WorkWithCanvas method and then rethrown. An exception is thrown using
the throw keyword:

In this example, the exception can be handled in the catch block as needed, and then
thrown back up the call stack to be handled again at another level. */
function example003() {
    try {
        processData();
    } catch (e) {
        console.log("From example003(): " + e.message);
    }
}

function processData() {
    try {
        window.unsupportedmethod();
    } catch (e) {
        console.log("From processData(): " + e.message);
        throw e; /* Exception is rethrown. */
    }
    finally{
    }    
} 

/* Now, with the structured error handling in place, when the line with the typo is hit,
processing jumps into the catch block. In this block, the error could be logged for future
diagnostics. After the catch block completes, the finally block runs. If any cleanup or variable
resetting needs to be done, it can be done here even though an exception occurs. The finally
block also runs. If the typo is fixed so that no exceptions occur in the try block, the catch
doesn’t occur because of nothing to catch, but the finally block still runs. The finally block
always runs as the last part of a try…catch…finally block.
Variable scope applies to each block within the try…catch block. If a variable is declared
within the try portion, it won’t be accessible from the catch or the finally.
If you want to have access in those blocks, the variables need to be declared outside the try block.

*/
function example002() {
    var canvas = null;  /* Variables available in the catch/finnaly method. */
    var context = null;

    try {
        canvas = document.getElementById("myCanvas");
        context = canvas.getContext("2d");
        context.fillStyle = "blue";
        context.arc(50, 50, 25, 0, 360);
        context.fill();
        context.strokeStyle = "red";
        context.stroke();
    }
    catch (e) {
        console.log(e.message);
    }
    finally {
        console.log("Finally is executed.");
        /*do any final logic before exiting the method The declaration for the reference to 
        the canvas and the canvas context is moved outside the try block so that it can be
        accessible in the catch block. The catch block can now write the error to the canvas.*/
        context.strokeText("Message from finnaly", 100, 50);        
    }
} 

/*The try…catch block is divided into two parts. The first part, the try portion, says, “Try to
do this work.” If anything goes wrong when trying to do the work, the catch block receives
an exception object with information about the error. Any code inside the try portion of the
try…catch block is protected against encountering an unhandled error.
The catch block is where the error can be handled as appropriate for the application. The
catch block receives a parameter that is an exception object.

Properties available on the exception object:
message     => A textual description of the error that occurred
number      => A numeric error code
name        => The name of the exception object

You can use the information provided in the exception object to decide what to do in
terms of overall program flow. For example, if the program needs access to a resource that
it can’t have and an exception is thrown, the program can fall back to a different process
to achieve the desired functionality or simply tell the user that something needs to be
changed—for example, if cookies or another HTML5 API are required for the site to work. */
function example001() {
    try{
        window.dosomeunsupportedmethod();
    } catch (e) {
        var msg = "Browser does not support the desired functionality, message: " 
            + e.message +", number: " + e.number + ", " + e.name;

        console.log(msg);    
        DisplayError(msg);   
    }
} 

function DisplayError(message) {
    var errElement = document.getElementById("error"); 
    errElement.innerHTML = "<strong>" + message + "</strong>";
}

