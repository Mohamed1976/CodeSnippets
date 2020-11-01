onmessage = function (evt) {    
    console.log("Entering CalculateWorker.js: " + (new Date()).toLocaleTimeString());
    //The function receives an event object with a property called data that contains 
    //whatever was passed into the worker.
    var value = evt.data;
    console.log("Value received in worker: " + value);

    var work = value; //50000000;
    var i = 0;
    var a = new Array(work);
    var sum = 0;
    var millionCntr = 0; 
    
    for (i = 0; i < work; i++) {
        a[i] = i * i;
        sum += i * i;
        millionCntr++;
        if(millionCntr > 1000000)
        {
            console.log("Another million: " + (new Date()).toLocaleTimeString());
            millionCntr = 0;
        }
    }

    console.log("Finished CalculateWorker.js: " + (new Date()).toLocaleTimeString());
    self.postMessage(sum);
}