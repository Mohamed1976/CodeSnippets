"use strict"

window.taskNamespace = (function() {
    const taskNamespace = {};

    /* Task class in the namespace */
    taskNamespace.Task = (function() {
        /* Contstructor */
        const Task = function(description, dateCreated) {
            this.description = description;
            this.dateCreated = dateCreated;
            this.isComplete = false;
            this.id = helpersNamespace.createGuid();
        }

        const prototype = Task.prototype;

        /* Public methods for the task class, add to the prototype */
        prototype.setComplete = function(isComplete) {
            this.isComplete = isComplete; 
        }        

        return Task;
    })();

    /* TaskManager class in the namespace */
    taskNamespace.TaskManager = (function() {
        
        /* Contstructor */
        const TaskManager = function(key) {
            this.key = key;
            this.tasks = new Array();
        }

        const prototype = TaskManager.prototype;

        /* Public methods for the task class, add to the prototype */
        prototype.add = function(task) {
            this.tasks.push(task);
        }        

        prototype.find = function(id) {
            const taskFound = this.tasks.find(task => task.id === id);
            return taskFound;
        }

        prototype.remove = function(id) {

            let removed = false;
            /* filter removes items when arrow function returns false.  */             
            this.tasks = this.tasks.filter((task) => 
            {
                if(task.id === id) {
                    removed = true;
                    return false; 
                }
                else {
                    return true;
                }
            });  

            return removed;         
        }        

        prototype.save = function() {
            const Json = JSON.stringify(this.tasks);
            localStorage.setItem(this.key, Json);    
        }        

        prototype.load = function() {
            const Json = localStorage.getItem(this.key);
            this.tasks.length = 0; //Clear array
            
            if(Json !== null) {
                const items = JSON.parse(Json);
                /* Deserialize objects to Task objects. */
                for (const item of items) {
                    const task = helpersNamespace.toType(item, taskNamespace.Task.prototype);    
                    this.tasks.push(task);
                }  
            }
        }        
        
        prototype.clear = function() {
            localStorage.clear();    
        }

        return TaskManager;
    })();

    return taskNamespace;
})();

/* Namespace with helper functions.  */
window.helpersNamespace = (function() {
    const helpersNamespace = {};

    /* Private method used for deserialization, so that deserialized task (task instanceof taskNamespace.Task) === true. */
    helpersNamespace.toType = function (obj, proto) {
        obj.__proto__ = proto;
        return obj;
    }

    helpersNamespace.createGuid = function() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    helpersNamespace.dateFormatter = function(date) {
        const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
        const month = monthNames[date.getMonth()];
        const day = String(date.getDate()).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getHours()).padStart(2, '0'); 
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        const formatter = day  + "-" + month  + "-" + year + " " + hours + ":" + minutes + ":" + seconds;
        return formatter; 
    }

    return helpersNamespace;
})();    