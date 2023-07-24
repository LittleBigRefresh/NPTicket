namespace NPTicket.Verification;

/// <summary>
/// Defines the parameters for a key used for verification.
/// </summary>
public interface ITicketSigningKey
{
    string HashAlgorithm { get; }
    string CurveTable { get; }
    TicketSignatureMessageType MessageType { get; }
    string CurveX { get; }
    string CurveY { get; }
}