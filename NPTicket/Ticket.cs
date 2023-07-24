using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NPTicket.Reader;
using NPTicket.Types;
using NPTicket.Verification;

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
    
    public DateTimeOffset IssuedDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }

    public ulong UserId { get; set; }
    public string Username { get; set; }

    public string Country { get; set; }
    public string Domain { get; set; }

    public string ServiceId { get; set; }
    public string TitleId { get; set; }
    
    public uint Status { get; set; }
    public ushort TicketLength { get; set; }
    public TicketDataSection BodySection { get; set; }
    
    public string SignatureIdentifier { get; set; }
    public byte[] SignatureData { get; set; }

    // TODO: Use GeneratedRegex, this is not in netstandard yet
    private static readonly Regex ServiceIdRegex = new("(?<=-)[A-Z0-9]{9}(?=_)", RegexOptions.Compiled);

    [Pure]
    public static Ticket ReadFromBytes(byte[] data)
    {
        using MemoryStream ms = new(data);
        return ReadFromStream(ms);
    }
    
    public static Ticket ReadFromStream(Stream stream)
    {
        Ticket ticket = new();
        using TicketReader reader = new(stream);

        ticket.Version = reader.ReadTicketVersion();
        ticket.TicketLength = reader.ReadTicketHeader();

        // ticket version (2 bytes), header (4 bytes), ticket length (2 bytes) = 8 bytes
        const int headerLength = sizeof(byte) + sizeof(byte) + sizeof(uint) + sizeof(ushort);
        Debug.Assert(headerLength == 8);

        long actualLength = stream.Length - headerLength;
        if (ticket.TicketLength != actualLength)
            throw new FormatException($"Expected ticket length to be {ticket.TicketLength} bytes, was {actualLength} bytes instead");

        ticket.BodySection = reader.ReadTicketSectionHeader();

        if (ticket.BodySection.Type != TicketDataSectionType.Body)
        {
            throw new FormatException($"Expected first section to be {nameof(TicketDataSectionType.Body)}, " +
                                      $"was really {ticket.BodySection.Type} ({(int)ticket.BodySection.Type})");
        }

        ticket.SerialId = reader.ReadTicketStringData(TicketDataType.Binary);

        ticket.IssuerId = reader.ReadTicketUInt32Data();

        ticket.IssuedDate = reader.ReadTicketTimestampData();
        ticket.ExpiryDate = reader.ReadTicketTimestampData();

        ticket.UserId = reader.ReadTicketUInt64Data();
        ticket.Username = reader.ReadTicketStringData();

        ticket.Country = reader.ReadTicketStringData(TicketDataType.Binary); // No I am not going to brazil
        ticket.Domain = reader.ReadTicketStringData();

        ticket.ServiceId = reader.ReadTicketStringData(TicketDataType.Binary);
        ticket.TitleId = ServiceIdRegex.Matches(ticket.ServiceId)[0].ToString();

        ticket.Status = reader.ReadUInt32();
        
        // Skip padding section in ticket
        reader.SkipTicketEmptyData(3);
        
        TicketDataSection footer = reader.ReadTicketSectionHeader();
        if (footer.Type != TicketDataSectionType.Footer)
        {
            throw new FormatException($"Expected last section to be {nameof(TicketDataSectionType.Footer)}, " +
                                      $"was really {footer.Type} ({(int)footer.Type})");
        }

        ticket.SignatureIdentifier = reader.ReadTicketStringData(TicketDataType.Binary);
        ticket.SignatureData = reader.ReadTicketBinaryData();
        
        return ticket;
    }
}