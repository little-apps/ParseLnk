using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class DarwinDataBlock : ExtraDataBase<Structs.DarwinDataBlock>
    {
        public DarwinDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x00000314);
        }
        
    }
}
