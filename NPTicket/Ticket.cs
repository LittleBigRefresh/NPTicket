using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using NPTicket.Reader;
using NPTicket.Types;

namespace NPTicket;

#nullable disable

[SuppressMessage("ReSharper", "NotAccessedField.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Ticket
{
    private Ticket() {}
    
    public TicketVersion Version { get; set; }
    public string SerialId { get; set; }
    public uint IssuerId { get; set; }
    
    public ulong IssuedDate { get; set; }
    public ulong ExpiryDate { get; set; }

    public ulong UserId { get; set; }
    public string Username { get; set; }

    public string Country { get; set; }
    public string Domain { get; set; }

    public string TitleId { get; set; }
    
    public uint Status { get; set; }

    [Pure]
    public static Ticket FromBytes(byte[] data)
    {
        using MemoryStream ms = new(data);
        return FromStream(ms);
    }
    
    public static Ticket FromStream(Stream stream)
    {
        Ticket ticket = new();
        using TicketReader reader = new(stream);

        ticket.Version = reader.ReadTicketVersion();
        reader.SkipTicketHeader();

        ticket.SerialId = reader.ReadTicketString();

        ticket.IssuerId = reader.ReadTicketUInt32();

        ticket.IssuedDate = reader.ReadTicketUInt64();
        ticket.ExpiryDate = reader.ReadTicketUInt64();

        ticket.UserId = reader.ReadTicketUInt64();
        ticket.Username = reader.ReadTicketString();

        ticket.Country = reader.ReadTicketString(); // No I am not going to brazil
        ticket.Domain = reader.ReadTicketString();

        ticket.TitleId = reader.ReadTicketString();

        ticket.Status = reader.ReadUInt32();

        return ticket;
    }
}