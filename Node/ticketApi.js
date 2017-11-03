var tickets = [];
var lastTicketId = 1;

module.exports = (req, res) => {
    console.log('Ticket received: ', req.body);
    let ticketId = lastTicketId + 1;
    var ticket = req.body;
    ticket.id = ticketId;
    tickets.push(ticket);

    res.send(ticketId.toString());
};