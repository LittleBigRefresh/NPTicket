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

    internal ushort ReadTicketHeader()
    {
        // Ticket header
        ReadBytes(4); // Header
        return ReadUInt16(); // Ticket length
    }

    internal TicketDataSection ReadTicketSectionHeader()
    {
        this.ReadByte(); // Skip first byte of type (which is a short)

        TicketDataSectionType type = (TicketDataSectionType)this.ReadByte();
        ushort length = this.ReadUInt16();
        long position = this.BaseStream.Position;

        return new TicketDataSection(type, length, position);
    }

    private TicketData ReadTicketData(TicketDataType expectedType)
    {
        TicketData data = new TicketData((TicketDataType)ReadUInt16(), ReadUInt16());
        if (data.Type != expectedType && expectedType != TicketDataType.Empty)
            throw new FormatException($"Expected data type to be {expectedType}, was really {data.Type} ({(int)data.Type})");
        
        return data;
    }

    internal byte[] ReadTicketBinaryData(TicketDataType type = TicketDataType.Binary)
        => ReadBytes(ReadTicketData(type).Length);
    internal string ReadTicketStringData(TicketDataType type = TicketDataType.String)
        => Encoding.Default.GetString(ReadTicketBinaryData(type)).TrimEnd('\0');

    internal uint ReadTicketUInt32Data()
    {
        ReadTicketData(TicketDataType.UInt32);
        return ReadUInt32();
    }
    
    internal ulong ReadTicketUInt64Data()
    {
        ReadTicketData(TicketDataType.UInt64);
        return ReadUInt64();
    }
    
    internal DateTimeOffset ReadTicketTimestampData()
    {
        ReadTicketData(TicketDataType.Timestamp);
        return DateTimeOffset.FromUnixTimeMilliseconds((long)ReadUInt64());
    }

    internal void SkipTicketEmptyData(int sections = 1)
    {
        for (int i = 0; i < sections; i++) ReadTicketData(TicketDataType.Empty);
    }
}