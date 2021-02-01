const form = document.getElementById('form');
const username = document.getElementById('username');
const email = document.getElementById('email');
const password = document.getElementById('password');
const password2 = document.getElementById('password2');

//* Event listeners
form.addEventListener('submit', function(e) {
    e.preventDefault();

    checkRequired([username, email, password, password2]);
    checkLength(username, 3, 15);
    checkLength(password, 6, 15);
    checkEmail(email);
    checkPasswordMatch(password, password2);
    // if(username.value === ''){
    //     showError(username, 'Username is required');
    // }else {
    //     showSuccess(username);
    // }
    // if(email.value === ''){
    //     showError(email, 'Email is required');
    // }else if(!validEmail(email.value)){
    //     showError(email, 'Email is not valid');
    // }
    // else {
    //     showSuccess(email);
    // }
    // if(password.value === ''){
    //     showError(password, 'Password is required');
    // }else {
    //     showSuccess(password);
    // }
    // if(password2.value === ''){
    //     showError(password2, 'Password 2 is required');
    // }else {
    //     showSuccess(password2);
    // }
});

//* Check required fields
function checkRequired(inputArr){
    inputArr.forEach(function(input){
        if(input.value.trim() === ''){
            showError(input, `${getFieldName(input)} is required`);
        }else{
            showSuccess(input);
        }
    })
}

//* Check input lengths
function checkLength(input, min, max){
    if(input.value.length < min ){
        showError(input, `${getFieldName(input)} must be at least ${min} characters`);
    }else if(input.value.length > max){
        showError(input, `${getFieldName(input)} must be less than ${max} characters`);
    }else{
        showSuccess(input);
    }
}

//Check email is valid
function checkEmail(input){
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if(re.test(input.value.trim())){
        showSuccess(input);
    }else{
        showError(input, 'Email is not valid')
    }
}

//* Check pw match
function checkPasswordMatch(input1, input2){
    if(input1.value !== input2.value ){
        showError(input2, 'Password do not match');
    }
}

// *Show input error msg
function showError(input, message){
    const formControl = input.parentElement;
    formControl.className = 'form-control error';
    const small = formControl.querySelector('small');
    small.innerText = message;
}

// *Show success msg
function showSuccess(input){
    const formControl = input.parentElement;
    formControl.className = 'form-control success';
}

//* Get field name
function getFieldName(input){
    return input.id.charAt(0).toUpperCase() + input.id.slice(1);
}