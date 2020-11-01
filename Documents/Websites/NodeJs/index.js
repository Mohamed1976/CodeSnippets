/* We can also install a library from https://www.npmjs.com/

npm i how-many-pizza 
or
npm install how-many-pizza

C:\Users\moham\Desktop\Websites\NodeJs>npm install how-many-pizza
npm WARN saveError ENOENT: no such file or directory, open 'C:\Users\moham\Desktop\Websites\NodeJs\package.json'
npm notice created a lockfile as package-lock.json. You should commit this file.
npm WARN enoent ENOENT: no such file or directory, open 'C:\Users\moham\Desktop\Websites\NodeJs\package.json'
npm WARN NodeJs No description
npm WARN NodeJs No repository field.
npm WARN NodeJs No README data
npm WARN NodeJs No license field.

+ how-many-pizza@1.1.2
added 1 package from 1 contributor and audited 1 package in 5.398s
found 0 vulnerabilities

Comment out block code  using (Ctrl + /)

*/

// Node.js program to demonstrate the 
// fs.writeFileSync() method

// Import the filesystem module 
const fs = require("fs");
const howManyPizza =  require("how-many-pizza");

let data = "This is a file containing a collection"
	+ " of programming languages.\n"
	+ "1. C\n2. C++\n3. Python"; 

fs.writeFileSync("programming.txt", data); 
console.log("File written successfully\n"); 
console.log("The written has the following contents:"); 
console.log(fs.readFileSync("programming.txt", "utf8"));
let people = 10000;
console.log("You need %s pizzas for %s people", howManyPizza(people), people);
//You need 3750 pizzas for 10000 people

const place = `World`;
const greeting = 'Hello';
const msg = "Hi Folks";

console.log("%s, %s, %s",place,  greeting, msg);
console.log(`${greeting}, ${place}`);

/*
Output is:
C:\Users\moham\Desktop\Websites\NodeJs>node index.js
File written successfully

The written has the following contents:
This is a file containing a collection of programming languages.
1. C
2. C++
3. Python

*/


