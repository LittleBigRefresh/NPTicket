using System.Text.Json;
using NPTicket;

var ticketData = await File.ReadAllBytesAsync(string.Join(' ', args));
var ticket = Ticket.FromBytes(ticketData);

Console.WriteLine(JsonSerializer.Serialize(ticket));