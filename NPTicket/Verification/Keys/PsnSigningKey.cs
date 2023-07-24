namespace NPTicket.Verification.Keys;

public abstract class PsnSigningKey : ITicketSigningKey
{
    public string HashAlgorithm => "SHA-1";
    public string CurveTable => "secp192r1";
    public abstract string CurveX { get; }
    public abstract string CurveY { get; }
}