/* Variables */
let drawingSurface;
let context;
let start;
let timePassed = 0;
let movingSpeed = 1;
/* Variables used for positioning rectangle. */
let rectX = 0;
let rectY = 0;
/* Used to display client Width and Height  */
let clientDimensions;
let delta = 0;
let drawingSurface5;

var canvas_;
var ctx_;
var x_;
var y_;
var dx_ = 2;
var dy_ = -2;

window.onload = function () {
    drawingSurface5 = document.getElementById("drawingSurface5").getContext("2d");;
    clientDimensions = document.getElementById("dimensions");
    drawingSurface = document.getElementById("drawingSurface");
    context = drawingSurface.getContext("2d");

    canvas_ = document.getElementById("myCanvas");
    ctx_ = canvas_.getContext("2d");
    x_ = canvas_.width/2;
    y_ = canvas_.height-30;
    dx_ = 2;
    dy_ = -2;

    setInterval(_draw, 100);

    //JavaScript Timing Events, you can start and stop the event handler
    //https://www.w3schools.com/js/js_timing.asp
    setInterval(myTimer, 1000);

    draw();

    addCircles();

    document.getElementById("svg3").addEventListener("click", function(event) {
        //console.log("click svg3" + event);
        delta +=10;
        var transformation = "translateX(" + delta + "px)";
        var svg3 = document.getElementById("svg3");
        //svg3.style.transform = transformation;
        this.style.transform = transformation;
      });

    /* The window.requestAnimationFrame() method tells the browser that you wish to perform an 
    animation and requests that the browser calls a specified function to update an animation
    before the next repaint. The method takes a callback as an argument to be invoked before the repaint.
    Your callback routine must itself call requestAnimationFrame() if you want to animate another frame
    at the next repaint. */
   window.requestAnimationFrame(animateLoop);
}

function _drawBall() {
    ctx_.beginPath();
    ctx_.arc(x_, y_, 10, 0, Math.PI*2);
    ctx_.fillStyle = "#0095DD";
    ctx_.fill();
    ctx_.closePath();
}

function _draw() {
    ctx_.clearRect(0, 0, canvas_.width, canvas_.height);
    _drawBall();
    x_ += dx_;
    y_ += dy_;
}

var circles = [];
circles[circles.length]=Circle({ x: 369, y: 116, label: "A" });
circles[circles.length]=Circle({ x: 231, y: 278, label: "1" });
circles[circles.length]=Circle({ x:133, y: 396, label: "D" });
circles[circles.length]=Circle({ x: 234, y: 360, label: "C" });
circles[circles.length]=Circle({ x: 351, y: 232, label: "B" });
circles[circles.length]=Circle({ x:348, y: 388, label: "4" });
circles[circles.length]=Circle({ x:164, y: 199, label: "5" });
circles[circles.length]=Circle({ x:522, y: 425, label: "3" });
circles[circles.length]=Circle({ x:229, y: 120, label: "E" });
circles[circles.length]=Circle({ x:493, y: 237, label: "2" });

function addCircles()
{
    /*var circles = [];
    circles[circles.length]=Circle({ x: 369, y: 116, label: "A" });
    circles[circles.length]=Circle({ x: 231, y: 278, label: "1" });
    circles[circles.length]=Circle({ x:133, y: 396, label: "D" });
    circles[circles.length]=Circle({ x: 234, y: 360, label: "C" });
    circles[circles.length]=Circle({ x: 351, y: 232, label: "B" });
    circles[circles.length]=Circle({ x:348, y: 388, label: "4" });
    circles[circles.length]=Circle({ x:164, y: 199, label: "5" });
    circles[circles.length]=Circle({ x:522, y: 425, label: "3" });
    circles[circles.length]=Circle({ x:229, y: 120, label: "E" });
    circles[circles.length]=Circle({ x:493, y: 237, label: "2" });*/

    //circles.forEach(function(circle) { circle.draw(); });
    for(var i =0; i < circles.length; i++)
    {
        circles[i].draw();
    }
}

function Circle(I) { 
    I.radius = 30;
    I.draw = function() {
        drawingSurface5.beginPath();
        drawingSurface5.arc(I.x, I.y, I.radius, 0, 2 * Math.PI, false);
        drawingSurface5.fillStyle = 'white';
        drawingSurface5.fill();
        drawingSurface5.lineWidth = 2;
        drawingSurface5.strokeStyle = '#003300';
        drawingSurface5.stroke();
        drawingSurface5.font = '15pt Calibri';
        drawingSurface5.fillStyle = 'black';
        drawingSurface5.textAlign = 'center';
        drawingSurface5.fillText(I.label, I.x, I.y);
     };
     return I;
}

function myTimer() {    
    var d = new Date();
    var txt = "";
    txt += "<p><b> Time: " + d.toLocaleTimeString() + "</b></p>";
    txt += "<p>innerWidth: " + window.innerWidth + "</p>";
    txt += "<p>innerHeight: " + window.innerHeight + "</p>";
    txt += "<p>outerWidth: " + window.outerWidth + "</p>";
    txt += "<p>outerHeight: " + window.outerHeight + "</p>";
    clientDimensions.innerHTML = txt;

    /* Update circle position  */
    updateCirclePos();
}

function draw() {
    var canvas = document.getElementById('drawingSurface2');
    if (canvas.getContext) {
       var ctx = canvas.getContext('2d');
  
      ctx.beginPath();
      ctx.arc(75, 75, 50, 0, Math.PI * 2, true); // Outer circle
      ctx.moveTo(110, 75);
      ctx.arc(75, 75, 35, 0, Math.PI, false);  // Mouth (clockwise)
      ctx.moveTo(65, 65);
      ctx.arc(60, 65, 5, 0, Math.PI * 2, true);  // Left eye
      ctx.moveTo(95, 65);
      ctx.arc(90, 65, 5, 0, Math.PI * 2, true);  // Right eye
      ctx.stroke();

      ctx.beginPath();
      ctx.arc(350, 75, 50, 0, Math.PI * 2, true); 
      ctx.stroke();
    }
  }


let elapsedCounter = 0;
function updateCirclePos() {
    elapsedCounter++;
    var circle = document.getElementById("Circle");
    var svgCircle2 = document.getElementById("svgCircle2");

    // `Math.min()` is used here to make sure that the element stops at exactly 200px.
    var transformation = 'translateX(' + Math.min(5 * elapsedCounter, 800) + 'px)';
    //console.log(transformation);
    circle.style.transform = transformation; //'translateX(' + Math.min(0.1 * elapsedCounter, 200) + 'px)';
    svgCircle2.style.transform = transformation; //'translateX(' + Math.min(0.1 * elapsedCounter, 200) + 'px)';
}

/* Event handlers used to color   */
function Red(evt) {
    var circle = evt.target;
    circle.setAttribute("style", "fill: red");
}

function Green(evt) {
    var circle = evt.target;
    circle.setAttribute("style", "fill: green");
}

/* https://www.paulirish.com/2011/requestanimationframe-for-smart-animating/ 
https://developer.mozilla.org/en-US/docs/Web/API/window/requestAnimationFrame 
https://spicyyoghurt.com/tutorials/html5-javascript-game-development/create-a-smooth-canvas-animation */
function animateLoop(timestamp) {
    
    if (start === undefined)
        start = timestamp;
  
    const elapsed = timestamp - start;
    
    //if(elapsed > 1000) {
    //    console.log("One second elapsed timestamp: " + timestamp);
    //    start = timestamp;
    //}

    updatePosition((elapsed / 1000));
    drawRect();

    window.requestAnimationFrame(animateLoop);
}

function updatePosition(secondsPassed) {
    // Use time to calculate new position
    rectX += (movingSpeed * secondsPassed * 2);
    rectY += (movingSpeed * secondsPassed);

    //timePassed += secondsPassed;
    // Use different easing functions for different effects.
    //rectX = easeInOutQuint(timePassed, 50, 500, 1.5);
    //rectY = easeLinear(timePassed, 50, 250, 1.5);
}

// Example easing functions
function easeInOutQuint (t, b, c, d) {
    if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
    return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
}

function easeLinear (t, b, c, d) {
    return c * t / d + b;
}

function drawRect() {
    //Clear the canvas before drawing
    // Clear the entire canvas
    context.clearRect(0, 0, drawingSurface.width, drawingSurface.height);

    context.fillStyle = '#ff8080';
    context.fillRect(rectX, rectY, 150, 100);
}

//https://www.sitepoint.com/community/t/detect-mouse-click-and-release-on-a-circle/242374/5
//https://jsfiddle.net/AllanP/2d3ukq8a/
var xPos, yPos, circX, circY, clickInfo=[]; // global
function clickIt(evt) { 
    var msgObj=document.getElementById("msg");
    var msg2Obj=document.getElementById("msg2");
    
    //Example create circle 
    var _circles =Circle({ x: 400, y: 250, label: "XY" });
    _circles.draw();

    if(clickInfo.length==2)     // after 2 clicks clear array
    { clickInfo.length=0; 
       msgObj.innerHTML="Please click on the page within the blue lines";
       msg2Obj.innerHTML="<br>&nbsp;";
     }
   var i, xDiff, yDiff, dist, result, cX, cY, startLength; 
   xPos=null; yPos=null; circX=null; circY=null; 
   evt= evt || event;
   xPos=evt.offsetX || evt.pageX;
   yPos=evt.offsetY || evt.pageY;
 // check posn against centres
   startLength= clickInfo.length;        
   for( i=0;i<circles.length;i++)
    { cX=circles[i].x; cY=circles[i].y;
      xDiff=Math.abs(cX-xPos);
      yDiff=Math.abs(cY-yPos);
      dist=Math.sqrt(Math.pow(xDiff,2)+Math.pow(yDiff,2)); 
     // add info on clicked circle to array       
      if(dist <=30)
       {clickInfo[clickInfo.length]={label:circles[i].label, circX:circles[i].x, circY:circles[i].y, xPos:xPos, yPos:yPos };
       }          
    } 
    result= (clickInfo.length!=startLength)? "You hit circle "+clickInfo[clickInfo.length-1].label+"" : "Try to click on a circle";  
    msgObj.innerHTML=result;
    // show click co-ordinates
    var firstClickInfo, secondClickInfo; 
    if(clickInfo.length==2) 
     { firstClickInfo="label: "+clickInfo[0].label+"; circX:"+clickInfo[0].circX+"; circY:"+clickInfo[0].circY+"; xPos:"+clickInfo[0].xPos+"; yPos:"+clickInfo[0].yPos+"<br>";
       secondClickInfo="label: "+clickInfo[1].label+"; circX:"+clickInfo[1].circX+"; circY:"+clickInfo[1].circY+"; xPos:"+clickInfo[1].xPos+"; yPos:"+clickInfo[1].yPos+"";            
       msg2Obj.innerHTML=firstClickInfo+secondClickInfo; 
     }
 } 
