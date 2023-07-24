using System.Text.Json;
using NPTicket;
using NPTicket.Test;
using NPTicket.Verification;

byte[] ticketData = await File.ReadAllBytesAsync(string.Join(' ', args));
Ticket ticket = Ticket.ReadFromBytes(ticketData);

Console.WriteLine(JsonSerializer.Serialize(ticket));

TicketVerifier verifier = new(ticketData, ticket, new LbpSigningKey());
Console.WriteLine(JsonSerializer.Serialize(verifier));
Console.WriteLine(verifier.IsTicketValid());