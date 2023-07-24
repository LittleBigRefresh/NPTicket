namespace NPTicket.Verification;

public readonly struct TicketSection
{
    public TicketSectionType Type { get; }
    public ushort Length { get; }
    public uint Position { get; }

    public TicketSection(TicketSectionType type, ushort length, uint position)
    {
        Type = type;
        Length = length;
        Position = position;
    }
}