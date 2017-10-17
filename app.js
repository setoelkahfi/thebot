require('dotenv').config();
const restify = require('restify');
const builder = require('botbuilder');


// Setup restify server
var server = restify.createServer();
server.listen(process.env.port || prosecc.env.PORT || 3978, () => {
    console.log('%s listening to %s', server.name, server.url);
});

// Create chat connector for communicating with the Bot Framework Service
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

// Listen to message from users
server.post('/api/messages', connector.listen());

// Receive messages from the user and respond by eching each message back (prefixed with 'You said:')
var bot = new builder.UniversalBot(connector, [
    (session, args, next) => {
        session.send('You said: ' + session.message.text + ' which was ' + session.message.text.length + ' characters');
    }
]);