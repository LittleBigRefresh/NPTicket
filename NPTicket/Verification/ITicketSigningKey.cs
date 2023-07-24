namespace NPTicket.Verification;

public interface ITicketSigningKey
{
    string HashAlgorithm { get; }
    string CurveTable { get; }
    string X { get; }
    string Y { get; }
}