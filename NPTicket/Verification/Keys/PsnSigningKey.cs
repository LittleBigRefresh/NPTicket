namespace NPTicket.Verification.Keys;

public abstract class PsnSigningKey : ITicketSigningKey
{
    public string HashAlgorithm => "SHA-1";
    public string CurveTable => "secp192r1";
    public TicketSignatureMessageType MessageType => TicketSignatureMessageType.Ticket;
    public abstract string CurveX { get; }
    public abstract string CurveY { get; }
}