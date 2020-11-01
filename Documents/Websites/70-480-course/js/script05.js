/*
Each event fires in the order in which it was added when the window is finished loading. To
remove the onloadHandler2 event, all that’s needed is a call to the removeEventListener:
window.removeEventListener("load", onloadHandler2, false);

When handling DOM events, the custom events you create are not a replacement for the
built-in functionality provided by the DOM element. The handling of the event allows you to
do some custom logic or manipulation, but when event handling is complete, the processing
returns back to the JavaScript API, which processes its own implementation for the event. If
this isn’t desirable, you can stop the event processing.
*/
window.addEventListener("load", onloadHandler, false);
window.addEventListener("load", onloadHandler2, false);
window.addEventListener("load", onloadHandler3, false);

function onloadHandler() {
    console.log("onloadHandler 1.");
}

function onloadHandler2() {
    console.log("onloadHandler 2.");
}

function onloadHandler3() {
    console.log("onloadHandler 3.");
}

/*
Using anonymous functions
In the examples so far, event handlers have been assigned via named functions. The advantage
to using named functions is that you can later remove event listeners as needed. You
can’t identify anonymous functions after they are assigned as event listeners to manipulate
them. In the example in the preceding section, three event listeners were added to the same
event, and then one event was removed. This was possible only because the name of the
event listener function was known.As expected, an anonymous function has no name. It’s completely 
anonymous and can’t be called from other code segments.

In JavaScript, functions are objects that can be assigned to variables. This is how the
anonymous function event listener works. It assigns a function object to the onload property
of the window object, which in turn handles the event when the window is completely loaded.
You can use anonymous functions in most cases where a function is expected as a parameter
also. Take the following code sample:

In this sample, the addEventListener method is used. But instead of passing in the function
name to call when the event is triggered, an anonymous function is passed in. The only
potential problem with this approach is the ability to later remove the event listener with the
removeEventListener method. That the following code would work might seem logical:

window.removeEventListener("load",
function () {
document.getElementById("outer").addEventListener("click", outerDivClick, false); },
false);

But this isn’t the case. Because the event listeners that the addEventListener method adds are
stored by their signatures, this removeEventHandler method can’t know the signature of the
previous anonymous function. Even passing in the exact same anonymous implementation
doesn’t work because this isn’t the same anonymous function; it’s a new one and therefore
doesn’t match the signature of the added one. */
window.addEventListener("load", 
    function () {
        console.log(`window.addEventListener("load", function () {`);
        document.getElementById("outerDiv").addEventListener("click", 
            function() {
                if(this.style.backgroundColor === "red")
                    console.log("backgroundColor is red.");    
                if(this.style.backgroundColor === "green")
                    console.log("backgroundColor is green.");
                
                this.style.backgroundColor = this.style.backgroundColor === "red" ? "green" : "red";     

                console.log("outerDiv is clicked.");       
            }, false);
    },
    false);

/*
Canceling an event
The ability to cancel event processing can be useful when you want to completely override
the implementation of the native functionality of a DOM element. A perfect example
is if it was required to override the inherent functionality of an anchor element. An event
listener would be set up for the click event. Then in the click event, via the event object, the
returnValue property is set to false or the function itself can return false. This tells the runtime
to stop any further processing of the event. The following code demonstrates this: 
In this case, when the anchor is clicked, the custom event handler runs but no further logic
is processed. Hence, the navigation typically provided by the <a> element is prevented from running. */
/*window.onload = function () {
    console.log("aLink function.");
    var aLink = document.getElementById("aLink");
    a.onclick = OverrideAnchorClick;
}*/

window.addEventListener("load", function() {
        console.log("aLink function.");
        var aLink = document.getElementById("aLink");
        aLink.onclick = OverrideAnchorClick;
    }, 
    false);


function OverrideAnchorClick() {
    //do custom logic for the anchor
    console.log("Link cannot be followed.");
    window.event.returnValue = false;
    var divFeedback = document.getElementById("feedback");
    divFeedback.innerText = "I told you so, link cannot be followed.";
    //or
    //return false;
}

/*
Declaring and handling bubbled events
*/
window.addEventListener("load", 
    function() {
        document.getElementById("outer").addEventListener("click", outerDivClick, false);
        document.getElementById("middle").addEventListener("click", middleDivClick, false);
        document.getElementById("inner").addEventListener("click", innerDivClick, false);
        document.getElementById("clearButton").addEventListener("click", clearList);
        //https://stackoverflow.com/questions/14544104/checkbox-check-event-listener
        var checkbox = document.querySelector("input[name=eventBehaviour]");
        checkbox.addEventListener( 'change', checkboxClick);

        var checkbox2 = document.querySelector("input[name=cancelBubble]");
        checkbox2.addEventListener( 'change', checkbox2Click);

        /* Range selector  */
        document.getElementById("aRange").addEventListener("change", rangeChangeEvent);
        /* OnChange TextBox */
        document.getElementById("aText").addEventListener("change",textChangeEvent);
        
        //document.getElementById("firstNameText").focus();
        document.getElementById("firstNameText").addEventListener("blur", function () {
            if (this.value.length < 5) {
                document.getElementById("ruleViolation").innerText = 'First Name is required to be 5 letters.';
                document.getElementById("ruleViolation").style.color = 'red';
                //this.focus();
            }
            else {
                document.getElementById("ruleViolation").innerText = "";
            }
        });
        
        //keydown
        document.querySelector('#_inputTextBox').addEventListener('keydown', logKey);
        //keyup
        document.querySelector('#keyupTextBox').addEventListener('keyup', logkeyup);
        //handler on doc
        document.addEventListener('keydown', documentOnkeydown);

        //https://developer.mozilla.org/en-US/docs/Web/API/Element/mouseenter_event
        document.getElementById("mouseTarget").addEventListener("click", divOnClick);
        document.getElementById("mouseTarget").addEventListener("mouseenter", divOnMouseEnter);
        document.getElementById("mouseTarget").addEventListener("mouseleave", divOnMouseLeave);
    
        /*  The CustomEvent object constructor accepts two parameters:
            1)The first parameter is the name of the event. This is anything that makes sense for what
            the event is supposed to represent. In this example, the event is called anAction.
            2) The second parameter is a dynamic object that contains a detail property that can
            have properties assigned to it containing information that should be passed to the
            event handler. Also, the parameter provides the ability to specify if the event should
            bubble and whether the event can be canceled.
            Create a new event, allow bubbling, and provide any data you want to pass to the "detail" property
            
            https://developer.mozilla.org/en-US/docs/Web/Guide/Events/Creating_and_triggering_events
        */
        //Creating custom events
        const form = document.querySelector('form');
        const textarea = document.querySelector('textarea');

        // The form element listens for the custom "awesome" event and then consoles the output of the passed text() method
        //document.addEventListener('awesome', (e) => {
        form.addEventListener('awesome', (e) => {
            console.log(e.detail.text() + ", " + e.detail.timeofevent + ", " + e.detail.description);
            console.log("window.event.detail.description: " + window.event.detail.description);
            console.log("window.event.detail.text(): " + window.event.detail.text());
        });

        // As the user types, the textarea inside the form dispatches/triggers the event to fire, and uses itself as the starting point
        textarea.addEventListener('input', e => {
            console.log("@textarea input.");
            
            /* Define custom event. */
            const eventAwesome = new CustomEvent('awesome', {
                bubbles: true,
                detail: { 
                        text: () => textarea.value,
                        description: "a description of the event",
                        timeofevent: new Date(),
                        eventcode: 2 
                },
                cancelable: true
            });        

            e.target.dispatchEvent(eventAwesome)
        });

    }, 
    false);

function divOnMouseEnter(e) {
    this.classList.add("scale"); /* Scale up Div element on mouse enter  */
    
    var text = "OnMouseEnter: " + e.clientX + "," + e.clientY + ", " + e.offsetX + ", " + 
    e.offsetY + ", " + e.screenX + "," +  e.screenY
    addListItem(text);
}

function divOnMouseLeave(e) {
    this.classList.remove("scale"); /* Scale down Div element on mouse enter  */

    var text = "OnMouseLeave: " + e.clientX + "," + e.clientY + ", " + e.offsetX + ", " + 
    e.offsetY + ", " + e.screenX + "," +  e.screenY
    addListItem(text);
}

function divOnClick(e) {
    var text = "OnClick: " + e.clientX + "," + e.clientY + ", " + e.offsetX + ", " + 
    e.offsetY + ", " + e.screenX + "," +  e.screenY
    addListItem(text);
    //console.log("OnClick: " + e.clientX + "," + e.clientY + ", " + e.offsetX + ", " + e.offsetY +
    //", " + e.screenX + "," +  e.screenY);
    //console.log(e);
}

function addListItem(text) {
    const unorderedList = document.getElementById('unorderedList');
    // Create a new text node using the supplied text
    var newTextNode = document.createTextNode(text);
  
    // Create a new li element
    var newListItem = document.createElement("li");
  
    // Add the text node to the li element
    newListItem.appendChild(newTextNode);
  
    // Add the newly created list item to list
    unorderedList.appendChild(newListItem);  
}

function documentOnkeydown(e) {
    if (window.event.ctrlKey && String.fromCharCode(window.event.keyCode) == 'M')
        console.log("ctrlKey: " + e.ctrlKey + ", code" + e.code);    

    /*console.log("documentOnkeydown" + ` ${e.code}`);
    if (window.event.ctrlKey && String.fromCharCode(window.event.keyCode) == 'F')
    document.getElementById("firstNameText").focus();
    if (window.event.ctrlKey && String.fromCharCode(window.event.keyCode) == 'L')
    document.getElementById("lastNameText").focus();
    return false;*/    
}

function logkeyup(e) {
    //console.log("logKey(e)" + ` ${e.code}`);
    document.getElementById('keyupLog').textContent += ` ${e.code}`;
}    
    
function logKey(e) {
    //console.log("logKey(e)" + ` ${e.code}`);
    document.getElementById('_log').textContent += ` ${e.code}`;
}    

/* This example uses the this keyword. In this context, the this keyword provides a direct
reference to the element that created the event. In this way, this provides shortcut access
to the element rather than gets a reference via one of the document search methods. */
function rangeChangeEvent() {
    document.getElementById("rangeValue").innerText = this.value;
}

function textChangeEvent() {
    document.getElementById("textValue").innerText = this.value;
}

var cancelBubble = false;     
function checkbox2Click() {
    if(this.checked) {
        cancelBubble = true;
    } 
    else {
        cancelBubble = false;
    }
}

function checkboxClick() {
    /* Remove  EventListeners */
    document.getElementById("outer").removeEventListener("click", outerDivClick);
    document.getElementById("middle").removeEventListener("click", middleDivClick);
    document.getElementById("inner").removeEventListener("click", innerDivClick);
    
    var bubbling = false; 
    if(this.checked) {
        bubbling = true; 
        console.log("Checkbox is checked.");
    } else {
        console.log("Checkbox is unchecked.");
    }

    document.getElementById("outer").addEventListener("click", outerDivClick, bubbling);
    document.getElementById("middle").addEventListener("click", middleDivClick, bubbling);
    document.getElementById("inner").addEventListener("click", innerDivClick, bubbling);
} 

function outerDivClick() {
    appendText("outer Div Clicked", "red");
}

function middleDivClick() {
    appendText("middle Div Clicked", "green");
}

function innerDivClick() {
    appendText("inner Div Clicked", "blue");
    //This stops only the bubbling or cascading behavior. The code to
    //cancel the bubbling of the event is added to the inner div element’s event listener:
    window.event.cancelBubble = cancelBubble;
}

var index = 0;
function appendText(s, color) {
    var li = document.createElement("li");    
    li.style.backgroundColor = color;
    li.innerText = s + ": (" + index++ + ")";
    document.getElementById("eventOrder").appendChild(li);
}

function clearList() {
    var ol = document.createElement("ol");
    ol.id = "eventOrder";
    document.getElementById("mainContainer").replaceChild(ol, document.getElementById("eventOrder"));
}


