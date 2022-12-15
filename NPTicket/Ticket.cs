using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using NPTicket.Reader;
using NPTicket.Types;

namespace NPTicket;

[SuppressMessage("ReSharper", "NotAccessedField.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Ticket
{
    private Ticket() {}
    
    public TicketVersion Version;
    public string SerialId;
    public uint IssuerId;
    
    public ulong IssuedDate;
    public ulong ExpiryDate;

    public ulong UserId;
    public string Username;

    public string Country;
    public string Domain;

    public string TitleId;
    
    public uint Status;

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