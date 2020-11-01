const BUFFER_NAME = "Tasks";
const DEBUG = false;
let taskManager = null;

window.addEventListener("load", () => {
    LogMessage("Entering Webpage load.");
    taskManager = new TaskManager();    
});
 
class TaskManager {
    constructor() {
        LogMessage("TaskManager constructor.");
        /* Task Textbox is referenced multiple times  */
        this.taskTextbox = document.getElementById("addTask");
        this.addEventListeners();

        /*this.tasks = new Array(new Task("Get groceries", false, DateFormatter(new Date())), 
            new Task("Go to Dentist", false, DateFormatter(new Date())),
            new Task("Do Gardening.", false, DateFormatter(new Date())),
            new Task("Renew Library Account", false, DateFormatter(new Date())));*/
  
        this.load();    
        //this.save();    
        //this.print();     
        /*for(var i = 0; i < tasks.length; i++)
        {
            console.log("Task[" + i + "]: " + tasks[i].description + ", " 
                + tasks[i].isComplete + ", " + tasks[i].dateCreated); //DateFormatter()
        }
        
        var tasksJson = JSON.stringify(tasks);
        console.log("tasksJson: " + tasksJson);
        var tasksFromJson = JSON.parse(tasksJson); 
        
        tasksFromJson[0].isComplete = true;

        for(i = 0; i < tasksFromJson.length; i++)
        {
            console.log("Task[" + i + "]: " + tasksFromJson[i].description + ", " 
                + tasksFromJson[i].isComplete + ", " + tasksFromJson[i].dateCreated);
        } */
    }

    addEventListeners() {
        //document.getElementById("addTaskBtn").addEventListener("click", this.onTaskAddClick);
        document.getElementById("addTaskBtn").addEventListener("click", () =>
        {
            var content = this.taskTextbox.value;
            LogMessage("OnTaskAddClick executed, value textbox: " + content);
            if(content !== "") {
                LogMessage("Calling add Task, task description: " + content);
                this.addTask(content)
                this.taskTextbox.value = "";
            }                
        });
                
        /* Note we can call local methods if we call them from the definition of the eventhandler (as shown below).
           We cannot reference a function and then call local function fromthat defined eventhandler method, 
           because the this pointer points to the control that initiaited the event.   
        */
        //keypress/event.keyCode event  is no longer recommended.
        //https://developer.mozilla.org/en-US/docs/Web/API/Document/keypress_event 
        //https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/keyCode
        //https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/key
        //document.getElementById("addTask").addEventListener("keypress", this.onKeyPress);
        //document.getElementById("addTask").addEventListener("keydown", (event) =>
        this.taskTextbox.addEventListener("keydown", (event) =>
        {
            LogMessage("Entering onKeyPress, event.key === " + event.key + ", value === " + event.target.value);
            //https://stackoverflow.com/questions/154059/how-can-i-check-for-an-empty-undefined-null-string-in-javascript
            if(event.key === "Enter" && event.target.value !== "") {
                LogMessage("Calling add Task, task description: " + event.target.value);
                this.addTask(event.target.value)
                event.target.value = "";
            }
        });

        LogMessage("addEventListeners");
    }

    /* Add task when enter is pressed in textbox. */
    onKeyPress(event) {
        LogMessage("Entering onKeyPress, event.keyCode === " + event.keyCode + ", value === " + event.target.value);
        //https://stackoverflow.com/questions/154059/how-can-i-check-for-an-empty-undefined-null-string-in-javascript
        if(event.keyCode === 13 && event.target.value !== "") {
            LogMessage("Calling add Task, task description: " + event.target.value);
            //self.addTask(event.target.value)
            event.target.value = "";
            //this.self.print(); /* This refers to the event source textbox */
        }
    }     

    onTaskAddClick() {
        LogMessage("OnTaskAddClick executed.");
    }

    load() {
        LogMessage("Entering loadTasks().");
        LogMessage("BUFFER_NAME: " + BUFFER_NAME);
        var Json = localStorage.getItem(BUFFER_NAME);
        LogMessage("Json (tasks serialized data read from localstorage): " + Json);        
        var tasksObj = JSON.parse(Json);
        LogMessage("Number of tasks deserialized: " + tasksObj.length);
        this.tasks = tasksObj;
        /* Add tasks to html page. */
        let tasksHtml = this.tasks.reduce((html, task, index) => html += this.generateTaskHtml(task, index), '');
        //var tasksHtml = this.generateTaskHtml(tasksObj[0], 0);
        LogMessage(tasksHtml);
        document.getElementById('taskList').innerHTML = tasksHtml;
    }

    save() {
        LogMessage("Entering save().");
        var Json = JSON.stringify(this.tasks);
        LogMessage("BUFFER_NAME: " + BUFFER_NAME);
        LogMessage("Json (tasks serialized): " + Json);
        localStorage.setItem(BUFFER_NAME, JSON.stringify(this.tasks));
        LogMessage("Exiting save().");           
    }

    clear() {
        localStorage.clear();
    }

    addTask(description) {
        LogMessage("Entering addTask(), description: " + description);
        //The push() method adds new items to the end of an array, and returns the new length. 
        //Note: The new item(s) will be added at the end of the array
        this.tasks.push(new Task(description, false, DateFormatter(new Date())));
        //let task = new Task(description, false, DateFormatter(new Date()));
        this.save();
        this.load();
    }

    print() {
        for(var i = 0; i < this.tasks.length; i++)
        {
            LogMessage("Task[" + i + "]: " + this.tasks[i].description + ", " 
                + this.tasks[i].isComplete + ", " + this.tasks[i].dateCreated);
        }
    }

    generateTaskHtml(task, index) {
        return `
          <li class="list-group-item checkbox">
            <div class="row">
              <div class="col-md-1 col-xs-1 col-lg-1 col-sm-1 checkbox">
                <label><input id="toggleTaskStatus" type="checkbox" onchange="taskManager.toggleTaskStatus(${index})" value="" class="" ${task.isComplete?'checked':''}></label>
              </div>
              <div class="col-md-6 col-xs-6 col-lg-6 col-sm-6 task-text ${task.isComplete?'complete':''}">
                ${task.description}
              </div>
              <div class="col-md-4 col-xs-4 col-lg-4 col-sm-4 task-date">
                ${task.dateCreated}
              </div>

              <div class="col-md-1 col-xs-1 col-lg-1 col-sm-1 delete-icon-area">
                <a class="" href="#0" onClick="taskManager.deleteTask(event, ${index})">
                <i id="deleteTask" data-id="${index}" class="fas fa-trash"></i></a>
              </div>
            </div>
          </li>
        `;
      }

      toggleTaskStatus(index) {
        LogMessage("toggleTaskStatus(index), index = " + index);
        this.tasks[index].isComplete = !this.tasks[index].isComplete;
        this.save();
        this.load();
      }

      deleteTask(event, index) {
        LogMessage("deleteTask(event, index), index = " + index + " event = " + event);
        //console.log(event);
        event.preventDefault();
        this.tasks.splice(index, 1);
        this.save();
        this.load();
      }
}

class Task {
    constructor(description, isComplete, dateCreated) {
        this.description = description;
        this.isComplete = isComplete;
        this.dateCreated = dateCreated;
        LogMessage("Task constructor.");
    }

    /* Members can als be accessed via setters, getters or via this.isComplete. */
    GetDateCreated() {
        return this.dateCreated;
    }
    
    GetDescription() {
        return this.description;
    }

    GetStatus() {
        return this.isComplete;
    }

    SetStatus(status) {
        this.isComplete = status;
    }
}

function LogMessage(message) {
    if(DEBUG) {
        var date = new Date();
        console.log(date.toLocaleTimeString() + ": " + message);    
    }
}

function DateFormatter(dateObj) {
    const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];
    let month = monthNames[dateObj.getMonth()];
    let day = String(dateObj.getDate()).padStart(2, '0');
    let year = dateObj.getFullYear();
    let hours = String(dateObj.getHours()).padStart(2, '0'); 
    let minutes = String(dateObj.getMinutes()).padStart(2, '0');
    let seconds = String(dateObj.getSeconds()).padStart(2, '0');
    let formatter = day  + "-" + month  + "-" + year + " " + hours + ":" + minutes + ":" + seconds;
    //let formatter = month  + '\n'+ day  + ',' + year;
    return formatter; 
}

/* References
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

Notes:
Json serialization and deserialization of new date() object gives rise to problems.  

*/