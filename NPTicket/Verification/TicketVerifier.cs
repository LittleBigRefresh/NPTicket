using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace NPTicket.Verification;

public class TicketVerifier
{
    private Ticket _ticket;
    // private ECPublicKeyParameters _publicKey;
    private ISigner _signer;

    public TicketVerifier(Ticket ticket, ITicketSigningKey key)
    {
        this._ticket = ticket;

        X9ECParameters xParams = ECNamedCurveTable.GetByName(key.CurveTable);
        ECDomainParameters domainParams = new(xParams.Curve, xParams.G, xParams.N, xParams.H, xParams.GetSeed());
        ECPoint ecPoint = domainParams.Curve.CreatePoint(new BigInteger(key.X, 16), new BigInteger(key.Y, 16));

        ECPublicKeyParameters publicKey = new ECPublicKeyParameters(ecPoint, domainParams);
        this._signer = SignerUtilities.GetSigner(key.HashAlgorithm + "withECDSA");
        _signer.Init(false, publicKey);
    }

    public bool IsTicketValid()
    {
        return false;
    }
}