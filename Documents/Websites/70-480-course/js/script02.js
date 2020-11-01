var counter = null; 

window.addEventListener("load", () => {
    DisplayPrimes();

    counter = new Counter();
});

function Counter() {
    var divElement = document.querySelector(".counter");
    var buttonElement = divElement.querySelector("button");
    /* Needs to be referenced by this object in order to update value. */
    this.spanElement = divElement.querySelector("span");
    /* Keep track of clicks. */
    this.counter = 0;

    buttonElement.addEventListener("click", () => {
        this.spanElement.innerText = ++this.counter;
    });    
}

//Array creates an instance using the new word.
//The list after the Array object type is called the object’s constructor. This information
//can be passed into the object as parameters to construct the initial state of the object.
//Some objects have many constructors to choose from, with differing sets of parameters. The
//addition of multiple constructors is called an overloaded constructor.
var listofPrimeNumbers = new Array(1, 2, 3, 5, 7, 11, 13, 17, 19, 23);
function DisplayPrimes() {
    var list = document.getElementById("primes");
    var content = "";

    for(var i = 0; i < listofPrimeNumbers.length; i++)
    {
        //Some native objects are available statically, which means you don’t need to create an instance of them.
        var squareValue = Math.sqrt(listofPrimeNumbers[i]);
        content += "<li>" + listofPrimeNumbers[i] + " (Math.sqrt) => " + squareValue + "</li>"
    }

    list.innerHTML = content; 

    //JavaScript also provides wrapper objects. These wrap up native types, for example. Native
    //types are defined as integer, string, char, and so on. When a variable is declared like this,
    var txt = "my long string";
    var num = 5;

    //you can access method on the variable like this:
    var index = txt.indexOf("long",0);
    var exp = num.toExponential(5);
    console.log("index: " + index + ", number: " + exp);

    /*The underlying types for string and integer don’t natively have methods or functionality;
    however, the JavaScript runtime creates a wrapper object for them dynamically so that some
    useful methods are available. For example, you could create the following string and number
    variables with the new keyword, but that’s not very common.*/
    var _txt = new String("#my long string");
    var _num = new Number(5);
    console.log("_txt: " + _txt + ", number: " + _num);

    /* Creating custom objects is standard practice when working with information in custom applications.
    Because JavaScript is an object-oriented language, you should apply proper objectoriented
    practices when developing JavaScript applications. In almost all cases, this involves
    creating custom objects to encapsulate functionality within logical entities. 

    The object created represents a book. It provides a way to encapsulate into a single object
    the properties that apply to a book

    In the book object, three methods have been added: turnPageForward, turnPageBackward,
    and flipTo. Each method provides some functionality to the book object, letting a reader move

    The interesting parts of this code are the function declarations themselves.
    For example, when you look at the code for the flipTo function, you might think that
    the function is called FlipToAPage because that’s what was declared. However, this isn’t the
    case. The methods are called using the alias property that assigned the function. When using
    the code, the runtime knows that it’s a method, not a property, and it expects the method to
    be called with parentheses:

    //This line throws an exception because the object does not support this method
    book.FlipToAPage(15);
    //This line works because this is what the method has been named.
    book.flipTo(15); */ 
    var book = {
        ISBN: "55555555",
        Length: 560,
        genre: "programming",
        covering: "soft",
        author: "John Doe",
        currentPage: 5,
        title: "My Big Book of Wonderful Things",
        
        flipTo: function flipToAPage(pNum) {
            this.currentPage = pNum;
        },
        
        turnPageForward: function turnForward() {
            this.currentPage++;
            //this.flipTo(this.currentPage++);
        },
        
        turnPageBackward: function turnBackward() {
            this.currentPage--;
            //this.flipTo(this.currentPage--);
        },
    
        printBook: function print() {
            console.log(this.ISBN + ", " + this.Length + ", " + this.genre  + ", " + this.covering  + ", " + 
            this.author + ", " + this.title  + ", " + this.currentPage);
        }
     }

     book.printBook();
     book.turnPageForward();
     book.turnPageForward();
     book.printBook();
     book.turnPageBackward();
     book.printBook();
     book.flipTo(220);
     book.printBook();

    /* Creating objects inline as the book object is in the previous code sample is useful only
    when it is used in the page where it’s defined, and perhaps only a few times. However, if you
    plan to use an object often, consider creating a prototype for it so that you can construct
    one whenever you need it. A prototype provides a definition of the object so that you can
    construct the object using the new keyword. When an object can be constructed, such as with
    the new keyword, the constructor can take parameters to initialize the state of the object, and
    the object itself can internally take extra steps as needed to initialize itself. The following code
    creates a prototype for the book object: */
    function Book() {
        this.ISBN = "55555555",
        this.Length = 560,
        this.genre = "programming",
        this.covering = "soft",
        this.author = "John Doe",
        this.currentPage = 5,
        this.title = "My Big Book of Wonderful Things",
        
        flipTo = function flipToAPage(pNum) {
            this.currentPage = pNum;
        },
        
        this.turnPageForward = function turnForward() {
            this.currentPage++;
        },
        
        this.turnPageBackward = function turnBackward() {
            this.currentPage--;
        },
    
        this.printBook = function print() {
            console.log(this.ISBN + ", " + this.Length + ", " + this.genre  + ", " + this.covering  + ", " + 
            this.author + ", " + this.title  + ", " + this.currentPage);
        }
     }

    console.log("\nBook object.");
    var books = new Array(new Book(), new Book(), new Book());
    console.log(books.length);
    books.forEach(book => book.printBook());

    /* Objects can contain other objects as needed. In this example, the Author property could
    easily be factored into a new prototype, making it more extensible and encapsulating the
    information related to an author. Add the following code to the Book prototype: 
    Now, the book’s Author is a custom object instead of just a string. This provides for more
    extensibility in the design. If you later decide that you need to add information about the
    author, you can simply add the property or properties to the Author prototype.
    Note multiple constructor functions are not supported. */    
    function Author(firstName, lastName, gender) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
    }
    
    Author.prototype = {
        firstName:"",
        lastName:"",
        gender:"",
        BookCount: 0,
        
        printAuthor: function print() {
            console.log(this.firstName + ", " + this.lastName + ", " + this.gender);
        }        
    }

    var author = new Author("Jane", "Trump", "Female");
    console.log("Author: " + author.firstName + ", " + author.lastName + ", " + author.gender);  
    author.printAuthor();

    /*JavaScript consists of objects. Everything in JavaScript is an object. Each object is based on
    a prototype. Whenever you create a new instance of an object, that instance is based on
    the object’s prototype. That way, you could create a blank book or create one with specific unique 
    properties. This is where prototyping comes in handy. The following code creates a prototype containing 
    two constructors that support the needs of any users of the Book object: 
    With this new code, you can create an empty Book object by using the constructor with no
    parameters, or you can create a Book object by using specific parameters to initialize some fields.*/
    function BookV2(title, length, author, genre, isbn, covering, currentPage) {
        this.ISBN = isbn;
        this.Length = length;
        this.genre = genre;
        this.covering = covering;
        this.author = author;
        this.currentPage = currentPage;
        this.title =   title;   
     } 

     BookV2.prototype = {
        ISBN: "Unknown ISBN",
        Length: -1,
        genre: "Unknown genre",
        covering: "Unknown covering",
        author: new Author("Unknown firstName", "Unknown lastName", "Unknown gender"),
        currentPage: 0,
        title: "Unknown title",         
        
        flipTo: function flipToAPage(pNum) {
            this.currentPage = pNum;
        },
        
        turnPageForward: function turnForward() {
            this.currentPage++;
        },
        
        turnPageBackward: function turnBackward() {
            this.currentPage--;
        },
    
        printBook: function print() {
            console.log(this.ISBN + ", " + this.Length + ", " + this.genre  + ", " + this.covering  + ", " + 
            this.title  + ", " + this.currentPage + ", " + this.author.firstName  + ", " + 
            this.author.lastName  + ", " + this.author.gender);
            this.author.printAuthor();
        }        
    };

    var book3 = new BookV2("The big Lebowski", 600, 
        new Author("Donald", "Duck", "Male"), "programming", "777", "soft", 8);

    book3.printBook();        

    var booksV2 = new Array(new BookV2("#1 Sunrise", 700, 
        new Author("Donald", "Duck", "Male"), "programming", "777", "soft", 8),
        new BookV2("#2 The rise of the machines.", 600, new Author("Snoop", "Dog", "Male"), 
        "programming", "777", "soft", 8),
        new BookV2("#3 Lord of the rings.", 600, new Author("Mickey", "Mouse", "Male"), 
        "programming", "777", "soft", 8));        
        
    console.log("\nBook objects using Prototype.");
    console.log(booksV2.length);
    booksV2.forEach(book => book.printBook());

    /*
    In object-oriented programming, inheritance is a fundamental concept. In standard objectoriented
    programming, classes are created in a relational hierarchy, so that the attributes and
    functionality of one entity can be reused within another entity without having to re-create all
    the code. In object-oriented parlance, if an entity satisfies the “is-a” relationship question, it’s
    a candidate for inheritance. For example, an organization is made up of employees, in which
    an employee entity has certain attributes (properties) and behaviors (methods). Management,
    executives, and staffers are all types of employees. A staffer “is-a” employee. So in an objectoriented
    design, a staffer object would inherit from an employee. This type of inheritance is
    quite easy to build in full-fledged object-oriented languages. However, JavaScript is a special
    situation because it doesn’t use classes. As you saw in the previous sections, everything is
    an object; a custom object is made up of properties where some properties are native types
    and some properties are assigned to functions to implement methods. This section examines
    object inheritance as it works in JavaScript. You can extend the Book object in a couple of ways.
    
    Object.create is a method available from the Object class in the global namespace. The
    create method takes two parameters: the object you want to create and a list of property descriptors.
    The first parameter expects to receive the prototype of the object to create or null. If
    null is specified, the object uses only those functions or properties specified in the second
    parameter. If an object prototype is specified, as in the case Book.prototype, the object
    is created with all the properties and functions declared on that object prototype. This is
    another reason designing code in a proper object-oriented way is important—so that you can
    leverage this type of functionality to keep code more readable and maintainable.
    */

   var popupBook = Object.create(book3, { 
       hasSound: {
           value:false
        },
    
        showPopUp: { 
            value: function showPop() {
                console.log("showPop");
                //do logic to show a popup
            }
        }
    });

    popupBook.showPopUp();
    popupBook.printBook();
    console.log(popupBook.hasSound);

    /*
    The second parameter enables you to add properties or behaviors to the object being
    created. Essentially, you define this additional prototype information inline with the object
    creation. This example adds the property hasSound, which has a default value specified as
    false. You could also specify additional information here, such as whether the property is
    read-only and whether it’s enumerable. Creating objects this way is similar to the inline example
    in the beginning of the earlier section on custom objects. Again, such an approach isn’t
    very modular or reusable. For every instance of a pop-up book, you’d need to declare the
    additional property and method. So again, for objects that you might want to reuse often,
    extending the Book prototype is better. Extending the Book prototype is much the same as 
    creating a new prototype. You need only one line of code to tell JavaScript to inherit the 
    functionality and attributes of another object.

    In this way, PopUpBook now extends the implementation of the Book object and adds its
    own functionality for reuse. The function PopUpBook makes a method call to Book.call(..). This
    is a call to the constructor of the super class (the class being inherited from). If the super class
    has a constructor that takes parameters, this method would enable you to pass the parameter
    values to the super-class constructors for object initialization. */
    //TODO make inheritance work. 
    function PopUpBook() {
        BookV2.call("#4 Road star.", 600, new Author("Tasmanjan", "Devil", "Male"), 
        "programming", "777", "soft", 8);
    }

    PopUpBook.prototype = BookV2.prototype;
    PopUpBook.prototype.hasSound = true;
    PopUpBook.prototype.showPopUp = function ShowPop() { console.log("function ShowPop()"); };

    var aPopUpBook = new PopUpBook();
    aPopUpBook.printBook();
    console.log(aPopUpBook.hasSound);
    aPopUpBook.showPopUp();

    /*https://css-tricks.com/the-flavors-of-object-oriented-programming-in-javascript/
    Programming in JavaScript: I’ve found there are four approaches to Object-Oriented.
    1) Using Constructor functions
    2) Using Classes
    3) Using Objects linking to other objects (OLOO)
    4) Using Factory functions

    Object-Oriented Programming is a way of writing code that allows you to create different objects from a 
    common object. The common object is usually called a blueprint while the created objects are called instances.

    Each instance has properties that are not shared with other instances. For example, if you have a Human blueprint, 
    you can create human instances with different names.

    The second aspect of Object-Oriented Programming is about structuring code when you have multiple levels of 
    blueprints. This is commonly called Inheritance or subclassing.

    The third aspect of Object Oriented Programming is about encapsulation where you hide certain pieces of 
    information within the object so they’re not accessible.    

    Using Constructor functions:
    Constructors are functions that contain a this keyword. this lets you store (and access) unique values 
    created for each instance. You can create an instance with the new keyword.

    There’s serious contention about whether Classes are bad.
    https://everyday.codes/javascript/please-stop-using-classes-in-javascript/
    https://www.toptal.com/javascript/es6-class-chaos-keeps-js-developer-up

    */

    function Human (firstName, lastName) {
        this.firstName = firstName
        this.lastName = lastName
    }

    const chris = new Human('Chris', 'Coyier');
    const zell = new Human('Zell', 'Liew')

    console.log(chris.firstName + ", " + chris.lastName); // Chris Coyier    
    console.log(zell.firstName  + ", " + zell.lastName); // Zell Liew

    class Human2 {
        constructor(firstName, lastName) {
            this.firstName = firstName
            this.lastName = lastName
        }
    }

    const chris2 = new Human2('Chris', 'Coyier')
    console.log(chris2.firstName + ", " + chris2.lastName) // Chris Coyier

    /* Objects Linking to Other Objects (OLOO)
    OLOO was coined and popularized by Kyle Simpson. In OLOO, you define the blueprint as a normal object.
    You then use a method (often named init, but that isn’t required in the way constructor is to a Class) 
    to initialize the instance. You use Object.create to create an instance. After creating the instance, 
    you need to run your init function.
    */

    const Human3 = {
        init (firstName, lastName ) {
            this.firstName = firstName
            this.lastName = lastName
        }
    } 

    const chris3 = Object.create(Human3);
    chris3.init('Chris', 'Coyier');    
    console.log(chris3.firstName + ", " + chris3.lastName); // Chris Coyier
    //You can chain init after Object.create if you returned this inside init.
    //const chris4 = Object.create(Human3).init('Chris', 'Coyier');
    //console.log(chris4.firstName + ", " + chris4.lastName);

    /* Factory functions
    Factory functions are functions that return an object. You can return any object. You can even 
    return a Class instance or OLOO instance — and it’ll still be a valid Factory function.
    You don’t need new to create instances with Factory functions. You simply call the function.    
    */

    function Human4 (firstName, lastName) {
        return {
            firstName,
        lastName
        }
    }

    const chris4 = Human4('Chris', 'Coyier');
    console.log(chris4.firstName + ", " + chris4.lastName); // Chris Coyier

    /*Declaring properties and methods
    In Object-Oriented Programming, there are two ways to declare properties and methods:
    1)Directly on the instance
    2)In the Prototype
    
    Declaring properties and methods with Constructors
    If you want to declare a property directly on an instance, you can write the property inside 
    the constructor function. Make sure to set it as the property for this.
    
    Methods are commonly declared on the Prototype because Prototype allows instances to use the 
    same method. It’s a smaller “code footprint.” To declare properties on the Prototype, you need 
    to use the prototype property.    
    */

    function Human5 (firstName, lastName) {
        // Declares properties
        this.firstName = firstName;
        this.lastname = lastName;
    
        // Declares methods
        this.sayHello = function () {
            console.log(`Hello, I'm ${this.firstName} ${this.lastname}`);
        }
    }
    
    var chris5 = new Human5('@Chris', '@Coyier'); 
    chris5.sayHello();
    console.log(chris5);

    function Human6 (firstName, lastName) {
        this.firstName = firstName
        this.lastname = lastName
    }
      
    // Declaring method on a prototype
    Human6.prototype.sayHello = function () {
        console.log(`Hello, I'm ${this.firstName} ${this.lastname}`)
    }

    var chris6 = new Human6("@#Chris", "@#Coyier");
    console.log(chris6);
    chris6.sayHello();
    //It can be clunky if you want to declare multiple methods in a Prototype.
    Human6.prototype.method1 = function () { console.log("method1"); }
    Human6.prototype.method2 = function () { console.log("method2"); }
    Human6.prototype.method3 = function () { console.log("method3"); }
    chris6.method1();
    chris6.method2();
    chris6.method3();

    //You can make things easier by using merging functions like Object.assign.
    Object.assign(Human6.prototype, {
        method4 () { console.log(`method4 ${this.firstName}`); },
        method5 () { console.log("method5"); },
        method6 () { console.log(`method6 ${this.lastname}`); }
    })

    chris6.method4();
    chris6.method5();
    chris6.method6();

    //Declaring properties and methods with Classes  

    class Human7 {
        constructor(firstName, lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
            /* Declare function in constructor. */
            this.sayHi = function() {
                console.log(`Hello ${this.firstName} ${this.lastName}`);
            }       
        }
    }

    var john = new Human7("John","Deer"); 
    john.sayHi();
    console.log(john);
    
    //It’s easier to declare methods on the prototype. You write the method after constructor like a normal function.
    //It’s easier to declare multiple methods on Classes compared to Constructors. You don’t need the Object.assign 
    //syntax. You just write more functions.
    class Human8 {
        constructor (firstName, lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
        }
      
        sayHello () {
            console.log(`Hello ${this.firstName} ${this.lastName}`); /* Defined on the prototype. */
        }
    }

    var jonathan = new Human8("Jonathan", "Tapper")
    console.log(jonathan);
    jonathan.sayHello();

    /*
    Should you declare properties and methods directly on the instance? Or should you use prototype as
    much as you can? Many people take pride that JavaScript is a “Prototypal Language” (which means it 
    uses prototypes). From this statement, you may make the assumption that using “Prototypes” is better.
    The real answer is: It doesn’t matter.
    If you declare properties and methods on instances, each instance will take up slightly more memory. 
    If you declare methods on Prototypes, the memory used by each instance will decrease, but not much. 
    This difference is insignificant with computer processing power what it is today. Instead, you want 
    to look at how easy it is to write code — and whether it is possible to use Prototypes in the first place.    
    */

    class Programmer {
        constructor (firstName, lastName) {
            this.firstName = firstName
            this.lastName = lastName
        }
  
        sayHello () {
            console.log(`Hello Programmer, I'm ${this.firstName}  ${this.lastName}`);
        }
    }

    var aProgrammer = new Programmer("Go","Bernie"); 
    console.log(aProgrammer);
    aProgrammer.sayHello();

    //The WebDeveloper class will extend Programmer like this:
    class WebDeveloper extends Programmer {
        constructor(firstName, lastName) {
            super(firstName, lastName)
        }
      
        /* Override base class method using sayHello() {. */
        showLanguage() {
            console.log(`I am a web developer, my name is ${this.firstName} ${this.lastName}`);
        }
    }

    var aWebDeveloper = new WebDeveloper("Carl","Bernstein"); 
    console.log(aWebDeveloper);
    aWebDeveloper.sayHello();
    aWebDeveloper.showLanguage();

    /*
    But where the inheritance comes in is on SuperHero.prototype. In order to ensure that it inherits 
    the methods from SuperHuman.prototype, we actually make it an instance of SuperHuman with new SuperHuman().
        
    It's important to note that even though constructors are often referred to as "classes," 
    they really aren't the same thing as classes in other languages. In JavaScript, a constructor 
    is just a function invoked by the new operator which builds a new object.
    http://adripofjavascript.com/blog/drips/basic-inheritance-with-javascript-constructors.html
    */

   function SuperHuman (name, superPower) {
        this.name = name;
        this.superPower = superPower;
        this.Greeting = function() {
            console.log("Greeting from SuperHuman: " + name);
        } 
    }

    SuperHuman.prototype.usePower = function (message) {
        console.log(this.superPower + "!, message: " + message);
    };

    /* Inherits from SuperHuman */
    function SuperHero (name, superPower, allegiance) {
        // Reuse SuperHuman initialization
        SuperHuman.call(this, name, superPower);
    
        this.allegiance = allegiance;

        this.personalGreeting = function(message) {
            console.log(this.name + " saved the day!, message: " + message + ", allegiance: " + this.allegiance);
        }
    }

    /* Needed for inheritance from  SuperHuman, both notations can be used, whats the difference?  */
    SuperHero.prototype = new SuperHuman();
    //SuperHero.prototype = SuperHuman.prototype;

    SuperHero.prototype.saveTheDay = function () {
        console.log(this.name + " saved the day!");
    };

    var aSuperHuman = new SuperHuman("Silver Banshee", "sonic wail");
    console.log(aSuperHuman);
    aSuperHuman.Greeting();
    aSuperHuman.usePower("Message in a bottle.");

    var aSuperHero = new SuperHero("Captain Marvel", "magic", "Good");
    console.log(aSuperHero);
    aSuperHero.Greeting();
    aSuperHero.usePower("@@@Message in a bottle.");
    aSuperHero.personalGreeting("Take a break.");
    aSuperHero.saveTheDay();
} 




