"use strict"

const EnterkeyCode = 13;
let taskManager = null;

/* Document Ready Event, initializing the event handlers. */
$(document).ready(function(){
    /* Initialize the Todo List. */
    init();

    /* Processing the enter key in textbox */
    $("#addTask").keydown(function(event) {
        if(event.keyCode === EnterkeyCode && event.target.value !== "") {
            addNewTask();
            event.target.value = ""; /* Clear textbox */
        }
    });

    /* Add task button clicked. */
    $("#addTaskBtn").click(function() {
        if($("#addTask").val() !== "") {
            addNewTask();
            $("#addTask").val(""); /* Clear textbox */    
        }    
    }); 
});

function init() {
    /* TaskManager is created and used by functions in this file. */
    taskManager = new taskNamespace.TaskManager("Tasks");
    taskManager.load(); /* Load localstorage */
    for (const task of taskManager.tasks) {
        addTaskToWebPage(task);       
    }
}

/* Adds task information to <ul> list. */
function addTaskToWebPage(task) {
    const content = generateTaskHtml(task);
    $("#taskList").append(content);    
} 

function addNewTask() {
    const description = $("#addTask").val();
    const task = new taskNamespace.Task(description, helpersNamespace.dateFormatter(new Date()));
    taskManager.add(task);
    taskManager.save();
    addTaskToWebPage(task);
}

/* Removes a task from the task list. */
function deleteTask(event, id) {
    event.preventDefault(); /* Prevents redirect by <a> link. */
    taskManager.remove(id);
    taskManager.save();
    $('#' + id).remove();
}

/* Set task status.  */
function toggleTaskStatus(sender, id) {    
    const task = taskManager.find(id);
    const divElement = sender.parentNode.parentNode.nextElementSibling;
    
    //task.isComplete = sender.checked;
    task.setComplete(sender.checked);
    taskManager.save();

    /* Update Web Page. */
    if(sender.checked === true) {
        divElement.classList.add("complete");
    }
    else {
        divElement.classList.remove("complete");
    }
}

/* Generates a HTML list item containing the task info. */
function generateTaskHtml(task) {
    return `
      <li id="${task.id}" class="list-group-item checkbox">
        <div class="row">
          <div class="col-md-1 col-xs-1 col-lg-1 col-sm-1 checkbox">
            <label><input type="checkbox" onchange="toggleTaskStatus(this, '${task.id}')"${task.isComplete ? ' checked':''}></label>
          </div>
          <div class="col-md-6 col-xs-6 col-lg-6 col-sm-6 task-text${task.isComplete ? ' complete':''}">
            ${task.description}
          </div>
          <div class="col-md-4 col-xs-4 col-lg-4 col-sm-4 task-date">
            ${task.dateCreated}
          </div>
          <div class="col-md-1 col-xs-1 col-lg-1 col-sm-1 delete-icon-area">
            <a href="#0" onClick="deleteTask(event, '${task.id}')">
            <i class="fas fa-trash"></i></a>
          </div>
        </div>
      </li>
    `;
}

/*
REFERENCES:

https://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid  How to generate a GUID.
https://gist.github.com/wch/7090027 Namespace example in Javascript. This also demonstrates the module pattern.

Defining classes
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes

Using arrays
https://www.w3schools.com/js/js_arrays.asp
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/Array

JavaScript HTML DOM EventListener
https://www.w3schools.com/js/js_htmldom_eventlistener.asp

Date formatter
https://www.w3schools.com/js/js_dates.asp
https://www.w3schools.com/js/js_date_formats.asp
https://www.w3schools.com/js/js_date_methods.asp
https://www.w3schools.com/js/js_date_methods_set.asp
https://stackoverflow.com/questions/12409299/how-to-get-current-formatted-date-dd-mm-yyyy-in-javascript-and-append-it-to-an-i

Non-Navigating Links for JavaScript Handling
https://weblog.west-wind.com/posts/2019/Jan/21/NonNavigating-Links-for-JavaScript-Handling

String empty check 
https://stackoverflow.com/questions/154059/how-can-i-check-for-an-empty-undefined-null-string-in-javascript

https://stackoverflow.com/questions/154059/how-can-i-check-for-an-empty-undefined-null-string-in-javascript

keypress/event.keyCode event  is no longer recommended.
https://developer.mozilla.org/en-US/docs/Web/API/Document/keypress_event 
https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/keyCode
https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/key

Notes:
Json serialization and deserialization of new date() object gives rise to problems.

-------------------------------------------------------------------------
- Namespace example below from: https://gist.github.com/wch/7090027
-------------------------------------------------------------------------
// mynamespace is an object to use as a namespace
mynamespace = (function() {

  // Variables in the namespace
  var mynamespace = {
    foo: "Yes, this is foo."
  };

  // "Public" methods for the namespace
  mynamespace.fooTwo = function() {
    return twice(this.foo);
  };

  // "Private" methods in the namespace
  function twice(x) {
    return x + x;
  }


  // A class in the namespace
  mynamespace.CoolClass = (function() {
    // Contstructor
    var coolclass = function() {
      this.bar = "A bar.";
    };

    // Convenience  var for the prototype
    var prototype = coolclass.prototype;

    // "Public" methods - add to the prototype
    prototype.barThree = function() {
      return thrice(this.bar);
    };

    // "Private" methods - functions starting with "_" are private only by
    // convention. 
    prototype._barNine = function() {
      return thrice(thrice(this.bar));
    };

    // Internal functions - these are captured in the closure, note that they
    // are not duplicated when you do `new CoolClass()`. They cannot access
    // `this`.
    function thrice(x) {
      return x + x + x;
    }

    return coolclass;
  })();

  // Instantiate the CoolClass (must be after CoolClass is defined)
  mynamespace.coolObject = new mynamespace.CoolClass();

  return mynamespace;
})();


// Outside of the anonymous function, can access the following:
console.log(mynamespace.foo);        // "Yes, this is foo."
console.log(mynamespace.fooTwo());   // "Yes, this is foo.Yes, this is foo."

// The instantiated CoolClass object, and public members
console.log(mynamespace.coolObject);
console.log(mynamespace.coolObject.bar);        // "A bar."
console.log(mynamespace.coolObject.barThree()); // "A bar.A bar.A bar."

// Constructor for CoolClass
console.log(new mynamespace.CoolClass());
*/
