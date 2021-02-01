"use strict"

function clickReporter(event) {
    console.log("function clickReporter(event)");
    console.log(event);    
}

function _drawBox(canvas, context) {
    context.fillStyle = "black";
    context.fillRect(20, 20, canvas.width - 50, canvas.height - 50);
}

window.addEventListener("load", function() {

    $("#userName").click(function(e){
        e.stopPropagation();
    })

    $(document).on("click", "#showDialogBtn", function(e) {
        console.log("#showDialogBtn is clicked.");
        console.log(e);
    });

    //https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/code
    //https://developer.mozilla.org/en-US/docs/Web/API/Document/keydown_event
    document.getElementById("usernameEx20").addEventListener("keydown", function(e) {
        console.log(`e.code: ${e.code}`);
        if(e.code === "KeyA") {
            e.preventDefault();
            return false;
        }
        else {
            return true;
        }
    });

    $("#addBoxOfItems").click(function(event){
        console.log(`#addBoxOfItems clicked.`);
        for(let i = 0; i < 10; i++) {
            $("#addOneItem").trigger("click");
            //$("#addOneItem").click();        
        }
    });

    $("#addOneItem").click(function(event){
        console.log(`#addOneItem clicked.`);
    });

    const mapContents = document.getElementById("mapContents");
    const usMap = document.getElementById("UsMap"); 

    usMap.addEventListener("mousemove", function(mouseEvent) {
        mapContents.innerHTML = "x: " + mouseEvent.x + "<br>y: " + mouseEvent.y + "<br>";
    });

    // * = if it contains the text
    // $ = if it ends in the text    
    $('input[name$="name"]').css({'background-color':'#E0ECF8'});

    //$ ("section article:first-child").css("background-color", "#f2f2f2");
    $ ("#Exercise48 article:first-of-type") .css("background-color", "#f2f2f2");

    const clickCanvas = document.getElementById("clickCanvas");
    clickCanvas.onclick = clickReporter;
    const clickCanvasContext = clickCanvas.getContext("2d"); 
    _drawBox(clickCanvas, clickCanvasContext);

    const AboutBtnE3 = document.getElementById("JavascriptFunctionsExercise3");
    AboutBtnE3.addEventListener("click", function() {
        console.log(this.type);
    });

    //console.log("load event.");
    const aboutBtn = document.getElementById("aboutBtn"); 
    aboutBtn.addEventListener("click", processAbout);
    //aboutBtn.attachEvent("onclick", processAboutV2);
        
    const form = document.querySelector("#form1");    
    form.addEventListener("submit", function(e) {
        e.preventDefault();
        const value = $("#emailAddressTxt1").val();
        console.log("processForm: " + value);
    });

    const form2 = document.querySelector("#form2");    
    form2.addEventListener("submit", function(e) {
        e.preventDefault();
        const value = $("#numberInput").val();
        console.log("processForm2: " + value);
    });

    const form3 = document.querySelector("#form3");    
    form3.addEventListener("submit", function(e) {
        e.preventDefault();
        const value = $("#threeLetterInput").val();
        console.log("processForm2: " + value);
    });

    const form4 = document.querySelector("#form4");    
    form4.addEventListener("submit", function(e) {
        e.preventDefault();
        const serialized = $("#form4").serialize();
        console.log(serialized);
        const uri_dec = decodeURIComponent(serialized);
        console.log(uri_dec);

        var formFieldArray = uri_dec.split("&");
        $.each(formFieldArray, function(i, pair){
            var nameValue = pair.split("=");
            var name = decodeURIComponent(nameValue[0]);
            var value = decodeURIComponent(nameValue[1]);
            console.log(`${name}, ${value}`);
        });
    });

    const form5 = document.querySelector("#form5");    
    form5.addEventListener("submit", function(e) {
        e.preventDefault();
        //console.log("form5 submit");
        const value = $("#secureUrl").val();
        console.log("processForm4: " + value);
    });

    const form6 = document.querySelector("#form6");    
    form6.addEventListener("submit", function(e) {
        
        const value_ = $("#form6 > input:first").val(); //$("#form6 input:first").val();
        console.log("value_: " + value_);
        const value = $("#inputOK").val();
        console.log(value);
        if(value === "OK") {
            console.log("SUCCESS");
            return true;
        }
        else {
            console.log("FAILURE");
            return false;
        }
    });


    //+ = One or more, \. = A period (special character: needs to be escaped by a \)
    const emailRegExPattern = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;  
    const emailTxt = document.getElementById("emailAddressTxt");
    emailTxt.addEventListener("blur", function()
    {
        const email = emailTxt.value; 

        if(!emailRegExPattern.test(email)) {
            emailTxt.style.border = "2px solid red";
        }
        else {
            emailTxt.style.border = "2px solid green";  
        }
    });

    //for example, 555555555 or 555-55-5555)
    const ssnRegExPattern = /^[0-9]{3}\-?[0-9]{2}\-?[0-9]{4}$/;  
    //const ssnRegExPattern = /^\d{3}[\-]?\d{2}[\-]?\d{4}$/;  
    const ssnTxt = document.getElementById("SSNTxt");
    ssnTxt.addEventListener("blur", function()
    {
        if(ssnRegExPattern.test(ssnTxt.value)) {
            ssnTxt.style.border = "2px green solid";
        }        
        else {
            ssnTxt.style.border = "2px red solid";    
        }
    });

    loadDestination();
});

$("document").ready(function () {
    //console.log("JQuery ready event.");

    const CanvasExercise1 = document.getElementById("CanvasExercise1");
    CanvasExercise1.style.backgroundColor = "lightgray";
    const CanvasExercise1Context =  CanvasExercise1.getContext("2d"); 
    CanvasExercise1Context.fillStyle = "rgb(255,0,0)"; 
    CanvasExercise1Context.fillRect(50,50,100,100);

    const showVersionBtn = document.querySelector("#showVersionBtn");    
    showVersionBtn.addEventListener("click", function(){
        console.log("Version 2.1.1.0.");
    })

    $("#UpdateTemperature").click(UpdateTemp);
        
    $("table td:nth-child(2) input").focus(
        function() {
            console.log("table td:nth-child(2) input focus");
            $(this).parent().next().css("background-color","#00ff00")
        }
    );

    $("table td:nth-child(2) input").blur(
        function() {
            console.log("table td:nth-child(2) input blur");
            $(this).parent().next().css("background-color","#ffffff")
        }
    );

    $("a > img").each(function(){
        $(this).parent().attr("target","_blank");
    }); 
    
    $(".HotArea1 > :header").css({ color: "red" });
    $(".HotArea1 > :header").addClass("underlineMe");
});

function validate() {
    console.log("validate()");
    const ageValue = $("#ageTxt").val();
    const ageRegExPattern = /^\d{1,3}$/;

    if(ageValue === "" || !ageRegExPattern.test(ageValue)) {
        console.log("Please enter a valid age.");
        $("#ageValidation").text("Please enter a valid age.");        
    }
    else {
        $("#ageValidation").text("");
    }
}

function DoWork() {
    try {
        console.log("DoWork");
        ThrowsError();
    }
    catch(exception) {
        console.log("Exception: " + exception.message +", "+ exception.ErrorCode);
        if(exception.ErrorCode === -2146823281) {
            console.log("Handle ErrorCode === -2146823281");
        }
    }
    finally {
        console.log("finally");
    }
}

function ThrowsError() {
    let err = new Error("Custom error description.");     
    err.ErrorCode = -2146823281;
    throw err; 
}

function CheckType() {
    const numberObj = new Number(12);
    console.log(GetType(numberObj));

    const stringObj = new String("A string value.");
    console.log(GetType(stringObj));    

    const errorObj = new Error("A error value.");
    console.log(GetType(errorObj));
}

function GetType(obj) {
    let type =  "Unknown"; 

    switch(obj.constructor) {
        case Number:
            type = "Object is of type Number."; 
            break;
        case String: 
            type = "Object is of type String."; 
            break;
        default:      
            type = "Object is of type Unknown."; 
            break;
    }

    return type; 
}

function CreateCustomer() {
    const customerObj = new Customer("John", "Deers"); 
    console.log(`${customerObj.firstName}, ${customerObj.lastName}`);
    customerObj.GetCommission();
}

function Customer(firstName, lastName) {
    this.firstName = firstName;
    this.lastName = lastName;
} 

Customer.prototype.GetCommission = function() {
    console.log(`Commission: ${this.firstName}, ${this.lastName}`);    
}

function CreateConsultant() {
    /*const emp = new Employee("Tom", "Tesla");
    console.log(`Consultant FullName: ${emp.FirstName}, ${emp.LastName}`);

    const emp2 = new Employee();
    console.log(`Consultant FullName: ${emp2.FirstName}, ${emp2.LastName}`);

    const emp3 = new Employee("John");
    console.log(`Consultant FullName: ${emp3.FirstName}, ${emp3.LastName}`);
    emp.PayEmployee();*/

    const consultant1 = new Consultant("Tom", "Woodward", 2100);
    consultant1.PayEmployee();

    const employee1 = new Employee("Mick", "Washington");
    employee1.PayEmployee();    
}

function Employee(firstName, lastName) {
    this.FirstName = firstName || "unknown";
    this.LastName = lastName || "unknown";            
}

Employee.prototype.PayEmployee = function (){
    console.log(`Pay Employee ${this.FirstName}, ${this.LastName}!`);
}

//You are creating a class named Consultant that must inherit from the Employee class. 
//The Consultant class must modify the inherited PayEmployee method
function Consultant(firstName, lastName, salary)
{
    Employee.call(this, firstName, lastName);
    this.Salary = salary || 0;
}

Consultant.prototype = new Employee();
Consultant.prototype.constructor = Consultant;

Consultant.prototype.PayEmployee = function (){
    console.log(`Pay Consultant ${this.FirstName}, ${this.LastName}, ${this.Salary}!`);
}

function updateTextInput(value) {
    console.log(value);
    $("#sliderVal").text(value);
}

function JsonExercise1() {
    const json= `
        {
            "Confirmation": "1234",
            "FirstName": "John"
        }`;
     
    const obj = JSON.parse(json);;
    console.log(obj.Confirmation);     
    $("#JsonExercise1").text(obj.Confirmation);
}

function UpdateTemp(event) {
    const thermometer = new Loader();
    thermometer.temp = Math.random(-20,45)*100; 
    thermometer.UpdateStatus("Done");
    console.log(thermometer.Display());
 
    $("#currentTemperature").text("Current temperature is: " + thermometer.temp); 
}

function Loader() {
    this.status = "ready";
    this.temp = 30;
    
    this.UpdateStatus = function (status) {
        this.status = status;
    }

    this.Display = function() {
        return `${this.status}, ${this.temp}`;
    }    
}

function resetStyle(sender) {
    sender.style.border = "2px green solid";
}

function GenerateErrorQ2() {
    try {
        ProcessCreditCard(123);
    }
    catch(ex) {
        const msg = `catch(ex): ${ex.message},  ${ex.number}`;
        console.log(msg);
        $("#ErrorhandlerQ2").text(msg);
    }
    finally {
        console.log(`finally`);
    }
}

function ProcessCreditCard(number) {
    let err = new Error("Invalid");
    err.number = 200;
    throw err; 
} 

function HtmlExercise7() {
    const value = $("#HtmlExercise7").val(); 
    const regExPattern = /^[a-zA-Z]{3}$/;

    if(regExPattern.test(value)) {
        console.log(`Validation Success: ${value}`);
    }
    else {
        console.log(`Validation Failed: ${value}`);
    }
}

function HtmlExercise8() {
    const value = $("#HtmlExercise8").val();

    if(value === null || value === "") {
        console.log(`Validation Failed: ${value}`);        
    }
    else {
        console.log(`Validation Success: ${value}`);
    }
}

function GetCountry() {
    const country = '{"country":' + "Us" + '}';
    console.log(country);

    var request = $.ajax({
        type: "GET",
        url: "./data/pies.json",
        contentype: "application/json; charset=utf-8",
        accept: "application/bint, text/xml, application/json",
        datatype: "json",
        data: country,
        success: function(response){
            console.log(request);
            //The response Content-Type, which can be used to process different types. 
            console.log("Content-Type: " + request.getResponseHeader("Content-Type"));
            console.log(response);
            console.log(`response: ${response.firstName}, ${response.lastName}`);
        },
        error: function(request, status, error) {
            console.log("MyError: " + error);
        }//,
        // dataFilter: function(date, type) {
        //     if(request.getResponseHeader("Content-Type") === "application/json") {
        //        processJson();
        //     }
        //     else {
        //        processAthorFormat();    
        //     }
        //     console.log("Enter Function: dataFilter: function(date, type)");
        //     console.log("Content-Type: " + request.getResponseHeader("Content-Type"));            
        //     console.log(date);
        //     console.log(type);
        // }
    });
}

function StartSimulation() {
    const progressBar = $("#progressFileCtrl");
    for(let i = 0; i <= 10; i++) {
        progressBar.val(i * 10);    
    }

    return
    // "/echo/json/" not allowed 
    // troublemaker is antivirus (bitdefender)
    // https://stackoverflow.com/questions/3352555/xhr-upload-progress-is-100-from-the-start
    $.ajax({
        xhr: function()
        {
          var xhr = new window.XMLHttpRequest();
          //Upload progress
          xhr.upload.addEventListener("progress", function(evt){
            if (evt.lengthComputable) {
              var percentComplete = evt.loaded / evt.total;
              console.log("Upload ", Math.round(percentComplete*100) + "% complete.");
            }
          }, false);
          return xhr;
        },
        type: 'POST',
        url: "/echo/json/",
        data: {json: JSON.stringify(new Array(200000))}
      });
}

function loadDestination() {
    console.log("loadDestination()");
    const country = localStorage.destination;
    console.log(`country: ${country}`);
    if(country !== null ) {
        document.getElementById("txtDestination").value = country;
    }      
}

function processDestination(target) {
    console.log("processDestination(target)");
    const country = document.getElementById(target).value;
    try {
        console.log(`country: ${country}`);
        localStorage.destination = country; 
    }
    catch(ex) {
        console.log(ex);
    } 
}

function ShowInputBox(target) {
    const divElement = document.createElement("div");
    divElement.innerHTML = "<input type='text'>";
    document.getElementById(target).appendChild(divElement);
}

function MoveLogo(target) {
    const divElement = document.getElementById(target);
    divElement.style.position = "relative";
    divElement.style.top = "5px";
}

var webWorker;
function startWorker() {

    if(typeof(Worker) === "undefined") {    
        document.getElementById("result").style.color = "red";
        document.getElementById("result").innerHTML = "Sorry, your browser does not support Web Workers..."; 
    }
    else {
        if(typeof(webWorker) === "undefined") {
            webWorker = new Worker("./js/demo_workers.js");
        }

        //Post message to worker
        webWorker.postMessage("Prefix: ");
        console.log('Message posted to worker');

        webWorker.onmessage = function(event) {
            document.getElementById("result").innerHTML = event.data;
        };        
    }
}

function stopWorker() { 
    webWorker.terminate();
    webWorker = undefined;
}

var webWorkerV2;
function startWorkerV2() {
    webWorkerV2 = new Worker("./js/MyWorker.js");

    webWorkerV2.onmessage = function(event) {
        console.log("Message rcvd from Worker.");
        console.log(event);
    };
    console.log("Worker started."); 
    webWorkerV2.postMessage("start");       
}

function stopWorkerV2() { 
    webWorkerV2.terminate();
    webWorkerV2 = undefined;
    console.log("Worker stoped.");        
}

function processAboutV2() {
    console.log("Process attachEvent, processAboutV2");
}

function processAbout() {
    console.log("Process AddEventListner, processAbout");
}

function ProcessDigits() {
    var length = "75";

    if(length == "75")
        console.log(`length == "75"`);

    if(length == 75)
        console.log(`length == 75`);        

}

function processInformation() {
    console.log("Enter process information.");    
    const cust = new CustomerV2("Mary","Sheen"); 
    cust.loadAddress();
    console.log(`${cust.firstName}, ${cust.lastName}, ${cust.address}`)
}

function CustomerV2(firstName, lastName) {
    this.firstName = firstName;
    this.lastName = lastName;
    this.address = "";
}

CustomerV2.prototype.parseAddress = function (data) {
    console.log("Enter parseAddress.");
    this.address = data;
}

CustomerV2.prototype.loadAddress = function () {
    var self = this;

    var request = $.ajax({
        type: "GET",
        url: "./data/pies.json",
        contentype: "application/json; charset=utf-8",
        accept: "application/bint, text/xml, application/json",
        datatype: "json",
        async: false,
        success: function(response){
            const msg = `response: ${response.firstName}, ${response.lastName}`;
            console.log(msg);
            self.parseAddress(msg)
        },
        error: function(request, status, error) {
            console.log("Error: " + error);
        }
    });
}

function ProcessAjaxCall() {
    var xhr  = new XMLHttpRequest();

    xhr.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
          document.getElementById("ProcessAjaxCallResult").innerHTML = this.responseText;
        }
    };

    xhr.open("GET", "./data/pies.json", true);
    xhr.send();    
}


var stockRate;
function ShowStockRate() {

    var request = $.ajax({
        type: "GET",
        url: "./data/pies.json",
        contentype: "application/json; charset=utf-8",
        accept: "application/bint, text/xml, application/json",
        datatype: "json",
        async: false,
        cache: false,
        success: function(response){
            stockRate = response; 
        },
        error: function(request, status, error) {
            console.log("Error: " + error);
        }        
    });
    
    ShowRate();
}

function ShowRate() {
    console.log(`ShowRate(): ${stockRate.firstName}, ${stockRate.lastName}`);
    console.log(stockRate);
}

function CreateCar() {
    const _car = new Car();        
    _car.fourDoor();
    _car.Greeting();

    const _sedan = new Sedan();        
    _sedan.fourDoor();
    _sedan.Greeting();
}


function Car() { }

Car.prototype.fourDoor = function() {
    console.log("Car four doors.");
}

Car.prototype.Greeting = function() {
    console.log("The cars says: hi there.");
}

function Sedan() {
    Car.call(this);    
}

Sedan.prototype = new Car();
Sedan.prototype.constructor = Sedan;

Sedan.prototype.fourDoor = function() {
    console.log("Sedan four doors.");
} 

function CheckValidCountries() {
    let arr = new Array();

    arr["US"] = true;
    arr["CA"] = true;
    arr["UK"] = false;

    const country = document.getElementById("CountryEx4").value;
    console.log(country);
    if(!arr[country]) {
        var txt = "Country is not valid.\n";
        txt += "Valid values are:";
        for(let i in arr) { //let i = 0; i < arr.length; i++) {
            console.log(`val ${i}`);
            if(arr[i])
                txt += i + " "; 
        }
        console.log(`${txt}`);
        document.getElementById("ValidCountries").innerText = txt; 
    }
}

function disable(sender) {
    console.log("function disable(sender)");
    sender.disabled = true;    
}

function MoveLower() {
    const pictureElement = document.getElementById("pictureEx26");

    pictureElement.style.position = "relative";
    pictureElement.style.top = "5px";
}

function Errorhandler3() {
    try {
        console.log("Entering Errorhandler3()");
        MySend();
    }
    catch(ex) {
        console.log(`Error: ${ex.errorCode}`);
    }
    finally {
        console.log(`finally`);
    }
}

function MySend() {
    throw new MyCustomError(300); 
}

function MyCustomError(code) {
    this.errorCode = code; 
}

MyCustomError.prototype = Error.prototype;

function ValidationException(number, message) {
    this.number = number;
    this.message = message;
    this.name = "MyLibrary Validation Exception.";
}

function LogicException(number, message) {
    this.number = number;
    this.message = message;
    this.name = "MyLibrary Logic Exception.";
}

function ScheduleDate(dayOfWeek) {
    if(dayOfWeek > 7)
        throw new ValidationException(1234, "Day of week must be less or equal than 7.");
}

function BookHotelRoom() {
    try {
        ScheduleDate(8);
    }
    catch(ex) {
        if(ex instanceof ValidationException) {
            console.log(`ValidationException: ${ex.number}, ${ex.message}, ${ex.name}`);
        }
        else if(ex instanceof LogicException) {
            console.log(`LogicException: ${ex.number}, ${ex.message}, ${ex.name}`);
        }
        else {
            console.log(`Unknown exception.`);
        }
    }
    finally {
        console.log(`finally`);
    }
}

function ProcessLanguages() {
    const listItems = document.querySelectorAll("#languages li")
    let languages = new Array(); 

    for(let i = 0; i < listItems.length; i++) {
        languages.push(listItems[i].innerHTML);
    }

    languages.sort();

    for(let i = 0; i < listItems.length; i++) {
        listItems[i].innerHTML = languages[i]; 
    }

    //languages.push(listItems[i].innerHTML);
    // for(let i = 0; i < languages.length; i++) {
    //     console.log(languages[i]);
    // }
    // console.log("\n\n");
    // for(let i = 0; i < languages.length; i++) {
    //     console.log(languages[i]);
    // }
    //console.log(listItems[i].innerHTML);
    // let languages = $.makeArray(listItems);
    // for(let i = 0; i < languages.length; i++) {
    //     console.log(languages[i].innerHTML);
    // }
    // languages.sort(compare) //(function(s) { return s.innerHTML })
    // console.log("Sorted:\n\n")
    // for(let i = 0; i < languages.length; i++) {
    //     console.log(languages[i].innerHTML);
    // }
    // for(let i = 0; i < listItems.length; i++) {
    //     listItems[i].innerHTML = languages[i].innerHTML;  
    //     //console.log(listItems[i].innerHTML);
    // }
}

//https://stackoverflow.com/questions/1129216/sort-array-of-objects-by-string-property-value
function compare( a, b ) {
    if ( a.innerHTML < b.innerHTML ){
      return -1;
    }
    if ( a.innerHTML > b.innerHTML ){
      return 1;
    }
    return 0;
}

//https://www.w3schools.com/js/js_json_objects.asp
function AppendFruitData() {
    const jsonFruit = { "apples" : "12", "bananas" : "8", "watermelon" : "3" };

    $.each(jsonFruit, function(key, value) {
        console.log(`${key}, ${value}`);
        const newRow = "<tr><td>" + key + "</td><td>" + value + "</td></tr>";        
        $(newRow).appendTo("#fruitTable");
    });
}

function ProcessImage3() {
    //const image = { "Description" : "Rose garden", "FileName" : "rose-garden.jpg" };
    //const imageObj = JSON.parse(image);
    const image = { Description: "Rose garden", FileName: "./Resources/img/rose-garden.jpg" };
    console.log(`${image.Description}, ${image.FileName}`);
    var personImage = new PersonImage(image); 
    //https://stackoverflow.com/questions/941206/jquery-add-image-inside-of-div-tag
    $('#happyFace').prepend(personImage.img)
}

function PersonImage(image) {
    var img = document.createElement("img");
    img.alt = image.Description; 
    img.src = image.FileName;
    img.width = "100";
    img.height = "100";
    this.img = img;
}

function SendMessage() {
    const recipient = $("#recipient").val();
    const message = $("#body").val();
    const attachment = $("#Attachment").val();
    console.log(`${recipient}, ${message}, ${attachment}`);
    send(recipient, { fileName:attachment, message:message })
}

function send(to, args) {
    if(args.fileName !== undefined) {
        sendFile(to, args.message, args.fileName);
    }
    else {
        sendMessage(to, args.message);
    }
}

function sendFile(to, message, fileName) {
    console.log(`sendFile: ${to}, ${message}, ${fileName}`);
} 

function sendMessage(to, message) {
    console.log(`sendMessage: ${to}, ${message}`);
} 

function GetPersonData() {
    const response = {
        "data" : {
           "GivenName" : "Guardian knight",
           "Surname" : "Robin Hood"
        }
     }

     const jsonContent = JSON.stringify(response);
     console.log(jsonContent);

     const obj = JSON.parse(jsonContent); 
     const givenName = obj.data.GivenName;
     const surname = obj.data.Surname;
     console.log(`givenName: ${givenName}, surname: ${surname}`);
     $("#givenName").text(givenName);
     $("#surname").text(surname);       
}

function DisplayLoanAmounts() {
    showLoanAmounts();
}

function showLoanAmounts() {

    var loanAmount = 400;
    function showSomeLoanAmount() {
        
        var loanAmount = 800;
        function showAnotherLoanAmount() {
            var loanAmount = 1000; 
            console.log(`showAnotherLoanAmount: ${loanAmount}`);
        }

        showAnotherLoanAmount();
        console.log(`showSomeLoanAmount: ${loanAmount}`);
    }

    showSomeLoanAmount();
    console.log(`showLoanAmounts: ${loanAmount}`);
}

function getText() {

    var req = new XMLHttpRequest();

    req.onreadystatechange = function () {
        console.log(`readyState: ${req.readyState}, status: ${req.status}, responseText: ${req.responseText}`);
    }

    req.open("GET", "./data/pies.json", true);
    req.send();
}

function ApplystyleToPElement() {
    const element = document.getElementById("testExercise33");

    element.style.padding = "15px";
    //element.style.top = "5px";
}

function ProcessLoan() {
    const calculator = new myApplication.LoanCalculator("paymentAmount", 1000, 5, 2400); 
    console.log(`${calculator.Principle}, ${calculator.Term}, ${calculator.Rate}`);
    calculator.CalculatePayment();
    calculator.showCanWeAfford();
}

var myApplication = myApplication || {}; 

myApplication.LoanCalculator = function(displayControl, principle, term, rate) {
    this.Principle = principle;
    this.Term = term;
    this.Rate = rate / 1200;
    this.PaymentAmount = 0;
    this.showPayment = document.getElementById(displayControl);
}

myApplication.LoanCalculator.prototype = {
    CalculatePayment: function() {
        this.PaymentAmount = this.Principle * this.Rate * this.Term;
        this.showPayment.innerHTML = "$" + this.PaymentAmount;        
    },

    showCanWeAfford: function() {
        if(this.PaymentAmount > 500) {
            console.log("Denied!");
        }
        else if(this.PaymentAmount < 300) {
            console.log("Approved!");
        }
        else {
            console.log("Approved with caution!");
        }
    }
}

function checkTxt(sender) {

    if(sender.value === null || sender.value === "") {
        sender.style.backgroundColor = "#fffacd";    
    }
    else {
        sender.style.backgroundColor = "#ffffff";    
    }
}

function ResetTxt() {
    const inputElements = document.querySelectorAll(".BackgroundInputTxt");

    for(let i = 0; i < inputElements.length; i++) {
        console.log(inputElements[i]);
        inputElements[i].style.backgroundColor = "pink";
    }

}

function processXmlfile() {
    let xmlhttp;     
    
    if (window.XMLHttpRequest) {
        // code for modern browsers
        xmlhttp = new XMLHttpRequest();
     } else {
        // code for old IE browsers
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.open("GET", "./data/message.xml", false);
    xmlhttp.send();

    const xmlDoc = xmlhttp.responseXML;
    const txtDoc = xmlhttp.responseText; 
    console.log(xmlDoc);
    console.log(txtDoc);

    const toVal = xmlDoc.getElementsByTagName("to")[0].childNodes[0].nodeValue;
    const fromVal = xmlDoc.getElementsByTagName("from")[0].childNodes[0].nodeValue;
    const messageVal = xmlDoc.getElementsByTagName("message")[0].childNodes[0].nodeValue;
    console.log(toVal);
    console.log(fromVal);
    console.log(messageVal);
    
    $("#toDiv").text(toVal);
    $("#fromDiv").text(fromVal);
    $("#messageDiv").text(messageVal);
}

function MathDivide() {
    onerror = unhandled;

    if(isNaN($("#inputIntVal1").val()) || isNaN($("#inputIntVal2").val())) {
        console.log("One or more values are non-numeric.");
        throw("One or more values are non-numeric.");
    }
    else {
        const dividedValue = $("#inputIntVal1").val() / $("#inputIntVal2").val(); 
        console.log(`dividedValue: ${dividedValue}`);
    }

    function unhandled(msg, url, line) {
        console.log("function unhandled(msg, url, line) {");
        console.log("There has been an unhandled exception.");
        console.log(msg);
        console.log(url);
        console.log(line);
    } 
}

function MathAdd() {    
    console.log(`${$("#inputIntVal1").val()}, ${$("#inputIntVal2").val()}`);       
    
    console.log(`val1 + val2 = ${$("#inputIntVal1").val() + $("#inputIntVal2").val()} `);
    console.log(`parseInt(val1) + parseInt(val2) = ${parseInt($("#inputIntVal1").val()) + parseInt($("#inputIntVal2").val())} `);
}

function AgeChanged(sender) {
    console.log(sender.value);
    document.getElementById("ageRangeLbl").innerText = sender.value;
}

function CheckOnlineStatus() {
    const statusElement = document.getElementById("OnlineStatusDiv"); 

    if(navigator.onLine) {
        statusElement.innerText = "onLine";
    }
    else {
        statusElement.innerText = "OffLine";
    }    
}

//https://www.w3schools.com/jquery/jquery_hide_show.asp
//https://www.tutorialrepublic.com/jquery-tutorial/jquery-show-and-hide-effects.php
function ShowParagraphs() {
    $(".dynamicP").show();
}

function HideParagraphs() {
    $(".dynamicP").hide();
}

function ProcessCustomerObj() {

    var Customer = (function() {
        var name = "Contoso";
        return {
            getName: function() {
                return name; 
            }, 
            setName(newName) {
                name = newName;     
            }                
        }
    })();

    console.log(Customer.getName());
    console.log(Customer.name); //Undefined
    Customer.setName("Donald Duck");
    console.log(Customer.getName());
}

function madeSelection(element, message) {
    console.log("function madeSelection(element, message) {");
    // console.log(element.value);
    // console.log(element);
    // console.log(message);

    if(element.value === "Please Choose") {
        console.log('element.value === "Please Choose"');
        element.focus();
        return false;
    }
    else {
        return true;
    }
}

function GetFormatter() {

    console.log("function GetFormatter() {");
    const _formatter = new Formatter();
    console.log(_formatter._public("Alfa"));

    console.log("John Connor: " + _formatter.parseValue("John Connor"));

    function Formatter() {
        var _private = function(data) {
            return LengthCalculator(data); //data.length; //this.CustomProcessor(data); 
        }

        this._public = function(data) {
            return this.CustomProcessor(data); 
        }

        this.CustomProcessor = function(data) {
            return data.length;        
        }

        this.parseValue = function(input) {
            return _private(input); 
        }
    } 
}

function LengthCalculator(str) {
    return str.length; 
}

function ValidateInputData(sender) {
    const regExPattern = /^[a-zA-Z0-9_]+$/;

    if(!regExPattern.test(sender.value)) {
        sender.value = "Invalid input";
        sender.focus(); 
    }      
}

var DeltaAngle;
var _rectangle;  
function StartAnimation(rectangle) {
    
    _rectangle = rectangle;
    DeltaAngle = 10;
    var myVar = setInterval(myTimer, 1000);
}

function myTimer() {
    _rectangle.setAttribute("transform", "rotate(" + DeltaAngle + ")");  
    DeltaAngle += 10;
    // var d = new Date();
    // var t = d.toLocaleTimeString();
    // document.getElementById("demo").innerHTML = t;
}

var MyWorker; 
function StartWorkerEx2() {
    if(typeof(Worker) === "undefined") {
        console.log("Sorry, your browser does not support Web Workers...");
    }
    else {
        if(typeof(webWorker) === "undefined") {
            MyWorker = new Worker("./js/task.js");
        }

        MyWorker.postMessage({ message: "Go", iterations: 1000000 });

        MyWorker.onmessage = function(event) {
            console.log("StartWorkerEx2() MyWorker.onmessage = function(event) {");
            console.log(event);
        };                
    }    
}

function ProcessSelection() {

    let selectedValues = "";
    let _selectedValues = "";

    $("#loanTypes option:selected").each(function() {
        console.log(this);
        selectedValues += this.text + ", ";
    });
    console.log("selectedValues: " + selectedValues);

    $("#section38 select option:selected").each(function() {
        _selectedValues += this.text + ", ";
    });

    console.log("_selectedValues: " + _selectedValues);
}

function JavascriptFunctionsExercise12() {
    var myAppV2 = {};

    (function() {

        this.loanAmount = 100;

        this.display = function(value) {
            document.getElementById("displayEx12").innerHTML += value; 
        } 

        this.increaseLoanAmount = function() {
            this.loanAmount += 1000;
            return;
        }

        this.increaseLoanAmountAgain = function() {
            this.loanAmount += 1000;
            return;
        }
    }).apply(myAppV2);

    myAppV2.increaseLoanAmount();
    myAppV2.increaseLoanAmountAgain();
    myAppV2.display(myAppV2.loanAmount);
}

function GetStockData() {
    const _stock = new Stock("AZPN");
    _stock.loadStock();
    console.log(`${_stock.symbol}, ${_stock.low}, ${_stock.high}`);
}

function Stock(symbol) {
    this.symbol = symbol;
    this.low = 0;
    this.high = 0;
}

Stock.prototype.parseStock = function(data) {
    console.log(data);
    this.low = Math.random()*10;
    this.high = Math.random()*10;
}

//https://stackoverflow.com/questions/133310/how-can-i-get-jquery-to-perform-a-synchronous-rather-than-asynchronous-ajax-re
Stock.prototype.loadStock = function() {
    var self = this;

    $.get({
        url: "./data/message.xml",// mandatory
        success: function(data) {
            self.parseStock(data); 
        },
        async:false // to make it synchronous
      });

    // $.get("./data/message.xml", function(data){
    //     self.parseStock(data);
    // });
}

function PerformCalculationV2() {
    addValue();
}

function mathAddV2(v1, v2) {
    for (var i=0; i < arguments.length; i++) {
        console.log(`arg[${i}]: ${arguments[i]}`);
    }

    return v1 + v2 + this.v3 + this.v4;   
}

function addValue() {
    var o = { v3: 10, v4: 13 };
    const result = mathAddV2.call(o, 15, 3);
    //If you call method without .call, you need to access to object in the method that is called.
    //const result = mathAddV2(15, 3, o);
    console.log(`result: ${result}`); 
}

/*
Creating custom events
DOM events provide a great deal of functionality. In some cases, you might want to create
a custom event to use more generically. To create a custom event, you use the CustomEvent
object. To use custom events, you first need to create one by using the window.CustomEvent object:

The CustomEvent object constructor accepts two parameters:
■■ The first parameter is the name of the event. This is anything that makes sense for what
the event is supposed to represent. In this example, the event is called anAction.
■■ The second parameter is a dynamic object that contains a detail property that can
have properties assigned to it containing information that should be passed to the
event handler. Also, the parameter provides the ability to specify if the event should
bubble and whether the event can be canceled.
*/
function CreateCustomEvent() {

    var ordersReceivedEvent = new CustomEvent(
        "ordersReceived", {
        detail: {
            //The event must pass a custom value named orderCount.
            orderCount: 5
        },
        bubbles: false,
        cancelable: true
    });    

    var orderlisting = document.getElementById("orderlisting");
    console.log(orderlisting);
    orderlisting.addEventListener("ordersReceived", showOrdersReceivedCount);
    orderlisting.dispatchEvent(ordersReceivedEvent);
}

function showOrdersReceivedCount(event) {
    console.log(event);
    console.log(event.detail.orderCount);
    console.log(window.event.detail.orderCount);
}

function zoomIn() {
    // const new_scalex = 1.5;
    // const new_scaley  = 1.5;
    let _circle = document.getElementById("myCircle");
    // console.log("_circle.r: " + _circle.r);
    // console.log(_circle);
    // _circle.setAttribute("transform", "scale(" + new_scalex + "," + new_scaley + ")");
    _circle.r.baseVal.value = _circle.r.baseVal.value * 1.5; 
}

function CalculateArea() {
    const _square = new square(12);
    console.log(`${_square.side}, ${_square.area()}`);
}

function square(side) {
    this.side = side; 
    this.area = calcArea; //A reference to the method 

    function calcArea() {
        return this.side * this.side;     
    } 
}

//HTML <input> disabled Attribute
//https://www.w3schools.com/tags/att_input_disabled.asp
//https://www.w3schools.com/cssref/sel_disabled.asp
//https://www.w3schools.com/tags/att_disabled.asp
function enable(sender, target) {

    if(sender.checked) {
        console.log("sender.checked");
        target.disabled = false;
    }
    else {
        console.log("NOT sender.checked");
        target.disabled = true;
    }

    console.log(target);

}

//https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest/response
//https://gist.github.com/santisbon/a7c221780b528bd3ebb8
function GetImage() {

    var xhr  = new XMLHttpRequest();

    console.log("function GetImage() {");    

    xhr.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            console.log("xhr.onreadystatechange = function() {");            
          //document.getElementById("ProcessAjaxCallResult").innerHTML = this.responseText;
        }
    };

    xhr.onload = function(e) {
        console.log("xhr.onload = function() {");
        console.log(`status: ${this.status}, readyState: ${this.readyState}`);

        var img = document.createElement("img");
        img.src = window.URL.createObjectURL(this.response); 
        document.getElementById("imageKeeper").appendChild(img);        
    };

    xhr.open("GET", "./data/recount-until-i.jpg", true);
    xhr.responseType = "blob";
    xhr.send();
}

function GetLoanTerm() {

    var loanTermInput = document.getElementById("loanTermTextBox");

    var userInput = loanTermInput.value;
    console.log(`isNaN(loanTermInput.value): ${isNaN(userInput)}, ${userInput}`);
    var loanTerm = parseInt(loanTermInput.value, 10);
    console.log(`loanTerm: ${loanTerm}`);

    if(isNaN(loanTerm) || loanTerm.toString() !== loanTermInput.value) {
        console.log("if(isNaN(loanTerm))");
    }

    var validateForm = function(value) {
        console.log(`validateForm value: ${value}`);    
    }

    validateForm(12);
}

function ProcessErrorEx5() {
    
    console.log("function ProcessErrorEx5() {");
    window.onerror = function(ex) {
        console.log("window.onerror = function() {" + ex.message);    
    }
    
    //throw new Error("Process error throws.");
}

//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/isPrototypeOf
function CheckPrototype() {
    var _employee = new Employee("Josh", "Beers");
    var _consultant = new Consultant("John", "Deer", 299);
    //const isParent = _employee.isPrototypeOf(_consultant);
    console.log(`isParent: ${_employee.isPrototypeOf(_consultant)}`);
}

function ChangeContentEx52() {
    const headerElement = document.getElementById("headerEx52");
    headerElement.innerHTML = "<strong>A dynamic content added.</strong>";
}

function ProcessRecords() {
    
    let counter = 0;

    process(true);
    counter++;

    while(process()) {
        counter++;
        if(!(counter % 25)) {
            console.log(`(counter % 25): ${counter}`);
        }
    }
}

var processCounter; 
function process(boolFirstTime = false) {
    const maxCounter = 200;

    if(boolFirstTime) {
        processCounter = 0;
    }

    processCounter++;

    if(processCounter < maxCounter) {
        return true; 
    } 
    else {
        return false;
    }
}

var myApp = myApp || {};

myApp.Vehicle = function(displayControl, color, seatCount, transType, wheelCount) {
    this.Color = color;
    this.numberOfSeats = seatCount;
    this.TransmissionType = transType;
    this.NumberOfWheels = wheelCount;
    this.showVehicle = document.getElementById(displayControl);
} 

myApp.Vehicle.prototype = {
    Describe: function () {
        var valueToDisplay = "";

        for(let property in this) {
            console.log(`this[property]: ${this[property]}, typeof(this[property]): ${typeof(this[property])}`);
        
            if(typeof(this[property]) === "string") {
                valueToDisplay += "Vehicle " + property + " is " + this[property] + "<br>";
            }
            else if(typeof(this[property]) === "number") {
                if(this[property] < 4) {
                    valueToDisplay += "Stay safe!" + "<br>";
                }
                else if(this[property] >= 10) {
                    valueToDisplay += "Big Machine!" + "<br>";;                        
                }
            }
        }

        this.showVehicle.innerHTML = valueToDisplay;
    }
}

function ProcessVehicle() {
    // const car = new myApp.Vehicle("displayVehicle", "Silver", 2, "Manual", 16);
    // car.Describe();

    const car = new myApp.Vehicle("displayVehicle", "Silver", 3, "Manual", 10);
    car.Describe();    
}

function CheckPercentage() {
    const percentageComplete = 100; 

    if(percentageComplete === 100)
        console.log("percentageComplete === 100");
}


function somevent() {
    console.log("function somevent() is called.");
}


function stopPropagation(event) {
    //console.log("function stopPropagation(event)");
    //console.log(event);

    event = event || window.event;
    if(event.stopPropagation) {
        event.stopPropagation();
        console.log("event.stopPropagation();");
    }
    else {
        event.cancelBubble =true;
        console.log("event.cancelBubble = true;");
    }
}

function someOtherEvent(sender, event) {
    console.log(sender);
    console.log(event);

    var target = event.srcElement || event.target;  

    if(sender === target)
        console.log("if(sender === target)");
}

function CalculateValue() {
    var counter = 1776;

    var fun = function() {
        counter = 2020;
    }

    //fun();
    return counter;
}

function testLength(target, minLength, maxLength) {
    console.log("function testLength(target, minLength, maxLength)");
    const targetLength = target.value.length;      
    console.log(target);
    console.log(targetLength);

    if(targetLength <  minLength || targetLength > maxLength) {
        console.error(`Please enter between ${minLength} and ${maxLength} characters.`);
        target.focus();
        return false;
    }
    else {
        return true;
    }    
}

function FindTheaters() {
    navigator.geolocation.getCurrentPosition(hasPosition, noposition)
}

function hasPosition(position) {
    console.log(`latitude: ${position.coords.latitude}, longitude: ${position.coords.longitude}`);
}

function noposition() {
    console.log("function noposition() is called.");
}

function displayJsonData() {
    console.log("function displayJsonData() {");
    var request = $.ajax({
        type: "GET",
        url: "./data/pies.json",
        success: function(response){
            console.log(request);
            console.log("Content-Type: " + request.getResponseHeader("Content-Type"));
            console.log(response);
            console.log(`response: ${response.firstName}, ${response.lastName}`);
        },
        error: function(request, status, error) {
            console.log("Ajax error: " + error);
        }
    });

    request.done = function(data) {
        console.log("request.done");
        console.log(data);
    } 

}

function myValidation1() {
    console.log("function myValidation1() {");    
}

function myValidation2() {
    console.log("function myValidation2() {");
}