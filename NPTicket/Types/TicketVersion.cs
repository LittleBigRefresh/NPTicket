namespace NPTicket.Types;

public readonly struct TicketVersion
{
    public readonly byte Major;
    public readonly byte Minor;

    internal TicketVersion(byte major, byte minor)
    {
        Major = major;
        Minor = minor;
    }
}