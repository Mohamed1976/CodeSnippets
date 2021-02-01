window.addEventListener("load", function() {
    const XMLHTTPReadyState_COMPLETE = 4;

    //XMLHttpRequest(), In its simplest form, a request to the server using the XMLHttpRequest object looks like this:
    // This script assumes a button on the HTML form and a div to show the results. A new
    // XMLHttpRequest object is created. The open method is called to specify the request type, URI,
    // and whether to make the call asynchronous.
    document.getElementById("SimpleGet").addEventListener("click", () => {
        console.log("click event");
        var xReq = new XMLHttpRequest();
        xReq.open("GET", "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=2020-02-15", false);
        xReq.send(null);
        const response = xReq.response; 
        console.log(response);
        document.querySelector("#response").innerText = response;
    });

    document.querySelector("#GetRequestWithTimeout").addEventListener("click", () => {
        console.log("GetRequestWithTimeout click");
        var xReq = new XMLHttpRequest();
        xReq.open("GET", "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=2020-02-15", true);
        
        // Timeout cannot be set on synchronous call!!!
        xReq.timeout = 2000;        
        xReq.ontimeout = function () {
            document.querySelector("#response2").innerText = "Request Timed out";
        }

        xReq.onreadystatechange = function (e) {
            if (xReq.readyState === XMLHTTPReadyState_COMPLETE) {
                if (xReq.status === 200) {
                    const response = xReq.response; 
                    console.log(response);
                    document.querySelector("#response2").innerText = response;                           
                    const NASAObj = JSON.parse(response);
                    console.log(NASAObj);

                    console.log("NASAObj.media_type: " + NASAObj.media_type);
                    if(NASAObj.media_type === "image") {
                        const divElement = document.getElementById("NASAResponse");

                        let header = document.createElement("h4");  
                        header.innerText = NASAObj.title; 
                        divElement.appendChild(header);

                        let img = document.createElement("img");
                        img.src = NASAObj.url; 
                        console.log("NASAObj.url: " + NASAObj.url);
                        divElement.appendChild(img);
                        
                        let description = document.createElement("p");
                        description.innerText = NASAObj.explanation;
                        divElement.appendChild(description);
                    }
                } 
                else {
                    const status = xReq.statusText; 
                    console.log(status);
                    document.querySelector("#response2").innerText = status;                           
                }
            }
        }

        xReq.send(null);
    });

    

});