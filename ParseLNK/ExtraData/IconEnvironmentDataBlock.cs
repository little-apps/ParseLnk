using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class IconEnvironmentDataBlock : ExtraDataBase<Structs.IconEnvironmentDataBlock>
    {
        public IconEnvironmentDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x00000314);
        }
    }
}
