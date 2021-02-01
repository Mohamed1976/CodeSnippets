/* Avoiding using the global namespace
The global namespace is where all the native JavaScript libraries live. In Internet Explorer, the
window object references the global namespace. Everything this object exposes is global.
The global namespace has far more functionality than this book can cover; however, you’ve
already seen some examples of the objects in the global namespace used in this chapter,
including the Web Storage API and the Geolocation API. The global namespace includes other
nested namespaces, such as Math, WebSocket, and JSON.
The global namespace is available to all code within an application session. With the
increasing number of third-party libraries in use, and as applications become more complex
and require the use of such libraries, the potential for naming conflicts increases. Names of
classes within a namespace must be unique. If multiple developers define a namespace with
the same name, the JavaScript runtime can’t identify which namespace they intended to use.
This is why keeping your objects out of the global namespace is important.
One strategy to avoid name collisions is to create your own namespaces for your JavaScript
libraries. One pattern to consider using to create unique namespace names is the name of
the domain in reverse, such as com.microsoft. Because domain names are unique, this pattern
helps reduce the possibility of naming collisions. The following code demonstrates this strategy
to create a namespace for a library developed for a bookstore:
By creating the objects in this way, you can be reasonably certain that if another developer
creates a useful library to manage books that you want to include in your site, you won’t have
to worry about a naming collision between your Book and Author objects and those provided
by the other library. When developing reusable JavaScript libraries, never implement your
objects in the global namespace. 

var com = {};
com.Bookstore = {};
com.Bookstore.Book = {
title: 'my book',
genre: 'fiction'
};
com.Bookstore.Author = {
firstName: 'R',
lastName: 'D'
}

Reference:
https://stackoverflow.com/questions/881515/how-do-i-declare-a-namespace-in-javascript
*/

//Registers the RandomString namespace in the global object. 
(function (global) {
    var RandomString = {};

    RandomString.AllowedCharacters  = {
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

    RandomString.RandomString = function () {
    
        this.Next = function (allowedCharacters, length, excludeSimilarChars) {        
            const randomGenerator = new RandomString.RandomGenerator();
            const charset = new RandomString.Charset();
            let randomString = new Array(); 
    
            const allowedChars = charset.GetCharacters(allowedCharacters, excludeSimilarChars);
            for(let index = 0; index < length; index++) {
                randomString.push(allowedChars[randomGenerator.Next(allowedChars.length)])
            }
            
            return randomString;
        }
    }

    RandomString.RandomGenerator = function() {
        this.Next = function(max) {
            return Math.floor(Math.random() * Math.floor(max));
        }
    }    

    RandomString.Charset = function () {

        this.GetCharacters = function(allowedCharacters, excludeSimilarChars) {
            let characters = new Array();
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.UpperCaseLetters) === RandomString.AllowedCharacters.UpperCaseLetters ) {
                
                characters = characters.concat(this.UpperCaseLetters);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.LowerCaseLetters) === RandomString.AllowedCharacters.LowerCaseLetters ) {
                
                characters = characters.concat(this.LowerCaseLetters);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.Digits) === RandomString.AllowedCharacters.Digits ) {
                
                characters = characters.concat(this.Digits);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.SpecialSymbols) === RandomString.AllowedCharacters.SpecialSymbols ) {
                
                characters = characters.concat(this.SpecialSymbols);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.Minus) === RandomString.AllowedCharacters.Minus ) {
                
                characters = characters.concat(this.Minus);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.Underscore) === RandomString.AllowedCharacters.Underscore ) {
                
                characters = characters.concat(this.Underscore);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.Space) === RandomString.AllowedCharacters.Space ) {
                
                characters = characters.concat(this.Space);
            }
    
            if ( (allowedCharacters & RandomString.AllowedCharacters.Brackets) === RandomString.AllowedCharacters.Brackets ) {
                
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

    RandomString.Charset.prototype.UpperCaseLetters = 
    [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
        'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
        'Q', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z'        
    ];
    
    RandomString.Charset.prototype.LowerCaseLetters = 
    [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
        'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
        'q', 'r', 's', 't', 'u', 'w', 'x', 'y', 'z'
    ];
    
    RandomString.Charset.prototype.Digits = 
    [
        '1','2','3','4','5','6','7','8','9','0'
    ];
    
    RandomString.Charset.prototype.SpecialSymbols = 
    [
        '!', '\"', '#', '$', '%', '&', '\'', '*',
        '+', ',', '.', '\/', ':', ';', '=', '?',
        '@', '\\', '^', '\u00B4', '`', '|', '~'
    ];
    
    RandomString.Charset.prototype.Minus = 
    [
        '-' 
    ];
    
    RandomString.Charset.prototype.Underscore = 
    [
        '_' 
    ];
    
    RandomString.Charset.prototype.Space = 
    [
        ' ' 
    ];
    
    RandomString.Charset.prototype.Brackets = 
    [
        '<', '>', '{', '}', '[', ']', '(', ')'
    ];
    
    RandomString.Charset.prototype.SimilarLookingCharacters = 
    [
        '1', 'l', 'I', '|', 'o', 'O', '0'
    ];

    global.$RandomString = RandomString;

})(window);
