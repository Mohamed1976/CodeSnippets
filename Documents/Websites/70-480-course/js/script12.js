const XMLHTTPReadyState_COMPLETE = 4;

function GetDate(dateObj) {
    const monthNames = ["January", "February", "March", "April", "May", "June", 
        "July", "August", "September", "October", "November", "December"];
    //const  dateObj = new Date();
    const month = String(dateObj.getMonth() + 1).padStart(2, '0');//monthNames[dateObj.getMonth()];
    const day = String(dateObj.getDate()).padStart(2, '0');
    const year = dateObj.getFullYear();
    //yyyy-mm-dd
    return `${year}-${month}-${day}`;
    //let output = month  + '\n'+ day  + ',' + year;    
}

$("document").ready(function () {

    document.querySelector("#fetchDate").addEventListener("click", () =>{
        console.log("fetchDate click");
        const url = 'http://time.jsontest.com';
        
        //getJSON
        $.getJSON(url, function(data) {
        
            const text = `Date: ${data.date}<br>
                        Time: ${data.time}<br>
                        Unix time: ${data.milliseconds_since_epoch}`;            
            $(".mypanel").html(text);
        }); 

        //AJAX, https://www.tutorialsteacher.com/jquery/jquery-ajax-method
        $.ajax(url, 
        {
            dataType: 'json', // type of response data
            timeout: 500,     // timeout milliseconds
            success: function (data,status,xhr) {   // success callback function
                const text = `<strong>Date: ${data.date}<br> 
                    Time: ${data.time}<br>
                    Unix time: ${data.milliseconds_since_epoch}</strong>`;            
                console.log(text);
                $("#div10").html(text);
            },
            error: function (jqXhr, textStatus, errorMessage) { // error callback 
                const msg = `Error: ${errorMessage}`; 
                console.log(msg);                    
                $("#div10").html(msg);
            }
        });

        //https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
        fetch(url)
            .then(res => res.json())
            .then((out) => {            
                console.log('Fetch Output: ', out);
                const text = `<strong>Date: ${out.date}<br> 
                    Time: ${out.time}<br>
                    Unix time: ${out.milliseconds_since_epoch}</strong>`;            
                console.log(text);
                $("#div11").html(text);            
            }).catch(err => console.error(err));

    }); 

    var _getJSON = function(url) {
        return new Promise(function(resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open('get', url, true);
            xhr.responseType = 'json';
            xhr.onload = function() {
                var status = xhr.status;
                if (status == 200) {
                    resolve(xhr.response);
                } else {
                    reject(status);
                }
            };
            xhr.send();
        });
    };

    document.querySelector("#getIPAddress").addEventListener("click", () => {
        console.log("#getIPAddress");

        _getJSON('https://mathiasbynens.be/demo/ip').then(function(data) {
            console.log('Your public IP address is: ' + data.ip);
            $("#div12").html('Your public IP address is: ' + data.ip);
        }, function(status) {
            console.log('Something went wrong.');
        });        
    });  


    /* The displayed date format will differ from the actual value — the displayed date is formatted based 
    on the locale of the user's browser, but the parsed value is always formatted yyyy-mm-dd.
    
    Todo: How to format Date using "yyyy-mm-dd"
    https://stackoverflow.com/questions/12409299/how-to-get-current-formatted-date-dd-mm-yyyy-in-javascript-and-append-it-to-an-i
    https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/getMonth */
    
    const dateControl = document.querySelector('input[type="date"]');
    dateControl.value = GetDate(new Date());
    //console.log(dateControl.value); // prints "2017-06-01"
    //console.log(dateControl.valueAsNumber); // prints 1496275200000, a JavaScript timestamp (ms)    
    
    //$("form").submit(function (e) {
    $("#signupForm").submit(function (e) {
        e.preventDefault();
        console.log(`$("form").submit(function () {`);
    
        var fName = $("#firstName").val();
        var lName = $("#lastName").val();
        var qString = "Last Name=" + lName + "&First Name=" + fName;        
        console.log(`qString: ${qString}`);

        //Using the jQuery.serialize method
        //The advantage of using this method— beyond saving a lot of code—is that the query string is also encoded.
        var _qString = $(this).serialize();
        console.log(`_qString: ${_qString}`);

        // $.ajax({
        //     url: 'processSignUp.aspx',
        //     type: "POST",
        //     data: qString,
        //     success: function (r) {
        //     }
        // });
    });

    $("#FetchImage").click(function() {
        //dateControl.value === YYYY-MM-DD format
        //A DOMString representing a date in YYYY-MM-DD format, or empty
        const url = `https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=${dateControl.value}`;        
        //console.log(`#FetchImage click: ${dateControl.value}`);
        console.log(url);

        var xReq = new XMLHttpRequest();
        xReq.open("GET", url, true);
        
        xReq.timeout = 2000;        
        xReq.ontimeout = function () {
            document.querySelector("#JsonResponse").innerHTML = "Request Timed out";
        }

        xReq.onreadystatechange = function (e) {
            if (xReq.readyState === XMLHTTPReadyState_COMPLETE) {
                if (xReq.status === 200) {
                    const response = xReq.response; 
                    document.querySelector("#JsonResponse").innerHTML = response; 
                    const info = JSON.parse(response);
                    console.log(info);
                    if(info.media_type === "image") {
                        document.querySelector("#ImageDescription").innerHTML = info.explanation; 
                        document.querySelector("#ImageOfTheDay").src = info.url;
                    }
                }
            }
        }

        xReq.onerror = function () {
            console.log("** An error occurred during the transaction");
        };

        xReq.send(null);
    }); 
}); /* $("document").ready(function () { */

window.addEventListener("load", function() {

    const getImage = document.querySelector("#GetImage");
    const myImage = document.getElementById("resultImage");

    getImage.addEventListener("click", () => {
        console.log("#GetImage click");

        var xReq = new XMLHttpRequest();
        xReq.open("GET", "data/recount-until-i.jpg", true);
        xReq.responseType = 'blob';
        
        xReq.onreadystatechange = function (e) {
            console.log("xReq.onreadystatechange");     
            if (xReq.readyState === XMLHTTPReadyState_COMPLETE) {
                console.log("xReq.readyState");
                if (xReq.status === 200) {
                    console.log("xReq.status === 200");
                    const response = xReq.response;
                    myImage.src = URL.createObjectURL(response);
                }
            }
        } 

        xReq.send(null);
        //var blob = xReq.response;                
        //document.getElementById("resultImage").src = URL.createObjectURL(blob);
    });

    const postData = document.querySelector("#PostRequest");
    postData.addEventListener("click", () => {
        //https://stackoverflow.com/questions/48096657/can-i-write-to-file-using-xmlhttprequest
        //
        //Can I write to file using XMLHttpRequest or any other javascript method and how do I do that?
        //No you can not! JavaScript is client-side programming language, that means it can't edit files on other computer or server.
        //We need to implement server side processing   

        //console.log("#PostRequest click");
        // const xmlData = "<Person firstname='Rick' lastName='Delorme' hairColor='Brown' " +
        // "eyeColor='Brown' /> ";
        // console.log(xmlData);
        // var xReq = new XMLHttpRequest();
        // xReq.open("POST", "/ReceiveXMLData.aspx", false);
        // xReq.responseType
        // xReq.send(xmlData);    
    });

    const postBtn = document.querySelector("#PostData");
    postBtn.addEventListener("click", processPost);    


    const log = document.getElementById('log');
    const logTxt = document.getElementById('logTxt');

    //Use blur to validate input data 
    console.log(log);
    log.addEventListener('blur', pause);
    log.addEventListener('focus', play); 


    function pause() {
        console.log("pause()");
        logTxt.classList.add('paused');
        //document.body.classList.add('paused');
        logTxt.textContent = 'FOCUS LOST!';
      }
      
      function play() {
        console.log("play()");
        logTxt.classList.remove('paused');
        //document.body.classList.remove('paused');
        logTxt.textContent = 'This document has focus. Click outside the document to lose focus.';
      }

});

function processPost() {
    console.log("processPost");
}