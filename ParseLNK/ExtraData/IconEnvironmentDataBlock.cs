using System.Diagnostics;
using System.IO;
using ParseLnk.Exceptions;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class IconEnvironmentDataBlock : ExtraDataBase<Structs.IconEnvironmentDataBlock>
    {
        public IconEnvironmentDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            if (Header.Size != 0x00000314)
                throw new ExtraDataException("Header size is not 0x314", nameof(Header.Size));
        }
    }
}
