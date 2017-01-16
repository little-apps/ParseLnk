using System.Diagnostics;
using System.IO;
using ParseLnk.Interop;

namespace ParseLnk.ExtraData
{
    public class ConsoleDataBlock : ExtraDataBase<Structs.ConsoleDataBlock>
    {
        public ConsoleDataBlock(Stream stream, Structs.ExtraDataHeader header) : base(stream, header)
        {
            Debug.Assert(Header.Size == 0x000000CC);
        }
    }
}
