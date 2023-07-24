using System.Text.Json;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;

byte[] ticketData = await File.ReadAllBytesAsync(string.Join(' ', args));
Ticket ticket = Ticket.ReadFromBytes(ticketData);

Console.WriteLine(JsonSerializer.Serialize(ticket));

TicketVerifier verifier = new(ticketData, ticket, RpcnSigningKey.Instance);
Console.WriteLine(JsonSerializer.Serialize(verifier));
Console.WriteLine(verifier.IsTicketValid());