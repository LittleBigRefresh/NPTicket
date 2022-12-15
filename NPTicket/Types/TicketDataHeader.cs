namespace NPTicket.Types;

public readonly struct TicketDataHeader
{
    public readonly ushort Type;
    public readonly ushort Length;

    public TicketDataHeader(ushort type, ushort length)
    {
        Type = type;
        Length = length;
    }
}