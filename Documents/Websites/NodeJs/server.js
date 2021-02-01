/*
https://flaviocopes.com/node-websockets/
https://glitch.com/edit/#!/flavio-websockets-server-example?path=server.js%3A1%3A0
https://glitch.com/edit/#!/flavio-websockets-client-example


https://github.com/websockets/ws
https://www.npmjs.com/package/ws
npm i ws
*/

const WebSocket = require('ws')

const wss = new WebSocket.Server({ port: 8080 })

wss.on('connection', (ws) => {
  ws.on('message', (message) => {
    console.log(`Received message => ${message}`);
    //Echo message
    ws.send(`Echo message from server: ${message}`);  
  })
  ws.send('hi!, reply from the server.')
})