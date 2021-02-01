"use strict"

window.onload = function () {

    const PasswordLengthValue = document.querySelector("#PasswordLengthValue");
    const lengthSlider = document.querySelector("#PasswordLength");
    const form = document.querySelector("form");
    const generatedPassword = document.querySelector("h3");
    const excludeSimilarChars = document.querySelector("#ExcludeSimilarChars");

    lengthSlider.addEventListener("input", () => {
        PasswordLengthValue.innerText = `Length: ${lengthSlider.value}`;
    });

    form.addEventListener("submit", (e) => {
        e.preventDefault();

        let allowedCharacters = window.$RandomString.AllowedCharacters.None;
        generatedPassword.innerText = "";
        const length = parseInt(lengthSlider.value);
        const removeSimilarChars = excludeSimilarChars.checked;
        
        if(document.querySelector("#UppercaseLetters").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.UpperCaseLetters;
        }

        if(document.querySelector("#LowercaseLetters").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.LowerCaseLetters;
        }
        
        if(document.querySelector("#Digits").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.Digits;    
        }

        if(document.querySelector("#SpecialSymbols").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.SpecialSymbols;    
        }

        if(document.querySelector("#MinusSymbol").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.Minus;    
        }

        if(document.querySelector("#UnderscoreSymbol").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.Underscore; 
        }

        if(document.querySelector("#SpaceSymbol").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.Space;    
        }

        if(document.querySelector("#BracketSymbols").checked) {
            allowedCharacters |= window.$RandomString.AllowedCharacters.Brackets;    
        }

        const randomString = new window.$RandomString.RandomString ();
        const randomStr = randomString.Next(allowedCharacters, length, removeSimilarChars);
        generatedPassword.innerText = randomStr.join(""); 
    });
}

function copy(sender) {
    //console.log(sender);

    const t = sender.innerText;
    const randomString = document.querySelector("h3").innerText;
    copyToClipboard(randomString) ? (sender.innerText = "Copied!") : 
        (sender.innerText = "Failed to copy");
    
    //Reset text to original.     
    setTimeout(() => (sender.innerText = t), 2e3)    
}

function copyToClipboard(e) {
    let returnVal = false;

    //Create temporary textarea needed to copy to clipboard.  
    const t = document.createElement("textarea");
    (t.style.position = "fixed"), (t.style.opacity = "0"), (t.value = e), 
    document.body.appendChild(t), t.focus(), t.select();
    
    try {
        //throw new Error("Unexpected error.");
        document.execCommand("copy");
        returnVal = true;
    } 
    catch (e) {
        alert("Failed to copy text to clipboard.");
    }

    document.body.removeChild(t)
    return returnVal;
}

/* Logic is placed in separate namespace.
window.onload = function () {

    const PasswordLengthValue = document.querySelector("#PasswordLengthValue");
    const lengthSlider = document.querySelector("#PasswordLength");
    const form = document.querySelector("form");
    const generatedPassword = document.querySelector("h3");
    const excludeSimilarChars = document.querySelector("#ExcludeSimilarChars");

    lengthSlider.addEventListener("input", () => {
        PasswordLengthValue.innerText = lengthSlider.value;
    });

    form.addEventListener("submit", (e) => {
        e.preventDefault();

        generatedPassword.innerText = "";
        const length = parseInt(PasswordLengthValue.innerText);
        const removeSimilarChars = excludeSimilarChars.checked;
        let allowedCharacters = AllowedCharacters.None; 

        if(document.querySelector("#UppercaseLetters").checked) {
            allowedCharacters |= AllowedCharacters.UpperCaseLetters;
        }

        if(document.querySelector("#LowercaseLetters").checked) {
            allowedCharacters |= AllowedCharacters.LowerCaseLetters;
        }
        
        if(document.querySelector("#Digits").checked) {
            allowedCharacters |= AllowedCharacters.Digits;    
        }

        if(document.querySelector("#SpecialSymbols").checked) {
            allowedCharacters |= AllowedCharacters.SpecialSymbols;    
        }

        if(document.querySelector("#MinusSymbol").checked) {
            allowedCharacters |= AllowedCharacters.Minus;    
        }

        if(document.querySelector("#UnderscoreSymbol").checked) {
            allowedCharacters |= AllowedCharacters.Underscore; 
        }

        if(document.querySelector("#SpaceSymbol").checked) {
            allowedCharacters |= AllowedCharacters.Space;    
        }

        if(document.querySelector("#BracketSymbols").checked) {
            allowedCharacters |= AllowedCharacters.Brackets;    
        }

        const randomString = new RandomString ();
        const randomStr = randomString.Next(allowedCharacters, length, removeSimilarChars);
        generatedPassword.innerText = randomStr.join(""); 

    }); 
}

const AllowedCharacters  = {
    None                : 0x00,
    UpperCaseLetters    : 0x01,
    LowerCaseLetters    : 0x02,
    Digits              : 0x04,
    SpecialSymbols      : 0x08,
    Minus               : 0x10,
    Underscore          : 0x20,
    Space               : 0x40,
    Brackets            : 0x80 
};

function RandomString () {
    
    this.Next = function (allowedCharacters, length, excludeSimilarChars) {        
        const randomGenerator = new RandomGenerator();
        const charset = new Charset();
        let randomString = new Array(); 

        const allowedChars = charset.GetCharacters(allowedCharacters, excludeSimilarChars);
        for(let index = 0; index < length; index++) {
            randomString.push(allowedChars[randomGenerator.Next(allowedChars.length)])
        }
        
        return randomString;
    }
}

function RandomGenerator() {
    this.Next = function(max) {
        return Math.floor(Math.random() * Math.floor(max));
    }
}

function Charset() {

    this.GetCharacters = function(allowedCharacters, excludeSimilarChars) {
        let characters = new Array();

        if ( (allowedCharacters & AllowedCharacters.UpperCaseLetters) === AllowedCharacters.UpperCaseLetters ) {
            
            characters = characters.concat(this.UpperCaseLetters);
        }

        if ( (allowedCharacters & AllowedCharacters.LowerCaseLetters) === AllowedCharacters.LowerCaseLetters ) {
            
            characters = characters.concat(this.LowerCaseLetters);
        }

        if ( (allowedCharacters & AllowedCharacters.Digits) === AllowedCharacters.Digits ) {
            
            characters = characters.concat(this.Digits);
        }

        if ( (allowedCharacters & AllowedCharacters.SpecialSymbols) === AllowedCharacters.SpecialSymbols ) {
            
            characters = characters.concat(this.SpecialSymbols);
        }

        if ( (allowedCharacters & AllowedCharacters.Minus) === AllowedCharacters.Minus ) {
            
            characters = characters.concat(this.Minus);
        }

        if ( (allowedCharacters & AllowedCharacters.Underscore) === AllowedCharacters.Underscore ) {
            
            characters = characters.concat(this.Underscore);
        }

        if ( (allowedCharacters & AllowedCharacters.Space) === AllowedCharacters.Space ) {
            
            characters = characters.concat(this.Space);
        }

        if ( (allowedCharacters & AllowedCharacters.Brackets) === AllowedCharacters.Brackets ) {
            
            characters = characters.concat(this.Brackets);
        }

        if(excludeSimilarChars) {
            for(let i = 0; i < this.SimilarLookingCharacters.length; i++) {
                const index = characters.indexOf(this.SimilarLookingCharacters[i]);
                if(index > -1) {
                    characters.splice(index, 1);
                }
            }
        }

        return characters; 
    }
}

Charset.prototype.UpperCaseLetters = 
[
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
    'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z'        
];

Charset.prototype.LowerCaseLetters = 
[
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
    'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
    'q', 'r', 's', 't', 'u', 'w', 'x', 'y', 'z'
];

Charset.prototype.Digits = 
[
    '1','2','3','4','5','6','7','8','9','0'
];

Charset.prototype.SpecialSymbols = 
[
    '!', '"', '#', '$', '%', '&', '\'', '*',
    '+', ',', '.', '/', ':', ';', '=', '?',
    '@', '\\', '^', '´', '`', '|', '~'
];

Charset.prototype.Minus = 
[
    '-' 
];

Charset.prototype.Underscore = 
[
    '_' 
];

Charset.prototype.Space = 
[
    ' ' 
];

Charset.prototype.Brackets = 
[
    '<', '>', '{', '}', '[', ']', '(', ')'
];

Charset.prototype.SimilarLookingCharacters = 
[
    '1', 'l', 'I', '|', 'o', 'O', '0'
];
*/
