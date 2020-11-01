window.requestAnimFrame = (function (callback) {
    return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame 
    || window.oRequestAnimationFrame || window.msRequestAnimationFrame ||
    
    function (callback) {
        window.setTimeout(callback, 1000 / 30);
    };
})();

window.setTimeout(getDirection, 30000);
var x = 176, y = 176, w = 600, h = 600, r = 26;
var d,c,s;
var rColor,gColor,bColor;
var hd = "r";
var horizontal = true;

/* Entry point when page is loaded. */
window.addEventListener("load", function() {

    try {        
        c = document.getElementById("c");
        s = parseInt( document.getElementById("speedy").value);

        w = c.width;
        h = c.height;
        
        getDirection();
        drawBall();

        /* Add EventListener. */
        document.getElementById("intensiveWork").addEventListener("click", DoIntensiveWork);
        document.addEventListener("keydown", processKeyInputs);        
    } catch (e) {
        console.log("Excepion in function, msg: window.addEventListener" + e.message);
    }
});

function increaseSpeed() {
    s++;
    document.getElementById("speedy").value = s;
}

function decreaseSpeed() {
    s--;
    document.getElementById("speedy").value = s;
}

function changeDirection() {
    var cx = window.event.offsetX;
    var cy = window.event.offsetY;
    x = cx;
    y = cy;
    document.getElementById("speedy").value = s;
}

function setNewPoint(d) {
    try {

        switch (horizontal) {
            case true:
                if (x < (w - r) && hd == "r")
                    x += s;
                else if(x > r && hd == "l")
                    x -= s;
                break;
            case false:
                if (y < (h - r) && hd == "d")
                    y += s;
                else if (y > r && hd == "u")
                    y -= s;
                break;
        }

        if (x >= (w - r))
            hd = "l";
        
        if (x <= r)
            hd = "r";
        
        if (y >= (h - r))
            hd = "u";
        
        if (y <= r)
            hd = "d";

    } catch (e) {
        console.log("Excepion in function setNewPoint(d), msg: " + e.message);
    }
}

function processKeyInputs() {
    
    console.log("rcvd: onkeydown: " + window.event.keyCode);
    switch (window.event.keyCode) {
        case 40:
            horizontal = false;
            hd = "d";
            break;
        case 37:
            horizontal = true;
            hd = "l";
            break;
        case 38:
            horizontal = false;
            hd = "u";
            break;
        case 39:
            horizontal = true;
            hd = "r";
            break;
    }
}

function getDirection() {
    
    horizontal = !horizontal;
    var d = Math.ceil(Math.random() * 2);
    if (horizontal) {
        if (d == 1) {
            hd = "r";
        } else {
            hd = "l";
        }
    } 
    else {
        if (d == 1) {
            hd = "u";
        } else {
            hd = "d";
        }
    }
}

function drawBall() {
    try {
        var rgbFill = "rgb(0,0,0)";
        var rgbStroke = "rgb(128,128,128)";
        setNewPoint(d);
        var ctxt = c.getContext("2d");
        ctxt.clearRect(0, 0, c.width, c.height);
        ctxt.beginPath();
        ctxt.lineWidth = "5";
        ctxt.strokeStyle = rgbStroke;
        ctxt.arc(x, y, r, 0, 360);
        ctxt.fillStyle = rgbFill;
        ctxt.fill();
        ctxt.stroke();
        s = parseInt( document.getElementById("speedy").value);

        requestAnimFrame(function () {
            drawBall();
        });
    } 
    catch (e) {
        console.log("Excepion in function drawBall(), msg: " + e.message);
    }
}

/* This code simply displays a small ball bouncing around inside a canvas. Users can use the
arrow keys to change the ball’s direction. Users would expect a smooth experience. Now
you can introduce an intensive mathematical operation to occur at the click of a button. The
button is on the form already and is called intensiveWork. Add the following function to the
bottom of the script block to do some intense math:

This function does nothing more than calculate the sum of a series of squares and display
the result to users. The amount of work to do is hard coded in this example but could be
extended to get the information from users as well.

The problem with this code is that although the math work is occurring, the ball interaction
is blocked completely. The ball stops moving and user input is seemingly ignored until
the math call returns. The call to run the calculations takes too long and interferes. You can
experiment with smaller numbers and see that eventually the number can be small enough so
the work happens fast enough that the ball isn’t stopped. This doesn’t mean that the application
is doing work concurrently, although visibly no interruption occurs.

This code starts with assigning the onmessage handler a function to run when spawned
within the context of a worker. At the end of the message, it calls postMessage to return
a result back to the caller. Save this file, and then change the click event handler for the
intensiveWork button in the bouncing ball code as follows:

*/
function DoIntensiveWork() {

    var result = document.getElementById("workResult");
    result.innerText = "";
    //You need to run webserver to access other files on file system.
    //https://stackoverflow.com/questions/21408510/chrome-cant-load-web-worker
    //https://stackoverflow.com/questions/30202529/physi-js-causes-the-error-script-cannot-be-accessed-from-origin-null/44468468
    //var url ="data:text/plain;base64,ZmlsZTovLy9DOi9Vc2Vycy9tb2hhbS9EZXNrdG9wL1dlYnNpdGVzLzcwLTQ4MC1jb3Vyc2UvanMvQ2FsY3VsYXRlV29ya2VyLmpz";
    //var url =`data:application/x-javascript;base64,ZmlsZTovLy9DOi9Vc2Vycy9tb2hhbS9EZXNrdG9wL1dlYnNpdGVzLzcwLTQ4MC1jb3Vyc2UvanMvQ2FsY3VsYXRlV29ya2VyLmpz`;
    //var worker = new Worker(url);
  
    var worker = new Worker("./js/CalculateWorker.js");
    
    //Specifies the function for the worker thread to call back to when complete. 
    //This function accepts a single parameter in the form of EventData with a property 
    //named data containing the values.
    worker.onmessage = function (evt) {
        try {
            result.innerText = evt.data;
        } catch (e) {
            console.log("Exception in worker.onmessage: " + e.message);
        }
    }

    // Specifies a function to call when an error occurs in the worker thread. 
    //The onerror method receives event data, including the following: message: 
    //textual message of the error filename: the filename the error occurred in 
    //lineno: the line number in the file that created the error
    worker.onerror = function (err) {
        console.log(err.message + err.filename + err.lineno);
    }
    
    //Starts the worker process. This method expects a single parameter containing the 
    //data to pass to the worker thread. If nothing is required in the worker thread, 
    //an empty string can be supplied.
    //An input box can be added to the HTML and the entered value can be passed to the worker.
    var value = document.getElementById("inputValue").value;
    console.log("value to worker: " + value);
    worker.postMessage(value);
    //worker.postMessage("");

    //To provide an option to stop the worker process, you need to implement the terminate method.
    //Because of closures, function has a reference to worker object   
    document.getElementById("stopWorker").onclick = function () {
        console.log("Request received, worker.terminate()");
        worker.terminate();
    }

    //Note the code below is replace by the worker thread above
    /*var work = 10000000;
    var i = 0;
    var a = new Array(work);
    var sum=0;

    console.log("Entering DoIntensiveWork()");
    var result = document.getElementById("workResult");
    result.innerText = "";
    
    for (i = 0; i < work; i++) {
        a[i] = i * i
        sum += i * i;
    }

    result.innerText = sum;*/
}