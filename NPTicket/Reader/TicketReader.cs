using System.Buffers.Binary;
using System.Text;
using NPTicket.Types;

namespace NPTicket.Reader;

internal class TicketReader : BinaryReader
{
    internal TicketReader(Stream input) : base(input)
    {}

    #region Big Endian Shenanigans

    // credit where credit is due: https://stackoverflow.com/a/71048495
    
    public override short ReadInt16() => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(2));
    public override int ReadInt32() => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(4));
    public override long ReadInt64() => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(8));

    public override ushort ReadUInt16() => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
    public override uint ReadUInt32() => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(4));
    public override ulong ReadUInt64() => BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(8));
    
    #endregion

    internal TicketVersion ReadTicketVersion() => new((byte)(ReadByte() >> 4), ReadByte());

    internal void SkipTicketHeader()
    {
        // Ticket header
        ReadBytes(4); // Header
        ReadUInt16(); // Ticket length
        
        // Section header
        ReadUInt16(); // Section Type
        ReadUInt16(); // Section Length
    }

    private TicketDataHeader ReadTicketDataHeader() => new(ReadUInt16(), ReadUInt16());

    private byte[] ReadTicketByteArray() => ReadBytes(ReadTicketDataHeader().Length);
    internal string ReadTicketString() => Encoding.Default.GetString(ReadTicketByteArray()).TrimEnd('\0');

    internal uint ReadTicketUInt32()
    {
        ReadTicketDataHeader();
        return ReadUInt32();
    }
    
    internal ulong ReadTicketUInt64()
    {
        ReadTicketDataHeader();
        return ReadUInt64();
    }
}