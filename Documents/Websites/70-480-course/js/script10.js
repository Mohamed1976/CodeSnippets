
function CheckString() {
    
    try {

        console.log("Entering CheckString");
        //const s = $('#regExString').val(); //JQuery syntax
        //const s = document.getElementById("regExString").value;
        const s = document.querySelector("#regExString").value;

        console.log(s);
        const regExpression = /^[A-Z,a-z]\d[A-Z,a-z][\s{1}]?\d[A-Z,a-z]\d$/;
        let msg = "";

        if (regExpression.test(s))
            msg ="Valid postal code.";
        else
            msg ="Invalid postal code.";

        const divElement = document.querySelector("#formatStatus");
        divElement.innerHTML = `<strong>${msg}</strong>`;
        console.log("Exiting CheckString");            
    }
    catch (e) {
        console.log(e.message);
    }    
}

function CheckStringExec() {

    try {    
        console.log("Entering CheckStringExec");
        //const s = $('#regExString').val(); //JQuery syntax
        //const s = document.getElementById("regExString").value;
        const s = document.querySelector("#regExStringV2").value;

        console.log(s);
        const regExpression = /^[A-Z,a-z]\d[A-Z,a-z][\s{1}]?\d[A-Z,a-z]\d$/;
        let msg = "";

        var results = regExpression.exec(s);

        console.log(results);
        if (results !== null)
            msg ="Valid postal code. " + results[0] +", length: " + results.length;
        else
            msg ="Invalid postal code.";

        const divElement = document.querySelector("#formatStatusV2");
        divElement.innerHTML = `<strong>${msg}</strong>`;
        console.log("Exiting CheckStringExec");            
    }
    catch (e) {
        console.log(e.message);
    }    
}


function CheckDate() {
    const value = document.querySelector("#birthYear").value;

    if (!isNaN(value)) {
        console.log("Is a number: " + value);
    }
    else {
        console.log("Is NOT a number: " + value);
    }    
}