"use strict";

window.addEventListener("load", () => {
    example001();
    example002();
    example003();
    example004();
    example005();
    example006();
    example007();
});

/* Implementing iterative control flow
You’ve seen how to use if statements to control program flow. Another concept you can use
to control the flow of JavaScript programs is iterative flow control, which enables you to loop
over a block of code many times. You’ve already seen some iterative operations when you
reviewed the advanced methods on the array object that use callbacks. There, the iterative
flow control was built into the various array methods. In this section, you’ll examine the native
iterative control statements, including for and while loops. */
function example007() {
    /*
    Using the for loop
    The for loop is useful in cases in which a block of code should run based on a deterministic
    number of items. In some cases, you might want to iterate over a list of items in an array
    or list; in other cases, you might want to run a block of code a specific number of times to
    perform some type of animation or to create a specific number of objects.
    The syntax of the for loop is as follows:
    for(<counter>;<expression>;<counter increment>)
    {
    <code to run>
    }
    The for loop needs three elements: a counter, an expression, and an increment.
    ■■ The counter variable holds the current number of times the loop has run. You need to
    initialize the counter appropriately, such as to 1 or 0.
    ■■ The expression element evaluates the counter against some other value. Its purpose is
    to set a limit to the number of times the for loop runs. For example, if you wanted the
    loop to run 10 times, you could initialize the counter to 0, and the expression would be
    counter < 10. Doing so would ensure that the loop would run only while the expression
    returns true, that is, while the counter variable is less than 10. As soon as the
    counter equals 10, loop processing would stop, and code processing would continue
    after the for loop.
    ■■ With the counter increment, the for loop must be told how to adjust the counter variable
    after each loop iteration. The increment can be positive or negative depending on
    how the loop is set up. You can set the increment so that the loop counts sequentially,
    or use mathematical operators to increment by a different value.
    The code or body of the loop is a block of code surrounded by curly braces. This code
    section runs for each loop iteration. The following code samples demonstrate various ways to
    use a for loop.
    First, here’s a simple for loop that runs a block of code 10 times:
    Notice that because the counter is starting at 0, the expression is to be less than 10. If the
    counter is to start at 1, the expression would be <= 10. This is important to keep an eye on.
    The counter increment uses the addition shorthand ++ to increase the counter by one on
    each iteration. The following code goes in the reverse order:
    In addition to using the increment or decrement operators, you can multiply or divide the
    counter value. The following code prints out a set of numbers that increase by a factor of 2 up to 100:
    The expression piece of the for loop doesn’t need to be a hard-coded value, like what has
    been shown so far. Instead, you can derive the expression from the length of an object or
    another variable, as in this example:

    Because a string is just an array of characters, this code can iterate over the string and print
    each character to the screen. The length of the string determines how many times the loop runs.    
    */

    for (var i = 0; i < 10; i++) {
        //document.write(i); 
        console.log("For loop: " + i);
    }

    for (var i = 10; i > 0; i--) {
        //document.write(i); 
        console.log("For loop: " + i);
    }

    for(var i= 1; i<100;i*=2){
        console.log("For loop <br/>: " + i);
        //document.write(i);
        //document.write("<br />");
    }    

    var reversedAlphabet = "";
    var alphabet = 'abcdefghijklmnopqrstuvwxyz';
    //Reverse using for loop
    for (var i = (alphabet.length -1); i >= 0; i--)
    {
        reversedAlphabet += alphabet[i];
    }
    console.log(reversedAlphabet);

    var counter = 5; 
 
    for(;loopCheck();) {
        console.log("loopCheck(), counter = " + counter);
    }

    function loopCheck()
    {
        return counter-- > 0; 
    }

    /*
    Using the for…in loop
    The for…in loop is a method for iterating over an object’s properties. Take the following example:
    This for loop prints out the name of each property on the custom person object. If you
    want the loop to print the property values instead, each property needs to be accessed via
    the property indexer of the object, as in this example:        */
    var person = { firstName: "Jane", lastName: "Doe", birthDate: "Jan 5, 1925", gender: "female" };
    for (var prop in person) {
        console.log(prop +", "+ person[prop]);
        //document.write(prop);
    }

    /*
    Using the while loop
    The while loop lets you run a code block until some condition evaluates to false. The construct
    of the while loop is as follows:
    while(<expression>){
        <code block>
    }
    The expression is something that evaluates to a Boolean. While the expression is true, the
    while loop continues to run. The code that runs is contained within the code block inside the
    braces. The condition must be true for the while loop to run at all. Because the while loop
    doesn’t use an incrementer like the for loop does, the code inside the while loop must be able
    to set the expression to false as appropriate; otherwise, the loop will be an endless loop. You
    might actually want to use an endless loop, but you must ensure that the processing of the
    loop doesn’t block the application’s main thread. The following code demonstrates a while loop:
    */

    var j = 0;
    while (j < 10) {
        console.log("while: " + j);
        //do some work here.
        j++;
    }

    /*
    Using the do…while loop
    The key difference between the while loop and the do…while loop is that do…while always
    runs at least the first time. In contrast, the while loop first evaluates the expression to
    determine whether it should run at all, and then continues to run as long as the expression
    evaluates to true. The do…while loop always runs once because in this form of loop,
    the expression logic is at the bottom. The do…while loop achieves this with the following
    structure:
    do{
        <code block>
    }while(<expression>)
    */
    var k = 0;
    do {

        if(k === 1) {
            console.log("Skipping this iteration @: " + k);
            continue;
        }

        console.log("Do While {}: " + k);
        if(k === 3) {
            console.log("Breaking the loop @: " + k);
            break;
        }

    } while (k++ < 5);

    /*
    Short-circuiting the loops
    Two mechanisms enable you to short-circuit a loop. The break keyword exits the current loop
    completely, whereas the continue keyword breaks out of the code block and continues to the
    next iteration of the loop.

    The break keyword breaks out of only the currently running loop. If the loop containing
    the break is nested inside another loop, the outer loop continues to iterate as controlled by
    its own expression.
    */
}

/* Using advanced array methods
This section examines some of the more advanced array methods. These methods all involve
the use of a callback.

Some advanced functions enable you to change the source array, whereas others don’t.
This is an important aspect to keep clear.
*/
function example006() {
    /*The every method lets you process specific logic for each array element to determine whether
    any of them meet some condition. Look at following code:

    In this code, assume that the evenNumber array is created with a list of what you expected
    to be all even numbers. To validate this, you can use the every method.
    The every method takes two parameters:
    ■ The name of the function that should be processed for each element
    ■ An optional reference to the array object
    The evenNumberCheck function called for each item in the array returns true or false for
    each item, depending on whether it meets the desired criteria. In this example, the value
    is tested to ensure that it’s an even number. If it is, the function returns true; otherwise, it
    returns false. As soon as the every method gets the first false result for any item in the array,
    it exits and returns false. Otherwise, if all elements in the array return true, the every method
    returns true. In the preceding code sample, an if statement was added to evaluate the return
    value of the every method and take an appropriate action. In this example, the evenNumber-
    Check function returns false on the sixth item in the array, because 9 is an odd number, so the
    test for even fails. */
    var evenNumbers = new Array(2, 4, 6, 8, 9, 10, 12, 7);
    console.log("Begin evenNumberCheck().");
    var allEven = evenNumbers.every(evenNumberCheck, this);
    console.log("End evenNumberCheck().");

    if (allEven) {
        console.log("All digits are even.");
    }
    else {
        console.log("NOT All digits are even.");
    }

    /* Predicate */
    function evenNumberCheck(value, index, array) {
        console.log(index + ", " + value + ", " + array[index]);
        return (value % 2) == 0;
    }

    /* Using the some method
    The some method works very much like the every method. The difference is that some checks
    only whether any item in the array meets the criteria. In this case, the some method returns
    true if the called function returns true for any single element. If all elements in the array return
    false, the some method returns false. By this definition, you can use some to achieve the
    exact opposite of the every method when the some method returns false. The following code
    is updated from the previous example so that it uses the some method:
    
    With the code updated to use the some method, the return result is true, because some
    of the values in the array are even numbers. Had this result returned false, you would know
    that all the elements in the array were odd numbers.    
    */
    var someEven = evenNumbers.some(evenNumberCheck, evenNumbers);    
    if (someEven) {
        console.log("Some digits are even.");
    }
    else {
        console.log("None of the digits are even.");
    }

    /* Using the forEach method
    The forEach method enables an application to process some logic against each item in the
    array. This method runs for every single item and doesn’t produce a return value. forEach has
    the same signature as the other methods you’ve seen so far in this section. The following code
    demonstrates the forEach method:
    In this sample, the code assumes that a list element on the HTML page is ready to be filled
    with the list of sports, each formatted as a child node. Each element in the list is passed to the
    function and added as an <li> element. The array elements aren’t sorted in this case. You can
    chain the methods together to ensure that the elements are, for example, alphabetized:
    Like with all the advanced methods shown thus far, the elements are passed to the function
    in ascending index order. So you could call the sort method and chain it together with
    the forEach method to ensure that the elements are displayed to the user in order.
    */
    var sportsArray = ['soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby'];
    var sportsList = document.getElementById("sports");
    //sportsArray.forEach(UpdateList); you can chain methods as shown below
    sportsArray.sort().forEach(UpdateList);

    function UpdateList(value, index, array) {
        var bullet = document.createElement("li");
        bullet.innerText = value;
        sportsList.appendChild(bullet);
    }

    /* Using the filter method
    The filter method provides a way to remove items for an array based on some processing
    done in the callback function. The filter method returns a new array containing the elements
    that are included based on a return value of true or false from the callback function. In the
    even number example, you can use the filter method to scrub the array and ensure that the
    program continues to use only an array that contains even numbers, as demonstrated here:
    
    In this example, the evenNumberCheck method is the same as the one used previously.
    However, rather than use the every or any method to determine the quality of the data with
    respect to containing only even numbers, the filter method simplifies the removal of the
    odd numbers. You can use any logic in the callback function to process the element and
    determine whether it should be included in the returned array, such as pattern matching or a
    database lookup.*/
    var onlyUnevenNumbers = evenNumbers.filter(unevenNumberCheck, evenNumbers);

    function unevenNumberCheck(value, index, array) {
        return value % 2 != 0; 
    }

    onlyUnevenNumbers.forEach(number => console.log("onlyUnevenNumbers: " + number));

    /* Using the map method
    The map method enables you to replace values in the array. Every element in the array is
    passed to a callback function. The callback function’s return value replaces the value for the
    position in the array that was passed in. The following example demonstrates having every
    number in an array rounded off appropriately: */
    var money = [12.8, 15.9, 21.7, 35.2];
    var roundedMoney = money.map(roundOff, money);

    function roundOff(value, position, array) {
        return Math.round(value);
    }

    //Display rounded and not rounded values.
    for(var i = 0 ; i < money.length; i++)
    {
        console.log("money: " + money[i] +", rounded to: " + roundedMoney[i]);
    }

    //This example provides the square of a series of numbers:
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8];
    var squares = numbers.map(squareNumber, numbers);

    function squareNumber(value, position, array) {
        return Math.pow(value,2);//  value * value;
    }    

    for(var i = 0 ; i < numbers.length; i++)
    {
        console.log("numbers: " + numbers[i] +", squares to: " + squares[i]);
    }

    /* Using the reduce and reduceRight methods
    The reduce and reduceRight methods are recursive. Each result of the callback function is
    passed back into the callback method as the previous return value along with the current
    element to be passed in. This provides some interesting scenarios. The reduce method processes
    the elements of the array in ascending order, whereas the reduceRight processes the
    elements of the array in descending order. The following example demonstrates using the
    reduce method to calculate a factorial:
    In this function, the factorial for 10 is calculated. In the math world, it’s denoted as 10! */
    var facNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    var factorials = numbers.reduce(factorial);
    
    function factorial(previous, current) {
        return previous * current;
    }

    console.log(factorials); //40320
} 

/* Working with arrays
Arrays are JavaScript objects and are created just like any other JavaScript object, with the
new keyword: This code example shows an Array object being instantiated and demonstrating the three
available constructors. The first line creates an empty Array object without a default size. The
second line creates an Array object with a default size. Each value in the array is undefined
because nothing is assigned to it yet. The last example creates an array initialized with data. In
addition to the object constructors, you can create an array as follows:
Under the hood, JavaScript converts the anArray variable to the Array object type. After
creating an array, you can access its elements by using square brackets following the variable
name, as shown in this example:

You access elements within an array by their indexed position. This example accesses the
element at index position 1 and assigns a value to it. Arrays in JavaScript are zero-based,
which means that the first element in the array is at index zero, not at index one. The last
element is at index Array.length –1—in the preceding example, 5–1=4. Hence, the array
element indexes are 0, 1, 2, 3, and 4.

Using the length property
The length property provides information on how long the array is—that is, how many elements
the array has allocated at the time the property is evaluated. This property is useful for
situations in which you need to iterate over an array or to show users how many items are in
the array at a specific point in time, such as in a queue. The following example shows how to
access the length property: */
function example005() {
    var anArray1 = new Array();
    var anArray2 = new Array(5);
    var anArray3 = new Array('soccer', "basketball", `badminton`);

    anArray1[1] = "Soccer"; 
    anArray1[0] = "Footbal";
    console.log(anArray1[1] + ", " + anArray1[0]);

    /*
    Sizing arrays is very dynamic. In the preceding example, even though the array is initially
    declared to have a length of 5, if you try to access the 10th element, the array automatically
    resizes to accommodate the requested length. The following example demonstrates this concept:
    */
    var anArray4 = new Array(5);
    console.log("Array 4 initial length: " + anArray4.length);
    anArray4[9] = "soccer";
    console.log("Array 4 length after adding item on pos 9: " + anArray4.length);

    /* A multi-dimensional array can contain other arrays. The following code demonstrates this:
        This example creates a two dimensional 3 × 4 array. Each array isn’t required to be the
        same size; this example was just coded that way. Accessing the elements of a two-dimensional
        array is much the same as accessing a one-dimensional array, but you use two indexes:
    */
    var multiArray = new Array(3);
    multiArray[0] = new Array(4);
    multiArray[1] = new Array(4);
    multiArray[2] = new Array(4);

    /*
    Using the concat method
    The concat method combines two or more arrays into one array:
    The array returned by the concat method and stored in the combinedSports variable
    contains all the elements from both arrays in sequence. The contents of the moreSports array
    appear after the elements of the sports array in this example.
    */
    var sports = new Array( 'football', 'cricket', 'rugby', 'tennis', 'badminton');
    var moreSports = new Array('soccer', 'basketball', 'hockey');
    var combinedSports = sports.concat(moreSports);
    console.log("combinedSports length: " + combinedSports.length);
    //combinedSports.forEach(sport => console.log(sport));

    /*  
    Using the indexOf and lastIndexOf methods
    The indexOf method provides a way to find the index of a known element. The following code
    sample demonstrates this:
    This example calls the indexOf method to determine the index of the element ‘football’.
    The indexOf method accepts two parameters: what to search for and the index at which to
    begin searching. This example searches the entire array, so the search starts at index 0. The
    result from this call to the indexOf method is 3, because the element is found in the fourth
    position. If the element being sought isn’t found, the method returns a value of –1.
    The indexOf method uses the identity operator to check for equality, which means that
    if an array contains strings such as ‘1’, ‘2’, and ‘3’ and you’re searching for the integer 3, the
    result is –1 because the equality operation returns false for all elements in the array. The
    indexOf method searches in ascending index order. To search in descending order—that is,
    to search from the end of the array to the beginning—use the lastIndexOf method, which
    accepts the same parameters.*/
    var sports = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'football', 'tennis', 'badminton');
    var index = sports.indexOf('football', 0);
    console.log("index: " + index); //index == 3
    var index2 = sports.lastIndexOf('football');
    console.log("index2: " + index2); //index == 6

    /*Using the join method
    The join method joins all the elements in an array into a single string separated by a specified
    string separator. For example, to convert an array of strings into a comma-separated list, you
    could use the following code: The join method accepts a string as a parameter, which is the string 
    used as a delimiter to separate the values in the array. The result is a string of all the elements 
    separated by the string passed into the join method. */
    var sports2 = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'tennis', 'badminton');
    var joined = sports2.join(', ');
    console.log(joined);

    /*Using the reverse method
    The reverse method reverses the sequence of all elements in the array. This example reverses
    the sports array: The method reverses all the items so that ‘soccer’ becomes the last item in 
    the array and ‘badminton’ becomes the first item. */
    var sports3 = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'tennis', 'badminton');
    sports3.reverse();
    var joined2 = sports3.join(', ');
    console.log(joined2);
    
    /*
    Using the sort method
    The sort method sequences the items in the array in ascending order. In the sports array, the
    sort would be alphabetical, as shown in the following example: The result is that the sports array 
    is now sorted. The alert boxes show the index of the ‘soccer’ element before and after the sort, 
    demonstrating that the element has moved from position 0 to position 6 in the array.
    */
   var sports4 = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'tennis', 'badminton');
   console.log(`Before sort, indexOf("cricket"): ` + sports4.indexOf("cricket"));
   sports4.sort();
   console.log(`After sort, indexOf("cricket"): ` + sports4.indexOf("cricket"));

    /* Using the slice method
    The slice method takes out one or more items in an array and moves them to a new array.
    Consider the following array with the list of sports:
    The slice method takes two parameters: the indexes where the slice operation should
    begin and end. The ending index isn’t included in the slice. All copied elements are returned
    as an array from the slice method. In this example, because ‘basketball’ is at index 1 and the
    ending index is specified at index 2, the resulting array someSports contains only one element:
    ‘basketball’. */
    var sports5 = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'tennis', 'badminton');
    var someSports = sports5.slice(1, 2); //Contains only element 1
    var someSports2 = sports5.slice(2, 5); //Contains elements 2 , 3 and 4
    console.log(someSports.join(", "));
    console.log(someSports2.join(", "));

    /*Using the splice method
    The splice method provides a way to replace items in an array with new items. The following
    code demonstrates this:
    The splice method returns an array containing the items that are spliced out of the source
    array. The first parameter is the index in the array where the splice operation should start.
    The second parameter is the number of items to splice, starting from the index specified in
    the first parameter. The optional last parameter lists items that are to replace the items being
    spliced out. The list doesn’t have to be the same length as the items being spliced out. In
    fact, if the last parameter is omitted, the spliced items are simply removed from the array and
    not replaced. In this example, three items are replaced, starting at index 1. So, ‘basketball’,
    ‘hockey’, and ‘football’ are replaced with ‘golf’, ‘curling’, and ‘darts’.    */
   var sports6 = new Array('soccer', 'basketball', 'hockey', 'football', 'cricket', 'rugby', 'tennis', 'badminton');
   var splicedItems = sports6.splice(1, 3, 'golf', 'curling', 'darts'); //splicedItems Array contains 'golf', 'curling', 'darts'     
   console.log(sports6.join(", "));
   console.log(splicedItems.join(", "));

    /*Stack Last in first out, LIFO 
    Using the pop and push methods 
    The pop and push methods provide stack functionality. The push method appends the specified
    items to the end of the array. The pop method removes the last item from the array. The
    following code demonstrates the push method:
    This code creates an Array object, and then inserts (pushes) three items into the array.
    The items are added to the stack in the same order in which they appear in the parameter
    list. Next, the code pushes one additional item onto the stack. The pop method removes and
    returns the last item in the array:
    When this code runs, the nextSport variable holds the value ‘football’ because that was the
    last value added to the array. 
    You can use the pop and push methods in any context to add and remove items from the
    end of an array. The stack concept is useful but isn’t a confining mechanism that limits use
    of these methods to just stack arrays. */
    var sports7 = new Array();
    sports7.push('soccer', 'basketball', 'hockey');
    sports7.push('football');
    console.log(sports7.join(",  "));
    console.log(sports7.pop());

    /*
    Using the shift and unshift methods
    The shift and unshift methods work in the exact opposite way from the pop and push
    methods. The shift method removes and returns the first element of the array, whereas the
    unshift method inserts new elements at the beginning of the array. The following code uses
    the shift and unshift methods:
    */
   var sports8 = new Array();
   sports8.unshift('soccer', 'basketball', 'hockey');
   sports8.unshift('football');
   console.log(sports8.join(",  "));   
   var nextSport = sports8.shift();
   console.log(nextSport);

    /* Heap First In First Out, FIFO  
    The net result of this code is exactly the same as for the pop and push code, except the
    operations occur at the front of the array instead of the end. In other words, this example still
    illustrates the stack functionality of “last in, first out.”
    Taken together, the two concepts you’ve just seen (pop/push and shift/unshift) can be
    combined to create the concept of a first-in-first-out queue. The following code demonstrates
    using a queue in which the front of the line is the beginning of the array and the end of the
    line is the end of the array:
    This code first pushes some items into the array. This means that each item is added to the
    end of the array. When an item is needed from the array, the shift method gets the first item
    out of the beginning of the array—the item at index 0. You can easily implement the opposite
    mechanism by using the unshift and pop methods, which would achieve the same results but
    enter and retrieve items from the opposite ends of the array from this example.
    */
    var myHeap = new Array();
    myHeap.push("Alfa");
    myHeap.push("Beta");
    myHeap.push("Gamma");
    var myValue = myHeap.shift(); //Should be Alfa
    console.log(myValue); 
    myHeap.push("Delta"); //added to the end of array
    var myValue = myHeap.shift(); //Beta
    console.log(myValue); 
}

/*
The ternary operator is essentially a shorthand mechanism for an if statement. The syntax of
the ternary operation is <expression> ? <true part>: <false part>
When the expression evaluates to true, the true part runs; otherwise, the false part runs.
This code demonstrates using the ternary operator to check the background color of the canvas:
*/
function example004() {
    var element = document.getElementById("mainHeader");

    element.style.color == "cornflowerblue" ? console.log("** cornflowerblue **") : 
        console.log("** NOT cornflowerblue **");
}  

/*
Structuring code this way is syntactically correct. However, lengthy if statements can
prove difficult to read and even harder to maintain. If your if statements are becoming quite
long—for example, if the previous code example had to test for 15 different colors—a switch
statement might be more appropriate.
The switch statement consists of several parts. The first is the switch keyword itself,
followed by parentheses surrounding an expression to evaluate. This particular example
evaluates the background color of the canvas element.
Following the switch line is a series of case statements enclosed in braces. The case statement
provides the values to evaluate against. This example provides three cases to evaluate:
one for each of the possible red, green, and yellow background colors.
Each case statement contains a required break keyword. This keyword denotes the end of
that particular case statement. Only the first case that evaluates to true in a switch statement
will be processed. Omitting the break keyword will cause unexpected behavior.
The last piece of the switch statement is the optional default keyword, which serves as a
failsafe. If none of the case statements evaluate to true, the default statement provides a way
to handle the situation. You might not want to take any action when none of the case statements
evaluates to true—in which case you can omit the default statement. However, it does
enable you to handle the scenario where one of the conditions should have been reached but
wasn’t, possibly due to bad data being passed into a method or a valid case being missed.
Including a default to account for both of those scenarios is good practice.

Note:
The values used in the case statement for the purposes of the evaluation must be expressed
as a constant. For example, switching on an integer value to determine whether
it’s divisible by another number won’t work because the case would require an expression
instead of a constant value. For example, case x / 10: would be an invalid case statement.
However, the switch statement itself can accept an expression to evaluate against all cases
inside the switch block.
*/
function example003() {
    var element = document.getElementById("mainHeader");

    switch(element.style.color)
    {
        case "cornflowerblue":
            console.log("switch: cornflowerblue color");
            break;
        case "blue":
            console.log("blue color");
            break;
        //The following code demonstrates a case in which you want the same code to run for 
        //both the red and yellow color conditions: the code block following that case statement is processed, thus
        //implying a logical OR. You don’t need to explicitly use the logical OR operator (||) to leverage logical OR semantics.    
        case "yellow":    
        case "red":
            console.log("red or yellow color");
            break;
        default:
            console.log("Unknown color.");
            break;
    }
}

/*
The following example examines the background color of an element and processes
specific behavior based on the color:
This code retrieves a reference to a page element called mainHeader and then evaluates that
element’s background color to determine an appropriate action to take.
*/
function example002() {
    var element = document.getElementById("mainHeader");

    if(element.style.color == "cornflowerblue") {
        console.log("mainHeader has color cornflowerblue: " + element.style.color);
        element.textContent = element.textContent + ", has color cornflowerblue." 
    }  
}

/*
Conditional statements such as the if statement can be nested, like in the following
example:
In this example, the logic tests whether a user is older than a specified age. If the user is
over the specified age, the logic in the true branch runs. At that point, another if statement
evaluates the user’s gender. If the user’s age is younger than the required minimum, the false
branch is processed. Here an else if statement performs additional conditional processing on
the false branch based on the gender. Again, the code processes a specific branch depending
on whether the user is male or female.
*/
function example001() {
    var userAge = 10, gender = 'M';
    var minimumAge = 11;
    
    if (userAge > minimumAge) {
        if (gender == 'M') {
            console.log("do logic for above age male");
        }
        else {
            console.log("do logic for above age female.");
        }
    } 
    else if (gender == 'M') {
        console.log("do logic for underage male");
    } 
    else {
        console.log("do logic for underage female.");
    }
}