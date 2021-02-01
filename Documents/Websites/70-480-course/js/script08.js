"use strict"

/*
In this case, $(this) provides a reference to the floorDiv element, and you can do whatever you
want with that element. In this case, you are only changing the background color style property of the div.
In this example, when floorDiv is clicked, $(“div”) finds all the div elements in the page.
Then it calls the each operator, which calls the callback function passed into it for each element
that’s returned. Then, $(this) is used to modify the background color of each div. In this
way, the use of the this keyword is extremely efficient because it provides quick direct access
to each element with very little code.
*/
$(document).ready(function() { 
    console.log("JQuery ready.");
    
    $("#floorDiv").click(function () {        
        $(this).css("background-color", "red");

        $("div").each(function () { 
            $(this).css("background-color", "red");
        });
    });
});

/* Entry point when page is loaded. */
window.addEventListener("load", function() {
    //The following script is the more traditional method to assign event handlers to the DOM elements:
    //Using anonymous functions Each button is given its own function inline, where the implementation can be customized
    //for each button click.
    document.getElementById("door1").onclick = function () { console.log("door1 clicked"); };
    document.getElementById("door2").onclick = function () { console.log("door2 clicked"); };
    document.getElementById("door3").onclick = SubmitClick; //function () { console.log("door3 clicked"); };

    /*    
    The this pointer is a special object provided by the jQuery framework. When running selections
    against the DOM using jQuery, this refers to the object that it finds or the collection of
    objects that it finds. It provides a shortcut to accessing the item within the current context of
    jQuery filter. In a simple example, the this keyword can be demonstrated as follows:
    */

    DoLongTask(2 /*$("#inputValue").val()*/, 
        function (result, data) {
            if (result === "SUCCESS")
                console.log(data + " is a Success");
            else
                console.log(data + " is a fail");
        });    

    function DoLongTask(value,func)
    {
        if (value < 10)
            func("SUCCESS", value);
        else
            func("FAIL", value);
    }

    /*
    Here a function is declared that can be used throughout the page. This function has
    a name: SubmitClick. Because this function has a name, it’s not an anonymous function.
    However, a named function like this can be assigned to as many button events as you want:

    With a named function, the convenience of reuse is there. However, in some cases this
    is more overhead than is necessary. This also can make the code more difficult to follow in
    terms of being able to easily see what’s actually happening in the click event handler. In a
    situation that specifies distinct behavior for each button, anonymous functions simplify things greatly. */
    function SubmitClick() {
        console.log("&&door3 clicked");
        //do some logic here
    }

    /* This fairly simple script has only three cells to add a click event to. But if the page is to
    get more complex and have up to 20 or 50 doors, this code becomes tedious. This is where
    jQuery can simplify things. The preceding code can be replaced with this code: 
    The click events are assigned only to the <td> elements that are children of an element
    named GameRow. jQuery provides advanced selector capabilities that allow fine control over
    how the DOM is manipulated.*/
    $("td").click(function () { 
        console.log("JQuery td clicked");
        console.log(this);
        console.log(this.id); 
    });

    $("#GameRow td").click(function () { 
        console.log("@@@#GameRow, JQuery td clicked");
        console.log(this);
        console.log(this.id); 
    });


    /*
    Notice how much easier this code is. In one line, all <td> elements are assigned a click event.
    This code applies to all <td> elements on the page. So, if some <td> elements aren’t part of the
    page, you need to ensure that the selector is unique to the required elements. This can be accomplished
    with cascading style sheets (CSS) or by using the DOM hierarchy.
    */

    /* jquery syntax
    In this sample, the jQuery selector syntax is used to find the search button on the page by
    its name. Then the click event is assigned a function that runs when the button is clicked.
    This syntax is quite powerful. Aside from being cross-browser friendly, it includes much
    flexibility in how event handlers are assigned to objects. This jQuery selector syntax supports
    all the same type of searches that the document object exposes. But the part that differentiates
    jQuery from the document object is that jQuery can assign styles or events to everything
    in the result set in one line. */
    $("#searchButton").click(processSearchRequest);

    // Call server to get the name
    $ajaxUtils.sendGetRequest("data/pies.json", 
        function (request) {
            console.log("Ajax callback.");    
            let content = request.responseText;
            let person = JSON.parse(content);
            console.log(content);
            console.log("firstName: " + person.firstName);
            console.log("lastName: " + person.lastName);
            //var name = request.responseText;
            //document.querySelector("#content")
            //    .innerHTML = "<h2>Hello " + name + "!</h2>";
        });
    
        callBackFunction((result) => {
            //Result in console after 5 second delay:
            setTimeout(function() {
                console.log("Resukt in callBackFunction: " + result);
            },5000);
            
        }, 12, 14);
    
    /*In this code example, two functions are declared: WillCallBackWhenDone and MyCallBack.
    One parameter to the WillCallBackWhenDone function is a function followed by two other
    variables, which in this case are numbers that will be multiplied. The product of the multiplication
    is passed to the callback function. This case is a bit over the top for the usage of
    callbacks, but it does demonstrate the pattern involved. Anytime a function is called that expects
    a function as a parameter, this is what it’s doing. Knowing what parameters the callback
    function will receive is important so that they can be specified in the parameter list. */
    callBackFunction(onCallBack, 3, 6); //Will Call Back When Done
    createQuote("eat your vegetables!", logQuote); // 1

    /* Websocket inputs. */
    const url = 'ws://localhost:8080';  //Secure protocol use wss
    let wsConnection;
    const chatBox = document.getElementById("chatWindow");
    const disconnectButton = document.getElementById("Disconnect");
    const connectButton = document.getElementById("Connect");
    const sendButton = document.getElementById("Send");
    const msgToSend = document.getElementById("msgSendText");    

    /* Websocket functions */ 
    //Or the use of wss for secure WebSockets, Opens the WebSocket.    
    connectButton.onclick = function () {
        //When the user clicks the button, the WebSocket is instantiated with the appropriate connection information:        
        // The URL of the server-side socket to connect to, which is always prefixed with ws or
        // ■ wss for secure WebSocket connections
        // ■ An optional list of subprotocols
        // When the WebSocket constructor is called, the WebSocket API establishes a connection
        // to the server. One of two things can happen at this stage. The WebSocket will successfully
        // connect to the server or the connection will fail, resulting in an error. Both cases should be        
        // handled so that the proper feedback is provided to the application user. The WebSocket API
        // provides an event for each, called onopen and onerror.        
        wsConnection= new WebSocket(url, ['soap', 'xmpp']);
        
        //As you can see eventhandlers are set in the connection method.
        //Else the wsConnection is undefined.   
        // event handler for when the WebSocket connection is established
        wsConnection.onopen = function () {
            chatBox.textContent = chatBox.textContent + "System: Connection has been established.";
            wsConnection.send('Greetings to server from client.');
        }

        //The error event could happen at any time, not just when establishing the initial connection.
        //event handler for when the WebSocket encounters an error
        wsConnection.onerror = function (err) {
            //write an error to the screen
            chatBox.value = chatBox.value + "System: Error Occurred. ";
        }

        // When finished with a chat session, a user should be able to exit cleanly. This is accomplished
        // by calling the close method of the WebSocket object. The close method can be called
        // with no parameters. It also allows the use of two optional parameters. A numerical code and a
        // reason can be provided but isn’t mandatory. In this example, the connection is closed with no
        // parameters. When a connection is closed, the onclose event handler is raised:
        wsConnection.onclose = function () {
            //write the connection has been closed.
            chatBox.value = chatBox.value + "\r\nSystem: Connection has been closed.";
        }

        // When other users of the chat application send messages into the system, the server calls
        // the event handler specified in onmessage. The onmessage event receives a message parameter
        // with the data property that contains the message. This message is extracted and displayed
        // in the chat window for users to see.        
        //To receive messages, the WebSocket API provides the onmessage event handler.         
        wsConnection.onmessage = function (msg) {
            //write message
            chatBox.value = chatBox.value + "\r\nThem: " + msg.data;
        }        
    }

    //When a successful connection is established, you can send and receive messages over the
    //socket. To send messages, the WebSocket API provides the Send function.   
    sendButton.onclick = function () {
        //check the state of the connection
        //The WebSocket API provides a mechanism to check the current status of the connection. 
        //To prevent an error, the readyState property is evaluated to ensure that it’s now open. 
        //readyState provides four possible values, as described below.
        //1) OPEN The connection is open.
        //2) CONNECTING The connection is in the process of connecting and not ready for use yet. This is the default value.
        //3) CLOSING The connection is in the process of being closed.
        //4) CLOSED The connection is closed.
        //
        // After confirming that the connection is in the appropriate state for sending a message, the
        // send method is called with the text that the user entered into the chat application.
        if (wsConnection != null && wsConnection.readyState == WebSocket.OPEN) {
            //send message to server.
            wsConnection.send(msgToSend.value);
            chatBox.value = chatBox.value + "\r\nYou: " + msgToSend.value;
            msgToSend.value = '';    
        }
    }

    disconnectButton.onclick = function () {
        if(wsConnection != null)
        {
            chatBox.value = chatBox.value + "\r\nSystem: Closing connection with server.";
            wsConnection.send('Bye bye server, it was a pleasure interacting with you.');
            wsConnection.close();
        }            
    } 
});

function createQuote(quote, callback){ 
    var myQuote = "Like I always say, " + quote;
    callback(myQuote);
}

function logQuote(quote){
    console.log(quote);
}
  
function onCallBack(result) {
    console.log("onCallBack: " + result);
}

async function callBackFunction(f, x, y) {
    console.log("Entering callBackFunction.");

    for (let i = 1; i < 20; i++) {        
        await sleep(250);
        console.log(`${i}, ${(new Date).toLocaleTimeString()}`);
    }

    f(x*y);
 }

 function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
 }

function processSearchRequest() {
    let searchPath = "data/employees.json";    
    $("#searchResults").empty();
    
    switch ($("#searchFruit").val()) {
        case "long":
            searchPath = "data/employees.json";
            break;
        case "round":
            searchPath = "data/employees.json";
            break;
        case "orange":
            searchPath = "data/employees.json";
            break;
        default:
            InvalidSearchTerm();
    }

    console.log(searchPath);
    GetData(searchPath);

    function InvalidSearchTerm() {
        $("#searchResults").empty();
        $("#searchResults").append("Invalid Search Term. Please try again.");
    }
}

/*
The data request is
made using AJAX and as such the entire page doesn’t need to refresh, only the area that displays
the results. The part of the page where the data is needed is the only part of the page
that is affected by the new data being received.
The first thing that this code does is set up an event listener for the search button click
event. All the magic occurs in this function. The search term is evaluated to ensure that it
matches one of the supported search terms. If it doesn’t, the user is presented with a message
indicating this. If it does, the code proceeds to make an AJAX call to the server for the correct
data set that matches the search term. In this case, it’s a hard-coded XML file. However, the
data source is irrelevant as long as the returned XML matches the schema that the webpage
expects so that it can be parsed and displayed.
The first parameter being set is the url that the AJAX call will be requesting. For security
reasons, to prevent cross-site scripting, this URL must be within the same domain as the webpage
itself.
The next parameter, cache, is optional and indicates whether the call can use a cached
copy. The third parameter, datatype, indicates the expected data type, which could be XML or
JavaScript Object Notation (JSON), for example.
The last parameter set in this example is the success property. This parameter takes a function
that the results of the AJAX calls should be passed into for the webpage to do some work
with. In this example, the results are parsed and added to the DOM so that users can see the
results.
Another property that can be set on the AJAX call, as good practice, is the error property
so that any error conditions can be handled gracefully.

The jQuery AJAX toolkit supports not only getting data, but also posting data to the
server. The default request type is GET. To change a call to a post, you change the value of the
property to POST:
*/
function GetData(searchPath) {
    $.ajax({
        type: "GET",
        url: searchPath,
        cache: false,
        //Example only retrieves json files.
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        
        success: function (response) {
            let len = response.Employees.length;
            //console.log("len: " + len);
            for(let i=0; i<len; i++){
                let id = response.Employees[i].userId;
                let firstName = response.Employees[i].firstName;
                let lastName = response.Employees[i].lastName;
                let content = "<strong>#id: " + id + ", firstName: " + firstName + ", lastName: " + lastName + "</strong>"; 
                console.log(content);

                $("#searchResults").append(content);
                $("#searchResults").append("<BR/>");

                // var tr_str = "<tr>" +
                //     "<td align='center'>" + (i+1) + "</td>" +
                //     "<td align='center'>" + username + "</td>" +
                //     "<td align='center'>" + name + "</td>" +
                //     "<td align='center'>" + email + "</td>" +
                //     "</tr>";
                //
                // $("#userTable tbody").append(tr_str);
            }

            $.each(response.Employees, function() {
                console.log(this["userId"] + ", " + this["firstName"] + ", " + this["lastName"]);
                //$("ul").append("<li>Name: "+this['name']+"</li>
                //<li>Age: "+this['age']+"</li>
                //<br />");
            });

            //return response.Employees;
        }, //End of AJAX Success function

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function
        
        /*
        The error function is passed three useful parameters:
        ■■ The HTTP request itself
        ■■ The HTTP error number (such as 404)
        ■■ The error text (such as Not Found)
        */
        error: function (xhr, textStatus, errorThrown) {
            $('#searchResults').append(errorThrown);
        }

        // error: function (data) {
        //     alert(data.responseText);
        // } //End of AJAX error function        
    });
} 



