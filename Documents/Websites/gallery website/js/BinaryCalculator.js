//DOM
const inputBinary = document.getElementById("inputBinary");
const inputBinaryLbl = document.getElementById("inputBinaryLbl");
const inputDecimal = document.getElementById("inputDecimal");
//const inputBinaryLbl = document.querySelector('#inputBinaryLbl');

//addEventListener
inputBinary.addEventListener("keypress", function(e) {
    console.log(e);
    if(e.keyCode === 49 || e.keyCode === 48) {
        console.log("e.keyCode === 49 || e.keyCode === 48, " + inputBinary.value +", "+ e.target.value);
        return true;
    }
    else {
        e.preventDefault();
        return false;
    }
});

inputBinary.addEventListener('change', (event) => {
    console.log(`You like change ${event.target.value} ${inputBinary.value}`);
    //const result = document.querySelector('.result');
    //result.textContent = `You like ${event.target.value}`;
});

inputBinary.addEventListener('keyup', (event) => {
    inputBinaryLbl.innerHTML = event.target.value;
    //console.log(`You like keyup ${event.target.value}, ${inputBinary.value}`);
    //const result = document.querySelector('.result');
    //result.textContent = `You like ${event.target.value}`;
});

document.getElementById("ConvertBtn").addEventListener("click", () => {
    //console.log("ConvertBtn, click: " + inputBinary.value);
    let resultConvert = 0;
    let biner = inputBinary.value;
    let splitBiner = (biner.split('')).reverse();

    for(let i = splitBiner.length - 1; i >= 0; i--) {
        resultConvert += (parseInt(splitBiner[i]) * (Math.pow(2, i)));
    }    

    inputDecimalLbl.innerHTML = resultConvert.toString(10);
    inputDecimal.value = resultConvert.toString(10);
    console.log(`resultConvert: ${resultConvert}`);
});

document.getElementById("ResetBtn").addEventListener("click", () => {
    console.log("ResetBtn, click");    
    inputBinaryLbl.innerHTML = ""; 
    inputDecimalLbl.innerHTML = "";

    inputBinary.value = "0"; 
    inputDecimal.value = "0";
});


