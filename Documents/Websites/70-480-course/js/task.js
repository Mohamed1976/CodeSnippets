//https://stackoverflow.com/questions/40887635/access-localstorage-from-service-worker?rq=1
//You cannot access localStorage (and also sessionStorage) from a webworker process, 
//they result will be undefined, this is for security reasons.
//Example using local storage in worker
// const storage = localStorage.destination;
// if(storage === null) {
//     console.log("storage === null");
//     localStorage.destination = "Message from Worker: Date: " + new Date();
// }
// console.log("localStorage.destination: " + localStorage.destination);

onmessage = function (evt) {
    var sum = 0;
    var value = evt.data;
    console.log("Value received in worker: " + value);
    console.log(evt.data.message + ", " + evt.data.iterations);

    if(evt.data.message === "Go") {
        for(let i = 0; i < evt.data.iterations; i++) {
            sum +=i;
        }
    }

    self.postMessage("After processing: " + evt.data.message + ", " + evt.data.iterations  + ", " + sum);
}