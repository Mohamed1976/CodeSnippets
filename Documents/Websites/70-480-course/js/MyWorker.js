self.addEventListener('message', function(e) {
    console.log(`Message rcvd in worker: ${e.data}`);

    switch(e.data) {
        case "start":
            self.postMessage("Processed by worker: " + e.data);
            break;
        case "stop":
            self.postMessage("Closing worker.");
            self.close();
            break;
        default:
            self.postMessage("Unknown message rcvd: " + e.data);
            break;
    }    

}, false);