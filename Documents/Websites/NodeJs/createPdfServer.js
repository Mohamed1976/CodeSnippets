 
/*
Now install Express in the myapp directory and save it in the dependencies list. For example:
npm install puppeteer --save //Note name is case sensitive 
npm install express --save

To install Express temporarily and not add it to the dependencies list:
npm install Puppeteer

Reference:
https://thenextweb.com/syndication/2021/01/10/how-to-turn-web-pages-into-pdfs-with-puppeteer-and-nodejs/
https://expressjs.com/en/starter/installing.html
https://docs.npmjs.com/cli/v6/configuring-npm/package-json
https://github.com/puppeteer/puppeteer/blob/v5.5.0/docs/api.md#pagepdfoptions

*/

//Simple hello example
/*
const express = require('express')
const app = express()
const port = 3000

app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`)
})
*/


const express = require("express");
const puppeteer = require("puppeteer");
const app = express();
const port = 3000;

app.get("/pdf", async(req, resp) => {
    const url = req.query.target;
    console.log(`Rcvd: ${url}`);

    const browser = await puppeteer.launch({
        headless: true
    }); 

    const webPage = await browser.newPage(); 

    await webPage.goto(url, {
        waitUntil: "networkidle0"
    }); 

    const pdf = await webPage.pdf({
        printBackground: true,
        format: "Letter",
        margin: {
            top: "0px",
            bottom: "40px",
            left: "20px",
            right: "20px"
        }
    });

    await browser.close(); 
    resp.contentType("application/pdf");
    resp.send(pdf);
    // resp.send(`Request rcvd: ${url}`);
});


/*
const browser = await puppeteer.launch(); 
const webpage = await browser.newPage(); 
const url = "https://themes.3rdwavemedia.com/demo/pillar/";

await webpage.goto(url, {
    waitUntil: "networkidle0"
}); 

await webpage.pdf({
    printBackground: true,
    path: "resume.pdf",
    format: "Letter",
    margin: {
        top: "20px",
        bottom: "40px",
        left: "20px",
        right: "20px"
    }
})

await browser.close(); 
*/

app.listen(port, () => {
    console.log(`Server started @: http://localhost:${port}`)
})