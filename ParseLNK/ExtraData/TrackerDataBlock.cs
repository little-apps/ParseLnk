using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class TrackerDataBlock : ExtraDataBase<Structs.TrackerDataBlock>
    {
        public TrackerDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x00000060)
                throw new ExtraDataException("Header Size is not 0x60", nameof(Header.Size));
        }

        public override void Read()
        {
            base.Read();

            if (Body.Length < 0x0000058)
                throw new ExtraDataException("Body length is not greater or equal to 0x58", nameof(Body.Length));

            if (Body.Version != 0)
                throw new ExtraDataException("Body version is not 0", nameof(Body.Version));
        }
    }
}
