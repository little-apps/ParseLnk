using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ConsoleFeDataBlock : ExtraDataBase<Structs.ConsoleFeDataBlock>
    {
        public ConsoleFeDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x0000000C);
        }
        
    }
}
