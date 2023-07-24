using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace NPTicket.Verification;

public class TicketVerifier
{
    private readonly Ticket _ticket;
    private readonly byte[] _ticketData;
    private readonly ISigner _signer;

    public TicketVerifier(byte[] ticketData, Ticket ticket, ITicketSigningKey key)
    {
        this._ticketData = ticketData;
        this._ticket = ticket;

        X9ECParameters xParams = ECNamedCurveTable.GetByName(key.CurveTable);
        ECDomainParameters domainParams = new(xParams.Curve, xParams.G, xParams.N, xParams.H, xParams.GetSeed());
        ECPoint ecPoint = domainParams.Curve.CreatePoint(new BigInteger(key.CurveX, 16), new BigInteger(key.CurveY, 16));

        ECPublicKeyParameters publicKey = new ECPublicKeyParameters(ecPoint, domainParams);
        this._signer = SignerUtilities.GetSigner(key.HashAlgorithm + "withECDSA");
        this._signer.Init(false, publicKey);
    }

    public bool IsTicketValid()
    {
        this._signer.BlockUpdate(this._ticketData, this._ticket.BodySection.Position, this._ticket.BodySection.Length);
        return this._signer.VerifySignature(this._ticketData);
    }
}